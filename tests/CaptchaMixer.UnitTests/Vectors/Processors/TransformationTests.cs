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

public class TransformationTests
{
	[Test]
	public void VectorProcessors_TransformationTests_AddSizeObjects_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().LineTo(10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 10, 10), new[] { layer }, Array.Empty<IRasterLayer>());

		new AddSizeObjects(10, 20, BasePointType.LayerCenter).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(15, 20));
	}

	[Test]
	public void VectorProcessors_TransformationTests_JustifyObjectsX_Test()
	{
		var layer = new VectorLayer(
			"layer",
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 20, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 30, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 100, 10), new[] { layer }, Array.Empty<IRasterLayer>());

		new JustifyObjectsX(10).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 0), new Vector2(20, 10));
		layer.Objects[1].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(30, 0), new Vector2(50, 10));
		layer.Objects[2].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(60, 0), new Vector2(90, 10));
	}

	[Test]
	public void VectorProcessors_TransformationTests_JustifyObjectsY_Test()
	{
		var layer = new VectorLayer(
			"layer",
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 20)),
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 30)));
		var context = new CaptchaContext(new CaptchaParameters("text", 10, 100), new[] { layer }, Array.Empty<IRasterLayer>());

		new JustifyObjectsY(10).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 10), new Vector2(10, 20));
		layer.Objects[1].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 30), new Vector2(10, 50));
		layer.Objects[2].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 60), new Vector2(10, 90));
	}

	[Test]
	public void VectorProcessors_TransformationTests_MoveObjects_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 60, 60), new[] { layer }, Array.Empty<IRasterLayer>());

		new MoveObjects(10, 20).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 20), new Vector2(20, 30));
	}

	[Test]
	public void VectorProcessors_TransformationTests_MovePoints_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 60, 60), new[] { layer }, Array.Empty<IRasterLayer>());

		new MovePoints(Carousel(10f, 20f), Carousel(30f, 40f)).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 30), new Vector2(30, 50));
	}

	[Test]
	public void VectorProcessors_TransformationTests_PopulateObjectsX_Test()
	{
		var layer = new VectorLayer(
			"layer",
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 20, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 30, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 60, 10), new[] { layer }, Array.Empty<IRasterLayer>());

		new PopulateObjectsX(10, 5).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(5, 0), new Vector2(10, 10));
		layer.Objects[1].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 0), new Vector2(30, 10));
		layer.Objects[2].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(40, 0), new Vector2(55, 10));
	}

	[Test]
	public void VectorProcessors_TransformationTests_PopulateObjectsY_Test()
	{
		var layer = new VectorLayer(
			"layer",
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)),
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 20)),
			new VectorObject(new VectorPath().AddRect(0, 0, 10, 30)));
		var context = new CaptchaContext(new CaptchaParameters("text", 10, 60), new[] { layer }, Array.Empty<IRasterLayer>());

		new PopulateObjectsY(10, 5).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 5), new Vector2(10, 10));
		layer.Objects[1].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 20), new Vector2(10, 30));
		layer.Objects[2].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 40), new Vector2(10, 55));
	}

	[Test]
	[TestCase(20f, 30f, false, 20f, 30f)]
	[TestCase(20f, null, false, 20f, 10f)]
	[TestCase(20f, null, true, 20f, 20f)]
	[TestCase(null, 30f, false, 10f, 30f)]
	[TestCase(null, 30f, true, 30f, 30f)]
	public void VectorProcessors_TransformationTests_ResizeObjects_Test(
		float? width, float? height, bool proportional,
		float right, float bottom)
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().LineTo(10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 20, 20), new[] { layer }, Array.Empty<IRasterLayer>());

		new ResizeObjects(width, height, BasePointType.LayerLeftTop) { Proportional = proportional }.Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(right, bottom));
	}

	[Test]
	public void VectorProcessors_TransformationTests_RotateObjects_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 60, 60), new[] { layer }, Array.Empty<IRasterLayer>());

		new RotateObjects(90, BasePointType.LayerLeftTop).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0, 0), new Vector2(-10, 10));
	}

	[Test]
	public void VectorProcessors_TransformationTests_RoundPoints_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0.24f, -0.26f, 10.11f, 9.53f)));
		var context = new CaptchaContext(new CaptchaParameters("text", 60, 60), new[] { layer }, Array.Empty<IRasterLayer>());

		new RoundPoints(1).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(0.2f, -0.3f), new Vector2(10.1f, 9.5f));
	}

	[Test]
	public void VectorProcessors_TransformationTests_ScaleObjects_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0, 0, 10, 10)));
		var context = new CaptchaContext(new CaptchaParameters("text", 20, 20), new[] { layer }, Array.Empty<IRasterLayer>());

		new ScaleObjects(2, 3, BasePointType.LayerCenter).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(-10, -20), new Vector2(10, 10));
	}

	[Test]
	public void VectorProcessors_TransformationTests_SnapObjects_Test()
	{
		var layer = new VectorLayer("layer", new VectorObject(new VectorPath().AddRect(0, 0, 10, 20)));
		var context = new CaptchaContext(new CaptchaParameters("text", 30, 30), new[] { layer }, Array.Empty<IRasterLayer>());

		new SnapObjects(RectAnchor.Center, BasePointType.LayerCenter).Process(layer, context);

		layer.Objects[0].Paths[0].Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 5), new Vector2(20, 25));
	}
}
