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
/// Sets all pixels color.
/// </summary>
/// <remarks>
/// As opposed to <see cref="Fill"/>, this processor simply rewrites pixels' colors.
/// If that's exactly what you need then it's the recommended choice since it
/// works faster.
/// </remarks>
public class SetPixels : IRasterProcessor
{
	/// <summary>
	/// Pixels' color.
	/// </summary>
	public ValueProvider<CMColor> Color { get; set; } = CMColor.White;

	public SetPixels() { }

	/// <param name="color"><inheritdoc cref="Color" path="/summary"/></param>
	public SetPixels(ValueProvider<CMColor> color)
		=> Color = color;

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		CMColor newColor = Color;
		layer.ProcessPixels(SetColor);

		void SetColor(ref CMColor color, int x, int y) => color.CopyFrom(newColor);
	}
}
