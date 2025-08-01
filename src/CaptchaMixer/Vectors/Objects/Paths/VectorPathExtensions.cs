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

namespace Kaspersky.CaptchaMixer;

public static class VectorPathExtensions
{
	#region Add instruction chaining shortcuts

	/// <summary>
	/// Adds <paramref name="instruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/> and returns the same path.
	/// </summary>
	/// <returns>
	/// The same <paramref name="path"/>.
	/// </returns>
	public static VectorPath Add(this VectorPath path, VectorPathInstruction instruction)
	{
		path.Instructions.Add(instruction);
		return path;
	}

	/// <summary>
	/// Adds a new <see cref="MoveToInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="MoveToInstruction(float, float)" path="/param"/>
	/// <returns><inheritdoc cref="Add(VectorPath, VectorPathInstruction)" path="/returns"/></returns>
	public static VectorPath MoveTo(this VectorPath path, float x, float y)
		=> path.Add(new MoveToInstruction(x, y));

	/// <summary><inheritdoc cref="MoveTo(VectorPath, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="MoveToInstruction(Vector2)" path="/param"/>
	public static VectorPath MoveTo(this VectorPath path, Vector2 point)
		=> path.Add(new MoveToInstruction(point));

	/// <summary>
	/// Adds a new <see cref="CloseInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <returns><inheritdoc cref="Add(VectorPath, VectorPathInstruction)" path="/returns"/></returns>
	public static VectorPath Close(this VectorPath path)
		=> path.Add(new CloseInstruction());

	/// <summary>
	/// Adds a new <see cref="LineToInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="LineToInstruction(float, float)" path="/param"/>
	public static VectorPath LineTo(this VectorPath path, float endX, float endY)
		=> path.Add(new LineToInstruction(endX, endY));

	/// <summary><inheritdoc cref="LineTo(VectorPath, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="LineToInstruction(Vector2)" path="/param"/>
	public static VectorPath LineTo(this VectorPath path, Vector2 end)
		=> path.Add(new LineToInstruction(end));

	/// <summary>
	/// Adds a new <see cref="QuadToInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="QuadToInstruction(float, float, float, float)" path="/param"/>
	public static VectorPath QuadTo(
		this VectorPath path,
		float controlX,
		float controlY,
		float endX,
		float endY)
		=> path.Add(new QuadToInstruction(controlX, controlY, endX, endY));

	/// <summary><inheritdoc cref="QuadTo(VectorPath, float, float, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="QuadToInstruction(Vector2, Vector2)" path="/param"/>
	public static VectorPath QuadTo(this VectorPath path, Vector2 control, Vector2 end)
		=> path.Add(new QuadToInstruction(control, end));

	/// <summary>
	/// Adds a new <see cref="QuadToInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="CubicToInstruction(float, float, float, float, float, float)" path="/param"/>
	public static VectorPath CubicTo(
		this VectorPath path,
		float control1X,
		float control1Y,
		float control2X,
		float control2Y,
		float endX,
		float endY)
		=> path.Add(new CubicToInstruction(control1X, control1Y, control2X, control2Y, endX, endY));

	/// <summary><inheritdoc cref="CubicTo(VectorPath, float, float, float, float, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="CubicToInstruction(Vector2, Vector2, Vector2)" path="/param"/>
	public static VectorPath CubicTo(
		this VectorPath path,
		Vector2 control1,
		Vector2 control2,
		Vector2 end)
		=> path.Add(new CubicToInstruction(control1, control2, end));

	/// <summary>
	/// Adds a new <see cref="RationalToInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="RationalToInstruction(float, float, float, float, float, float, float)" path="/param"/>
	public static VectorPath RationalTo(
		this VectorPath path,
		float startWeight,
		float controlX,
		float controlY,
		float controlWeight,
		float endX,
		float endY,
		float endWeight)
		=> path.Add(new RationalToInstruction(startWeight, controlX, controlY, controlWeight, endX, endY, endWeight));

	/// <summary><inheritdoc cref="RationalTo(VectorPath, float, float, float, float, float, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="RationalToInstruction(float, Vector3, Vector3)" path="/param"/>
	public static VectorPath RationalTo(
		this VectorPath path,
		float startWeight,
		Vector3 control,
		Vector3 end)
		=> path.Add(new RationalToInstruction(startWeight, control, end));

	/// <summary><inheritdoc cref="RationalTo(VectorPath, float, float, float, float, float, float, float)" path="/summary"/></summary>
	/// <inheritdoc cref="RationalToInstruction(float, Vector2, float, Vector2, float)" path="/param"/>
	public static VectorPath RationalTo(
		this VectorPath path,
		float startWeight,
		Vector2 control,
		float controlWeight,
		Vector2 end,
		float endWeight)
		=> path.Add(new RationalToInstruction(startWeight, control, controlWeight, end, endWeight));

