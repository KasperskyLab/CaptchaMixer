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

public class ColoringTests : RasterProcessorsTestsBase
{
	[Test]
	[TestCase(-1, -1, -1, -1, 0, 0, 0, 0)]
	[TestCase(1, 2, 3, 4, 1, 2, 3, 4)]
	[TestCase(256, 256, 256, 256, 255, 255, 255, 255)]
	public void RasterProcessors_Coloring_AddColors_Test(
		int ra, int ga, int ba, int aa,
		byte rr, byte gr, byte br, byte ar)
	{
		new AddColors(ra, ga, ba, aa).Process(_rasterLayer1, _context);
		AssertAllColors(new CMColor(rr, gr, br, ar));
	}

	[Test]
	public void RasterProcessors_Coloring_Fill_Test()
	{
		var color = new CMColor(1, 2, 3, 4);
		new Fill(color).Process(_rasterLayer1, _context);
		AssertAllColors(color);
	}

	[Test]
	public void RasterProcessors_Coloring_Grayscale_Test()
	{
		var color = new CMColor(255, 127, 0);
		new Fill(color).Process(_rasterLayer1, _context);
		new GrayscaleColors().Process(_rasterLayer1, _context);
		color.SetGrayscale();
		AssertAllColors(color);
	}

	[Test]
	public void RasterProcessors_Coloring_HueShift_Test()
	{
		var color = new CMColor(255, 0, 0);
		new Fill(color).Process(_rasterLayer1, _context);
		new HueShift(255).Process(_rasterLayer1, _context);
		color.SetHue(255);
		AssertAllColors(color);
	}

	[Test]
	public void RasterProcessors_Coloring_InvertColors_Test(
		[Values(true, false)] bool r,
		[Values(true, false)] bool g,
		[Values(true, false)] bool b,
		[Values(true, false)] bool a)
	{
		new InvertColors(r, g, b, a).Process(_rasterLayer1, _context);
		var color = new CMColor(r ? 255 : 0, g ? 255 : 0, b ? 255 : 0, a ? 255 : 0);
		AssertAllColors(color);
	}

	[Test]
	public void RasterProcessors_Coloring_SetPixels_Test()
	{
		var color = new CMColor(1, 2, 3, 4);
		new SetPixels(color).Process(_rasterLayer1, _context);
		AssertAllColors(color);
	}
}
