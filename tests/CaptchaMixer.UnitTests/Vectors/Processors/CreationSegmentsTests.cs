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

public class CreationSegmentsTests
{
	[Test]
	public void VectorProcessors_CreationSegments_CurveSegmentationX_Test()
	{
		var layer = new VectorLayer(null);
		new CurveSegmentationX(2)
		{
			Area = new RectangleF(10, 10, 10, 10),
			Angle1 = Carousel(90f, -90f),
			Angle2 = Carousel(90f, -90f),
			OffsetSnapsLine1 = Carousel(1f, -1f),
			OffsetSnapsLine2 = Carousel(1f, -1f),
			Radius1 = Carousel(2f, 3f),
			Radius2 = Carousel(2f, 3f)
		}.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[1].Should().BeOfType<CubicToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(14, 10), new Vector2(18, 20), new Vector2(16, 20));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();

		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[1].Should().BeOfType<CubicToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(14, 10), new Vector2(18, 20), new Vector2(16, 20));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();
	}

	[Test]
	public void VectorProcessors_CreationSegments_CurveSegmentationY_Test()
	{
		var layer = new VectorLayer(null);
		new CurveSegmentationY(2)
		{
			Area = new RectangleF(10, 10, 10, 10),
			Angle1 = Carousel(90f, -90f),
			Angle2 = Carousel(90f, -90f),
			OffsetSnapsLine1 = Carousel(1f, -1f),
			OffsetSnapsLine2 = Carousel(1f, -1f),
			Radius1 = Carousel(2f, 3f),
			Radius2 = Carousel(2f, 3f)
		}.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[1].Should().BeOfType<CubicToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 18), new Vector2(20, 14), new Vector2(20, 16));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 10));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();

		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[1].Should().BeOfType<CubicToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 18), new Vector2(20, 14), new Vector2(20, 16));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 20));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();
	}

	[Test]
	public void VectorProcessors_CreationSegments_LineSegmentationX_Test()
	{
		var layer = new VectorLayer(null);
		new LineSegmentationX(2)
		{
			Area = new RectangleF(10, 10, 10, 10),
			OffsetSnapsLine1 = Carousel(1f, -1f),
			OffsetSnapsLine2 = Carousel(1f, -1f)
		}.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 20));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();

		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 20));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(16, 10));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();
	}

	[Test]
	public void VectorProcessors_CreationSegments_LineSegmentationY_Test()
	{
		var layer = new VectorLayer(null);
		new LineSegmentationY(2)
		{
			Area = new RectangleF(10, 10, 10, 10),
			OffsetSnapsLine1 = Carousel(1f, -1f),
			OffsetSnapsLine2 = Carousel(1f, -1f)
		}.Process(layer, null);

		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 16));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 10));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 10));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();

		path = layer.Objects[0].Paths[1];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>();
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[1].Should().BeOfType<LineToInstruction>();
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 16));
		path.Instructions[2].Should().BeOfType<LineToInstruction>();
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 20));
		path.Instructions[3].Should().BeOfType<LineToInstruction>();
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 20));
		path.Instructions[4].Should().BeOfType<LineToInstruction>();
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 16));
		path.Instructions[5].Should().BeOfType<CloseInstruction>();
	}
}