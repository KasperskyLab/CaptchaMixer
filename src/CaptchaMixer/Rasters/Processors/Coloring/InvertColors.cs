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
/// Inverts colors completely or their separate components.
/// </summary>
public class InvertColors : IRasterProcessor
{
	/// <summary>
	/// Invert red component.
	/// </summary>
	public ValueProvider<bool> InvertR { get; set; } = true;

	/// <summary>
	/// Invert green component.
	/// </summary>
	public ValueProvider<bool> InvertG { get; set; } = true;

	/// <summary>
	/// Invert blue component.
	/// </summary>
	public ValueProvider<bool> InvertB { get; set; } = true;

	/// <summary>
	/// Invert alpha component.
	/// </summary>
	public ValueProvider<bool> InvertA { get; set; } = false;

	public InvertColors() { }

	/// <param name="invertRGB">Invert color components.</param>
	/// <param name="invertA"><inheritdoc cref="InvertA" path="/summary"/></param>
	public InvertColors(
		ValueProvider<bool> invertRGB,
		ValueProvider<bool> invertA)
		: this(invertRGB, invertRGB, invertRGB, invertA) { }

	/// <param name="invertR"><inheritdoc cref="InvertR" path="/summary"/></param>
	/// <param name="invertG"><inheritdoc cref="InvertG" path="/summary"/></param>
	/// <param name="invertB"><inheritdoc cref="InvertB" path="/summary"/></param>
	/// <param name="invertA"><inheritdoc cref="InvertA" path="/summary"/></param>
	public InvertColors(
	ValueProvider<bool> invertR,
	ValueProvider<bool> invertG,
	ValueProvider<bool> invertB,
	ValueProvider<bool> invertA)
	{
		InvertR = invertR;
		InvertG = invertG;
		InvertB = invertB;
		InvertA = invertA;
	}

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		var data = layer.Data;
		var length = data.Length;

		bool invertR = InvertR;
		bool invertG = InvertG;
		bool invertB = InvertB;
		bool invertA = InvertA;

		for (var i = 0; i < length; i += 4)
		{
			if (invertR) data[i] = (byte)(255 - data[i]);
			if (invertG) data[i + 1] = (byte)(255 - data[i + 1]);
			if (invertB) data[i + 2] = (byte)(255 - data[i + 2]);
			if (invertA) data[i + 3] = (byte)(255 - data[i + 3]);
		}
	}
}
