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

public class StructureTests
{
	[Test]
	public void VectorProcessors_Structure_CopyVectorLayer_Test()
	{
		var layer1 = new VectorLayer("layer1", new VectorObject(new VectorPath().LineTo(10, 10)));
		var layer2 = new VectorLayer("layer2", new VectorObject(new VectorPath().LineTo(20, 20)));
		var context = new CaptchaContext(
			new CaptchaParameters("text", 200, 60),
			new[] { layer1, layer2 },
			Array.Empty<IRasterLayer>());

		new CopyVectorLayer(layer2.Name).Process(layer1, context);

		layer1.Objects.Should().HaveCount(2);
		layer1.Objects[0].Paths[0].Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
		layer1.Objects[1].Paths[0].Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 20));
		layer1.Objects[1].Paths[0].Instructions[0].Should().NotBe(layer2.Objects[0].Paths[0].Instructions[0]);
	}

	[Test]
	public void VectorProcessors_Structure_DisassembleContours_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().AddRect(10, 10, 20, 20)));

		new DisassembleContours().Process(layer, null);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 10));
		path.Instructions[2].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 20));
		path.Instructions[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();
	}

	[Test]
	public void VectorProcessors_Structure_GranulatePaths_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 0)));

		new GranulatePaths(5).Process(layer, null);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(2);
		path.Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(5, 0));
		path.Instructions[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
	}

	[Test]
	public void VectorProcessors_Structure_MergeObjects_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(new VectorPath().LineTo(10, 0)),
			new VectorObject(new VectorPath().LineTo(20, 0)));

		new MergeObjects().Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);
		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(1);
		path.Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(1);
		path.Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 0));
	}

	[Test]
	public void VectorProcessors_Structure_PartitionPaths_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(
				new VectorPath()
					.LineTo(10, 0)
					.LineTo(20, 0)
					.LineTo(30, 0)
					.LineTo(40, 0)));

		new PartitionPaths
		{
			Count = 2,
			Probability = Carousel(0f, 1f)
		}
		.Process(layer, null);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(7);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(0, 0));
		path.Instructions[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
		path.Instructions[2].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 0));
		path.Instructions[3].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 0));
		path.Instructions[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(30, 0));
		path.Instructions[5].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(30, 0));
		path.Instructions[6].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(40, 0));
	}

	[Test]
	public void VectorProcessors_Structure_SplitObjects_Test()
	{
		var path1 = new VectorPath().LineTo(10, 0);
		var path2 = new VectorPath().LineTo(20, 0);
		var layer = new VectorLayer(null, new VectorObject(path1, path2));

		new SplitObjects().Process(layer, null);

		layer.Objects.Should().HaveCount(2);
		layer.Objects[0].Paths.Should().HaveCount(1);
		layer.Objects[0].Paths[0].As<object>().Should().Be(path1);
		layer.Objects[1].Paths.Should().HaveCount(1);
		layer.Objects[1].Paths[0].As<object>().Should().Be(path2);
	}

	[Test]
	public void VectorProcessors_Structure_SplitPaths_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(
				new VectorPath()
					.LineTo(10, 0)
					.MoveTo(20, 0)
					.LineTo(30, 0)));

		new SplitPaths().Process(layer, null);

		layer.Objects[0].Paths.Should().HaveCount(2);
		var path = layer.Objects[0].Paths[0];
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(0, 0));
		path.Instructions[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
		path = layer.Objects[0].Paths[1];
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(20, 0));
		path.Instructions[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(30, 0));
	}

	[Test]
	public void VectorProcessors_Structure_ToLines_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().QuadTo(10, 10, 20, 0)));

		new ToLines().Process(layer, null);

		var path = layer.Objects[0].Paths[0];
		path.Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 0));
	}

	[Test]
	public void VectorProcessors_Structure_UnclosePaths_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 10).Close()));

		new UnclosePaths().Process(layer, null);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(1);
		path.Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
	}
}
