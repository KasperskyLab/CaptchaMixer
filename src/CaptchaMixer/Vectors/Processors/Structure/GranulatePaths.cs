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
/// Granulates path instructions.
/// </summary>
/// <remarks>
/// Granulation is refactoring instructions by splitting them into smaller parts
/// so that their rendering result remains the same. It's primary purpose is to
/// control the detail level of further distortion applied by other processors.
/// </remarks>
public class GranulatePaths : OneByOnePathInstructionsProcessor
{
	/// <summary>
	/// Maximal linear length of each granulated segment.
	/// </summary>
	public ValueProvider<float> MaxLength { get; set; } = 2;

	public GranulatePaths() { }

	/// <param name="maxLength"><inheritdoc cref="MaxLength" path="/summary"/></param>
	public GranulatePaths(ValueProvider<float> maxLength)
		=> MaxLength = maxLength;

	protected override IEnumerable<VectorPathInstruction> Process(
		VectorPathInstruction instruction,
		Vector2 position,
		ICaptchaContext context)
		=> instruction.Granulate(position, MaxLength);
}
