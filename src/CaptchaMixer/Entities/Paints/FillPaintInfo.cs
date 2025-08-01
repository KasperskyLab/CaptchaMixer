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
/// Fill paint.
/// </summary>
public class FillPaintInfo : PaintInfo
{
	/// <summary>
	/// Disabled fill paint common instance.
	/// </summary>
	public static readonly ValueProvider<FillPaintInfo> Disabled = new FillPaintInfo { Enabled = false };

	public FillPaintInfo()
		: base() { }

	/// <param name="color">Fill color.</param>
	public FillPaintInfo(ValueProvider<PaintColor> color)
		: base(color) { }

	/// <param name="solid">Solid fill paint.</param>
	public FillPaintInfo(ValueProvider<CMColor> solid)
		: this(new SolidPaintColor(solid)) { }
}
