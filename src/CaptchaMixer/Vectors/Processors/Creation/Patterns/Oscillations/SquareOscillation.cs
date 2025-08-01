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
/// Creates "square" oscillations: ⎍⎍⎍.
/// </summary>
public class SquareOscillation : OscillationCreator
{
	public SquareOscillation()
		: base(5) { }

	/// <inheritdoc cref="OscillationCreator(int, ValueProvider{float}, ValueProvider{float}, ValueProvider{float})"/>
	public SquareOscillation(
		ValueProvider<float> spacing,
		ValueProvider<float> amplitude,
		ValueProvider<float> period)
		: base(5, spacing, amplitude, period) { }

	protected override IEnumerable<VectorPathInstruction> CreateOscillation(float x, float y, float amplitude, float period)
	{
		var halfPeriod = period / 2;
		yield return new LineToInstruction(x, y - amplitude);
		yield return new LineToInstruction(x + halfPeriod, y - amplitude);
		yield return new LineToInstruction(x + halfPeriod, y + amplitude);
		yield return new LineToInstruction(x + period, y + amplitude);
		yield return new LineToInstruction(x + period, y);
	}
}
