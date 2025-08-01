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

namespace Kaspersky.CaptchaMixer.UnitTests.Builders;

public class CaptchaMixerDebugBuilderTests
{
	[Test]
	[Timeout(10000)]
	[Retry(3)]
	public void CaptchaMixerDebugBuilder_Test()
	{
		const string DebugDir = nameof(CaptchaMixerDebugBuilder_Test);
		Directory.CreateDirectory(DebugDir);

		var image = new CaptchaMixerDebugBuilder(10, 5, DebugDir, 5)
			.AddVectorProcessors("vector", new Rect())
			.AddRasterProcessors("raster", new DrawVectorLayer("vector", new FillPaintInfo(CMColor.Black)))
			.AddMasterProcessors(new CopyRasterLayer("raster"), new InvertColors())
			.Build()
			.CreateCaptcha(null);

		using (var bitmap = SKBitmap.Decode(image.Data))
		{
			bitmap.Pixels.Select(SKExtensions.ToCMColor).Should().AllSatisfy(c => c.Should().Be(CMColor.White));
		}

		Directory.Exists(DebugDir).Should().BeTrue();
		var debugFiles = Directory.EnumerateFiles(DebugDir).ToList();
		debugFiles.Should().HaveCount(4);
		foreach (var debugFile in debugFiles)
		{
			using var fs = new FileStream(debugFile, FileMode.Open);
			using var bitmap = SKBitmap.Decode(fs);
			bitmap.Width.Should().Be(50);
			bitmap.Height.Should().Be(25);
		}
	}
}
