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
/// Base class for oscillation creators.
/// </summary>
public abstract class OscillationCreator : PatternCreator
{
	private readonly int _instructionsPerPeriod;

	/// <summary>
	/// Distance between oscillation elements.
	/// </summary>
	public ValueProvider<float> Spacing { get; set; } = 4;

	/// <summary>
	/// Oscillation amplitude.
	/// </summary>
	public ValueProvider<float> Amplitude { get; set; } = 2;

	/// <summary>
	/// Oscillation period.
	/// </summary>
	public ValueProvider<float> Period { get; set; } = 8;

	/// <summary>
	/// Deviation of oscillation start points along X axis.
	/// </summary>
	public ValueProvider<float> OffsetX { get; set; } = 0;

	/// <summary>
	/// Deviation of oscillation start points along Y axis.
	/// </summary>
	public ValueProvider<float> OffsetY { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="OffsetX"/> and <see cref="OffsetY"/>.
	/// </summary>
	public ValueProvider<float> Offset
	{
		set
		{
			OffsetX = value;
			OffsetY = value;
		}
	}

	protected OscillationCreator(int instructionsPerPeriod)
		=> _instructionsPerPeriod = instructionsPerPeriod;

	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <param name="amplitude"><inheritdoc cref="Amplitude" path="/summary"/></param>
	/// <param name="period"><inheritdoc cref="Period" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected OscillationCreator(
		int instructionsPerPeriod,
		ValueProvider<float> spacing,
		ValueProvider<float> amplitude,
		ValueProvider<float> period)
		: this(instructionsPerPeriod)
	{
		Spacing = spacing ?? throw new ArgumentNullException(nameof(spacing));
		Amplitude = amplitude ?? throw new ArgumentNullException(nameof(spacing));
		Period = period ?? throw new ArgumentNullException(nameof(spacing));
	}

	protected override VectorPath CreatePattern(RectangleF area, int maxCount)
	{
		var path = new VectorPath();
		var instructions = path.Instructions;

		var spacing = Spacing;
		var periodRepeat = Period.Repeat(2);

		for (var y = area.Top; y <= area.Bottom && (instructions.Count / _instructionsPerPeriod < maxCount); y += 1 + spacing)
		{
			var startPoint = new Vector2(area.Left + OffsetX, y + OffsetY);
			instructions.Add(new MoveToInstruction(startPoint));
			for (var x = startPoint.X; x < area.Right && (instructions.Count / _instructionsPerPeriod < maxCount); x += periodRepeat)
				instructions.AddRange(CreateOscillation(x, startPoint.Y, Amplitude, periodRepeat));
		}

		return path;
	}

	protected abstract IEnumerable<VectorPathInstruction> CreateOscillation(float x, float y, float amplitude, float period);
}
