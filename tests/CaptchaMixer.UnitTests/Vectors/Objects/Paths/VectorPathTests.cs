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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Objects.Paths;

public class VectorPathTests
{
	private static VectorPath ComplexPath
		=> new VectorPath()
			.LineTo(10, 10)
			.MoveTo(10, 0)
			.LineTo(20, 0)
			.LineTo(20, 10)
			.Close()
			.MoveTo(30, 0)
			.QuadTo(30, 10, 40, 10)
			.CubicTo(45, 10, 50, 15, 50, 20)
			.RationalTo(2, 50, 30, 3, 60, 30, 1)
			.AddRect(70, 0, 80, 10)
			.AddRoundRect(90, 0, 100, 10, 3)
			.AddOval(110, 0, 120, 10)
			.LineTo(0, 10)
			.Close();

	[Test]
	public void VectorPath_Enumeration_Test()
	{
		var path = ComplexPath;
		var items = path.ToList();
		items.Should().HaveCount(14);
		items.Should().ContainInConsecutiveOrder(
			(new(0, 0), path.Instructions[0]),
			(new(10, 10), path.Instructions[1]),
			(new(10, 0), path.Instructions[2]),
			(new(20, 0), path.Instructions[3]),
			(new(20, 10), path.Instructions[4]),
			(new(10, 0), path.Instructions[5]),
			(new(30, 0), path.Instructions[6]),
			(new(40, 10), path.Instructions[7]),
			(new(50, 20), path.Instructions[8]),
			(new(60, 30), path.Instructions[9]),
			(new(70, 0), path.Instructions[10]),
			(new(90, 7), path.Instructions[11]),
			(new(120, 5), path.Instructions[12]),
			(new(0, 10), path.Instructions[13]));
	}

	[Test]
	public void VectorPath_Clone_Test()
	{
		var path1 = new VectorPath().LineTo(10, 10);
		var path2 = path1.Clone();
		path1.Instructions[0].Points[0] = new(20, 20);
		path2.Instructions[0].Should().NotBe(path1.Instructions[0]);
	}

	[Test]
	public void VectorPath_GetBounds_Test()
		=> ComplexPath
			.GetBounds()
			.Should()
			.Be(new RectangleF(0, 0, 120, 30));

	[Test]
	public void VectorPath_ConstructorParams_Test()
	{
		var instruction1 = new MoveToInstruction(10, 0);
		var instruction2 = new LineToInstruction(10, 10);
		var path = new VectorPath(instruction1, instruction2);
		path.Instructions.Should().HaveCount(2);
		path.Instructions.Should().ContainInConsecutiveOrder(instruction1, instruction2);
	}

	[Test]
	public void VectorPath_ConstructorEnumerable_WithItems_Test()
	{
		var instruction1 = new MoveToInstruction(10, 0);
		var instruction2 = new LineToInstruction(10, 10);
		var path = new VectorPath(new VectorPathInstruction[] { instruction1, instruction2 }.AsEnumerable());
		path.Instructions.Should().HaveCount(2);
		path.Instructions.Should().ContainInConsecutiveOrder(instruction1, instruction2);
	}

	[Test]
	public void VectorPath_ConstructorEnumerable_WithoutItems_Test()
	{
		var path = new VectorPath(Array.Empty<VectorPathInstruction>().AsEnumerable());
		path.Instructions.Should().HaveCount(0);
	}

	[Test]
	public void VectorPath_ConstructorEnumerable_Null_Test()
	{
		var path = new VectorPath(null);
		path.Instructions.Should().HaveCount(0);
	}
}