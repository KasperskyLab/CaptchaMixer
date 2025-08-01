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

public class VectorObjectTests
{
	[Test]
	public void VectorObject_Clone_Test()
	{
		var obj1 = new VectorObject(new VectorPath().LineTo(10, 10));
		var obj2 = obj1.Clone();
		obj1.Paths[0].Instructions[0].Points[0] = new(20, 20);
		obj2.Paths[0].Instructions[0].Should().NotBe(obj1.Paths[0].Instructions[0]);
	}

	[Test]
	public void VectorObject_GetBounds_Test()
		=> new VectorObject(
			new VectorPath().LineTo(-10, -10),
			new VectorPath().LineTo(10, 10))
			.GetBounds()
			.Should()
			.Be(new RectangleF(-10, -10, 20, 20));

	[Test]
	public void VectorObject_ConstructorParams_Test()
	{
		var path1 = new VectorPath(new MoveToInstruction(10, 0));
		var path2 = new VectorPath(new LineToInstruction(10, 10));
		var obj = new VectorObject(path1, path2);
		obj.Paths.Should().HaveCount(2);
		obj.Paths.Should().ContainInConsecutiveOrder(path1, path2);
	}

	[Test]
	public void VectorObject_ConstructorEnumerable_WithItems_Test()
	{
		var path1 = new VectorPath(new MoveToInstruction(10, 0));
		var path2 = new VectorPath(new LineToInstruction(10, 10));
		var obj = new VectorObject(new[] { path1, path2 }.AsEnumerable());
		obj.Paths.Should().HaveCount(2);
		obj.Paths.Should().ContainInConsecutiveOrder(path1, path2);
	}

	[Test]
	public void VectorObject_ConstructorEnumerable_WithoutItems_Test()
	{
		var obj = new VectorObject(Array.Empty<VectorPath>().AsEnumerable());
		obj.Paths.Should().HaveCount(0);
	}

	[Test]
	public void VectorObject_ConstructorEnumerable_Null_Test()
	{
		var obj = new VectorObject(null);
		obj.Paths.Should().HaveCount(0);
	}
}