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
/// Splits paths in objects into multiple paths - one new path for each contour
/// of the original path.
/// </summary>
/// <remarks>
/// Based on <see cref="VectorPathExtensions.EnumerateContours(VectorPath, bool)"/>.
/// </remarks>
public class SplitPaths : OneByOnePathsProcessor
{
	/// <summary>
	/// Add <see cref="CloseInstruction"/>s to newly created paths.
	/// </summary>
	public ValueProvider<bool> AddCloses { get; set; } = true;

	public SplitPaths() { }

	/// <param name="addCloses"><inheritdoc cref="AddCloses" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public SplitPaths(ValueProvider<bool> addCloses)
		=> AddCloses = addCloses ?? throw new ArgumentNullException(nameof(addCloses));

	protected override IEnumerable<VectorPath> Process(VectorPath path, ICaptchaContext context)
		=> path.EnumerateContours().Select(c => new VectorPath(c));
}
