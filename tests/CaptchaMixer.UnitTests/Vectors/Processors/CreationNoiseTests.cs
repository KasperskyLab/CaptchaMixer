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

public class CreationNoiseTests
{
	[Test]
	public void VectorProcessors_CreationNoise_RandomCurves_Test()
	{
		var layer = new VectorLayer(null);
		new RandomCurves(2, 3) { Area = new RectangleF(10, 10, 10, 10), Close = true }.Process(layer, null);
		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		for (var i = 0; i < 2; i++)
		{
			var path = layer.Objects[0].Paths[i];
			path.Instructions.Should().HaveCount(4);
			path.Instructions[0].Should().BeOfType<MoveToInstruction>();
			path.Instructions[1].Should().BeOfType<CubicToInstruction>();
			path.Instructions[2].Should().BeOfType<CubicToInstruction>();
			path.Instructions[3].Should().BeOfType<CloseInstruction>();
			path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
			{
				p.X.Should().BeInRange(10, 20);
				p.Y.Should().BeInRange(10, 20);
			});
		}
	}

	[Test]
	public void VectorProcessors_CreationNoise_RandomLines_Test()
	{
		var layer = new VectorLayer(null);
		new RandomLines(2, 3) { Area = new RectangleF(10, 10, 10, 10), Close = true }.Process(layer, null);
		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		for (var i = 0; i < 2; i++)
		{
			var path = layer.Objects[0].Paths[i];
			path.Instructions.Should().HaveCount(4);
			path.Instructions[0].Should().BeOfType<MoveToInstruction>();
			path.Instructions[1].Should().BeOfType<LineToInstruction>();
			path.Instructions[2].Should().BeOfType<LineToInstruction>();
			path.Instructions[3].Should().BeOfType<CloseInstruction>();
			path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
			{
				p.X.Should().BeInRange(10, 20);
				p.Y.Should().BeInRange(10, 20);
			});
		}
	}

	[Test]
	public void VectorProcessors_CreationNoise_RandomOvals_Test()
	{
		var layer = new VectorLayer(null);
		new RandomOvals(2, 10) { Area = new RectangleF(10, 10, 10, 10) }.Process(layer, null);
		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		for (var i = 0; i < 2; i++)
		{
			var path = layer.Objects[0].Paths[0];
			path.Instructions.Should().HaveCount(1);
			path.Instructions[0].Should().BeOfType<AddOvalInstruction>();
			path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
			{
				p.X.Should().BeInRange(0, 30);
				p.Y.Should().BeInRange(0, 30);
			});
		}
	}

	[Test]
	public void VectorProcessors_CreationNoise_RandomRects_Test()
	{
		var layer = new VectorLayer(null);
		new RandomRects(2, 10) { Area = new RectangleF(10, 10, 10, 10) }.Process(layer, null);
		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths.Should().HaveCount(2);

		for (var i = 0; i < 2; i++)
		{
			var path = layer.Objects[0].Paths[0];
			path.Instructions.Should().HaveCount(1);
			path.Instructions[0].Should().BeOfType<AddRectInstruction>();
			path.Instructions.SelectMany(i => i.Points).Should().AllSatisfy(p =>
			{
				p.X.Should().BeInRange(5, 25);
				p.Y.Should().BeInRange(5, 25);
			});
		}
	}
}