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

public class CreationPathsLinesTests
{
	[Test]
	public void VectorProcessors_CreationPathsLines_PointsLinePaths_Test()
	{
		var layer = new VectorLayer(null);

		new PointsLinePaths(2, 3)
		{
			Area = new RectangleF(10, 10, 10, 10),
			Close = true,
			Beyonds = false
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		for (var i = 0; i < 2; i++)
		{
			var path = layer.Objects[0].Paths[i];
			path.Instructions.Should().HaveCount(5);
			path.Instructions[0].Should().BeOfType<MoveToInstruction>();
			path.Instructions[1].Should().BeOfType<LineToInstruction>();
			path.Instructions[2].Should().BeOfType<LineToInstruction>();
			path.Instructions[3].Should().BeOfType<LineToInstruction>();
			path.Instructions[4].Should().BeOfType<CloseInstruction>();
			path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
			{
				p.X.Should().BeInRange(10, 20);
				p.Y.Should().BeInRange(10, 20);
			});
		}
	}

	[Test]
	public void VectorProcessors_CreationPathsLines_SquirmingLinePaths_Test()
	{
		var layer = new VectorLayer(null);

		new SquirmingLinePaths(2, 3)
		{
			Area = new RectangleF(0, 0, 10, 10),
			Close = true,
			Beyonds = false,
			Points = Carousel(new Vector2(0, 0), new Vector2(0, 5)),
			Angles = Carousel(0f, -90f, -180f),
			Radiuses = 5
		}
		.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		// beyonds are off, the angles and radiuses create a rect to top-right from starting point.
		// so the first path will try to go beyond 0 along Y axis and thus break after first line.
		// the second path is drawn below and it's points all fit in area, so it contains 3 lines.

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(3);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Should().BeOfType<CloseInstruction>();
		path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
		{
			p.X.Should().BeInRange(0, 10);
			p.Y.Should().BeInRange(0, 10);
		});

		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(5);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Should().BeOfType<CloseInstruction>();
		path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
		{
			p.X.Should().BeInRange(0, 10);
			p.Y.Should().BeInRange(0, 10);
		});
	}
}