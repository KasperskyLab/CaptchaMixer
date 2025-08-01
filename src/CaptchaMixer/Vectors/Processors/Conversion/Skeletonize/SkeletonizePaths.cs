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
/// Transforms fillable shapes into a sequence of straight segments running approximately
/// through the center of their filled parts.
/// </summary>
public class SkeletonizePaths : OneByOnePathsProcessor
{
	/// <summary>
	/// Skeletonization quality. Linearly affects width and height of the raster used for
	/// pixels analysis. That is, a value of 2 will lead to a 4-fold drop in performance.
	/// </summary>
	public ValueProvider<float> Quality { get; set; } = 1;

	public SkeletonizePaths() { }

	/// <param name="quality"><inheritdoc cref="Quality" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public SkeletonizePaths(ValueProvider<float> quality)
		=> Quality = quality ?? throw new ArgumentNullException(nameof(quality));

	protected override IEnumerable<VectorPath> Process(VectorPath path, ICaptchaContext context)
	{
		path.Skeletonize(Quality);
		yield return path;
	}
}
