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

public static partial class VectorMath
{
	/// <returns>
	/// <paramref name="point"/> shifted by <paramref name="dX"/> along X axis.
	/// </returns>
	public static Vector2 MoveX(this Vector2 point, float dX)
	{
		point.X += dX;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> shifted by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static Vector2 MoveY(this Vector2 point, float dY)
	{
		point.Y += dY;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> shifted by <paramref name="dX"/> along X axis and
	/// by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static Vector2 Move(this Vector2 point, float dX, float dY)
	{
		point.X += dX;
		point.Y += dY;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> scaled by <paramref name="scaleX"/> along X axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static Vector2 ScaleX(this Vector2 point, Vector2 basePoint, float scaleX)
	{
		point.X = scaleX != 1 ? basePoint.X + (point.X - basePoint.X) * scaleX : point.X;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> scaled by <paramref name="scaleY"/> along Y axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static Vector2 ScaleY(this Vector2 point, Vector2 basePoint, float scaleY)
	{
		point.Y = scaleY != 1 ? basePoint.Y + (point.Y - basePoint.Y) * scaleY : point.Y;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> scaled by <paramref name="scaleX"/> along X and by
	/// <paramref name="scaleY"/> along Y axis relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static Vector2 Scale(this Vector2 point, Vector2 basePoint, float scaleX, float scaleY)
	{
		point.X = scaleX != 1 ? basePoint.X + (point.X - basePoint.X) * scaleX : point.X;
		point.Y = scaleY != 1 ? basePoint.Y + (point.Y - basePoint.Y) * scaleY : point.Y;
		return point;
	}

	/// <returns>
	/// <paramref name="point"/> scaled by <paramref name="scale"/> along X and Y axes
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static Vector2 Scale(this Vector2 point, Vector2 basePoint, float scale)
		=> point.Scale(basePoint, scale, scale);

	/// <returns>
	/// <paramref name="point"/> rotated by <paramref name="angle"/> degrees
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static Vector2 Rotate(this Vector2 point, Vector2 basePoint, float angle)
	{
		if (angle == 0 || point.Equals(basePoint)) return point;
		var dx = point.X - basePoint.X;
		var dy = point.Y - basePoint.Y;
		var l = (float)Math.Sqrt(dx * dx + dy * dy);
		var cos = dx / l;
		cos = cos.Clamp(-1, 1); // rounding errors may happen
		var a = (float)Math.Acos(cos);
		if (dy < 0) a = -a; // l is always >= 0
		a += angle.DegToRad();
		point.X = basePoint.X + (float)(Math.Cos(a) * l);
		point.Y = basePoint.Y + (float)(Math.Sin(a) * l);
		return point;
	}

	/// <returns>
	/// A minimal rectangle that contains all <paramref name="points"/>.
	/// </returns>
	public static RectangleF GetPointsBounds(this IEnumerable<Vector2> points)
	{
		var minX = float.MaxValue;
		var maxX = float.MinValue;
		var minY = float.MaxValue;
		var maxY = float.MinValue;

		foreach (var point in points)
		{
			minX = Math.Min(minX, point.X);
			maxX = Math.Max(maxX, point.X);
			minY = Math.Min(minY, point.Y);
			maxY = Math.Max(maxY, point.Y);
		}

		return new(minX, minY, maxX - minX, maxY - minY);
	}

	/// <inheritdoc cref="GetPointsBounds(IEnumerable{Vector2})"/>
	public static RectangleF GetPointsBounds(this Span<Vector2> points)
	{
		var minX = float.MaxValue;
		var maxX = float.MinValue;
		var minY = float.MaxValue;
		var maxY = float.MinValue;

		for (var i = 0; i < points.Length; i++)
		{
			var point = points[i];
			minX = Math.Min(minX, point.X);
			maxX = Math.Max(maxX, point.X);
			minY = Math.Min(minY, point.Y);
			maxY = Math.Max(maxY, point.Y);
		}

		return new(minX, minY, maxX - minX, maxY - minY);
	}
}
