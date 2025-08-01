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
	private const string CurveAngleSplitInvalidAngleExceptionMessage
		= "Failed to perform curve angle split: minimal angle must be in range [0; 180)";

	private const string CurveAngleSplitOneLineExceptionMessage
		= "Failed to perform curve angle split: points lie on one straight line";

	private const string CurveAngleSplitNanExceptionMessage
		= "Failed to calculate angle during curve angle split";

	/// <summary>
	/// Splits a non-rational Bezier <paramref name="curve"/> onto such line sections that the angle between
	/// any two adjacent sections is not less than <paramref name="minLinearAngle"/>°.
	/// </summary>
	internal static Vector2[] CurveAngleSplit(Vector2[] curve, float minLinearAngle)
	{
		if (minLinearAngle >= 180 || minLinearAngle < 0)
			throw new ArgumentException(CurveAngleSplitInvalidAngleExceptionMessage, nameof(minLinearAngle));

		var endIndex = curve.Length - 1;

		var curves = new List<Vector2[]> { curve };
		var angles = new List<float>();

		// we need to initially split the curve at least several times
		// for the same reason we do it in CurveGranulate
		for (var i = 0; i < 3; i++)
			for (var j = curves.Count - 1; j >= 0; j--)
				Split(j);

		// detail the sections with a too small angle in between
		var split = true;
		while (split)
		{
			split = false;

			for (var i = angles.Count - 1; i >= 0; i--)
			{
				var angle = angles[i];
				if (angle >= minLinearAngle) continue;

				Split(i + 1);
				Split(i);

				split = true;
			}
		}

		var result = new Vector2[curves.Count + 1];
		result[0] = curves[0][0];
		var index = 1;
		foreach (var point in curves.Select(c => c[endIndex]))
		{
			result[index] = point;
			index++;
		}

		return result;

		void Split(int index)
		{
			var (curve1, curve2) = CurveSplit(curves[index], 0.5f);
			curves.Insert(index, curve1);
			curves[index + 1] = curve2;

			angles.Insert(index, 0);
			var affectFrom = Math.Max(index - 1, 0);
			var affectTo = Math.Min(index + 1, angles.Count - 1);
			for (var i = affectFrom; i <= affectTo; i++)
			{
				angles[i] = AngleBetween(curves[i][0], curves[i][endIndex], curves[i + 1][endIndex]);
				if (float.IsNaN(angles[i]))
				{
					var message = curve.AreOneLine() ? CurveAngleSplitOneLineExceptionMessage : CurveAngleSplitNanExceptionMessage;
					throw new ArgumentException(CurveAngleSplitOneLineExceptionMessage, nameof(curve));
				}
			}
		}
	}

	/// <summary>
	/// Splits a rational Bezier <paramref name="curve"/> onto such line sections that the angle between
	/// any two adjacent sections is not less than <paramref name="minLinearAngle"/>°.
	/// </summary>
	internal static Vector2[] CurveRationalAngleSplit(Vector3[] curve, float minLinearAngle)
	{
		if (minLinearAngle >= 180 || minLinearAngle < 0)
			throw new ArgumentException(CurveAngleSplitInvalidAngleExceptionMessage, nameof(minLinearAngle));

		var endIndex = curve.Length - 1;

		var curves = new List<Vector3[]> { curve };
		var angles = new List<float>();

		// we need to initially split the curve at least several times
		// for the same reason we do it in CurveGranulate
		for (var i = 0; i < 3; i++)
			for (var j = curves.Count - 1; j >= 0; j--)
				Split(j);

		// detail the sections with a too small angle in between
		var split = true;
		while (split)
		{
			split = false;

			for (var i = angles.Count - 1; i >= 0; i--)
			{
				var angle = angles[i];
				if (angle >= minLinearAngle) continue;

				Split(i + 1);
				Split(i);

				split = true;
			}
		}

		var result = new Vector2[curves.Count + 1];
		result[0] = curves[0][0].TruncateZ();
		var index = 1;
		foreach (var point in curves.Select(c => c[endIndex].TruncateZ()))
		{
			result[index] = point;
			index++;
		}

		return result;

		void Split(int index)
		{
			var (curve1, curve2) = CurveRationalSplit(curves[index], 0.5f);
			curves.Insert(index, curve1);
			curves[index + 1] = curve2;

			angles.Insert(index, 0);
			var affectFrom = Math.Max(index - 1, 0);
			var affectTo = Math.Min(index + 1, angles.Count - 1);
			for (var i = affectFrom; i <= affectTo; i++)
			{
				angles[i] = AngleBetween(curves[i][0], curves[i][endIndex], curves[i + 1][endIndex]);
				if (float.IsNaN(angles[i]))
				{
					var message = curve.AreOneLine() ? CurveAngleSplitOneLineExceptionMessage : CurveAngleSplitNanExceptionMessage;
					throw new ArgumentException(CurveAngleSplitOneLineExceptionMessage, nameof(curve));
				}
			}
		}
	}

	/// <summary>
	/// Splits a cubic Bezier <paramref name="curve"/> onto such line sections that the angle between
	/// any two adjacent sections is not less than <paramref name="minLinearAngle"/>°.
	/// </summary>
	public static Vector2[] CurveCubicAngleSplit(CurveCubic2 curve, float minLinearAngle)
		=> CurveAngleSplit(curve.ToArray(), minLinearAngle);

	/// <summary>
	/// Splits a quadratic Bezier <paramref name="curve"/> onto such line sections that the angle between
	/// any two adjacent sections is not less than <paramref name="minLinearAngle"/>°.
	/// </summary>
	public static Vector2[] CurveQuadAngleSplit(CurveQuad2 curve, float minLinearAngle)
		=> CurveAngleSplit(curve.ToArray(), minLinearAngle);

	/// <summary>
	/// Splits a rational Bezier <paramref name="curve"/> onto such line sections that the angle between
	/// any two adjacent sections is not less than <paramref name="minLinearAngle"/>°.
	/// </summary>
	public static Vector2[] CurveRationalAngleSplit(CurveQuad3 curve, float minLinearAngle)
		=> CurveRationalAngleSplit(curve.ToArray(), minLinearAngle);
}
