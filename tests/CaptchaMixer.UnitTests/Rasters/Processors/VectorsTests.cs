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

public class VectorsTests : RasterProcessorsTestsBase
{
	[Test]
	public void RasterProcessors_Vectors_DrawVectorLayer_Antialiasing_Test()
	{
		var stroke = new StrokePaintInfo(CMColor.Black) { Antialiasing = true };
		new DrawVectorLayer(VectorLayerName, stroke).Process(_rasterLayer1, _context);
		new CMColor(_rasterLayer1, 0, 0).Should().Be(new CMColor(0, 0, 0, 184));
		new CMColor(_rasterLayer1, 1, 0).Should().Be(new CMColor(0, 0, 0, 213));
		new CMColor(_rasterLayer1, 2, 0).Should().Be(new CMColor(0, 0, 0, 99));
		new CMColor(_rasterLayer1, 0, 1).Should().Be(new CMColor(0, 0, 0, 0));
		new CMColor(_rasterLayer1, 1, 1).Should().Be(new CMColor(0, 0, 0, 42));
		new CMColor(_rasterLayer1, 2, 1).Should().Be(new CMColor(0, 0, 0, 156));
	}

	[Test]
	public void RasterProcessors_Vectors_DrawVectorLayer_NoAntialiasing_Test()
	{
		var stroke = new StrokePaintInfo(CMColor.Black) { Antialiasing = false };
		new DrawVectorLayer(VectorLayerName, stroke).Process(_rasterLayer1, _context);
		new CMColor(_rasterLayer1, 0, 0).Should().Be(CMColor.Black);
		new CMColor(_rasterLayer1, 1, 0).Should().Be(CMColor.Black);
		new CMColor(_rasterLayer1, 0, 1).Should().Be(CMColor.Empty);
		new CMColor(_rasterLayer1, 1, 1).Should().Be(CMColor.Empty);
	}
}
