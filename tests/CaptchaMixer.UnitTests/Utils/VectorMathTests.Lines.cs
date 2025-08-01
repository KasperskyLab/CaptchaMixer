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
	[TestCase(0, 0, 20, 10, 0.7f, 14, 7)]
	[TestCase(0, 0, 20, 10, 0, 0, 0)]
	[TestCase(0, 0, -20, -10, 0.7f, -14, -7)]
	[TestCase(0, 0, -20, -10, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, 0.5f, 0, 0)]
	[TestCase(0, 0, 0, 0, 1, 0, 0)]
	public void VectorMath_LinePoint_Vector2_Test(
		float x1, float y1,
		float x2, float y2,
		float t,
		float xr, float yr)
		=> VectorMath.LinePoint(
			new Vector2(x1, y1),
			new Vector2(x2, y2),
			t)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 20, 20, 20, 20, 0.5f, 10, 10, 20)]
	[TestCase(0, 0, 20, 20, 20, 20, 0, 0, 0, 20)]
	[TestCase(0, 0, 20, -20, -20, -20, 0.5f, -10, -10, 0)]
	[TestCase(0, 0, 20, -20, -20, -20, 0, 0, 0, 20)]
	[TestCase(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, 0, 0, 0.5f, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, 0, 0, 1, 0, 0, 0)]
	public void VectorMath_LinePoint_Vector3_Test(
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		float t,
		float xr, float yr, float zr)
		=> VectorMath.LinePoint(
			new Vector3(x1, y1, z1),
			new Vector3(x2, y2, z2),
			t)
		.Should()
		.Be(new Vector3(xr, yr, zr));

	[Test]
	[TestCase(10, 10, 20, 20, 14.142136f)]
	[TestCase(-10, -10, -20, -20, 14.142136f)]
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, -10, 0, 10, 20)]
	public void VectorMath_LineLength_Vector2_Test(
		float x1, float y1,
		float x2, float y2,
		float r)
		=> VectorMath.LineLength(
			new Vector2(x1, y1),
			new Vector2(x2, y2))
		.Should()
		.Be(r);

	[Test]
	[TestCase(10, 10, 10, 20, 20, 20, 17.320509f)]
	[TestCase(-10, -10, -10, -20, -20, -20, 17.320509f)]
	[TestCase(0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, -10, 0, 0, 10, 20)]
	public void VectorMath_LineLength_Vector3_Test(
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		float r)
		=> VectorMath.LineLength(
			new Vector3(x1, y1, z1),
			new Vector3(x2, y2, z2))
		.Should()
		.Be(r);

	[Test]
	[TestCase(10, 10, 20, 20, 3)]
	[TestCase(-10, -10, -20, -20, 3)]
	[TestCase(10, 10, 20, 20, 1)]
	[TestCase(-10, -10, -20, -20, 1)]
	[TestCase(0, 0, 0, 0, 1)]
	[TestCase(0, 0, 0, 0, 3)]
	public void VectorMath_LinePartsSplit_Vector2_Test(
		float x1, float y1,
		float x2, float y2,
		int parts)
	{
		var p1 = new Vector2(x1, y1);
		var p2 = new Vector2(x2, y2);
		var partLen = VectorMath.LineLength(p1, p2) / parts;
		var points = VectorMath.LinePartsSplit(p1, p2, parts);
		points.Should().HaveCount(parts + 1);
		points
			.Take(parts)
			.Select((p, i) => VectorMath.LineLength(p, points[i + 1]))
			.Should()
			.AllSatisfy(len => len.Should().BeApproximately(partLen, 0.00001f));
	}

	[Test]
	[TestCase(10, 10, 10, 20, 20, 20, 3)]
	[TestCase(-10, -10, -10, -20, -20, -20, 3)]
	[TestCase(10, 10, 10, 20, 20, 20, 1)]
	[TestCase(-10, -10, -10, -20, -20, -20, 1)]
	[TestCase(0, 0, 0, 0, 0, 0, 1)]
	[TestCase(0, 0, 0, 0, 0, 0, 3)]
	public void VectorMath_LinePartsSplit_Vector3_Test(
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		int parts)
	{
		var p1 = new Vector3(x1, y1, z1);
		var p2 = new Vector3(x2, y2, z2);
		var partLen = VectorMath.LineLength(p1, p2) / parts;
		var points = VectorMath.LinePartsSplit(p1, p2, parts);
		points.Should().HaveCount(parts + 1);
		points
			.Take(parts)
			.Select((p, i) => VectorMath.LineLength(p, points[i + 1]))
			.Should()
			.AllSatisfy(len => len.Should().BeApproximately(partLen, 0.00001f));
	}

	[Test]
	[TestCase(10, 10, 20, 20, 3)]
	[TestCase(-10, -10, -20, -20, 3)]
	[TestCase(10, 10, 20, 20, 1)]
	[TestCase(-10, -10, -20, -20, 1)]
	[TestCase(0, 0, 0, 0, 1)]
	[TestCase(0, 0, 0, 0, 3)]
	public void VectorMath_LineGranulate_Vector2_Test(
		float x1, float y1,
		float x2, float y2,
		int l)
	{
		var points = VectorMath.LineGranulate(
			new Vector2(x1, y1),
			new Vector2(x2, y2),
			l);

		points
			.Take(points.Length - 1)
			.Select((p, i) => VectorMath.LineLength(p, points[i + 1]))
			.Should()
			.AllSatisfy(len => len.Should().BeLessThanOrEqualTo(l));
	}

	[Test]
	[TestCase(10, 10, 10, 20, 20, 20, 3)]
	[TestCase(-10, -10, -10, -20, -20, -20, 3)]
	[TestCase(10, 10, 10, 20, 20, 20, 1)]
	[TestCase(-10, -10, -10, -20, -20, -20, 1)]
	[TestCase(0, 0, 0, 0, 0, 0, 1)]
	[TestCase(0, 0, 0, 0, 0, 0, 3)]
	public void VectorMath_LineGranulate_Vector3_Test(
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		int l)
	{
		var points = VectorMath.LineGranulate(
			new Vector3(x1, y1, z1),
			new Vector3(x2, y2, z2),
			l);

		points
			.Take(points.Length - 1)
			.Select((p, i) => VectorMath.LineLength(p, points[i + 1]))
			.Should()
			.AllSatisfy(len => len.Should().BeLessThanOrEqualTo(l));
	}

	[Test]
	[TestCase(10, 10, 20, 10, 30, 0, 135)]
	[TestCase(10, 10, 20, 10, 30, 20, 135)]
	[TestCase(0, 0, 10, 10, -10, -10, 0)]
	public void VectorMath_AngleBetween_Vector2_Test(
		float x1, float y1,
		float xc, float yc,
		float x2, float y2,
		float r)
		=> VectorMath.AngleBetween(
			new Vector2(x1, y1),
			new Vector2(xc, yc),
			new Vector2(x2, y2))
		.Should()
		.Be(r);

	[Test]
	[TestCase(10, 10, 10, 20, 10, 10, 30, 0, 10, 135)]
	[TestCase(10, 10, 10, 20, 10, 10, 30, 20, 10, 135)]
	[TestCase(0, 0, 0, 10, 10, 10, -10, -10, -10, 0)]
	public void VectorMath_AngleBetween_Vector3_Test(
		float x1, float y1, float z1,
		float xc, float yc, float zc,
		float x2, float y2, float z2,
		float r)
		=> VectorMath.AngleBetween(
			new Vector3(x1, y1, z1),
			new Vector3(xc, yc, zc),
			new Vector3(x2, y2, z2))
		.Should()
		.Be(r);

	[Test]
	[TestCase(10, 10, 20, 10, 30, 0, false)]
	[TestCase(-10, -10, 0, 0, 10, 10, true)]
	[TestCase(10, 10, -10, -10, 0, 0, true)]
	[TestCase(0, 0, 0, 0, 0, 0, true)]
	public void VectorMath_AreOneLine_Vector2_Test(
		float x1, float y1,
		float x2, float y2,
		float x3, float y3,
		bool r)
		=> VectorMath.AreOneLine(new[]
		{
			new Vector2(x1, y1),
			new Vector2(x2, y2),
			new Vector2(x3, y3)
		})
		.Should()
		.Be(r);

	[Test]
	[TestCase(10, 10, 10, 20, 10, 10, 30, 0, 10, false)]
	[TestCase(-10, -10, -10, 0, 0, 0, 10, 10, 10, true)]
	[TestCase(10, 10, 10, -10, -10, -10, 0, 0, 0, true)]
	[TestCase(0, 0, 0, 0, 0, 0, 0, 0, 0, true)]
	public void VectorMath_AreOneLine_Vector3_Test(
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		float x3, float y3, float z3,
		bool r)
		=> VectorMath.AreOneLine(new[]
		{
			new Vector3(x1, y1, z1),
			new Vector3(x2, y2, z2),
			new Vector3(x3, y3, z3)
		})
		.Should()
		.Be(r);
}
