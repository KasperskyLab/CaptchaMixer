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

public class ProxiesTests
{
	[Test]
	public void VectorProcessors_Proxies_PngExporting_Test()
	{
		const int scale = 5;

		var layer = new VectorLayer("root");
		var parameters = new CaptchaParameters("text", 3, 3);
		var context = new CaptchaContext(parameters, new[] { layer }, Array.Empty<IRasterLayer>());

		var filePathBase = nameof(VectorProcessors_Proxies_PngExporting_Test);
		var proxy = new PngExportingVectorProcessorProxy(new Rect(), filePathBase, true, scale);
		proxy.Process(layer, context);

		var colors = LoadImage($"{filePathBase}_0.png", out var width, out var height);
		width.Should().Be(parameters.Size.Width * scale);
		height.Should().Be(parameters.Size.Height * scale);
		colors.Should().AllSatisfy(c => c.Should().Be(CMColor.Black));

		colors = LoadImage($"{filePathBase}_1.png", out width, out height);
		width.Should().Be(parameters.Size.Width * scale);
		height.Should().Be(parameters.Size.Height * scale);
		colors.Should().AllSatisfy(c => c.Should().NotBe(CMColor.Black));
	}

	[Test]
	public void VectorProcessors_Proxies_Probability_Test()
	{
		var layer = new VectorLayer("root");
		var called = false;

		var processorMoq = new Mock<IVectorProcessor>();
		processorMoq.Setup(p => p.Process(layer, null)).Callback(() => called = true);
		var processor = processorMoq.Object;

		new ProbabilityVectorProcessorProxy(processor, 0).Process(layer, null);
		called.Should().BeFalse();
		new ProbabilityVectorProcessorProxy(processor, 1).Process(layer, null);
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
