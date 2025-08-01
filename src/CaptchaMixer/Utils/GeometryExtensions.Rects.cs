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

public static partial class GeometryExtensions
{
	/// <returns>
	/// <paramref name="rect"/> converted to <see cref="SKRect"/>.
	/// </returns>
	public static SKRect ToSKRect(this RectangleF rect)
		=> new(rect.Left, rect.Top, rect.Right, rect.Bottom);

	/// <returns>
	/// <paramref name="rect"/> converted to <see cref="RectangleF"/>.
	/// </returns>
	public static RectangleF ToRectangleF(this Rectangle rect)
		=> new(rect.Left, rect.Top, rect.Width, rect.Height);

	/// <returns>
	/// <paramref name="size"/> converted to <see cref="RectangleF"/>.
	/// </returns>
	public static RectangleF ToRectangleF(this Size size)
		=> new(0, 0, size.Width, size.Height);

	/// <returns>
	/// Center of <paramref name="rect"/>.
	/// </returns>
	public static Vector2 GetCenter(this Rectangle rect)
		=> new(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);

	/// <returns>
	/// <paramref name="rect"/>'s anchor point of type <paramref name="anchor"/>.
	/// </returns>
	public static Vector2 GetAnchorPoint(this RectangleF rect, RectAnchor anchor)
		=> anchor switch
		{
			RectAnchor.Center => new(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2),
			RectAnchor.LeftTop => new(rect.Left, rect.Top),
			RectAnchor.CenterTop => new(rect.Left + rect.Width / 2, rect.Top),
			RectAnchor.RightTop => new(rect.Right, rect.Top),
			RectAnchor.RightCenter => new(rect.Right, rect.Top + rect.Height / 2),
			RectAnchor.RightBottom => new(rect.Right, rect.Bottom),
			RectAnchor.CenterBottom => new(rect.Left + rect.Width / 2, rect.Bottom),
			RectAnchor.LeftBottom => new(rect.Left, rect.Bottom),
			RectAnchor.LeftCenter => new(rect.Left, rect.Top + rect.Height / 2),
			_ => throw new ArgumentOutOfRangeException(nameof(anchor)),
		};

	/// <returns>
	/// Shifts by X and Y axes required to move <paramref name="rect"/>'s
	/// <paramref name="anchor"/> onto <paramref name="target"/> point.
	/// </returns>
	public static (float dx, float dy) CalcMoveShift(this RectangleF rect, RectAnchor anchor, Vector2 target)
	{
		var anchorPoint = rect.GetAnchorPoint(anchor);
		return (target.X - anchorPoint.X, target.Y - anchorPoint.Y);
	}

	/// <returns>
	/// Top left corner of <paramref name="rect"/> as <see cref="Vector2"/>.
	/// </returns>
	public static Vector2 GetLeftTop(this RectangleF rect)
		=> new(rect.Left, rect.Top);

	/// <returns>
	/// Right top corner of <paramref name="rect"/> as <see cref="Vector2"/>.
	/// </returns>
	public static Vector2 GetRightTop(this RectangleF rect)
		=> new(rect.Right, rect.Top);

	/// <returns>
	/// Left bottom corner of <paramref name="rect"/> as <see cref="Vector2"/>.
	/// </returns>
	public static Vector2 GetLeftBottom(this RectangleF rect)
		=> new(rect.Left, rect.Bottom);

	/// <returns>
	/// Right bottom corner of <paramref name="rect"/> as <see cref="Vector2"/>.
	/// </returns>
	public static Vector2 GetRightBottom(this RectangleF rect)
		=> new(rect.Right, rect.Bottom);

	/// <returns>
	/// Minimal rectangle which contains all of <paramref name="rects"/>.
	/// </returns>
	public static RectangleF GetRectsBounds(this IEnumerable<RectangleF> rects)
	{
		var minX = float.MaxValue;
		var minY = float.MaxValue;
		var maxX = float.MinValue;
		var maxY = float.MinValue;

		foreach (var rect in rects)
		{
			rect.GetAbsCoordinates(out var l, out var t, out var r, out var b);
			minX = Math.Min(minX, l);
			minY = Math.Min(minY, t);
			maxX = Math.Max(maxX, r);
			maxY = Math.Max(maxY, b);
		}

		return new(minX, minY, maxX - minX, maxY - minY);
	}

	/// <returns>
	/// <see langword="true"/> if <paramref name="rect"/> contains <paramref name="point"/>.
	/// </returns>
	public static bool Contains(this RectangleF rect, Vector2 point)
	{
		rect.GetAbsCoordinates(out var l, out var t, out var r, out var b);
		return point.X >= l && point.X < r && point.Y >= t && point.Y < b;
	}

	/// <summary>
	/// Writes out actual painting boundaries of <paramref name="rect"/>,
	/// taking into account its possibly negative width and/or height.
	/// </summary>
	private static void GetAbsCoordinates(this RectangleF rect, out float l, out float t, out float r, out float b)
	{
		if (rect.Width >= 0)
		{
			l = rect.Left;
			r = rect.Right;
		}
		else
		{
			l = rect.Right;
			r = rect.Left;
		}

		if (rect.Height >= 0)
		{
			t = rect.Top;
			b = rect.Bottom;
		}
		else
		{
			t = rect.Bottom;
			b = rect.Top;
		}
	}
}
