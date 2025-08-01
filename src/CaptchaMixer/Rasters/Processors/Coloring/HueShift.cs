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
/// Shifts pixels' colors hue.
/// </summary>
public class HueShift : IRasterProcessor
{
	/// <summary>
	/// Hue shift.
	/// </summary>
	public ValueProvider<int> Shift { get; set; } = 510;

	public HueShift() { }

	/// <param name="shift"><inheritdoc cref="Shift" path="/summary"/></param>
	public HueShift(ValueProvider<int> shift)
		=> Shift = shift;

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		int shift = Shift;
		if (shift == 0) return;

		layer.ProcessPixels(ChangeHue);

		void ChangeHue(ref CMColor color, int x, int y) => color.SetHue(color.GetHue() + shift);
	}
}
