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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Objects.Paths.Instructions;

public class CubicToInstructionsTests
{
	[Test]
	[TestCase(10, 0, 20, 0, 30, 0, 0, 0, 0, 0, 30, 0)]
	[TestCase(7, 50, 10, 25, 50, 30, 100, 7, 23.556496f, 7, 76.443504f, 25.748219f)]
	public void VectorPathInstructions_CubicTo_GetBounds_Test(
		float xc1, float yc1,
		float xc2, float yc2,
		float xe, float ye,
		float xp, float yp,
		float l, float t, float w, float h)
		=> new CubicToInstruction(
			new(xc1, yc1),
			new(xc2, yc2),
			new(xe, ye))
		.GetBounds(new(xp, yp))
		.Should()
		.Be(new RectangleF(l, t, w, h));

	[Test]
	public void VectorPathInstructions_CubicTo_Clone_Test()
	{
		var c1 = new Vector2(0, 10);
		var c2 = new Vector2(10, 10);
		var e = new Vector2(10, 0);
		var instruction = new CubicToInstruction(c1, c2, e);
		var clone = instruction.Clone();
		clone.Points.Should().ContainInConsecutiveOrder(c1, c2, e);
		clone.Points[0] = new Vector2(-5, -5);
		instruction.Points[0].Should().Be(c1);
	}
}