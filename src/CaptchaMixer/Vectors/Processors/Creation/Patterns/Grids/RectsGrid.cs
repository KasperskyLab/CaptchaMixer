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
/// Creates a grid of rectangles.
/// </summary>
public class RectsGrid : PatternCreator
{
	/// <summary>
	/// Rectangle width.
	/// </summary>
	public ValueProvider<float> Width { get; set; } = 5;

	/// <summary>
	/// Rectangle height.
	/// </summary>
	public ValueProvider<float> Height { get; set; } = 5;

	/// <summary>
	/// Spacing along X axis.
	/// </summary>
	public ValueProvider<float> SpacingX { get; set; } = 0;

	/// <summary>
	/// Spacing along Y axis.
	/// </summary>
	public ValueProvider<float> SpacingY { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="SpacingX"/> and <see cref="SpacingY"/>.
	/// </summary>
	public ValueProvider<float> Spacing
	{
		set
		{
			SpacingX = value;
			SpacingY = value;
		}
	}

	/// <summary>
	/// Deviation of left border coordinate.
	/// </summary>
	public ValueProvider<float> OffsetLeft { get; set; } = 0;

	/// <summary>
	/// Deviation of top border coordinate.
	/// </summary>
	public ValueProvider<float> OffsetTop { get; set; } = 0;

	/// <summary>
	/// Deviation of right border coordinate.
	/// </summary>
	public ValueProvider<float> OffsetRight { get; set; } = 0;

	/// <summary>
	/// Deviation of bottom border coordinate.
	/// </summary>
	public ValueProvider<float> OffsetBottom { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="OffsetLeft"/> and <see cref="OffsetRight"/>.
	/// </summary>
	public ValueProvider<float> OffsetX
	{
		set
		{
			OffsetLeft = value;
			OffsetRight = value;
		}
	}

	/// <summary>
	/// Sets <see cref="OffsetTop"/> and <see cref="OffsetBottom"/>.
	/// </summary>
	public ValueProvider<float> OffsetY
	{
		set
		{
			OffsetTop = value;
			OffsetBottom = value;
		}
	}

	/// <summary>
	/// Sets <see cref="OffsetLeft"/>, <see cref="OffsetRight"/>,
	/// <see cref="OffsetTop"/> and <see cref="OffsetBottom"/>.
	/// </summary>
	public ValueProvider<float> Offset
	{
		set
		{
			OffsetX = value;
			OffsetY = value;
		}
	}

	public RectsGrid() { }

	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	/// <param name="height"><inheritdoc cref="Height" path="/summary"/></param>
	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public RectsGrid(
		ValueProvider<float> width,
		ValueProvider<float> height,
		ValueProvider<float> spacing)
	{
		Width = width ?? throw new ArgumentNullException(nameof(spacing));
		Height = height ?? throw new ArgumentNullException(nameof(spacing));
		Spacing = spacing ?? throw new ArgumentNullException(nameof(spacing));
	}

	protected override VectorPath CreatePattern(RectangleF area, int maxCount)
	{
		var path = new VectorPath();
		var instructions = path.Instructions;

		float width = Width;
		float height = Height;
		float spacingX = SpacingX;
		float spacingY = SpacingY;

		for (var y = area.Top; y < area.Bottom && instructions.Count < maxCount; y += height + spacingY)
			for (var x = area.Left; x < area.Right && instructions.Count < maxCount; x += width + spacingX)
				instructions.Add(new AddRectInstruction(
					x + OffsetLeft,
					y + OffsetTop,
					x + OffsetRight + width,
					y + OffsetBottom + height));

		return path;
	}
}
