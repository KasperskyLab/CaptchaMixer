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

public class CompositesTests
{
	[Test]
	public void VectorProcessors_Composites_IsolatedSequence_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 10)));

		new VectorProcessorsIsolatedSequence(
			new Rect() { Area = new RectangleF(0, 0, 10, 10) },
			new MoveObjects(10, 10))
			.Process(layer, null);

		layer.Objects.Should().HaveCount(2);

		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(10, 10));

		var addRect = layer.Objects[1].Paths[0].Instructions[0].As<AddRectInstruction>();
		addRect.LeftTop.Should().Be(new Vector2(10, 10));
		addRect.RightBottom.Should().Be(new Vector2(20, 20));
	}

	[Test]
	public void VectorProcessors_Composites_PerObjectSequence_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(new VectorPath().LineTo(10, 10)),
			new VectorObject(new VectorPath().LineTo(10, 10)));

		new VectorProcessorsPerObjectSequence(new MoveObjects(Carousel(10f, 20f), 0)).Process(layer, null);

		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(20, 10));
		layer.Objects[1].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 10));
	}

	[Test]
	public void VectorProcessors_Composites_Selector_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 10)));

		var processor = new VectorProcessorsSelector(Carousel<IVectorProcessor>(new MoveObjects(10, 0), new MoveObjects(20, 0)));

		processor.Process(layer, null);
		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(20, 10));

		processor.Process(layer, null);
		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(40, 10));
	}

	[Test]
	public void VectorProcessors_Composites_Sequence_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 10)));
		new VectorProcessorsSequence(new MoveObjects(10, 0), new MoveObjects(20, 0)).Process(layer, null);
		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(40, 10));
	}
}