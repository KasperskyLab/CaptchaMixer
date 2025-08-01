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
/// Stroke paint.
/// </summary>
public class StrokePaintInfo : PaintInfo
{
	/// <summary>
	/// Disabled stroke paint common instance.
	/// </summary>
	public static readonly ValueProvider<StrokePaintInfo> Disabled = new StrokePaintInfo { Enabled = false };

	/// <summary>
	/// Stroke width.
	/// </summary>
	public ValueProvider<float> Width { get; set; } = 1;

	/// <inheritdoc cref="StrokeCap"/>
	public ValueProvider<StrokeCap> Cap { get; set; } = StrokeCap.Butt;

	/// <inheritdoc cref="StrokeJoin"/>
	public ValueProvider<StrokeJoin> Join { get; set; } = StrokeJoin.Miter;

	/// <summary>
	/// Float value pairs provider. Must sequentially return painted and non-painted
	/// area length.
	/// </summary>
	public ValueProvider<float> Dash { get; set; } = Carousel<float>(4, 2);

	/// <summary>
	/// Count of dash pairs returned by <see cref="Dash"/>. 0 means solid line.
	/// </summary>
	public ValueProvider<int> DashPairs { get; set; } = 0;

	public StrokePaintInfo()
		: base() { }

	/// <param name="color">Stroke paint color.</param>
	public StrokePaintInfo(ValueProvider<PaintColor> color)
		: base(color) { }

	/// <param name="solid">Stroke solid color.</param>
	public StrokePaintInfo(ValueProvider<CMColor> solid)
		: this(new SolidPaintColor(solid)) { }

	/// <inheritdoc cref="StrokePaintInfo(ValueProvider{PaintColor})" path="/param[@name='color']"/>
	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	public StrokePaintInfo(ValueProvider<PaintColor> color, ValueProvider<float> width)
		: this(color)
		=> Width = width;

	/// <inheritdoc cref="StrokePaintInfo(ValueProvider{CMColor})" path="/param[@name='solid']"/>
	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	public StrokePaintInfo(ValueProvider<CMColor> solid, ValueProvider<float> width)
		: this(new SolidPaintColor(solid), width) { }
}