	/// <summary>
	/// Adds a new <see cref="AddRectInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddRect(
		this VectorPath path,
		float left,
		float top,
		float right,
		float bottom,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRectInstruction(left, top, right, bottom, direction));

	/// <summary><inheritdoc cref="AddRect(VectorPath, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddRectInstruction(Vector2, Vector2, PathDirection)" path="/param"/>
	public static VectorPath AddRect(
		this VectorPath path,
		Vector2 leftTop,
		Vector2 rightBottom,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRectInstruction(leftTop, rightBottom, direction));

	/// <summary>
	/// Adds a new <see cref="AddRoundRectInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="AddRoundRectInstruction(float, float, float, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddRoundRect(
		this VectorPath path,
		float left,
		float top,
		float right,
		float bottom,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRoundRectInstruction(left, top, right, bottom, radius, direction));

	/// <summary><inheritdoc cref="AddRoundRect(VectorPath, float, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddRoundRectInstruction(float, float, float, float, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddRoundRect(
		this VectorPath path,
		float left,
		float top,
		float right,
		float bottom,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRoundRectInstruction(left, top, right, bottom, radiusX, radiusY, direction));

	/// <summary><inheritdoc cref="AddRoundRect(VectorPath, float, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddRoundRectInstruction(Vector2, Vector2, float, PathDirection)" path="/param"/>
	public static VectorPath AddRoundRect(
		this VectorPath path,
		Vector2 leftTop,
		Vector2 rightBottom,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRoundRectInstruction(leftTop, rightBottom, radius, direction));

	/// <summary><inheritdoc cref="AddRoundRect(VectorPath, float, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddRoundRectInstruction(Vector2, Vector2, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddRoundRect(
		this VectorPath path,
		Vector2 leftTop,
		Vector2 rightBottom,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddRoundRectInstruction(leftTop, rightBottom, radiusX, radiusY, direction));

	/// <summary>
	/// Adds a new <see cref="AddOvalInstruction"/> to <paramref name="path"/>'s
	/// <see cref="VectorPath.Instructions"/>.
	/// </summary>
	/// <inheritdoc cref="AddOvalInstruction(float, float, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddOval(
		this VectorPath path,
		float left,
		float top,
		float right,
		float bottom,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddOvalInstruction(left, top, right, bottom, direction));

	/// <summary><inheritdoc cref="AddOval(VectorPath, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddOvalInstruction(Vector2, Vector2, PathDirection)" path="/param"/>
	public static VectorPath AddOval(
		this VectorPath path,
		Vector2 leftTop,
		Vector2 rightBottom,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddOvalInstruction(leftTop, rightBottom, direction));

	/// <summary><inheritdoc cref="AddOval(VectorPath, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddOvalInstruction(Vector2, float, PathDirection)" path="/param"/>
	public static VectorPath AddOval(
		this VectorPath path,
		Vector2 center,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddOvalInstruction(center, radius, direction));

	/// <summary><inheritdoc cref="AddOval(VectorPath, float, float, float, float, PathDirection)" path="/summary"/></summary>
	/// <inheritdoc cref="AddOvalInstruction(Vector2, float, float, PathDirection)" path="/param"/>
	public static VectorPath AddOval(
		this VectorPath path,
		Vector2 center,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		=> path.Add(new AddOvalInstruction(center, radiusX, radiusY, direction));

	#endregion

	/// <returns>
	/// <paramref name="path"/> converted to <see cref="SKPath"/>.
	/// </returns>
	/// <exception cref="NotSupportedException"/>
	public static SKPath ToSKPath(this VectorPath path)
	{
		var result = new SKPath();

		foreach (var (position, instruction) in path)
		{
			switch (instruction)
			{
				case LineToInstruction line:
					result.LineTo(line.End.ToSKPoint());
					break;
				case MoveToInstruction move:
					result.MoveTo(move.Point.ToSKPoint());
					break;
				case CloseInstruction _:
					result.Close();
					break;
				case CubicToInstruction cubic:
					result.CubicTo(cubic.Control1.ToSKPoint(), cubic.Control2.ToSKPoint(), cubic.End.ToSKPoint());
					break;
				case QuadToInstruction quad:
					result.QuadTo(quad.Control.ToSKPoint(), quad.End.ToSKPoint());
					break;
				case RationalToInstruction rational:

					// SkiaSharp does not support full-fledged rational curves. it only provides one specific
					// case - a so-called "conic" curve which is a rational curve with the weights of start and
					// end points set to 1 and the weight of control point is the only one left customizable.
					// in order to support rational curves we will have to convert them into lines which has
					// accuracy limitations.

					// however drawing a SkiaSharp's native "conic" curve is faster then splitting a rational
					// curve into lines and then drawing all of them, so let's leave a small optimization.
					if (rational.StartWeight == 1f && rational.EndWeight == 1f)
					{
						result.ConicTo(rational.Control.ToSKPoint(), rational.End.ToSKPoint(), rational.ControlWeight);
						break;
					}

					// 170° is a very roughly estimated angle at which it does not catch the eye and at the
					// same time the curve is split into not too many segments.
					var points = VectorMath.CurveRationalAngleSplit(
						new CurveQuad3(
							new(position, rational.StartWeight),
							new(rational.Control, rational.ControlWeight),
							new(rational.End, rational.EndWeight)),
						170);

					foreach (var point in points.Skip(1))
						result.LineTo(point.ToSKPoint());

					break;

				case AddOvalInstruction oval:
					result.AddOval(oval.ToSKRect(), oval.Direction.ToSKPathDirection());
					break;
				case AddRoundRectInstruction roundRect:
					result.AddRoundRect(roundRect.ToSKRect(), roundRect.RadiusX, roundRect.RadiusY, roundRect.Direction.ToSKPathDirection());
					break;
				case AddRectInstruction rect:
					result.AddRect(rect.ToSKRect(), rect.Direction.ToSKPathDirection());
					break;
				default:
					throw new NotSupportedException($"Cannot convert {instruction.GetType().Name} to {nameof(SKPath)} steps");
			}
		}

		return result;
	}

	/// <returns>
	/// <see langword="true"/> if <paramref name="path"/> contains any instruction which causes
	/// anything to be actually drawn.
	/// </returns>
	public static bool IsMeaningful(this VectorPath path)
		=> path?.Instructions.IsMeaningful() ?? false;

	/// <summary>
	/// Split path onto separate contours.
	/// </summary>
	/// <remarks>
	/// The following instructions are used as contour markers:<br/>
	/// - <see cref="CloseInstruction"/>;<br/>
	/// - <see cref="MoveToInstruction"/>;<br/>
	/// - <see cref="ContourInstruction"/> - closes the current contour and draws an entire new\
	/// contour from a different point.<br/>
	/// </remarks>
	/// <param name="addCloses">
	/// Add a <see cref="CloseInstruction"/> to the end of each recognized controur.
	/// </param>
	/// <returns>
	/// A sequence of instruction lists each of which describes a contour.
	/// </returns>
	public static IEnumerable<List<VectorPathInstruction>> EnumerateContours(this VectorPath path, bool addCloses = true)
	{
		var newContour = new List<VectorPathInstruction>();
		var newContours = new List<List<VectorPathInstruction>>();

		foreach (var (position, instruction) in path)
		{
			switch (instruction)
			{
				case CloseInstruction close:
					// closes the current contour by adding a straight line to contour start point
					// if we're not standing right on it already
					if (addCloses) newContour.Add(close);
					StartNext();
					break;
				case MoveToInstruction _:
					// starts a new contour regardless of whether the current one has been closed or not
					StartNext();
					newContour.Add(instruction);
					break;
				case ContourInstruction contour:
					// works like MoveTo + draws and immediately closes an entire contour
					StartNext();
					newContour.Add(contour);
					StartNext();
					break;
				default:
					// if we're at the start of a new contour, then pen stands at the end of the previous one.
					// we must take that into account, otherwise the new contour would be drawn from (0, 0) point.
					if (newContour.Count == 0)
						newContour.Add(new MoveToInstruction(position));

					newContour.Add(instruction);
					break;
			}
		}

		AddCurrent();
		return newContours;

		void AddCurrent()
		{
			if (newContour.IsMeaningful())
				newContours.Add(newContour);
		}

		void StartNext()
		{
			AddCurrent();
			newContour = new List<VectorPathInstruction>();
		}
	}

	/// <returns>
	/// <paramref name="path"/> shifted by <paramref name="dX"/> along X axis.
	/// </returns>
	public static VectorPath MoveX(this VectorPath path, float dX)
		=> ProcessInstructionsPoints(path, p => p.MoveX(dX));

	/// <returns>
	/// <paramref name="path"/> shifted by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static VectorPath MoveY(this VectorPath path, float dY)
		=> ProcessInstructionsPoints(path, p => p.MoveY(dY));

	/// <returns>
	/// <paramref name="path"/> shifted by <paramref name="dX"/> along X axis and
	/// by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static VectorPath Move(this VectorPath path, float dX, float dY)
		=> ProcessInstructionsPoints(path, p => p.Move(dX, dY));

	/// <returns>
	/// <paramref name="path"/> scaled by <paramref name="scaleX"/> along X axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorPath ScaleX(this VectorPath path, Vector2 basePoint, float scaleX)
		=> ProcessInstructionsPoints(path, p => p.ScaleX(basePoint, scaleX));

	/// <returns>
	/// <paramref name="path"/> scaled by <paramref name="scaleY"/> along Y axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorPath ScaleY(this VectorPath path, Vector2 basePoint, float scaleY)
		=> ProcessInstructionsPoints(path, p => p.ScaleY(basePoint, scaleY));

	/// <returns>
	/// <paramref name="path"/> scaled by <paramref name="scaleX"/> along X and by
	/// <paramref name="scaleY"/> along Y axis relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorPath Scale(this VectorPath path, Vector2 basePoint, float scaleX, float scaleY)
		=> ProcessInstructionsPoints(path, p => p.Scale(basePoint, scaleX, scaleY));

	/// <returns>
	/// <paramref name="path"/> scaled by <paramref name="scale"/> along X and Y axes
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorPath Scale(this VectorPath path, Vector2 basePoint, float scale)
		=> ProcessInstructionsPoints(path, p => p.Scale(basePoint, scale));

	/// <returns>
	/// <paramref name="path"/> rotated by <paramref name="angle"/> degrees
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorPath Rotate(this VectorPath path, Vector2 basePoint, float angle)
		=> ProcessInstructionsPoints(path, p => p.Rotate(basePoint, angle));

	private static VectorPath ProcessInstructionsPoints(VectorPath path, Func<Vector2, Vector2> processor)
	{
		foreach (var instruction in path.Instructions)
			for (var i = 0; i < instruction.Points.Length; i++)
				instruction.Points[i] = processor(instruction.Points[i]);

		return path;
	}

	/// <summary>
	/// Alters <paramref name="path"/> instructions in such a way so the path becomes less "shaky".
	/// Or more "shaky" when unusual <paramref name="ratio"/> values are used.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Straightening is performed by detecting "beacon points" and "attractable points" on paths
	/// and then moving attractable points closer to beacon points depending on <paramref name="ratio"/>.
	/// </para>
	/// <para>
	/// Attractable points:<br/>
	/// - For Bezier curves - their control points;<br/>
	/// - For line segments - points which serve as an end for one segment and a start for the next one
	/// (only sequential pairs of line segments are processed).
	/// </para>
	/// <para>
	/// Beacon points:<br/>
	/// - For Bezier quadratic curves - center of line between start and end points;<br/>
	/// - For Bezier cubic curves - start point for first control point, end point for second;<br/>
	/// - For pairs of adjacent line segments - center of line between the first segment's start and the
	/// second segment's end.
	/// </para>
	/// <para>
	/// Ratio = 1 moves attractable points right onto their beacon points effectively transforming
	/// Bezier curves and pairs of adjacent line segments into straight lines. Ratio = 0 leaves
	/// attractable points where they were, ratio &lt; 0 moves them in the opposite direction
	/// (increases "shakiness"), ratio &gt; 1 moves them beyond beacon points.
	/// </para>
	/// <para>
	/// If the distance between a beacon and a attractable point exceeds <paramref name="maxDistance"/>
	/// then the point is ignored. This adjusts the algorithm's granularity.
	/// </para>
	/// </remarks>
	/// <param name="ratio">
	/// Straghtening ratio:<br/>
	/// - Values in range (0; 1) are for normal straightening;<br/>
	/// - Values &lt; 0 cause de-straightening;<br/>
	/// - Values &gt; 1 mostly cause distortions (which may also sometimes be useful), especially for
	/// Bezier cubic curves.
	/// </param>
	/// <param name="maxDistance">
	/// Maximal distance between attractable and beacon points. If the distance between points exceed
	/// this value then the attractable point is left untouched.
	/// </param>
	/// <returns>
	/// The same <paramref name="path"/> with changed instructions.
	/// </returns>
	public static VectorPath Straighten(this VectorPath path, float ratio, float maxDistance)
	{
		if (ratio == 0) return path;

		(Vector2 position, VectorPathInstruction instruction) prev = default;
		var newInstructions = new List<VectorPathInstruction>();

		foreach (var (position, instruction) in path)
		{
			switch (instruction)
			{
				case QuadToInstruction quad:
					var beacon = VectorMath.LineCenter(position, quad.End);
					quad.Control = MovePoint(quad.Control, beacon);
					goto default;
				case CubicToInstruction cubic:
					cubic.Control1 = MovePoint(cubic.Control1, position);
					cubic.Control2 = MovePoint(cubic.Control2, cubic.End);
					goto default;
				case RationalToInstruction rational:
					// same as QuadToInstruction
					beacon = VectorMath.LineCenter(position, rational.End);
					rational.Control = MovePoint(rational.Control, beacon);
					goto default;
				case LineToInstruction line:
					// only line pairs are processable
					if (prev.instruction is not LineToInstruction prevLine) goto default;
					beacon = VectorMath.LineCenter(prev.position, line.End);
					prevLine.End = MovePoint(prevLine.End, beacon);
					newInstructions.Add(instruction);
					// since we've modified the previous line's end, the current pen position
					// has also changed. if we don't reflect it in the "prev" tuple and the
					// next instruction is also a line then the next iteration would use an
					// unchanged pen position and produce poor results.
					prev = (prevLine.End, instruction);
					break;
				default:
					newInstructions.Add(instruction);
					prev = (position, instruction);
					break;
			}
		}

		path.Instructions = newInstructions;
		return path;

		Vector2 MovePoint(Vector2 point, Vector2 beacon)
		{
			if (VectorMath.LineLength(point, beacon) > maxDistance) return point;
			return VectorMath.LinePoint(point, beacon, ratio);
		}
	}

	/// <summary>
	/// Lowers <paramref name="path"/> detail level.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Lowering detail level is performed by detecting sequences of instructions which have little
	/// effect on the path trajectory and replacing them with a single <see cref="LineToInstruction"/>.
	/// </para>
	/// <para>
	/// A "sequence with little effect on path trajectory" is a sequence whose all non-control points
	/// are no further than <paramref name="baseLineDistance"/> from a line segment between the very
	/// first and the very last points of the entire sequence.
	/// </para>
	/// <para>
	/// Since Bezier curves' control points are ignored, the resultant path never contains them - they
	/// are always replaced with straight lines. This fits well with the method's purpose of lowering
	/// detail since curves are smooth (very detailed) by nature.
	/// However for big single curves this leads to significant path visual changes. You can mitigate
	/// this by granulating paths prior to primitivizing them
	/// (<see cref="VectorPathInstructionExtensions.Granulate(VectorPathInstruction, Vector2, float)"/>).
	/// Such combination will eventually transform a curve into a sequence of smaller line segments
	/// which follow the trajectory of the original curve.
	/// </para>
	/// </remarks>
	/// <param name="baseLineDistance">
	/// Maximal distance between instructions' points and their corresponding "base line" which is used
	/// to recognize instruction sequences to be replaced with a single straight line. Higher values
	/// mean lower resultant detail level and vise-versa.
	/// </param>
	/// <returns>
	/// The same <paramref name="path"/> with changed instructions.
	/// </returns>
	public static VectorPath Primitivize(this VectorPath path, float baseLineDistance)
	{
		var newInstructions = new List<VectorPathInstruction>();

		foreach (var contour in path.EnumerateContours())
		{
			if (contour.Count <= 1) continue;

			var points = contour
				.SelectMany(i => i.EnumeratePoints())
				.Where(t => t.type == PointType.Contour)
				.Select(t => t.point)
				.ToList();

			if (points.Count < 2) continue;

			// detect flat sequences of points such that a perpendicular from any point to the line
			// between the first and last points in sequence does not exceed baseLineDistance

			if (newInstructions.Count > 0 || !(points[0].X == 0 && points[0].Y == 0))
				newInstructions.Add(new MoveToInstruction(points[0]));

			var start = 0;

			for (var i = 1; i < points.Count; i++)
			{
				var contained = true;
				for (var j = start + 1; j < i; j++)
				{
					var angle = VectorMath.AngleBetweenRad(points[start], points[i], points[j]);
					var distance = VectorMath.LineLength(points[i], points[j]);
					var perpendicular = distance * Math.Sin(angle);
					if (perpendicular > baseLineDistance)
					{
						contained = false;
						break;
					}
				}

				if (contained) continue;

				// if any intermediate point does not fit the distance rule then the previous
				// point has ended the sequence.
				newInstructions.Add(new LineToInstruction(points[i - 1]));
				start = i - 1;
			}

			// the last point is added anyway, it had no chance in cycle
			newInstructions.Add(new LineToInstruction(points.Last()));

			// the contour may had been closed or not - it's unclear by points only
			if (contour.Last() is CloseInstruction)
				newInstructions.Add(new CloseInstruction());
		}

		path.Instructions = newInstructions;
		return path;
	}
}
