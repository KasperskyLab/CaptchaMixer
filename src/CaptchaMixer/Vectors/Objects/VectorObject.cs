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
/// Vector object, basically a list of <see cref="VectorPath"/>s.
/// </summary>
public class VectorObject : IEnumerable<VectorPath>
{
	/// <summary>
	/// Vector paths.
	/// </summary>
	public List<VectorPath> Paths { get; set; }

	/// <param name="paths"><inheritdoc cref="Paths" path="/summary"/></param>
	public VectorObject(params VectorPath[] paths)
		: this(paths as IEnumerable<VectorPath>) { }

	/// <param name="paths"><inheritdoc cref="Paths" path="/summary"/></param>
	public VectorObject(IEnumerable<VectorPath> paths = null)
		=> Paths = paths?.ToList() ?? new();

	/// <returns>
	/// A completely independent (new memory-allocated) copy of this vector object.
	/// </returns>
	public VectorObject Clone()
		=> new(Paths.Select(p => p.Clone()));

	/// <returns>
	/// Minimal rectangle which contains this entire object.
	/// </returns>
	public RectangleF GetBounds()
		=> Paths
			.Select(p => p.GetBounds())
			.GetRectsBounds();

	IEnumerator<VectorPath> IEnumerable<VectorPath>.GetEnumerator()
		=> Paths.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> ((IEnumerable<VectorPath>)this).GetEnumerator();
}
