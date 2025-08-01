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

public class RectsTests
{
	[Test]
	[TestCase(RectAnchor.Center, -5, -5, 5, 5)]
	[TestCase(RectAnchor.LeftTop, 0, 0, 10, 10)]
	[TestCase(RectAnchor.CenterTop, -5, 0, 5, 10)]
	[TestCase(RectAnchor.RightTop, -10, 0, 0, 10)]
	[TestCase(RectAnchor.RightCenter, -10, -5, 0, 5)]
	[TestCase(RectAnchor.RightBottom, -10, -10, 0, 0)]
	[TestCase(RectAnchor.CenterBottom, -5, -10, 5, 0)]
	[TestCase(RectAnchor.LeftBottom, 0, -10, 10, 0)]
	[TestCase(RectAnchor.LeftCenter, 0, -5, 10, 5)]
	public void Values_Rects_AnchorRectProvider_Test(
		RectAnchor anchorType,
		float expectedLeft,
		float expectedTop,
		float expectedRight,
		float expectedBottom)
	{
		var rect = new AnchorRectProvider(anchorType, new Vector2(0, 0), 10, 10).GetNext();
		rect.Left.Should().Be(expectedLeft);
		rect.Top.Should().Be(expectedTop);
		rect.Right.Should().Be(expectedRight);
		rect.Bottom.Should().Be(expectedBottom);
	}

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, 10, 10)]
	[TestCase(1, 1, 1, 1, 1, 1, 9, 9)]
	[TestCase(-1, -1, -1, -1, -1, -1, 11, 11)]
	[TestCase(10, 10, 10, 10, 10, 10, 0, 0)]
	public void Values_Rects_MarginRectProvider_Test(
		float marginLeft,
		float marginTop,
		float marginRight,
		float marginBottom,
		float expectedLeft,
		float expectedTop,
		float expectedRight,
		float expectedBottom)
	{
		var rect = new MarginRectProvider(new RectangleF(0, 0, 10, 10), marginLeft, marginTop, marginRight, marginBottom).GetNext();
		rect.Left.Should().Be(expectedLeft);
		rect.Top.Should().Be(expectedTop);
		rect.Right.Should().Be(expectedRight);
		rect.Bottom.Should().Be(expectedBottom);
	}
}