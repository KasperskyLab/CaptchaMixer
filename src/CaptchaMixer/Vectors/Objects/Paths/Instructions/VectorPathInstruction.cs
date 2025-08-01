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
/// Base class for vector path instructions.
/// </summary>
public abstract class VectorPathInstruction
{
	/// <summary>
	/// All points of instruction.
	/// </summary>
	public Vector2[] Points { get; }

	/// <summary>
	/// Types of <see cref="Points"/> in the same order.
	/// </summary>
	public PointType[] PointTypes { get; }

	/// <summary>
	/// Creates an instruction from points count.
	/// All points are considered to be of type <see cref="PointType.Contour"/>.
	/// </summary>
	/// <param name="pointsCount">Points count.</param>
	protected VectorPathInstruction(int pointsCount)
	{
		Points = new Vector2[pointsCount];
		PointTypes = Enumerable.Repeat(PointType.Contour, pointsCount).ToArray();
	}

	/// <summary>
	/// Creates an instruction from points' types.
	/// Points count is defined by <paramref name="pointTypes"/> array length.
	/// </summary>
	/// <param name="pointTypes">Point types.</param>
	protected VectorPathInstruction(params PointType[] pointTypes)
	{
		Points = new Vector2[pointTypes.Length];
		PointTypes = pointTypes;
	}

	/// <returns>
	/// A completely independent (new memory-allocated) copy of this instruction.
	/// </returns>
	public abstract VectorPathInstruction Clone();

	/// <returns>
	/// Minimal rectangle which contains this instruction.
	/// </returns>
	public virtual RectangleF GetBounds()
		=> Points.AsSpan().GetPointsBounds();

	/// <returns>
	/// Minimal rectangle which contains this instruction when being drawn from <paramref name="position"/>.
	/// </returns>
	public virtual RectangleF GetBounds(Vector2 position)
		=> GetBounds();
}
