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

public class DistortionTests
{
	[Test]
	public void VectorProcessors_Distortion_ChopPaths_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(
				new VectorPath()
					.MoveTo(10, 0)
					.LineTo(20, 0)
					.MoveTo(30, 0)
					.MoveTo(40, 0)
					.LineTo(50, 0)
					.LineTo(60, 0)
					.LineTo(70, 0)
					.LineTo(80, 0)
					.LineTo(90, 0)));

		new ChopPaths(Carousel(0f, 1f), 2).Process(layer, null);

		// this processor is pretty stupid in terms of leaving a lot of superfluous MoveTo
		// instructions. but they generally create almost no overhead. so we'll fix that
		// later. maybe.
		// the processed path has been intentionally made that way so the processor has to
		// behave in an inobvious manner thus utilizing more code paths.
		// so it's not easy to explain why the results must be like this, but they are
		// correct, this has been thoroughly analyzed.
		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(6);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 0));
		path.Instructions[1].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(20, 0));
		path.Instructions[2].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(30, 0));
		path.Instructions[3].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(40, 0));
		path.Instructions[4].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(60, 0));
		path.Instructions[5].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(70, 0));
	}

	[Test]
	public void VectorProcessors_Distortion_DamagePaths_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(
				new VectorPath()
					.MoveTo(10, 0)
					.LineTo(20, 0)
					.LineTo(30, 0)
					.LineTo(40, 0)
					.LineTo(50, 0)
					.LineTo(60, 0)
					.LineTo(70, 0)
					.LineTo(80, 0)
					.LineTo(90, 0)));

		new DamagePaths(Carousel(0f, 1f), 2).Process(layer, null);

		// this one just cuts out any instructions on the way and replaces them with MoveTo.
		// it's simpler to understand, but still not that easy. once again, these results are correct.
		var path = layer.Objects[0].Paths[0];
		path.Instructions.Should().HaveCount(5);
		path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 0));
		path.Instructions[1].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(30, 0));
		path.Instructions[2].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(40, 0));
		path.Instructions[3].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(60, 0));
		path.Instructions[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(70, 0));
	}

	[Test]
	public void VectorProcessors_Distortion_PointsRadialDistortion_Test()
	{
		var layer = new VectorLayer(
			null,
			new VectorObject(
				new VectorPath()
					.LineTo(0, 0)
					.LineTo(25, 0)
					.LineTo(50, 0)
					.LineTo(75, 0)
					.LineTo(100, 0)));

		new PointsRadialDistortion
		{
			Area = new RectangleF(-100, -10, 200, 20),
			Count = 3,
			Positions = Carousel(0f, 0.5f, 1f),
			Scales = Carousel(10f, 0f, 4f)
		}
		.Process(layer, null);

		// well, this one is real fun.
		// the area defines an ellipse 200x20 with (0, 0) as its center.
		//
		// line 0 is (0, 0) - the very center. it fits in the first distortion range [0; 0.5].
		// since it's at the left edge of the range, it's scaling factor shall be 10, but since
		// the point's distance to ellipse center is 0, it won't move.
		//
		// line 1 is (25, 0) which is the center of the first range [0; 0.5]. scaling factors for
		// the range are [10; 0], so at the range center it shall be 5, thus moving the point to
		// 25 * 5 = 125.
		//
		// line 2 is (50, 0) which is still in range [0; 0.5] but on its right edge. the scaling
		// factor there is 0 which moves the point to 0.
		//
		// line 3 is (75, 0) which is the center of the second range [0.5; 1]. scaling factor
		// there is 2 - center of scaling range [0; 4]. thus the point moves to 75 * 2 = 150.
		//
		// line 4 is (100, 0) and it may seem this is the right edge of the second range with
		// scaling factor of 4, the point is formally outside of the ellipse, so it does not move.

		var path = layer.Objects[0].Paths[0];
		path.Instructions[0].Points[0].Should().Be(new Vector2(0, 0));
		path.Instructions[1].Points[0].Should().Be(new Vector2(125, 0));
		path.Instructions[2].Points[0].Should().Be(new Vector2(0, 0));
		path.Instructions[3].Points[0].Should().Be(new Vector2(150, 0));
		path.Instructions[4].Points[0].Should().Be(new Vector2(100, 0));
	}
}