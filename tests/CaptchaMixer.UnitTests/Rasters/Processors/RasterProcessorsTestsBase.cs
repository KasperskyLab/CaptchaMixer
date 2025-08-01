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

public abstract class RasterProcessorsTestsBase
{
	protected const string VectorLayerName = "vector-test";
	protected const string RasterLayerName1 = "raster-test-1";
	protected const string RasterLayerName2 = "raster-test-2";
	protected static readonly Size LayerSize = new(10, 5);

	protected IVectorLayer _vectorLayer;
	protected IRasterLayer _rasterLayer1;
	protected IRasterLayer _rasterLayer2;
	protected ICaptchaContext _context;

	[SetUp]
	public void SetUp()
	{
		_vectorLayer = new VectorLayer(VectorLayerName);

		_vectorLayer.Objects.Add(
			new VectorObject(
				new VectorPath(
					new MoveToInstruction(LayerSize.Width - 1, LayerSize.Height - 1),
					new LineToInstruction(0, 0)),
				new VectorPath(
					new MoveToInstruction(LayerSize.Width - 1, 0),
					new LineToInstruction(0, LayerSize.Height - 1))));

		_rasterLayer1 = new RasterLayer(RasterLayerName1, LayerSize);
		_rasterLayer2 = new RasterLayer(RasterLayerName2, LayerSize);

		_context = new CaptchaContext(
			new CaptchaParameters("ABCDEFG", LayerSize),
			new[] { _vectorLayer },
			new[] { _rasterLayer1, _rasterLayer2 });
	}

	protected void AssertAllColors(CMColor color)
		=> AssertAllColors(color, _rasterLayer1);

	protected static void AssertAllColors(CMColor color, IRasterLayer layer)
		=> layer.ProcessPixels((c, _, _) => c.Should().Be(color));
}
