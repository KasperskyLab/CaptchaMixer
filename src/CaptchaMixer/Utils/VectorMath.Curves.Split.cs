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
	/// <summary>
	/// Splits a 2-dimensional Bezier curve of arbitrary order into two curves
	/// at the point <paramref name="t"/> using the De Casteljau's algorithm.
	/// </summary>
	internal static (Vector2[] curve1, Vector2[] curve2) CurveSplit(Vector2[] curve, float t)
	{
		// we will be using the geometrical interpretation, it's easier to understand
		// https://en.wikipedia.org/wiki/De_Casteljau%27s_algorithm#Geometric_interpretation

		// initially there are N points. first and last ones are start and end points of the curve.
		// all the others are intermediate control points which affect the curve's shape.
		// N points make a polygon of N-1 linear segments. these segments must be divided in the
		// ratio t / 1-t until there is only one point left.

		// this means that, for example, for a quartic curve, where N = 5, i.e. 3 control points,
		// starting with 5 points, we will end up sequentially dividing the segments into 9 points,
		// where points 1 to 5 will describe the first curve, from 5 to 9 - the second.

		var count = curve.Length;
		var center = count - 1;

		// allocate an array of the required total length and fill it with first elements
		var splits = new Vector2[center * 2 + 1];
		Array.Copy(curve, splits, curve.Length);

		// here's how the cycle below changes the points in array:
		// initially:   [ s, c1, c2, e , ? , ? , ? ]
		// iteration 1: [ s, r1, r1, r1, e , ? , ? ]
		// iteration 2: [ s, r1, r2, r2, r1, e , ? ]
		// iteration 3: [ s, r1, r2, r3, r2, r1, e ]
		// where r1...r3 are the points calculated during the corresponding iteration

		for (var i = 1; i < count; i++)
		{
			// shift those elements on the right which should not be touched anymore
			for (var j = center + i; j > center; j--)
				splits[j] = splits[j - 1];
			// process elements from center to the left, except for left elements which should
			// also not be touched
			for (var j = center; j >= i; j--)
				splits[j] = LinePoint(splits[j - 1], splits[j], t);
		}

		var curve1 = new Vector2[count];
		var curve2 = new Vector2[count];
		Array.Copy(splits, 0, curve1, 0, count);
		Array.Copy(splits, center, curve2, 0, count);
		return (curve1, curve2);
	}

	/// <summary>
	/// Splits a 3-dimensional Bezier curve of arbitrary order into two curves
	/// at the point <paramref name="t"/> using the De Casteljau's algorithm.
	/// </summary>
	private static (Vector3[] curve1, Vector3[] curve2) CurveSplit(Vector3[] curve, float t)
	{
		// see algorithm explanation above in a method for 2-dimensional curves

		var count = curve.Length;
		var center = count - 1;

		var splits = new Vector3[center * 2 + 1];
		Array.Copy(curve, splits, curve.Length);

		for (var i = 1; i < count; i++)
		{
			for (var j = center + i; j > center; j--)
				splits[j] = splits[j - 1];
			for (var j = center; j >= i; j--)
				splits[j] = LinePoint(splits[j - 1], splits[j], t);
		}

		var curve1 = new Vector3[count];
		var curve2 = new Vector3[count];
		Array.Copy(splits, 0, curve1, 0, count);
		Array.Copy(splits, center, curve2, 0, count);
		return (curve1, curve2);
	}

	/// <summary>
	/// Splits a 2-dimensional Bezier rational curve of arbitrary order into two curves
	/// at the point <paramref name="t"/> using the De Casteljau's algorithm.
	/// </summary>
	internal static (Vector3[] curve1, Vector3[] curve2) CurveRationalSplit(Vector3[] curve, float t)
	{
		// splitting a rational curve is somewhat more complicated

		var count = curve.Length;

		// 1. translate points into a higher dimensional space by multiplying the coordinates by a
		//    homogeneous one, and using the homogeneous itself as a coordinate in a new dimension
		var pointsUp = curve
			.Select(UpgradePoint)
			.ToArray();

		// 2. process point in that space using the De Casteljau's algorithm
		var (curve1, curve2) = CurveSplit(pointsUp, t);

		// 3. translate points into the space of the original dimension using perspective projection
		for (var i = 0; i < count; i++)
		{
			curve1[i] = DowngradePoint(curve1[i]);
			curve2[i] = DowngradePoint(curve2[i]);
		}

		return (curve1, curve2);

		// the homogeneous coordinate is the weight of the point at the end of the array
		static Vector3 UpgradePoint(Vector3 point)
			=> new(point.X * point.Z, point.Y * point.Z, point.Z);

		// perspective projection is done by dividing the coordinates by a homogeneous one
		static Vector3 DowngradePoint(Vector3 point)
			=> new(point.X / point.Z, point.Y / point.Z, point.Z);
	}

	/// <summary>
	/// Splits a Bezier cubic curve onto two sub-curves at point <paramref name="t"/>.
	/// </summary>
	public static (CurveCubic2 curve1, CurveCubic2 curve2) CurveCubicSplit(CurveCubic2 curve, float t)
	{
		var (curve1, curve2) = CurveSplit(curve.ToArray(), t);
		return (new(curve1), new(curve2));
	}

	/// <summary>
	/// Splits a Bezier quadratic curve onto two sub-curves at point <paramref name="t"/>.
	/// </summary>
	public static (CurveQuad2 curve1, CurveQuad2 curve2) CurveQuadSplit(CurveQuad2 curve, float t)
	{
		var (curve1, curve2) = CurveSplit(curve.ToArray(), t);
		return (new(curve1), new(curve2));
	}

	/// <summary>
	/// Splits a Bezier quadratic rational curve onto two sub-curves at point <paramref name="t"/>.
	/// </summary>
	public static (CurveQuad3 curve1, CurveQuad3 curve2) CurveRationalSplit(CurveQuad3 curve, float t)
	{
		var (curve1, curve2) = CurveRationalSplit(curve.ToArray(), t);
		return (new(curve1), new(curve2));
	}
}
