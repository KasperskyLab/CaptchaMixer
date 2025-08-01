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
/// Creates a grid of straight lines along X axis.
/// </summary>
public class LinesGridX : PatternCreator
{
	/// <summary>
	/// Spacing between grid lines.
	/// </summary>
	public ValueProvider<float> Spacing { get; set; } = 1;

	/// <summary>
	/// Deviation along X axis from grid line's start point.
	/// </summary>
	public ValueProvider<float> OffsetFromX { get; set; } = 0;

	/// <summary>
	/// Deviation along Y axis from grid line's start point.
	/// </summary>
	public ValueProvider<float> OffsetFromY { get; set; } = 0;

	/// <summary>
	/// Deviation along X axis from grid line's end point.
	/// </summary>
	public ValueProvider<float> OffsetToX { get; set; } = 0;

	/// <summary>
	/// Deviation along Y axis from grid line's end point.
	/// </summary>
	public ValueProvider<float> OffsetToY { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="OffsetFromX"/> and <see cref="OffsetToX"/>.
	/// </summary>
	public ValueProvider<float> OffsetX
	{
		set
		{
			OffsetFromX = value;
			OffsetToX = value;
		}
	}

	/// <summary>
	/// Sets <see cref="OffsetFromY"/> and <see cref="OffsetToY"/>.
	/// </summary>
	public ValueProvider<float> OffsetY
	{
		set
		{
			OffsetFromY = value;
			OffsetToY = value;
		}
	}

	/// <summary>
	/// Sets <see cref="OffsetFromX"/>, <see cref="OffsetToX"/>,
	/// <see cref="OffsetFromY"/> and <see cref="OffsetToY"/>.
	/// </summary>
	public ValueProvider<float> Offset
	{
		set
		{
			OffsetX = value;
			OffsetY = value;
		}
	}

	public LinesGridX() { }

	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public LinesGridX(ValueProvider<float> spacing)
		=> Spacing = spacing ?? throw new ArgumentNullException(nameof(spacing));

	protected override VectorPath CreatePattern(RectangleF area, int maxCount)
	{
		var path = new VectorPath();
		var instructions = path.Instructions;
		var spacing = Spacing;

		for (var y = area.Top; y < area.Bottom && instructions.Count / 2 < maxCount; y += 1 + spacing)
		{
			instructions.Add(new MoveToInstruction(area.Left + OffsetFromX, y + OffsetFromY));
			instructions.Add(new LineToInstruction(area.Right + OffsetToX, y + OffsetToY));
		}

		return path;
	}
}
