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

public class CompositesTests : RasterProcessorsTestsBase
{
	[Test]
	public void RasterProcessors_Composites_Sequence_Test()
	{
		new RasterProcessorsSequence(
			new Fill(CMColor.Black),
			new InvertColors(true, false, false, false),
			new InvertColors(false, true, false, false))
			.Process(_rasterLayer1, _context);
		AssertAllColors(CMColor.Yellow);
	}

	[Test]
	public void RasterProcessors_Composites_IsolatedSequence_Test()
	{
		new Fill(CMColor.Red).Process(_rasterLayer1, _context);
		new RasterProcessorsIsolatedSequence(new InvertColors(true, true)).Process(_rasterLayer1, _context);
		AssertAllColors(CMColor.White);
	}

	[Test]
	public void RasterProcessors_Composites_Selector_Test()
	{
		var provider = Carousel<IRasterProcessor>(new Fill(CMColor.Red), new Fill(CMColor.Green));
		var processor = new RasterProcessorsSelector(provider);
		processor.Process(_rasterLayer1, _context);
		AssertAllColors(CMColor.Red);
		processor.Process(_rasterLayer1, _context);
		AssertAllColors(CMColor.Green);
	}
}
