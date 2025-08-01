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
/// Fills entire layer.
/// </summary>
/// <remarks>
/// As opposed to <see cref="SetPixels"/>, this processor uses <see cref="PaintColor"/>,
/// thus enabling the usage of gradient colors such as
/// <see cref="LinearGradientPaintColor"/> or <see cref="RadialGradientPaintColor"/>.<br />
/// <see cref="SetPixels"/> simply replaces all pixels' colors. If that's exaclty what you
/// need then it's the recommended choice since it works faster.
/// </remarks>
public class Fill : SKBitmapRasterProcessor
{
	/// <summary>
	/// Fill paint color.
	/// </summary>
	public ValueProvider<PaintColor> Color { get; set; } = new SolidPaintColor(CMColor.White);

	public Fill() { }

	/// <param name="solid">Solid fill color.</param>
	public Fill(ValueProvider<CMColor> solid)
		: this(new SolidPaintColor(solid)) { }

	/// <param name="color">Fill color.</param>
	public Fill(ValueProvider<PaintColor> color)
		=> Color = color;

	protected override void Process(SKBitmap bitmap, ICaptchaContext context)
	{
		using var canvas = new SKCanvas(bitmap);
		var rect = context.Parameters.Size.ToRectangleF();
		var paintInfo = new FillPaintInfo(Color);
		var paint = paintInfo.ToSKPaint(rect);
		canvas.DrawRect(rect.ToSKRect(), paint);
	}
}
