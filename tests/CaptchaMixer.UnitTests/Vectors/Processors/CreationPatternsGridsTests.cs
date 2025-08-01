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

public class CreationPatternsGridsTests
{
	[Test]
	public void VectorProcessors_CreationPatternsGrids_LinesGridX_Test()
	{
		var layer = new VectorLayer(null);

		new LinesGridX(4)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			OffsetFromX = Carousel(1f, -1f),
			OffsetFromY = Carousel(1f, -1f),
			OffsetToX = Carousel(1f, -1f),
			OffsetToY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(4);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points[0].Should().Be(new Vector2(21, 11));
		path.Instructions[2].Should().BeOfType<MoveToInstruction>();
		path.Instructions[2].Points[0].Should().Be(new Vector2(9, 14));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points[0].Should().Be(new Vector2(19, 14));
	}

	[Test]
	public void VectorProcessors_CreationPatternsGrids_LinesGridY_Test()
	{
		var layer = new VectorLayer(null);

		new LinesGridY(4)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			OffsetFromX = Carousel(1f, -1f),
			OffsetFromY = Carousel(1f, -1f),
			OffsetToX = Carousel(1f, -1f),
			OffsetToY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(4);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points[0].Should().Be(new Vector2(11, 21));
		path.Instructions[2].Should().BeOfType<MoveToInstruction>();
		path.Instructions[2].Points[0].Should().Be(new Vector2(14, 9));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points[0].Should().Be(new Vector2(14, 19));
	}

	[Test]
	public void VectorProcessors_CreationPatternsGrids_OvalsGrid_Test()
	{
		var layer = new VectorLayer(null);

		new OvalsGrid(4, 2, 1)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 3,
			OffsetLeft = Carousel(1f, -1f),
			OffsetRight = Carousel(1f, -1f),
			OffsetTop = Carousel(1f, -1f),
			OffsetBottom = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(3);
		path.Instructions[0].Should().BeOfType<AddOvalInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[0].Points[1].Should().Be(new Vector2(15, 13));
		path.Instructions[1].Should().BeOfType<AddOvalInstruction>();
		path.Instructions[1].Points[0].Should().Be(new Vector2(14, 9));
		path.Instructions[1].Points[1].Should().Be(new Vector2(18, 11));
		path.Instructions[2].Should().BeOfType<AddOvalInstruction>();
		path.Instructions[2].Points[0].Should().Be(new Vector2(11, 14));
		path.Instructions[2].Points[1].Should().Be(new Vector2(15, 16));
	}

	[Test]
	public void VectorProcessors_CreationPatternsGrids_RectsGrid_Test()
	{
		var layer = new VectorLayer(null);

		new RectsGrid(4, 2, 1)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 3,
			OffsetLeft = Carousel(1f, -1f),
			OffsetRight = Carousel(1f, -1f),
			OffsetTop = Carousel(1f, -1f),
			OffsetBottom = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(3);
		path.Instructions[0].Should().BeOfType<AddRectInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[0].Points[1].Should().Be(new Vector2(15, 13));
		path.Instructions[1].Should().BeOfType<AddRectInstruction>();
		path.Instructions[1].Points[0].Should().Be(new Vector2(14, 9));
		path.Instructions[1].Points[1].Should().Be(new Vector2(18, 11));
		path.Instructions[2].Should().BeOfType<AddRectInstruction>();
		path.Instructions[2].Points[0].Should().Be(new Vector2(11, 14));
		path.Instructions[2].Points[1].Should().Be(new Vector2(15, 16));
	}
}