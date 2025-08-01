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
/// Lowers paths' detail level by replacing sequences of instructions which have little
/// effect on the path trajectory with a single <see cref="LineToInstruction"/>.
/// </summary>
/// <inheritdoc cref="VectorPathExtensions.Primitivize(VectorPath, float)" path="/remarks"/>
public class PrimitivizePaths : OneByOnePathsProcessor
{
	/// <summary>
	/// <inheritdoc cref="VectorPathExtensions.Primitivize(VectorPath, float)" path="/param[@name='baseLineDistance']"/>
	/// </summary>
	public ValueProvider<float> BaseLineDistance { get; set; } = 3;

	public PrimitivizePaths() { }

	/// <param name="baseLineDistance"><inheritdoc cref="BaseLineDistance" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public PrimitivizePaths(ValueProvider<float> baseLineDistance)
		=> BaseLineDistance = baseLineDistance ?? throw new ArgumentNullException(nameof(baseLineDistance));

	protected override IEnumerable<VectorPath> Process(
		VectorPath path,
		ICaptchaContext context)
	{
		yield return path.Primitivize(BaseLineDistance);
	}
}
