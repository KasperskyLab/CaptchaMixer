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
	/// Linear length from <paramref name="point1"/> to <paramref name="point2"/>.
	/// </returns>
	public static float LineLength(Vector2 point1, Vector2 point2)
		=> (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) +
			Math.Pow(point2.Y - point1.Y, 2));

	/// <inheritdoc cref="LineLength(Vector2, Vector2)"/>
	public static float LineLength(Vector3 point1, Vector3 point2)
		=> (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) +
			Math.Pow(point2.Y - point1.Y, 2) +
			Math.Pow(point2.Z - point1.Z, 2));

	/// <returns>
	/// Point on a linear segment from <paramref name="point1"/> to <paramref name="point2"/>
	/// placed on distance <paramref name="t"/> from <paramref name="point1"/> where
	/// <paramref name="t"/> is in range [0; 1].
	/// </returns>
	public static Vector2 LinePoint(Vector2 point1, Vector2 point2, float t)
		=> new(
			point1.X + (point2.X - point1.X) * t,
			point1.Y + (point2.Y - point1.Y) * t);

	/// <inheritdoc cref="LinePoint(Vector2, Vector2, float)"/>
	public static Vector3 LinePoint(Vector3 point1, Vector3 point2, float t)
		=> new(
			point1.X + (point2.X - point1.X) * t,
			point1.Y + (point2.Y - point1.Y) * t,
			point1.Z + (point2.Z - point1.Z) * t);

	/// <returns>
	/// Array of points resultant from dividing a linear segment from <paramref name="point1"/>
	/// to <paramref name="point2"/> onto equal <paramref name="parts"/>.
	/// </returns>
	public static Vector2[] LinePartsSplit(Vector2 point1, Vector2 point2, int parts)
	{
		if (parts <= 1)
			return new[] { point1, point2 };

		var result = new Vector2[parts + 1];
		result[0] = point1;
		result[parts] = point2;

		var step = 1 / (float)parts;
		for (var i = 1; i < parts; i++)
		{
			result[i] = LinePoint(point1, point2, step * i);
		}

		return result;
	}

	/// <inheritdoc cref="LinePartsSplit(Vector2, Vector2, int)"/>
	public static Vector3[] LinePartsSplit(Vector3 point1, Vector3 point2, int parts)
	{
		if (parts <= 1)
			return new[] { point1, point2 };

		var result = new Vector3[parts + 1];
		result[0] = point1;
		result[parts] = point2;

		var step = 1 / (float)parts;
		for (var i = 1; i < parts; i++)
		{
			result[i] = LinePoint(point1, point2, step * i);
		}

		return result;
	}

	/// <returns>
	/// Array of points resultant from dividing a linear segment from <paramref name="point1"/>
	/// to <paramref name="point2"/> onto as many equal parts as required for each part's
	/// length to not exceed <paramref name="maxLength"/>.
	/// </returns>
	public static Vector2[] LineGranulate(Vector2 point1, Vector2 point2, float maxLength)
	{
		var length = LineLength(point1, point2);
		var parts = (int)Math.Ceiling(length / maxLength);
		return LinePartsSplit(point1, point2, parts);
	}

	/// <inheritdoc cref="LineGranulate(Vector2, Vector2, float)"/>
	public static Vector3[] LineGranulate(Vector3 point1, Vector3 point2, float maxLength)
	{
		var length = LineLength(point1, point2);
		var parts = (int)Math.Ceiling(length / maxLength);
		return LinePartsSplit(point1, point2, parts);
	}

	/// <returns>
	/// Center of a linear segment from <paramref name="point1"/> to <paramref name="point2"/>.
	/// </returns>
	public static Vector2 LineCenter(Vector2 point1, Vector2 point2)
		=> LinePoint(point1, point2, 0.5f);

	/// <returns>
	/// Angle in radians between line segment <paramref name="point1"/> -
	/// <paramref name="center"/> and <paramref name="point2"/> - <paramref name="center"/>.
	/// </returns>
	public static float AngleBetweenRad(Vector2 point1, Vector2 center, Vector2 point2)
	{
		var p1_p2 = LineLength(point1, point2);
		if (p1_p2 == 0) return 0;
		var c_p1 = LineLength(center, point1);
		var c_p2 = LineLength(center, point2);
		var cos = (c_p1 * c_p1 + c_p2 * c_p2 - p1_p2 * p1_p2) / (2 * c_p1 * c_p2);
		cos = cos.Clamp(-1, 1); // rounding errors may happen
		return (float)Math.Acos(cos);
	}

	/// <returns>
	/// Angle in degrees between line segment <paramref name="point1"/> -
	/// <paramref name="center"/> and <paramref name="point2"/> - <paramref name="center"/>.
	/// </returns>
	public static float AngleBetween(Vector2 point1, Vector2 center, Vector2 point2)
		=> AngleBetweenRad(point1, center, point2).RadToDeg();

	/// <inheritdoc cref="AngleBetweenRad(Vector2, Vector2, Vector2)"/>
	public static float AngleBetweenRad(Vector3 point1, Vector3 center, Vector3 point2)
	{
		var p1_p2 = LineLength(point1, point2);
		if (p1_p2 == 0) return 0;
		var c_p1 = LineLength(center, point1);
		var c_p2 = LineLength(center, point2);
		var cos = (c_p1 * c_p1 + c_p2 * c_p2 - p1_p2 * p1_p2) / (2 * c_p1 * c_p2);
		cos = cos.Clamp(-1, 1); // rounding errors may happen
		return (float)Math.Acos(cos);
	}

	/// <inheritdoc cref="AngleBetween(Vector2, Vector2, Vector2)"/>
	public static float AngleBetween(Vector3 point1, Vector3 center, Vector3 point2)
		=> AngleBetweenRad(point1, center, point2).RadToDeg();

	/// <returns>
	/// <see langword="true"/> if all <paramref name="points"/> lie on one straight line.
	/// </returns>
	public static bool AreOneLine(this Vector2[] points)
		=> points.Length < 3
		|| points
			.Take(points.Length - 2)
			.Select((p, i) => AngleBetween(p, points[i + 1], points[i + 2]))
			.All(a => a == 180 || a == 0);

	/// <inheritdoc cref="AreOneLine(Vector2[])"/>
	public static bool AreOneLine(this Vector3[] points)
		=> points.Length < 3
		|| points
			.Take(points.Length - 2)
			.Select((p, i) => AngleBetween(p, points[i + 1], points[i + 2]))
			.All(a => a == 180 || a == 0);
}
