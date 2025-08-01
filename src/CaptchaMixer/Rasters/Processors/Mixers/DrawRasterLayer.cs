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
/// Draws a raster layer on top of the processed one.
/// </summary>
/// <remarks>
/// Translucent colors are mixed using the most popular "add" method.
/// </remarks>
public class DrawRasterLayer : RasterLayerMixer
{
	/// <param name="rasterLayerName">The drawn layer name.</param>
	public DrawRasterLayer(ValueProvider<string> rasterLayerName)
		: base(rasterLayerName) { }

	/// <param name="mix">Drawn raw data.</param>
	public DrawRasterLayer(byte[] mix)
		: base(mix) { }

	protected override void Mix(byte[] src, byte[] mix, ICaptchaContext context)
	{
		var length = src.Length;
		for (var i = 0; i < length; i += 4)
		{
			var mixA = mix[i + 3];
			if (mixA == 0) continue;

			src[i] = MixColor(src[i], mix[i], mixA);
			src[i + 1] = MixColor(src[i + 1], mix[i + 1], mixA);
			src[i + 2] = MixColor(src[i + 2], mix[i + 2], mixA);
			src[i + 3] = MixAlpha(src[i + 3], mixA);
		}
	}

	private static byte MixColor(byte srcC, byte mixC, byte mixA)
		=> (byte)Math.Round(srcC + (mixC - srcC) * ((double)mixA / 255));

	private static byte MixAlpha(byte srcA, byte mixA)
		=> srcA != 0
			? (byte)Math.Round(srcA + (255 - srcA) * ((double)mixA / 255))
			: mixA;
}
