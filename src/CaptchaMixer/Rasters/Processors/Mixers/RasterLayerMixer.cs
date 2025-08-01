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

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// Raster layer mixer base class.
/// </summary>
public abstract class RasterLayerMixer : IRasterProcessor
{
	private readonly Func<ICaptchaContext, byte[]> _mixFunc;

	protected RasterLayerMixer(ValueProvider<string> rasterLayerName)
		=> _mixFunc = c => c.GetRasterLayer(rasterLayerName).Data;

	protected RasterLayerMixer(byte[] mix)
		=> _mixFunc = _ => mix;

	public void Process(IRasterLayer layer, ICaptchaContext context)
		=> Mix(layer.Data, _mixFunc(context), context);

	protected abstract void Mix(byte[] src, byte[] mix, ICaptchaContext context);
}
