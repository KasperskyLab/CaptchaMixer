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
/// Applies a mask to raster layer.
/// </summary>
/// <remarks>
/// Mask is described by alpha components of pixels colors.
/// </remarks>
public class ApplyMask : RasterLayerMixer
{
	/// <param name="rasterLayerName">Name of the raster layer to use as a mask.</param>
	public ApplyMask(ValueProvider<string> rasterLayerName)
		: base(rasterLayerName) { }

	/// <param name="mix">Mask pixels data.</param>
	public ApplyMask(byte[] mix)
		: base(mix) { }

	protected override void Mix(byte[] src, byte[] mix, ICaptchaContext context)
	{
		var length = src.Length;
		for (var i = 0; i < length; i += 4)
		{
			var mixA = mix[i + 3];
			if (mixA == 0) src[i + 3] = 0;
			else if (mixA == 255) continue;
			else src[i + 3] = (byte)Math.Round(src[i + 3] * ((double)mixA / 255));
		}
	}
}
