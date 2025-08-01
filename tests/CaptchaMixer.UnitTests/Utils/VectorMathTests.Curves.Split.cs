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
	[TestCase(10, 10, 20, 13, 30, 10, 0.5f, 15, 11.5f, 25, 11.5f)]
	[TestCase(10, 10, 15, 15, 20, 20, 0.5f, 12.5f, 12.5f, 17.5f, 17.5f)]
	public void VectorMath_CurveSplit_Vector2_Quad_Test(
		float xs, float ys,
		float xc, float yc,
		float xe, float ye,
		float t,
		float xr1c, float yr1c,
		float xr2c, float yr2c)
	{
		var points = new[]
		{
			new Vector2(xs, ys),
			new Vector2(xc, yc),
			new Vector2(xe, ye)
		};

		var span = new Span<Vector2>(points);
		var tp = VectorMath.CurveEvaluatePoint(span, t);

		var (curve1, curve2) = VectorMath.CurveSplit(points, t);

		curve1.Should().HaveCount(3);
		curve1.Should().ContainInConsecutiveOrder(
			new Vector2(xs, ys),
			new Vector2(xr1c, yr1c),
			tp);

		curve2.Should().HaveCount(3);
		curve2.Should().ContainInConsecutiveOrder(
			tp,
			new Vector2(xr2c, yr2c),
			new Vector2(xe, ye));
	}

	[Test]
	[TestCase(10, 10, 0.5f, 20, 13, 0.5f, 30, 10, 0.5f, 0.5f, 15, 11.5f, 0.5f, 0.5f, 25, 11.5f, 0.5f)]
	[TestCase(10, 10, 1, 20, 20, 2, 30, 10, 1, 0.5f, 16.666666f, 16.666666f, 1.5f, 1.5f, 23.333334f, 16.666666f, 1.5f)]
	public void VectorMath_CurveRationalSplit_Quad_Test(
		float xs, float ys, float ws,
		float xc, float yc, float wc,
		float xe, float ye, float we,
		float t,
		float xr1c, float yr1c, float wr1c,
		float wrt,
		float xr2c, float yr2c, float wr2c)
	{
		var points = new[]
		{
			new Vector3(xs, ys, ws),
			new Vector3(xc, yc, wc),
			new Vector3(xe, ye, we),
		};

		var span = new Span<Vector3>(points);
		var tp = new Vector3(VectorMath.CurveRationalEvaluatePoint(span, t), wrt);

		var (curve1, curve2) = VectorMath.CurveRationalSplit(points, t);

		curve1.Should().HaveCount(3);
		curve1.Should().ContainInConsecutiveOrder(
			new Vector3(xs, ys, ws),
			new Vector3(xr1c, yr1c, wr1c),
			tp);

		curve2.Should().HaveCount(3);
		curve2.Should().ContainInConsecutiveOrder(
			tp,
			new Vector3(xr2c, yr2c, wr2c),
			new Vector3(xe, ye, we));
	}
}
