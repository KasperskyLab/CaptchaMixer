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

namespace Kaspersky.CaptchaMixer.UnitTests.Values;

public class PointsTests
{
	private const int RandomPointsCount = 100;

	[Test]
	public void Values_Points_PointProvider_Test()
	{
		var values = new PointProvider(Carousel(0f, 1f), Carousel(2f, 3f)).Take(2);
		values.Should().ContainInConsecutiveOrder(new Vector2(0, 2), new Vector2(1, 3));
	}

	[Test]
	[TestCase(RectBorder.Left, 0f, 0f, 0f, 10f)]
	[TestCase(RectBorder.Top, 0f, 10f, 0f, 0f)]
	[TestCase(RectBorder.Right, 10f, 10f, 0f, 10f)]
	[TestCase(RectBorder.Bottom, 0f, 10f, 10f, 10f)]
	public void Values_Points_RectBorderPointProvider_Test(
		RectBorder border,
		float minX,
		float maxX,
		float minY,
		float maxY)
	{
		var values = new RectBorderPointProvider(new RectangleF(0, 0, 10, 10), border).Take(RandomPointsCount);
		values.Should().AllSatisfy(p =>
		{
			p.X.Should().BeInRange(minX, maxX);
			p.Y.Should().BeInRange(minY, maxY);
		});
	}

	[Test]
	public void Values_Points_RectRandomPointProvider_Test()
	{
		var values = new RectRandomPointProvider(new RectangleF(0, 0, 10, 10)).Take(RandomPointsCount);
		values.Should().AllSatisfy(p =>
		{
			p.X.Should().BeInRange(0, 10);
			p.Y.Should().BeInRange(0, 10);
		});
	}
}