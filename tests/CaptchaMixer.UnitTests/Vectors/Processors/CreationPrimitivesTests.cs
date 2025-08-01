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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Processors;

public class CreationPrimitivesTests
{
	[Test]
	public void VectorProcessors_CreationPrimitives_Oval_Test()
	{
		var layer = new VectorLayer(null);
		new Oval() { Area = new RectangleF(10, 10, 10, 10) }.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(1);
		path.Instructions[0].Should().BeOfType<AddOvalInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10), new Vector2(20, 20));
	}

	[Test]
	public void VectorProcessors_CreationPrimitives_Rect_Test()
	{
		var layer = new VectorLayer(null);
		new Rect() { Area = new RectangleF(10, 10, 10, 10) }.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(1);
		path.Instructions[0].Should().BeOfType<AddRectInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10), new Vector2(20, 20));
	}
}