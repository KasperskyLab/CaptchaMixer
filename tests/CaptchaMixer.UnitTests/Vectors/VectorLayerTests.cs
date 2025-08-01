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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors;

public class VectorLayerTests
{
	[Test]
	public void VectorLayer_Clone_Test()
	{
		var obj1 = new VectorObject(new VectorPath().LineTo(10, 10));
		var obj2 = new VectorLayer(null, obj1).Clone().Objects[0];
		obj2.As<object>().Should().NotBe(obj1);
		obj2.Paths[0].As<object>().Should().NotBe(obj1.Paths[0]);
		obj2.Paths[0].Instructions[0].Should().NotBe(obj1.Paths[0].Instructions[0]);
	}

	[Test]
	public void VectorLayer_ConstructorParams_Test()
	{
		var obj1 = new VectorObject(new VectorPath(new MoveToInstruction(10, 0)));
		var obj2 = new VectorObject(new VectorPath(new MoveToInstruction(20, 0)));
		var layer = new VectorLayer(null, obj1, obj2);
		layer.Objects.Should().HaveCount(2);
		layer.Objects.Should().ContainInConsecutiveOrder(obj1, obj2);
	}

	[Test]
	public void VectorLayer_ConstructorEnumerable_WithItems_Test()
	{
		var obj1 = new VectorObject(new VectorPath(new MoveToInstruction(10, 0)));
		var obj2 = new VectorObject(new VectorPath(new MoveToInstruction(20, 0)));
		var layer = new VectorLayer(null, new[] { obj1, obj2 }.AsEnumerable());
		layer.Objects.Should().HaveCount(2);
		layer.Objects.Should().ContainInConsecutiveOrder(obj1, obj2);
	}

	[Test]
	public void VectorLayer_ConstructorEnumerable_WithoutItems_Test()
	{
		var layer = new VectorLayer(null, Array.Empty<VectorObject>().AsEnumerable());
		layer.Objects.Should().HaveCount(0);
	}

	[Test]
	public void VectorLayer_ConstructorEnumerable_Null_Test()
	{
		var layer = new VectorLayer(null, null);
		layer.Objects.Should().HaveCount(0);
	}
}