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

public class MixersTests : RasterProcessorsTestsBase
{
	[Test]
	public void RasterProcessors_Mixers_CopyRasterLayer_Test()
	{
		new Fill(CMColor.Red).Process(_rasterLayer1, _context);
		new Fill(CMColor.Yellow).Process(_rasterLayer2, _context);
		new CopyRasterLayer(RasterLayerName2).Process(_rasterLayer1, _context);
		AssertAllColors(CMColor.Yellow, _rasterLayer1);
	}

	[Test]
	public void RasterProcessors_Mixers_DrawRasterLayer_Test()
	{
		new Fill(new CMColor(255, 0, 0, 127)).Process(_rasterLayer1, _context);
		new Fill(new CMColor(0, 255, 0, 127)).Process(_rasterLayer2, _context);
		new DrawRasterLayer(RasterLayerName2).Process(_rasterLayer1, _context);
		var color = new CMColor(128, 127, 0, 191);
		AssertAllColors(color, _rasterLayer1);
	}

	[Test]
	public void RasterProcessors_Mixers_ApplyMask_Test()
	{
		new Fill(CMColor.Red).Process(_rasterLayer1, _context);
		_ = new CMColor(_rasterLayer2.Data, 0) { A = 255 };
		new ApplyMask(RasterLayerName2).Process(_rasterLayer1, _context);
		new CMColor(_rasterLayer1.Data, 0).A.Should().Be(255);
		new CMColor(_rasterLayer1.Data, 4).A.Should().Be(0);
	}
}
