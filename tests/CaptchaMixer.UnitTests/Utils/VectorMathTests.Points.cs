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
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 10, 0)]
	public void VectorMath_PointMoveX_Test(
		float x, float y,
		float mX,
		float xr, float yr)
		=> VectorMath.MoveX(new Vector2(x, y), mX)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 0, 10)]
	public void VectorMath_PointMoveY_Test(
		float x, float y,
		float mY,
		float xr, float yr)
		=> VectorMath.MoveY(new Vector2(x, y), mY)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 10, 10, 10)]
	public void VectorMath_PointMove_Test(
		float x, float y,
		float mX, float mY,
		float xr, float yr)
		=> VectorMath.Move(new Vector2(x, y), mX, mY)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 0, 0)]
	[TestCase(10, 0, 10, 100, 0)]
	[TestCase(0, 10, 10, 0, 10)]
	public void VectorMath_PointScaleX_Test(
		float x, float y,
		float sX,
		float xr, float yr)
		=> VectorMath.ScaleX(new Vector2(x, y), new Vector2(0, 0), sX)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 0, 0)]
	[TestCase(0, 10, 10, 0, 100)]
	[TestCase(10, 0, 10, 10, 0)]
	public void VectorMath_PointScaleY_Test(
		float x, float y,
		float sY,
		float xr, float yr)
		=> VectorMath.ScaleY(new Vector2(x, y), new Vector2(0, 0), sY)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 10, 10, 0, 0)]
	[TestCase(0, 10, 10, 10, 0, 100)]
	[TestCase(10, 0, 10, 10, 100, 0)]
	public void VectorMath_PointScale_Test(
		float x, float y,
		float sX, float sY,
		float xr, float yr)
		=> VectorMath.Scale(new Vector2(x, y), new Vector2(0, 0), sX, sY)
		.Should()
		.Be(new Vector2(xr, yr));

	[Test]
	[TestCase(0, 0, 0, 0, 0)]
	[TestCase(0, 0, 90, 0, 0)]
	[TestCase(10, 0, 90, 0, 10)]
	[TestCase(10, 0, 360, 10, 0)]
	[TestCase(10, 0, -360, 10, 0)]
	[TestCase(10, 0, 450, 0, 10)]
	[TestCase(10, 0, -450, 0, -10)]
	public void VectorMath_PointRotate_Test(
		float x, float y,
		float a,
		float xr, float yr)
	{
		var result = VectorMath.Rotate(new Vector2(x, y), new Vector2(0, 0), a);
		result.X.Should().BeApproximately(xr, 0.00001f);
		result.Y.Should().BeApproximately(yr, 0.00001f);
	}
}
