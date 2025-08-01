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
	[TestCase(10, 10, 20, 13, 30, 10, 170)]
	[TestCase(-10, 0, 0, 100, 10, 0, 175)]
	public void VectorMath_CurveAngleSplit_Quad_Test(
		float xs, float ys,
		float xc, float yc,
		float xe, float ye,
		float a)
	{
		var points = VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(xs, ys),
				new Vector2(xc, yc),
				new Vector2(xe, ye)
			},
			a);

		points
			.Take(points.Length - 2)
			.Select((p, i) => VectorMath.AngleBetween(p, points[i + 1], points[i + 2]))
			.Should()
			.AllSatisfy(angle => angle.Should().BeGreaterThanOrEqualTo(a));
	}

	[Test]
	[TestCase(0, 0, -100, 10, 500, 10, 10, 0, 170)]
	[TestCase(0, 0, -100, 100, 100, 100, 0, 0, 175)]
	public void VectorMath_CurveAngleSplit_Cubic_Test(
		float xs, float ys,
		float xc1, float yc1,
		float xc2, float yc2,
		float xe, float ye,
		float a)
	{
		var points = VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(xs, ys),
				new Vector2(xc1, yc1),
				new Vector2(xc2, yc2),
				new Vector2(xe, ye)
			},
			a);

		points
			.Take(points.Length - 2)
			.Select((p, i) => VectorMath.AngleBetween(p, points[i + 1], points[i + 2]))
			.Should()
			.AllSatisfy(angle => angle.Should().BeGreaterThanOrEqualTo(a));
	}

	[Test]
	[TestCase(10, 10, 0.4f, 20, 13, 0.6f, 30, 10, 0.8f, 170)]
	[TestCase(-10, 0, 10, 0, 100, 1, 10, 0, 5, 175)]
	public void VectorMath_CurveRationalAngleSplit_Quad_Test(
		float xs, float ys, float ws,
		float xc, float yc, float wc,
		float xe, float ye, float we,
		float a)
	{
		var points = VectorMath.CurveRationalAngleSplit(
			new[]
			{
				new Vector3(xs, ys, ws),
				new Vector3(xc, yc, wc),
				new Vector3(xe, ye, we)
			},
			a);

		points
			.Take(points.Length - 2)
			.Select((p, i) => VectorMath.AngleBetween(p, points[i + 1], points[i + 2]))
			.Should()
			.AllSatisfy(angle => angle.Should().BeGreaterThanOrEqualTo(a));
	}

	[Test]
	[TestCase(-1)]
	[TestCase(180)]
	[TestCase(181)]
	public void VectorMath_CurveAngleSplit_Quad_InvalidAngle_Test(float a)
	{
		var action = () => VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(0, 0),
				new Vector2(10, 10),
				new Vector2(20, 0)
			},
			a);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	[TestCase(-1)]
	[TestCase(180)]
	[TestCase(181)]
	public void VectorMath_CurveAngleSplit_Cubic_InvalidAngle_Test(float a)
	{
		var action = () => VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(10, 0),
				new Vector2(10, 10),
				new Vector2(20, 10),
				new Vector2(20, 0)
			},
			a);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	[TestCase(-1)]
	[TestCase(180)]
	[TestCase(181)]
	public void VectorMath_CurveRationalAngleSplit_Quad_InvalidAngle_Test(float a)
	{
		var action = () => VectorMath.CurveRationalAngleSplit(
			new[]
			{
				new Vector3(0, 0, 1),
				new Vector3(10, 10, 2),
				new Vector3(20, 0, 1)
			},
			a);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	public void VectorMath_CurveAngleSplit_Quad_InvalidCurve_Test()
	{
		var action = () => VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(0, 0),
				new Vector2(-10, -10),
				new Vector2(10, 10)
			},
			90);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	public void VectorMath_CurveAngleSplit_Cubic_InvalidCurve_Test()
	{
		var action = () => VectorMath.CurveAngleSplit(
			new[]
			{
				new Vector2(0, 0),
				new Vector2(-10, -10),
				new Vector2(20, 20),
				new Vector2(10, 10)
			},
			90);

		action.Should().Throw<ArgumentException>();
	}

	[Test]
	public void VectorMath_CurveRationalAngleSplit_Quad_InvalidCurve_Test()
	{
		var action = () => VectorMath.CurveRationalAngleSplit(
			new[]
			{
				new Vector3(0, 0, 1),
				new Vector3(-10, -10, 1),
				new Vector3(20, 20, 1)
			},
			90);

		action.Should().Throw<ArgumentException>();
	}
}
