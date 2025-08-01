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
/// Provides colors.
/// </summary>
public class ColorProvider : GrayscaleColorProvider
{
	/// <summary>
	/// Color hue.
	/// </summary>
	public ValueProvider<int> Hue { get; set; } = RandomHue();

	/// <summary>
	/// Color saturation.
	/// </summary>
	public ValueProvider<float> Saturation { get; set; } = RandomFloat(1);

	public override CMColor GetNext()
	{
		byte alpha = (byte)(int)Alpha;
		int hue = Hue;
		float brightness = Brightness;
		float saturation = Saturation;

		var color = new CMColor(hue, alpha);
		color.SetSaturation(saturation);
		color.SetBrightness(brightness);
		return color;
	}
}
