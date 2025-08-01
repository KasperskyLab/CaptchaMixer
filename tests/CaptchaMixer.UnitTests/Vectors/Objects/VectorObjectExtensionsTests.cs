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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Objects;

public class VectorObjectExtensionsTests
{
	[Test]
	public void VectorObjectExtensions_EnumeratePathInstructions_Test()
	{
		var instruction1 = new MoveToInstruction(10, 10);
		var instruction2 = new LineToInstruction(10, 10);

		var instructions = new VectorObject(
			new VectorPath(instruction1),
			new VectorPath(instruction2))
			.EnumeratePathInstructions()
			.ToList();

		instructions.Should().HaveCount(2);
		instructions[0].Should().Be(instruction1);
		instructions[1].Should().Be(instruction2);
	}

	[Test]
	public void VectorObjectExtensions_MoveX_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).MoveX(10);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(20, 10));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 20));
	}

	[Test]
	public void VectorObjectExtensions_MoveY_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).MoveY(10);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 20));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(20, 30));
	}

	[Test]
	public void VectorObjectExtensions_Move_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).Move(10, 10);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(20, 20));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	public void VectorObjectExtensions_ScaleX_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).ScaleX(new(10, 10), 2);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 20));
	}

	[Test]
	public void VectorObjectExtensions_ScaleY_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).ScaleY(new(10, 10), 2);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(20, 30));
	}

	[Test]
	public void VectorObjectExtensions_ScaleXY_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).Scale(new(10, 10), 2, 2);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	public void VectorObjectExtensions_Scale_Test()
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).Scale(new(10, 10), 2);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path2.Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	[TestCase(0, 20, 20)]
	[TestCase(90, 0, 20)]
	[TestCase(-90, 20, 0)]
	[TestCase(360, 20, 20)]
	[TestCase(-360, 20, 20)]
	[TestCase(450, 0, 20)]
	[TestCase(-450, 20, 0)]
	public void VectorObjectExtensions_Rotate_Test(float angle, float lineToX, float lineToY)
	{
		var path1 = new VectorPath().MoveTo(10, 10);
		var path2 = new VectorPath().LineTo(20, 20);
		new VectorObject(path1, path2).Rotate(new(10, 10), angle);
		path1.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		var lineToEnd = path2.Instructions[0].As<LineToInstruction>().End;
		lineToEnd.X.Should().BeApproximately(lineToX, 0.00001f);
		lineToEnd.Y.Should().BeApproximately(lineToY, 0.00001f);
	}
}