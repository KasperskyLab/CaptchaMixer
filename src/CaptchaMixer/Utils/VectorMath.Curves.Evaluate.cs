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
	/// Calculates coordinates of a non-rational Bezier curve at point <paramref name="t"/>.
	/// </summary>
	/// <param name="points">
	/// Curve points.
	/// </param>
	/// <param name="t">
	/// Curve position from 0 to 1.
	/// </param>
	internal static Vector2 CurveEvaluatePoint(Span<Vector2> points, float t)
	{
		// https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/curves/beziers#the-cubic-b%C3%A9zier-curve
		// cubic curve:
		// x(t) = (1-t)^3 * x0 + 3 * t * (1-t)^2 + x1 + 3 * t^2 * (1-t) * x2 + t^3 * x3
		//
		// https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/curves/beziers#the-quadratic-b%C3%A9zier-curve
		// quadratic curve:
		// x(t) = (1-t)^2 * x0 + 2 * t * (1-t) * x1 + t^2 * x2
		//
		// count of terms linearly depends on the total count of coordinate and control points of the curve.
		// if you supplement the formula of the cubic curve with the multipliers that were omitted because
		// they are equal to 1, then it will turn into the sum of multiplications, which is easy to translate
		// into an algorithm:
		// 1 * t^0 * (1-t)^3 * x0 +
		// 3 * t^1 * (1-t)^2 * x1 +
		// 3 * t^2 * (1-t)^1 * x2 +
		// 1 * t^3 * (1-t)^0 * x3

		if (t == 0) return points[0];
		else if (t == 1) return points[^1];

		var count = points.Length;
		var result = new Vector2();
		var st = 1f - t;

		for (var j = 0; j < count; j++)
		{
			var m1 = j > 0 && j < count - 1 ? count - 1 : 1;
			var m2 = (float)Math.Pow(t, j);
			var m3 = (float)Math.Pow(st, count - j - 1);
			result.X += m1 * m2 * m3 * points[j].X;
			result.Y += m1 * m2 * m3 * points[j].Y;
		}

		return result;
	}

	/// <summary>
	/// Calculates coordinates of a rational Bezier curve at point <paramref name="t"/>.
	/// </summary>
	/// <param name="points">
	/// Curve points.
	/// </param>
	/// <param name="t">
	/// Curve position from 0 to 1.
	/// </param>
	internal static Vector2 CurveRationalEvaluatePoint(Span<Vector3> points, float t)
	{
		// https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/curves/beziers#the-conic-b%C3%A9zier-curve
		// this article only describes a specific rational curve case - the one supported by SkiaSharp.
		// it's a quadratic rational curve with weights of start and end points set to 1:
		// > In theory, a rational quadratic can involve three separate weight values, one for each of the three terms,
		// > but these can be simplified to just one weight value on the middle term.
		//
		// the formulas below are the formulas from the article, modified for full-fledged rational curves.
		//
		// a rational curve with two control points:
		// d(t) = (1-t)^3 * w0 + 3 * t * (1-t)^2 * w1 + 3 * t^2 * (1-t) * w2 + t^3 * w3
		// x(t) = ((1-t)^3 * x0 * w0 + 3 * t * (1-t)^2 * x1 * w1 + 3 * t^2 * (1-t) * x2 * w2 + t^3 * x3 * w3)) / d(t)
		//
		// by analogy with how we did it above for non-rational curves, we factorize the terms of the divisible and divisor.
		//
		// divisible:
		// 1 * t^0 * (1-t)^3 * x0 * w0 +
		// 3 * t^1 * (1-t)^2 * x1 * w1 +
		// 3 * t^2 * (1-t)^1 * x2 * w2 +
		// 1 * t^3 * (1-t)^0 * x3 * w3 +
		//
		// divisor:
		// 1 * t^0 * (1-t)^3 * w0 +
		// 3 * t^1 * (1-t)^2 * w1 +
		// 3 * t^2 * (1-t)^1 * w2 +
		// 1 * t^3 * (1-t)^0 * w3 +

		if (t == 0) return points[0].TruncateZ();
		else if (t == 1) return points[^1].TruncateZ();

		var count = points.Length;
		var result = new Vector2();
		var st = 1f - t;

		float nX = 0, nY = 0;
		var d = 0f;
		for (var j = 0; j < count; j++)
		{
			var m1 = j > 0 && j < count - 1 ? count - 1 : 1;
			var m2 = (float)Math.Pow(t, j);
			var m3 = (float)Math.Pow(st, count - j - 1);
			var m4 = points[j].Z;
			nX += m1 * m2 * m3 * m4 * points[j].X;
			nY += m1 * m2 * m3 * m4 * points[j].Y;
			d += m1 * m2 * m3 * m4;
		}
		result.X = nX / d;
		result.Y = nY / d;

		return result;
	}

	/// <summary>
	/// Calculates the points of the Bezier cubic curve for gradually changing t from 0 to 1.
	/// </summary>
	/// <param name="result">
	/// Memory region to write result to. Span length defines the count of evaluated points.
	/// </param>
	public static void CurveCubicEvaluate(CurveCubic2 curve, Span<Vector2> result)
	{
		Span<Vector2> points = stackalloc Vector2[4];
		curve.CopyTo(points);

		var step = 1f / (result.Length - 1);
		for (var i = 0; i < result.Length; i++)
			result[i] = CurveEvaluatePoint(points, step * i);
	}

	/// <summary>
	/// Calculates the points of the Bezier quadratic curve for gradually changing t from 0 to 1.
	/// </summary>
	/// <param name="result">
	/// <inheritdoc cref="CurveCubicEvaluate(CurveCubic2, Span{Vector2})" path="/param[@name='result']"/>
	/// </param>
	public static void CurveQuadEvaluate(CurveQuad2 curve, Span<Vector2> result)
	{
		Span<Vector2> points = stackalloc Vector2[3];
		curve.CopyTo(points);

		var step = 1f / (result.Length - 1);
		for (var i = 0; i < result.Length; i++)
			result[i] = CurveEvaluatePoint(points, step * i);
	}

	/// <summary>
	/// Calculates the points of the Bezier rational curve for gradually changing t from 0 to 1.
	/// </summary>
	/// <param name="result">
	/// <inheritdoc cref="CurveCubicEvaluate(CurveCubic2, Span{Vector2})" path="/param[@name='result']"/>
	/// </param>
	public static void CurveRationalEvaluate(CurveQuad3 curve, Span<Vector2> result)
	{
		Span<Vector3> points = stackalloc Vector3[3];
		curve.CopyTo(points);

		var step = 1f / (result.Length - 1);
		for (var i = 0; i < result.Length; i++)
			result[i] = CurveRationalEvaluatePoint(points, step * i);
	}
}
