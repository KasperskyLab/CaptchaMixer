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

namespace Kaspersky.CaptchaMixer.UnitTests.Rasters.Processors;

public class ProxiesTests : RasterProcessorsTestsBase
{
	[Test]
	public void RasterProcessors_Proxies_PngExporting_Test()
	{
		const int scale = 5;

		var filePathBase = nameof(RasterProcessors_Proxies_PngExporting_Test);
		new Fill(CMColor.Red).Process(_rasterLayer1, _context);
		var proxy = new PngExportingRasterProcessorProxy(new Fill(CMColor.Yellow), filePathBase, true, scale);
		proxy.Process(_rasterLayer1, _context);

		var colors = LoadImage($"{filePathBase}_0.png", out var width, out var height);
		width.Should().Be(_rasterLayer1.Size.Width * scale);
		height.Should().Be(_rasterLayer1.Size.Height * scale);
		colors.Should().AllSatisfy(c => c.Should().Be(CMColor.Red));

		colors = LoadImage($"{filePathBase}_1.png", out width, out height);
		width.Should().Be(_rasterLayer1.Size.Width * scale);
		height.Should().Be(_rasterLayer1.Size.Height * scale);
		colors.Should().AllSatisfy(c => c.Should().Be(CMColor.Yellow));
	}

	[Test]
	public void RasterProcessors_Proxies_Probability_Test()
	{
		var called = false;

		var processorMoq = new Mock<IRasterProcessor>();
		processorMoq.Setup(p => p.Process(_rasterLayer1, _context)).Callback(() => called = true);
		var processor = processorMoq.Object;

		new ProbabilityRasterProcessorProxy(processor, 0).Process(_rasterLayer1, _context);
		called.Should().BeFalse();
		new ProbabilityRasterProcessorProxy(processor, 1).Process(_rasterLayer1, _context);
		called.Should().BeTrue();
	}

	private static CMColor[] LoadImage(string filePath, out int width, out int height)
	{
		File.Exists(filePath).Should().BeTrue();
		using var fs = new FileStream(filePath, FileMode.Open);
		using var bitmap = SKBitmap.Decode(fs);
		width = bitmap.Width;
		height = bitmap.Height;
		return bitmap.Pixels.Select(SKExtensions.ToCMColor).ToArray();
	}
}
