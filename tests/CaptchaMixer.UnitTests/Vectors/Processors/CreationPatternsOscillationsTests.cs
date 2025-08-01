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

public class CreationPatternsOscillationsTests
{
	[Test]
	public void VectorProcessors_CreationPatternsOscillations_CurveOscillation_Test()
	{
		var layer = new VectorLayer(null);

		new CurveOscillation(2, 4, 8)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			ControlLength = 0.5f,
			OffsetX = Carousel(1f, -1f),
			OffsetY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(9);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[1].Should().BeOfType<CubicToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(11, 9), new Vector2(12, 7), new Vector2(13, 7));
		path.Instructions[2].Should().BeOfType<CubicToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(14, 7), new Vector2(15, 9), new Vector2(15, 11));
		path.Instructions[3].Should().BeOfType<CubicToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(15, 13), new Vector2(16, 15), new Vector2(17, 15));
		path.Instructions[4].Should().BeOfType<CubicToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(18, 15), new Vector2(19, 13), new Vector2(19, 11));
		path.Instructions[5].Should().BeOfType<CubicToInstruction>();
		path.Instructions[5].Points.Should().ContainInConsecutiveOrder(new Vector2(19, 9), new Vector2(20, 7), new Vector2(21, 7));
		path.Instructions[6].Should().BeOfType<CubicToInstruction>();
		path.Instructions[6].Points.Should().ContainInConsecutiveOrder(new Vector2(22, 7), new Vector2(23, 9), new Vector2(23, 11));
		path.Instructions[7].Should().BeOfType<CubicToInstruction>();
		path.Instructions[7].Points.Should().ContainInConsecutiveOrder(new Vector2(23, 13), new Vector2(24, 15), new Vector2(25, 15));
		path.Instructions[8].Should().BeOfType<CubicToInstruction>();
		path.Instructions[8].Points.Should().ContainInConsecutiveOrder(new Vector2(26, 15), new Vector2(27, 13), new Vector2(27, 11));
	}

	[Test]
	public void VectorProcessors_CreationPatternsOscillations_SawOscillation_Test()
	{
		var layer = new VectorLayer(null);

		new SawOscillation(2, 4, 8)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			OffsetX = Carousel(1f, -1f),
			OffsetY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(7);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points[0].Should().Be(new Vector2(15, 7));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points[0].Should().Be(new Vector2(15, 15));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points[0].Should().Be(new Vector2(19, 11));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points[0].Should().Be(new Vector2(23, 7));
		path.Instructions[5].Should().BeOfType<LineToInstruction>();
		path.Instructions[5].Points[0].Should().Be(new Vector2(23, 15));
		path.Instructions[6].Should().BeOfType<LineToInstruction>();
		path.Instructions[6].Points[0].Should().Be(new Vector2(27, 11));
	}

	[Test]
	public void VectorProcessors_CreationPatternsOscillations_SquareOscillation_Test()
	{
		var layer = new VectorLayer(null);

		new SquareOscillation(2, 4, 8)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			OffsetX = Carousel(1f, -1f),
			OffsetY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(11);
		path.Instructions[00].Should().BeOfType<MoveToInstruction>();
		path.Instructions[00].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[01].Should().BeOfType<LineToInstruction>();
		path.Instructions[01].Points[0].Should().Be(new Vector2(11, 7));
		path.Instructions[02].Should().BeOfType<LineToInstruction>();
		path.Instructions[02].Points[0].Should().Be(new Vector2(15, 7));
		path.Instructions[03].Should().BeOfType<LineToInstruction>();
		path.Instructions[03].Points[0].Should().Be(new Vector2(15, 15));
		path.Instructions[04].Should().BeOfType<LineToInstruction>();
		path.Instructions[04].Points[0].Should().Be(new Vector2(19, 15));
		path.Instructions[05].Should().BeOfType<LineToInstruction>();
		path.Instructions[05].Points[0].Should().Be(new Vector2(19, 11));
		path.Instructions[06].Should().BeOfType<LineToInstruction>();
		path.Instructions[06].Points[0].Should().Be(new Vector2(19, 7));
		path.Instructions[07].Should().BeOfType<LineToInstruction>();
		path.Instructions[07].Points[0].Should().Be(new Vector2(23, 7));
		path.Instructions[08].Should().BeOfType<LineToInstruction>();
		path.Instructions[08].Points[0].Should().Be(new Vector2(23, 15));
		path.Instructions[09].Should().BeOfType<LineToInstruction>();
		path.Instructions[09].Points[0].Should().Be(new Vector2(27, 15));
		path.Instructions[10].Should().BeOfType<LineToInstruction>();
		path.Instructions[10].Points[0].Should().Be(new Vector2(27, 11));
	}

	[Test]
	public void VectorProcessors_CreationPatternsOscillations_TriangleOscillation_Test()
	{
		var layer = new VectorLayer(null);

		new TriangleOscillation(2, 4, 8)
		{
			Area = new RectangleF(10, 10, 10, 10),
			MaxCount = 2,
			OffsetX = Carousel(1f, -1f),
			OffsetY = Carousel(1f, -1f)
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(1);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(7);
		path.Instructions[00].Should().BeOfType<MoveToInstruction>();
		path.Instructions[00].Points[0].Should().Be(new Vector2(11, 11));
		path.Instructions[01].Should().BeOfType<LineToInstruction>();
		path.Instructions[01].Points[0].Should().Be(new Vector2(13, 7));
		path.Instructions[02].Should().BeOfType<LineToInstruction>();
		path.Instructions[02].Points[0].Should().Be(new Vector2(17, 15));
		path.Instructions[03].Should().BeOfType<LineToInstruction>();
		path.Instructions[03].Points[0].Should().Be(new Vector2(19, 11));
		path.Instructions[04].Should().BeOfType<LineToInstruction>();
		path.Instructions[04].Points[0].Should().Be(new Vector2(21, 7));
		path.Instructions[05].Should().BeOfType<LineToInstruction>();
		path.Instructions[05].Points[0].Should().Be(new Vector2(25, 15));
		path.Instructions[06].Should().BeOfType<LineToInstruction>();
		path.Instructions[06].Points[0].Should().Be(new Vector2(27, 11));
	}
}