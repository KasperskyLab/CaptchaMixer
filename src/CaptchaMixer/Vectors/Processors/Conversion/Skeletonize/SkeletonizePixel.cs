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
/// Information about a specific pixel in the skeletonization process.
/// </summary>
internal class SkeletonizePixel
{
	// pre-allocated array of existing neighboring pixels to not allocate it on every iteration
	private readonly bool[] _surroundExists = new bool[8];

	// false means there is no sense in recalculating the pixel's state anymore.
	// gets set to true after removing neighboring pixels.
	private bool _updateRequired;

	/// <summary>
	/// Surrounding pixels. Indexes match directions from <see cref="SkeletonizeDirections"/>.
	/// </summary>
	internal Pixel[] Surrounds { get; } = new Pixel[8];

	/// <summary>
	/// X coordinate.
	/// </summary>
	internal int X { get; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	internal int Y { get; }

	/// <summary>
	/// Does the pixel exist.
	/// </summary>
	internal bool Exists { get; private set; }

	/// <summary>
	/// Pixel's state.
	/// </summary>
	internal State State { get; private set; }

	/// <summary>
	/// Role assigned to the pixel.
	/// </summary>
	internal Role Role { get; private set; }

	/// <summary>
	/// Count of "gaps" in neighboring pixels.
	/// </summary>
	internal int Gaps { get; private set; }

	internal SkeletonizePixel(int x, int y, bool exists)
	{
		X = x;
		Y = y;
		Exists = exists;
		State = exists ? State.Unknown : State.Empty;
		_updateRequired = exists;
	}

	internal void Init(
		Pixel lt,
		Pixel t,
		Pixel rt,
		Pixel r,
		Pixel rb,
		Pixel b,
		Pixel lb,
		Pixel l)
	{
		Surrounds[LeftTop] = lt;
		Surrounds[Top] = t;
		Surrounds[RightTop] = rt;
		Surrounds[Right] = r;
		Surrounds[RightBottom] = rb;
		Surrounds[Bottom] = b;
		Surrounds[LeftBottom] = lb;
		Surrounds[Left] = l;
	}

	private void SetState(State value, Role? role = null)
	{
		if (role.HasValue) Role = role.Value;
		if (State == value) return;

		// when a pixel is removed, all its alive neighbours shall recalculate their states
		if (value == State.Removed)
			foreach (var surround in Surrounds)
			{
				if (!surround.Exists) continue;
				surround._updateRequired = true;
			}

		State = value;
		Exists = value != State.Removed; // no need to check for Empty, they won't get this far
	}

	/// <summary>
	/// Updates the pixel's state if needed.
	/// </summary>
	/// <remarks>
	/// Checks the state of neighboring pixels and actualizes
	/// <see cref="Exists"/>, <see cref="State"/>, <see cref="Role"/> and <see cref="Gaps"/>.
	/// </remarks>
	internal void Update()
	{
		if (!_updateRequired) return;

		// no way back from these states
		if (State == State.Final || State == State.Removed) return;

		// cache these to not read them from other objects every time and to return them from SurroundExists
		for (var i = 0; i < 8; i++)
			_surroundExists[i] = Surrounds[i].Exists;

		// a pixel is considered to be removable if it's placed on the shape's edge.
		// that is, there are some other pixels next to it, but they are all standing
		// next to each other, forming a continuous sequence.
		// so we run a circle around the pixel and analyze the gaps in its neighbours.

		// we start from any existing neighbour
		var start = -1;
		for (var i = 0; i < 8; i++)
		{
			if (!_surroundExists[i]) continue;
			start = i;
			break;
		}

		if (start == -1)
		{
			// our pixel is a loner. this could happen if there once was a circle which
			// was eventually "eaten".
			SetState(State.Final, Role.Alone);
			return;
		}

		Gaps = 0; // count of gaps in surrounding pixels
		var gapLength = 0; // count of empty pixels, when gaps = 1 also means the gap length
		var index = start;

		do
		{
			var curr = _surroundExists[index];
			var prev = index > 0 ? _surroundExists[index - 1] : _surroundExists[7];

			if (!curr)
			{
				if (prev) Gaps++;
				gapLength++;
			}

			index++;
			if (index == 8) index = 0;
		} while (index != start);

		// gaps > 1 indicates that the pixel is a link and shall not be removed.
		// up to 4 gaps possible.
		if (Gaps == 1)
		{
			if (gapLength >= 6)
			{
				// a single gap of 6 or 7 indicates that the pixel ends a line.
				SetState(State.Final, Role.End);
				return;
			}
			else if (gapLength > 1)
			{
				// if we consider pixels with a single gap of just 1 around removable then
				// this would cause a chain reaction in many cases - the next pixel in the
				// same direction would also be removed on the next iteration and so on,
				// gradually "eating the shape inside".
				SetState(State.Removable);
				return;
			}
		}

		// this does not mean that the pixel would never be removed.
		// only that it shall not be removed right now.
		Role? role = Gaps > 2 ? Role.Node : Gaps == 2 ? Role.Line : null;
		SetState(State.Unremovable, role);
	}

	/// <returns>
	/// <see langword="true"/> if a neighbouring pixel in the <paramref name="direction"/> exists.
	/// </returns>
	internal bool SurroundExists(int direction)
		=> _surroundExists[direction];

	/// <summary>
	/// Removes a pixel.
	/// </summary>
	/// <remarks>
	/// Sets <see cref="State"/> = <see cref="State.Removed"/> and
	/// <see cref="Exists"/> = <see langword="false"/>.
	/// </remarks>
	internal void Remove()
		=> SetState(State.Removed);

	/// <returns>
	/// Pixel coordinates as <see cref="Vector2"/>.
	/// </returns>
	internal Vector2 ToVector2()
		=> new(X + 0.5f, Y + 0.5f); // for better antialiased rendering

	public override string ToString()
		=> $"({X}, {Y}): {State}, {Role}, {Gaps} gaps";
}
