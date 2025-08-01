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

public partial class GeometryExtensionsTests
{
	[Test]
	[TestCase(0, 0, 0, 0, RectAnchor.Center, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftTop, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.CenterTop, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightTop, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightCenter, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightBottom, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.CenterBottom, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftBottom, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftCenter, 0, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.Center, 0, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftTop, -5, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.CenterTop, 0, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightTop, 5, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightCenter, 5, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightBottom, 5, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.CenterBottom, 0, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftBottom, -5, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftCenter, -5, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.Center, 0, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftTop, 5, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.CenterTop, 0, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightTop, -5, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightCenter, -5, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightBottom, -5, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.CenterBottom, 0, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftBottom, 5, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftCenter, 5, 0)]
	public void GeometryExtensions_Rects_GetAnchorPoint_Test(
		float l, float t, float w, float h,
		RectAnchor a,
		float rx, float ry)
		=> new RectangleF(l, t, w, h)
		.GetAnchorPoint(a)
		.Should()
		.Be(new Vector2(rx, ry));

	[Test]
	[TestCase(0, 0, 0, 0, RectAnchor.Center, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftTop, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.CenterTop, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightTop, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightCenter, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.RightBottom, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.CenterBottom, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftBottom, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0, RectAnchor.LeftCenter, 0, 0, 0, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.Center, 0, 0, 0, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftTop, 0, 0, 5, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.CenterTop, 0, 0, 0, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightTop, 0, 0, -5, 5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightCenter, 0, 0, -5, 0)]
	[TestCase(-5, -5, 10, 10, RectAnchor.RightBottom, 0, 0, -5, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.CenterBottom, 0, 0, 0, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftBottom, 0, 0, 5, -5)]
	[TestCase(-5, -5, 10, 10, RectAnchor.LeftCenter, 0, 0, 5, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.Center, 0, 0, 0, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftTop, 0, 0, -5, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.CenterTop, 0, 0, 0, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightTop, 0, 0, 5, -5)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightCenter, 0, 0, 5, 0)]
	[TestCase(5, 5, -10, -10, RectAnchor.RightBottom, 0, 0, 5, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.CenterBottom, 0, 0, 0, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftBottom, 0, 0, -5, 5)]
	[TestCase(5, 5, -10, -10, RectAnchor.LeftCenter, 0, 0, -5, 0)]
	public void GeometryExtensions_Rects_CalcMoveShift_Test(
		float l, float t, float w, float h,
		RectAnchor a,
		float tx, float ty,
		float rx, float ry)
		=> new RectangleF(l, t, w, h)
		.CalcMoveShift(a, new Vector2(tx, ty))
		.Should()
		.Be((rx, ry));

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
	[TestCase(1, 1, 0, 0, -1, -1, 0, 0, -1, -1, 2, 2)]
	[TestCase(1, 1, 1, 1, -1, -1, 1, 1, -1, -1, 3, 3)]
	[TestCase(2, 2, -1, -1, 0, 0, -1, -1, -1, -1, 3, 3)]
	public void GeometryExtensions_Rects_GetRectsBounds_Test(
		float l1, float t1, float w1, float h1,
		float l2, float t2, float w2, float h2,
		float lr, float tr, float wr, float hr)
		=> GeometryExtensions.GetRectsBounds(
			new RectangleF[]
			{
				new(l1, t1, w1, h1),
				new(l2, t2, w2, h2)
			})
		.Should()
		.Be(new RectangleF(lr, tr, wr, hr));

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, false)]
	[TestCase(0, 0, 0, 0, 1, 0, false)]
	[TestCase(0, 0, 0, 0, 0, 1, false)]
	[TestCase(0, 0, 0, 0, -1, 0, false)]
	[TestCase(0, 0, 0, 0, 0, -1, false)]
	[TestCase(0, 0, 2, 2, -1, -1, false)]
	[TestCase(0, 0, 2, 2, 0, 0, true)]
	[TestCase(0, 0, 2, 2, 1, 1, true)]
	[TestCase(0, 0, 2, 2, 2, 2, false)]
	[TestCase(2, 2, -2, -2, -1, -1, false)]
	[TestCase(2, 2, -2, -2, 0, 0, true)]
	[TestCase(2, 2, -2, -2, 1, 1, true)]
	[TestCase(2, 2, -2, -2, 2, 2, false)]
	public void GeometryExtensions_Rects_Contains_Test(
		float l, float t, float w, float h,
		float x, float y,
		bool r)
		=> new RectangleF(l, t, w, h)
		.Contains(new Vector2(x, y))
		.Should()
		.Be(r);
}
