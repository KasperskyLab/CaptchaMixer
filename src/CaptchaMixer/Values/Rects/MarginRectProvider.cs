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
/// Generates rectangles from borders' margins relative to borders of <see cref="BaseRect"/>.
/// Positive margin values mean stepping inside base rect, negative - outside.
/// </summary>
public class MarginRectProvider : ValueProvider<RectangleF>
{
	/// <summary>
	/// Base rectangle.
	/// </summary>
	public ValueProvider<RectangleF> BaseRect { get; set; }

	/// <summary>
	/// Distance from left border of <see cref="BaseRect"/>.
	/// </summary>
	public ValueProvider<float> MarginLeft { get; set; } = RandomMirroredFloat(10);

	/// <summary>
	/// Distance from top border of <see cref="BaseRect"/>.
	/// </summary>
	public ValueProvider<float> MarginTop { get; set; } = RandomMirroredFloat(10);

	/// <summary>
	/// Distance from right border of <see cref="BaseRect"/>.
	/// </summary>
	public ValueProvider<float> MarginRight { get; set; } = RandomMirroredFloat(10);

	/// <summary>
	/// Distance from bottom border of <see cref="BaseRect"/>.
	/// </summary>
	public ValueProvider<float> MarginBottom { get; set; } = RandomMirroredFloat(10);

	/// <summary>
	/// Sets <see cref="MarginLeft"/> and <see cref="MarginRight"/>.
	/// </summary>
	public ValueProvider<float> MarginX
	{
		set
		{
			MarginLeft = value;
			MarginRight = value;
		}
	}

	/// <summary>
	/// Sets <see cref="MarginTop"/> and <see cref="MarginBottom"/>.
	/// </summary>
	public ValueProvider<float> MarginY
	{
		set
		{
			MarginTop = value;
			MarginBottom = value;
		}
	}

	/// <summary>
	/// Sets <see cref="MarginLeft"/>, <see cref="MarginTop"/>,
	/// <see cref="MarginRight"/> and <see cref="MarginBottom"/>.
	/// </summary>
	public ValueProvider<float> Margin
	{
		set
		{
			MarginX = value;
			MarginY = value;
		}
	}

	/// <param name="baseRect"><inheritdoc cref="BaseRect" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public MarginRectProvider(
		ValueProvider<RectangleF> baseRect)
		=> BaseRect = baseRect ?? throw new ArgumentNullException(nameof(baseRect));

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF})"/>
	public MarginRectProvider(
		ValueProvider<Size> baseRect)
		: this(baseRect.Convert(GeometryExtensions.ToRectangleF)) { }

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF})"/>
	/// <param name="margin"><inheritdoc cref="Margin" path="/summary"/></param>
	public MarginRectProvider(
		ValueProvider<RectangleF> baseRect,
		ValueProvider<float> margin)
		: this(baseRect, margin, margin, margin, margin) { }

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF},ValueProvider{float})"/>
	public MarginRectProvider(
		ValueProvider<Size> baseRect,
		ValueProvider<float> margin)
		: this(baseRect, margin, margin, margin, margin) { }

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF})"/>
	/// <param name="marginX"><inheritdoc cref="MarginX" path="/summary"/></param>
	/// <param name="marginY"><inheritdoc cref="MarginY" path="/summary"/></param>
	public MarginRectProvider(
		ValueProvider<RectangleF> baseRect,
		ValueProvider<float> marginX,
		ValueProvider<float> marginY)
		: this(baseRect, marginX, marginY, marginX, marginY) { }

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF},ValueProvider{float},ValueProvider{float})"/>
	public MarginRectProvider(
		ValueProvider<Size> baseRect,
		ValueProvider<float> marginX,
		ValueProvider<float> marginY)
		: this(baseRect, marginX, marginY, marginX, marginY) { }

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF})"/>
	/// <param name="marginLeft"><inheritdoc cref="MarginLeft" path="/summary"/></param>
	/// <param name="marginTop"><inheritdoc cref="MarginTop" path="/summary"/></param>
	/// <param name="marginRight"><inheritdoc cref="MarginRight" path="/summary"/></param>
	/// <param name="marginBottom"><inheritdoc cref="MarginBottom" path="/summary"/></param>
	public MarginRectProvider(
		ValueProvider<RectangleF> baseRect,
		ValueProvider<float> marginLeft,
		ValueProvider<float> marginTop,
		ValueProvider<float> marginRight,
		ValueProvider<float> marginBottom)
		: this(baseRect)
		=> InitMargins(marginLeft, marginTop, marginRight, marginBottom);

	/// <inheritdoc cref="MarginRectProvider(ValueProvider{RectangleF},ValueProvider{float},ValueProvider{float},ValueProvider{float},ValueProvider{float})"/>
	public MarginRectProvider(
		ValueProvider<Size> baseRect,
		ValueProvider<float> marginLeft,
		ValueProvider<float> marginTop,
		ValueProvider<float> marginRight,
		ValueProvider<float> marginBottom)
		: this(baseRect)
		=> InitMargins(marginLeft, marginTop, marginRight, marginBottom);

	private void InitMargins(
		ValueProvider<float> marginLeft,
		ValueProvider<float> marginTop,
		ValueProvider<float> marginRight,
		ValueProvider<float> marginBottom)
	{
		MarginLeft = marginLeft ?? throw new ArgumentNullException(nameof(marginLeft));
		MarginTop = marginTop ?? throw new ArgumentNullException(nameof(marginTop));
		MarginRight = marginRight ?? throw new ArgumentNullException(nameof(marginRight));
		MarginBottom = marginBottom ?? throw new ArgumentNullException(nameof(marginBottom));
	}

	public override RectangleF GetNext()
	{
		RectangleF baseRect = BaseRect;
		float left = baseRect.Left + MarginLeft;
		float top = baseRect.Top + MarginTop;
		var width = baseRect.Right - MarginRight - left;
		var height = baseRect.Bottom - MarginBottom - top;
		return new(left, top, width, height);
	}
}
