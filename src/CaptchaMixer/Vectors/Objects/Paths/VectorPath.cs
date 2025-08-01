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
/// Vector path, basically a list of <see cref="VectorPathInstruction"/>s.
/// </summary>
public class VectorPath : IEnumerable<(Vector2 position, VectorPathInstruction instruction)>
{
	/// <summary>
	/// Vector path instructions.
	/// </summary>
	public List<VectorPathInstruction> Instructions { get; set; }

	/// <param name="instructions"><inheritdoc cref="Instructions" path="/summary"/></param>
	public VectorPath(params VectorPathInstruction[] instructions)
		: this(instructions as IEnumerable<VectorPathInstruction>) { }

	/// <param name="instructions"><inheritdoc cref="Instructions" path="/summary"/></param>
	public VectorPath(IEnumerable<VectorPathInstruction> instructions = null)
		=> Instructions = instructions?.ToList() ?? new List<VectorPathInstruction>();

	/// <returns>
	/// A completely independent (new memory-allocated) copy of this vector path.
	/// </returns>
	public VectorPath Clone()
		=> new(Instructions.Select(i => i.CastClone()));

	/// <returns>
	/// Minimal rectangle which contains this entire path.
	/// </returns>
	public RectangleF GetBounds()
		=> this
			.Select(tuple => tuple.instruction.GetBounds(tuple.position))
			.GetRectsBounds();

	public IEnumerator<(Vector2 position, VectorPathInstruction instruction)> GetEnumerator()
		=> new VectorPathInstructionsIterator(Instructions);

	IEnumerator IEnumerable.GetEnumerator()
		=> ((IEnumerable<(Vector2 position, VectorPathInstruction instruction)>)this).GetEnumerator();
}
