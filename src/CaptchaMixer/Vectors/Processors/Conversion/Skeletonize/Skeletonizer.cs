// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using static Kaspersky.CaptchaMixer.SkeletonizeDirections;

using Pixel = Kaspersky.CaptchaMixer.SkeletonizePixel;
using Role = Kaspersky.CaptchaMixer.SkeletonizePixelRole;
using State = Kaspersky.CaptchaMixer.SkeletonizePixelState;

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// Implements the algorithm of paths skeletonization by rendering them, then reducing
/// raster image to a set of lines of 1 pixel width, and then transforming those lines
/// into a vector path.
/// </summary>
public static class Skeletonizer
{
	private static readonly SKColor BackColor = new(0, 0, 0, 255);
	private static readonly SKColor ForeColor = new(255, 255, 255, 255);
	private static readonly SKPaint ForePaint = new()
	{
		IsAntialias = false,
		Style = SKPaintStyle.Fill,
		Color = ForeColor
	};

	/// <summary>
	/// Skeletonizes <paramref name="path"/>. Contents of the path will be replaced.
	/// </summary>
	/// <remarks>
	/// Skeletonization involves rendering, pixels analysis and paths tracing:<br/>
	/// - Render the path on a raster layer;<br/>
	/// - Gradually remove pixels which stand of the shapes' borders;<br/>
	/// - When only 1-pixel-width lines remain, recognize their trajectories and
	/// convert them into a new vector path.
	/// </remarks>
	/// <param name="quality">
	/// Skeletonization quality ratio. Defines the scale factor used for rendering.
	/// Bigger quality = bigger raster image to analyze, longer calculations, and
	/// better result detail level.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static void Skeletonize(this VectorPath path, float quality = 1.0f)
	{
		if (quality <= 0)
			throw new ArgumentOutOfRangeException(nameof(quality), "Quality must be > 0");

		var bounds = path.GetBounds();
		var size = new Size((int)Math.Ceiling(bounds.Width * quality), (int)Math.Ceiling(bounds.Height * quality));

		// move path to the top-left corner and scale it. we'll later revert this.
		path.Move(-bounds.Left, -bounds.Top);
		path.Scale(new Vector2(0, 0), quality);

		// render path
		var layer = new RasterLayer(null, size);
		using var bitmap = layer.ToSKBitmap();
		using var canvas = new SKCanvas(bitmap);
		canvas.Clear(BackColor);
		canvas.DrawPath(path.ToSKPath(), ForePaint);

		// the order of scanning directions.
		//
		// each iteration will only remove pixels which are removable by criteria of surrounding
		// pixels, and at the same time have no surrounding pixel on current direction.
		//
		// per-direction scanning is required. otherwise lines with width of 2 pixels would be
		// removed completely - pixels on both their sides match removal criterias.
		//
		// using all 8 scanning direction instead of just 4 has also been tested, but in most
		// cases led to overall poorer results - skeleton lines had smoother trajectories but
		// more losses, especially in sharp letters' angles.
		var directions = new[] { Top, Bottom, Left, Right };

		// existing pixels count will steadily decrease during the algorithm's work.
		// but on each iteration we only need to analyze potentially removable pixels.
		// in order to not allocate memory every time we will use two arrays - one for the current
		// iteration and one for the next one. at the end of iteration we will be swapping them.
		//
		// also note the call to UpdateIfExists. the map does not update pixels state itself.
		// we can do it right now to not perform a whole cycle later.
		var map = new SkeletonizeMap(layer);
		var pixelsCurr = map.Where(UpdateIfExists).ToArray();
		var lengthCurr = pixelsCurr.Length;
		var pixelsNext = new Pixel[lengthCurr];
		var lengthNext = 0;

		// var iteration = 1;
		// bitmap.Export(@"C:\Temp\skeleton.png");

		while (true)
		{
			var removed = 0;

			foreach (var direction in directions)
			{
				for (var i = 0; i < lengthCurr; i++)
				{
					var pixel = pixelsCurr[i];
					var state = pixel.State;

					if (state != State.Removable && state != State.Unremovable)
						continue;

					if (state == State.Removable && !pixel.SurroundExists(direction))
					{
						removed++;
						pixel.Remove();
						// bitmap.SetPixel(pixel.X, pixel.Y, BackColor);
					}
					else
					{
						// the exact reason of why we haven't removed this pixel on this iteration
						// does not matter. we'll anyway have to reconsider it later on.
						pixelsNext[lengthNext++] = pixel;
					}
				}

				// swap arrays
				(pixelsCurr, pixelsNext) = (pixelsNext, pixelsCurr);
				(lengthCurr, lengthNext) = (lengthNext, lengthCurr);

				if (lengthCurr == 0) break;
				lengthNext = 0;

				// update the states of pixels which would be interesting on the next iteration
				for (var i = 0; i < lengthCurr; i++)
					pixelsCurr[i].Update();

				// bitmap.Export($@"C:\Temp\skeleton-{iteration++:00}-{direction}.png");
			}

			if (removed == 0) break;
		}

		// now we have a set of pixels with information about each of them:
		// - type: a loner, line end, intermediate or a node
		// - gaps in surrounding pixels - tells about the count of "branches" in different directions
		// - which neighbour pixels exist
		//
		// using this information we detect connected pixel sequences of several types:
		// - a single dot
		// - "end - end"
		// - "end - node"
		// - "node - node"
		// - "ring"
		//
		// then we transform each sequence into a non-closed vector contour.
		// a contour starts at some point of a sequence and proceeds by walking through
		// centers and angles of all straight parts (2 pixels or more) until it finds an end.

		var sequences = new List<List<(Pixel pixel, int direction)>>();

		// dictionary: pixel > count of times it shall be used in resultant vectors
		var pixels = map.Where(p => p.Exists).ToArray();
		var usages = pixels.ToDictionary(p => p, p => p.Role == Role.Node ? p.Gaps : 1);

		// the order of detecting types of sequences affects the eventual composition of vectors.
		//
		// we'll start with lonely pixels to simply get rid of them.
		// then we'll build sequences from line ends to isolate standalone sequences as well as
		// sequences which end on nodes.
		// then we'll build sequences from nodes to nodes.
		// and finally we'll detect rings.
		//
		// ideally we shall not use "node - node" sequences at all because they add unnecessary
		// fragmentation to vectors. think of the letter "A". we usually imagine it as two tilted
		// lines with a bridge in the middle - not like a set of shorter lines drawn separately.
		// but unfortunately fixing this would require a way more complicated algorithm.

		foreach (var alone in pixels.Where(p => p.Role == Role.Alone))
			sequences.Add(BuildSequence(alone));

		while (true)
		{
			var end = pixels.FirstOrDefault(p => p.Role == Role.End && usages[p] > 0);
			if (end == null) break;
			sequences.Add(BuildSequence(end));
		}

		while (true)
		{
			var node = pixels.FirstOrDefault(p => p.Role == Role.Node && usages[p] > 0);
			if (node == null) break;
			sequences.Add(BuildSequence(node));
		}

		while (true)
		{
			var kv = usages.FirstOrDefault(kv => kv.Value > 0);
			if (kv.Key == null) break;
			sequences.Add(BuildSequence(kv.Key));
		}

		path.Instructions.Clear();

		foreach (var sequence in sequences)
		{
			if (sequence.Count == 1)
			{
				var alone = sequence[0].pixel.ToVector2();
				path.Instructions.Add(new MoveToInstruction(alone));
				path.Instructions.Add(new LineToInstruction(alone));
				continue;
			}

			var start = sequence[0].pixel.ToVector2();
			var points = new List<Vector2> { start };
			var sectionStart = start;
			var dirPrev = sequence[1].direction;

			for (var i = 1; i < sequence.Count; i++)
			{
				var dirCurr = sequence[i].direction;
				if (dirCurr == dirPrev) continue;

				// trajectory direction has changed. now it's important to look ahead for better results.
				// if the next pixel has the same direction we'be had on the previous step - pixels form
				// a "ladder" which would be best to draw smoothly - through the middle of the finished
				// section. but if the next pixel's direction either remains the same or changes to
				// something else different - then we have either a straight or a sharp angle - and then
				// we shall use exactly the point where the turn occurs.

				var sectionEnd = sequence[i - 1].pixel.ToVector2();
				var dirNext = dirPrev;
				if (i < sequence.Count - 1) dirNext = sequence[i + 1].direction;
				points.Add(dirNext == dirPrev ? VectorMath.LineCenter(sectionStart, sectionEnd) : sectionEnd);
				sectionStart = sectionEnd;
				dirPrev = dirCurr;

				// if the last three point lie on one line then remove the intermediate one
				var count = points.Count;
				if (count <= 2) continue;
				var d12 = VectorMath.LineLength(points[count - 3], points[count - 2]);
				var d23 = VectorMath.LineLength(points[count - 2], points[count - 1]);
				var d13 = VectorMath.LineLength(points[count - 3], points[count - 1]);
				if (d12 + d23 > d13) continue;
				points.RemoveAt(count - 2);
			}

			points.Add(sequence[^1].pixel.ToVector2());

			path.Instructions.Add(new MoveToInstruction(points[0]));
			path.Instructions.AddRange(points.Skip(1).Select(p => new LineToInstruction(p)));
		}

		// restore original scale and position
		path.Scale(new Vector2(0, 0), 1 / quality);
		path.Move(bounds.Left, bounds.Top);

		static bool UpdateIfExists(Pixel pixel)
		{
			if (!pixel.Exists) return false;
			pixel.Update();
			return true;
		}

		List<(Pixel pixel, int direction)> BuildSequence(Pixel start)
		{
			var result = new List<(Pixel pixel, int direction)>();

			UsePixel(start, -1, result);

			if (start.Role == Role.Alone) return result;

			var pixel = start;
			int direction = -1;
			int backwards = -1;

			while (true)
			{
				// find a pixel in one of directions which can be used, but without
				// check reverse direction
				(pixel, direction, backwards) = OrderSurrounds(pixel)
					.FirstOrDefault(t =>
						t.pixel.Exists &&
						t.direction != backwards &&
						usages[t.pixel] > 0);

				if (pixel == null)
				{
					// this is a ring which has started not from a node because in such case a
					// node would have had at least one usage left and we wouldn't have got here.
					// the ring's start has already been used, so that's why it wasn't found.
					result.Add((start, direction));
					break;
				}

				UsePixel(pixel, direction, result);

				// we came up either to the line end or to a node - sequence is finished
				if (pixel.Role != Role.Line) break;
			}

			return result;
		}

		void UsePixel(Pixel pixel, int direction, List<(Pixel pixel, int direction)> list)
		{
			usages[pixel] = usages[pixel] - 1;
			list.Add((pixel, direction));
		}

		IEnumerable<(Pixel pixel, int direction, int backwards)> OrderSurrounds(Pixel pixel)
		{
			var surrounds = pixel.Surrounds;
			yield return (surrounds[Top], Top, Bottom);
			yield return (surrounds[Bottom], Bottom, Top);
			yield return (surrounds[Left], Left, Right);
			yield return (surrounds[Right], Right, Left);
			yield return (surrounds[LeftTop], LeftTop, RightBottom);
			yield return (surrounds[RightBottom], RightBottom, LeftTop);
			yield return (surrounds[LeftBottom], LeftBottom, RightTop);
			yield return (surrounds[RightTop], RightTop, LeftBottom);
		}
	}
}
