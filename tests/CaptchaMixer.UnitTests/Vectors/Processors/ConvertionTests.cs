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

public class ConvertionTests
{
	[Test]
	public void VectorProcessors_Convertion_Primitivize_Test()
	{
		var layer = new VectorLayer(null, new VectorObject(new VectorPath().LineTo(10, 1).LineTo(20, -1).LineTo(30, 0)));
		new PrimitivizePaths().Process(layer, null);
		layer.Objects.Should().HaveCount(1);
		layer.Objects[0].Paths[0].Instructions.Should().HaveCount(1);
		layer.Objects[0].Paths[0].Instructions[0].As<LineToInstruction>().End.Should().Be(new Vector2(30, 0));
	}

	[Test]
	public void VectorProcessors_Convertion_Skeletonize_Test()
	{
		// using some differently skeletonized characters in captcha text
		const string text = "AXCO8Til";
		const int charWidth = 30;

		// 30x30 area for each character + 1 pixel of spacing between them
		var size = new Size((charWidth + 1) * text.Length - 1, 30);

		var layer = new VectorLayer("root");
		var context = new CaptchaContext(new CaptchaParameters(text, size), new[] { layer }, Array.Empty<IRasterLayer>());

		// now place all objects in layer center and resize all them to fit the entire layer
		new CaptchaChars(FontInfo.Arial).Process(layer, context);
		new SnapObjects(RectAnchor.LeftTop, BasePointType.LayerLeftTop).Process(layer, context);
		new ResizeObjects(charWidth, size.Height, BasePointType.LayerLeftTop).Process(layer, context);
		new JustifyObjectsX().Process(layer, context);

		// raster before
		var rasterLayer1 = new RasterLayer("raster1", size);
		new DrawVectorLayer(layer.Name, new FillPaintInfo(CMColor.Red) { Antialiasing = false }).Process(rasterLayer1, context);

		// skeletonize
		new SkeletonizePaths(1).Process(layer, context);

		// raster after
		var rasterLayer2 = new RasterLayer("raster2", size);
		new DrawVectorLayer(layer.Name, new StrokePaintInfo(CMColor.Red) { Antialiasing = false }).Process(rasterLayer2, context);

		// we will not validate each and every instruction because the skeletonization algorithm is
		// highly likely to change in future releases. moreover, the algorithm is in-house developed,
		// so there are no formal requirements to how it should process vectors.
		// first we check that the result contains only MoveTos and LineTos within the layer area.
		layer.Objects.Should().HaveCount(text.Length);
		layer.Objects.Should().AllSatisfy(o =>
		{
			var bounds = o.GetBounds();
			bounds.Left.Should().BeGreaterThanOrEqualTo(0);
			bounds.Top.Should().BeGreaterThanOrEqualTo(0);
			bounds.Right.Should().BeLessThanOrEqualTo(size.Width);
			bounds.Bottom.Should().BeLessThanOrEqualTo(size.Height);
			o.Paths.SelectMany(p => p.Instructions).Should().AllSatisfy(i =>
			{
				var isMoveTo = i is MoveToInstruction;
				var isLineTo = i is LineToInstruction;
				(isMoveTo || isLineTo).Should().BeTrue("Only move and line instructions shall remain after skeletonization");
			});
		});

		// uncomment to debug images
		/* using var bitmap1 = rasterLayer1.ToSKBitmap();
		bitmap1.Export("VectorProcessors_Convertion_Skeletonize_Test_1.png");
		using var bitmap2 = rasterLayer2.ToSKBitmap();
		bitmap2.Export("VectorProcessors_Convertion_Skeletonize_Test_2.png"); */

		// then we check that the processed image approximately matches the original by pixel colors
		for (var x = 0; x < size.Width; x++)
			for (var y = 0; y < size.Height; y++)
			{
				var pixel1 = rasterLayer1.GetPixel(x, y);
				var pixel2 = rasterLayer2.GetPixel(x, y);
				// red pixels of skeletonized image should match red pixels on non-skeletonized image.
				if (pixel2.R == 255) pixel1.R.Should().Be(255);
				// black pixels of original image should remain black on skeletonized image.
				if (pixel1.R == 0) pixel2.R.Should().Be(0);
			}
	}
}