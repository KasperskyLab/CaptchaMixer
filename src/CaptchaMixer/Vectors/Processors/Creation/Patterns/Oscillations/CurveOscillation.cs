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
/// Creates "sine" oscillations: ∿∿∿.
/// </summary>
public class CurveOscillation : OscillationCreator
{
	/// <summary>
	/// Value from 0 to 1 which defines the length of line segment from start/end
	/// points to control1/control2 points relative to 1/4 of period or amplitude.
	/// </summary>
	/// <remarks>
	/// Each period consists of 4 cubic curves. Controls points are always either
	/// on one vertical line or one horizontal line with corresponding base points.
	/// This parameter adjusts the distance between control and base points:<br/>
	/// - If the points are on one horizontal line then the distance between them
	/// is calculated relative to 1/4 of oscillation period.<br/>
	/// - If they are on one vertical line then it calculated relative to amplitude.
	/// </remarks>
	public ValueProvider<float> ControlLength { get; set; } = 0.5f;

	public CurveOscillation()
		: base(4) { }

	/// <inheritdoc cref="OscillationCreator(int, ValueProvider{float}, ValueProvider{float}, ValueProvider{float})"/>
	public CurveOscillation(
		ValueProvider<float> spacing,
		ValueProvider<float> amplitude,
		ValueProvider<float> period)
		: base(4, spacing, amplitude, period) { }

	protected override IEnumerable<VectorPathInstruction> CreateOscillation(float x, float y, float amplitude, float period)
	{
		var quarterPeriod = period / 4;
		var center = x + period / 2;
		var top = y - amplitude;
		var bottom = y + amplitude;
		var controlLength = MathUtils.Clamp(ControlLength, 0f, 1f);
		var controlLengthX = controlLength * quarterPeriod;
		var controlLengthY = controlLength * amplitude;

		yield return new CubicToInstruction(x, y - controlLengthY, x + quarterPeriod - controlLengthX, top, x + quarterPeriod, top);
		yield return new CubicToInstruction(x + quarterPeriod + controlLengthX, top, center, y - controlLengthY, center, y);
		yield return new CubicToInstruction(center, y + controlLengthY, center + quarterPeriod - controlLengthX, bottom, center + quarterPeriod, bottom);
		yield return new CubicToInstruction(center + quarterPeriod + controlLengthX, bottom, x + period, y + controlLengthY, x + period, y);
	}
}
