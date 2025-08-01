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
/// Creates "triangle" oscillations: /\/\/\.
/// </summary>
public class TriangleOscillation : OscillationCreator
{
	public TriangleOscillation()
		: base(3) { }

	/// <inheritdoc cref="OscillationCreator(int, ValueProvider{float}, ValueProvider{float}, ValueProvider{float})"/>
	public TriangleOscillation(
		ValueProvider<float> spacing,
		ValueProvider<float> amplitude,
		ValueProvider<float> period)
		: base(3, spacing, amplitude, period) { }

	protected override IEnumerable<VectorPathInstruction> CreateOscillation(float x, float y, float amplitude, float period)
	{
		var quarterPeriod = period / 4;
		yield return new LineToInstruction(x + quarterPeriod, y - amplitude);
		yield return new LineToInstruction(x + quarterPeriod * 3, y + amplitude);
		yield return new LineToInstruction(x + period, y);
	}
}
