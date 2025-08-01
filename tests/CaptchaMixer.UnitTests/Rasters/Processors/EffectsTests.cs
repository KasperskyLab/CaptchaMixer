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

public class EffectsTests : RasterProcessorsTestsBase
{
	[Test]
	public void RasterProcessors_Effects_Blur_Test()
	{
		_ = new CMColor(_rasterLayer1, 0, 0) { R = 255, A = 255 };
		new Blur(1).Process(_rasterLayer1, _context);
		new CMColor(_rasterLayer1, 0, 0).Should().Be(new CMColor(255, 0, 0, 255));
		new CMColor(_rasterLayer1, 1, 0).Should().Be(new CMColor(255, 0, 0, 21));
		new CMColor(_rasterLayer1, 2, 0).Should().Be(new CMColor(255, 0, 0, 7));
		new CMColor(_rasterLayer1, 3, 0).Should().Be(new CMColor(0, 0, 0, 0));
		new CMColor(_rasterLayer1, 0, 1).Should().Be(new CMColor(255, 0, 0, 21));
		new CMColor(_rasterLayer1, 1, 1).Should().Be(new CMColor(255, 0, 0, 16));
		new CMColor(_rasterLayer1, 2, 1).Should().Be(new CMColor(255, 0, 0, 5));
		new CMColor(_rasterLayer1, 3, 1).Should().Be(new CMColor(0, 0, 0, 0));
	}
}
