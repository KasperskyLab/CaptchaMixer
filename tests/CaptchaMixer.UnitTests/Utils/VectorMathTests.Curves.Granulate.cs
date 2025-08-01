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
	[TestCase(10, 10, 20, 20, 30, 10, 2)]
	[TestCase(0, 0, 0, 0, 0, 0, 1)]
	public void VectorMath_CurveGranulate_Quad_Test(
		float xs, float ys,
		float xc, float yc,
		float xe, float ye,
		float l)
		=> VectorMath.CurveGranulate(
			new[]
			{
				new Vector2(xs, ys),
				new Vector2(xc, yc),
				new Vector2(xe, ye)
			},
			l)
		.Select(c => VectorMath.LineLength(c[0], c[^1]))
		.Should()
		.AllSatisfy(len => len.Should().BeLessThanOrEqualTo(l));

	[Test]
	[TestCase(0, 0, 0, 10, 10, 10, 10, 0, 2)]
	[TestCase(0, 0, 0, 0, 0, 0, 0, 0, 1)]
	public void VectorMath_CurveGranulate_Cubic_Test(
		float xs, float ys,
		float xc1, float yc1,
		float xc2, float yc2,
		float xe, float ye,
		float l)
		=> VectorMath.CurveGranulate(
			new[]
			{
				new Vector2(xs, ys),
				new Vector2(xc1, yc1),
				new Vector2(xc2, yc2),
				new Vector2(xe, ye)
			},
			l)
		.Select(c => VectorMath.LineLength(c[0], c[^1]))
		.Should()
		.AllSatisfy(len => len.Should().BeLessThanOrEqualTo(l));

	[Test]
	[TestCase(10, 10, 0.8f, 20, 20, 0.6f, 30, 10, 1f, 2)]
	[TestCase(0, 0, 0.8f, 0, 0, 0.6f, 0, 0, 1f, 1)]
	public void VectorMath_CurveRationalGranulate_Quad_Test(
		float xs, float ys, float ws,
		float xc, float yc, float wc,
		float xe, float ye, float we,
		float l)
		=> VectorMath.CurveRationalGranulate(
			new[]
			{
				new Vector3(xs, ys, ws),
				new Vector3(xc, yc, wc),
				new Vector3(xe, ye, we)
			},
			l)
		.Select(c => VectorMath.LineLength(c[0].TruncateZ(), c[^1].TruncateZ()))
		.Should()
		.AllSatisfy(len => len.Should().BeLessThanOrEqualTo(l));

	[Test]
	[TestCase(0)]
	[TestCase(-1)]
	public void VectorMath_CurveGranulate_InvalidLength_Test(float l)
	{
		var action = () => VectorMath.CurveGranulate(
			new[]
			{
				new Vector2(0, 0),
				new Vector2(10, 10),
				new Vector2(20, 0)
			},
			l);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	[TestCase(0)]
	[TestCase(-1)]
	public void VectorMath_CurveRationalGranulate_InvalidLength_Test(float l)
	{
		var action = () => VectorMath.CurveRationalGranulate(
			new[]
			{
				new Vector3(0, 0, 1),
				new Vector3(10, 10, 1),
				new Vector3(20, 0, 1)
			},
			l);

		action.Should().Throw<ArgumentException>();
	}
}
