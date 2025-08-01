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
/// Alters paths' instructions in such a way so they become less "shaky".
/// Or more "shaky" when unusual <see cref="Ratio"/> values are used.
/// </summary>
/// <inheritdoc cref="VectorPathExtensions.Straighten(VectorPath, float, float)" path="/remarks"/>
public class StraightenPaths : OneByOnePathsProcessor
{
	/// <summary>
	/// <inheritdoc cref="VectorPathExtensions.Straighten(VectorPath, float, float)" path="/param[@name='ratio']"/>
	/// </summary>
	public ValueProvider<float> Ratio { get; set; } = 1;

	/// <summary>
	/// <inheritdoc cref="VectorPathExtensions.Straighten(VectorPath, float, float)" path="/param[@name='maxDistance']"/>
	/// </summary>
	public ValueProvider<float> MaxDistance { get; set; } = 100;

	public StraightenPaths() { }

	/// <param name="ratio"><inheritdoc cref="Ratio" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public StraightenPaths(ValueProvider<float> ratio)
		=> Ratio = ratio ?? throw new ArgumentNullException(nameof(ratio));

	/// <param name="ratio"><inheritdoc cref="Ratio" path="/summary"/></param>
	/// <param name="maxDistance"><inheritdoc cref="MaxDistance" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public StraightenPaths(ValueProvider<float> ratio, ValueProvider<float> maxDistance)
		: this(ratio)
		=> MaxDistance = maxDistance ?? throw new ArgumentNullException(nameof(maxDistance));

	protected override IEnumerable<VectorPath> Process(
		VectorPath path,
		ICaptchaContext context)
	{
		yield return path.Straighten(Ratio, MaxDistance);
	}
}
