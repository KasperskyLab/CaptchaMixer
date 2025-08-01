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
/// Removes all <see cref="CloseInstruction"/>s from paths.
/// </summary>
public class UnclosePaths : OneByOnePathInstructionsProcessor
{
	protected override IEnumerable<VectorPathInstruction> Process(
		VectorPathInstruction instruction,
		Vector2 position,
		ICaptchaContext context)
	{
		if (instruction is CloseInstruction) yield break;
		yield return instruction;
	}
}
