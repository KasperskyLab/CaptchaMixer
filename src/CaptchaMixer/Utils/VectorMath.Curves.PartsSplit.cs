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
	/// Splits the Bezier curve onto <paramref name="parts"/> sub-curves by
	/// gradually changing t from 0 to 1.
	/// </summary>
	internal static IEnumerable<TPoint[]> CurvePartsSplit<TPoint>(
		Func<TPoint[], float, (TPoint[], TPoint[])> splitFunc,
		TPoint[] curve,
		int parts)
	{
		if (parts <= 0)
			throw new ArgumentException("Failed to perform curve parts split: parts count must be > 0", nameof(parts));

		var step = 1 / (float)parts;

		TPoint[] curve1;
		TPoint[] curve2 = curve;

		for (var i = 0; i < parts - 1; i++)
		{
			var l = 1f - i * step;
			var t = step / l;
			(curve1, curve2) = splitFunc(curve2, t);
			yield return curve1;
		}

		yield return curve2;
	}

	/// <summary>
	/// Splits the Bezier cubic curve onto <paramref name="parts"/> sub-curves by
	/// gradually changing t from 0 to 1.
	/// </summary>
	public static CurveCubic2[] CurveCubicPartsSplit(CurveCubic2 curve, int parts)
		=> CurvePartsSplit(CurveSplit, curve.ToArray(), parts)
			.Select(a => new CurveCubic2(a))
			.ToArray();

	/// <summary>
	/// Splits the Bezier quadratic curve onto <paramref name="parts"/> sub-curves by
	/// gradually changing t from 0 to 1.
	/// </summary>
	public static CurveQuad2[] CurveQuadPartsSplit(CurveQuad2 curve, int parts)
		=> CurvePartsSplit(CurveSplit, curve.ToArray(), parts)
			.Select(a => new CurveQuad2(a))
			.ToArray();

	/// <summary>
	/// Splits the Bezier rational curve onto <paramref name="parts"/> sub-curves by
	/// gradually changing t from 0 to 1.
	/// </summary>
	public static CurveQuad3[] CurveRationalPartsSplit(CurveQuad3 curve, int parts)
		=> CurvePartsSplit(CurveRationalSplit, curve.ToArray(), parts)
			.Select(a => new CurveQuad3(a))
			.ToArray();
}
