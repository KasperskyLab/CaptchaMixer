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

namespace Kaspersky.CaptchaMixer.UnitTests.Utils;

public partial class VectorMathTests
{
	[Test]
	[TestCase(10, 10, 20, 20, 30, 10, 0, 10, 10)]
	[TestCase(10, 10, 20, 20, 30, 10, 0.5f, 20, 15)]
	[TestCase(10, 10, 20, 20, 30, 10, 1, 30, 10)]
	public void VectorMath_CurveEvaluatePoint_Quad_Test(
		float xs, float ys,
		float xc, float yc,
		float xe, float ye,
		float t,
		float xr, float yr)
	{
		Span<Vector2> points = new Vector2[]
		{
			new(xs, ys),
			new(xc, yc),
			new(xe, ye)
		};

		VectorMath
			.CurveEvaluatePoint(points, t)
			.Should()
			.Be(new Vector2(xr, yr));
	}

	[Test]
	[TestCase(10, 10, 10, 20, 30, 20, 30, 10, 0, 10, 10)]
	[TestCase(10, 10, 10, 20, 30, 20, 30, 10, 0.5f, 20, 17.5f)]
	[TestCase(10, 10, 10, 20, 30, 20, 30, 10, 1, 30, 10)]
	public void VectorMath_CurveEvaluatePoint_Cubic_Test(
		float xs, float ys,
		float xc1, float yc1,
		float xc2, float yc2,
		float xe, float ye,
		float t,
		float xr, float yr)
	{
		Span<Vector2> points = new Vector2[]
		{
			new(xs, ys),
			new(xc1, yc1),
			new(xc2, yc2),
			new(xe, ye)
		};

		VectorMath
			.CurveEvaluatePoint(points, t)
			.Should()
			.Be(new Vector2(xr, yr));
	}

	[Test]
	[TestCase(10, 10, 1, 20, 20, 1, 30, 10, 1, 0, 10, 10)]
	[TestCase(10, 10, 2, 20, 20, 1, 30, 10, 2, 0, 10, 10)]
	[TestCase(10, 10, 1, 20, 20, 2, 30, 10, 1, 0, 10, 10)]
	[TestCase(10, 10, 1, 20, 20, 1, 30, 10, 1, 0.5f, 20, 15)]
	[TestCase(10, 10, 2, 20, 20, 1, 30, 10, 2, 0.5f, 20, 13.333333f)]
	[TestCase(10, 10, 1, 20, 20, 2, 30, 10, 1, 0.5f, 20, 16.666666f)]
	[TestCase(10, 10, 1, 20, 20, 1, 30, 10, 1, 1, 30, 10)]
	[TestCase(10, 10, 2, 20, 20, 1, 30, 10, 2, 1, 30, 10)]
	[TestCase(10, 10, 1, 20, 20, 2, 30, 10, 1, 1, 30, 10)]
	public void VectorMath_CurveRationalEvaluatePoint_Quad_Test(
		float xs, float ys, float ws,
		float xc, float yc, float wc,
		float xe, float ye, float we,
		float t,
		float xr, float yr)
	{
		Span<Vector3> points = new Vector3[]
		{
			new(xs, ys, ws),
			new(xc, yc, wc),
			new(xe, ye, we)
		};

		VectorMath
			.CurveRationalEvaluatePoint(points, t)
			.Should()
			.Be(new Vector2(xr, yr));
	}
}
