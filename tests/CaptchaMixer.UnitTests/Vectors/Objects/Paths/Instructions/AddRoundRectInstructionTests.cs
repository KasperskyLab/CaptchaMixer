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

public class AddRoundRectInstructionTests
{
	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 5, 5, 2, 2, 0, 0, 5, 5)]
	[TestCase(0, 0, -5, -5, 2, 2, -5, -5, 5, 5)]
	public void VectorPathInstructions_AddRoundRect_GetBounds_Test(
		float l, float t, float r, float b, float rx, float ry,
		float lr, float tr, float wr, float hr)
	{
		var instruction = new AddRoundRectInstruction(l, t, r, b, rx, ry);
		instruction.GetBounds(new Vector2(100, 100)).Should().Be(new RectangleF(lr, tr, wr, hr));
		instruction.GetBounds().Should().Be(new RectangleF(lr, tr, wr, hr));
	}

	[Test]
	public void VectorPathInstructions_AddRoundRect_Clone_Test()
	{
		var lt = new Vector2(0, 0);
		var rb = new Vector2(5, 5);
		var instruction = new AddRoundRectInstruction(lt, rb, 2, 2);
		var clone = instruction.Clone();
		clone.Points.Should().ContainInConsecutiveOrder(lt, rb);
		clone.Points[0] = new Vector2(-5, -5);
		instruction.Points[0].Should().Be(lt);
	}
}