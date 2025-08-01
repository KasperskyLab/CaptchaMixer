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

public class RationalToInstructionTests
{
	[Test]
	[TestCase(2, 10, 0, 3, 20, 0, 1, 0, 0, 0, 0, 20, 0)]
	[TestCase(1, 10, 25, 0.8f, 50, 30, 0.7f, 100, 7, 38.220276f, 7, 61.779724f, 23)]
	public void VectorPathInstructions_RationalTo_GetBounds_Test(
		float ws,
		float xc, float yc, float wc,
		float xe, float ye, float we,
		float xp, float yp,
		float l, float t, float w, float h)
		=> new RationalToInstruction(
			ws,
			new(xc, yc, wc),
			new(xe, ye, we))
		.GetBounds(new(xp, yp))
		.Should()
		.Be(new RectangleF(l, t, w, h));

	[Test]
	public void VectorPathInstructions_RationalTo_Clone_Test()
	{
		var c = new Vector3(0, 10, 3);
		var e = new Vector3(10, 0, 1);
		var instruction = new RationalToInstruction(2, c, e);
		var clone = (RationalToInstruction)instruction.Clone();
		clone.Points.Should().ContainInConsecutiveOrder(c.TruncateZ(), e.TruncateZ());
		clone.StartWeight.Should().Be(2);
		clone.ControlWeight.Should().Be(c.Z);
		clone.EndWeight.Should().Be(e.Z);
		clone.Points[0] = new Vector2(-5, -5);
		instruction.Points[0].Should().Be(c.TruncateZ());
	}
}