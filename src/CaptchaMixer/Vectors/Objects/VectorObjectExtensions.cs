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

public static class VectorObjectExtensions
{
	/// <returns>
	/// <paramref name="obj"/> shifted by <paramref name="dX"/> along X axis.
	/// </returns>
	public static VectorObject MoveX(this VectorObject obj, float dX)
		=> ProcessInstructionsPoints(obj, p => p.MoveX(dX));

	/// <returns>
	/// <paramref name="obj"/> shifted by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static VectorObject MoveY(this VectorObject obj, float dY)
		=> ProcessInstructionsPoints(obj, p => p.MoveY(dY));

	/// <returns>
	/// <paramref name="obj"/>, shifted by <paramref name="dX"/> along X axis
	/// and by <paramref name="dY"/> along Y axis.
	/// </returns>
	public static VectorObject Move(this VectorObject obj, float dX, float dY)
		=> ProcessInstructionsPoints(obj, p => p.Move(dX, dY));

	/// <returns>
	/// <paramref name="obj"/> scaled by <paramref name="scaleX"/> along X axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorObject ScaleX(this VectorObject obj, Vector2 basePoint, float scaleX)
		=> ProcessInstructionsPoints(obj, p => p.ScaleX(basePoint, scaleX));

	/// <returns>
	/// <paramref name="obj"/> scaled by <paramref name="scaleY"/> along Y axis relatively
	/// to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorObject ScaleY(this VectorObject obj, Vector2 basePoint, float scaleY)
		=> ProcessInstructionsPoints(obj, p => p.ScaleY(basePoint, scaleY));

	/// <returns>
	/// <paramref name="obj"/> scaled by <paramref name="scaleX"/> along X and by
	/// <paramref name="scaleY"/> along Y axis relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorObject Scale(this VectorObject obj, Vector2 basePoint, float scaleX, float scaleY)
		=> ProcessInstructionsPoints(obj, p => p.Scale(basePoint, scaleX, scaleY));

	/// <returns>
	/// <paramref name="obj"/> scaled by <paramref name="scale"/> along X and Y axes
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorObject Scale(this VectorObject obj, Vector2 basePoint, float scale)
		=> ProcessInstructionsPoints(obj, p => p.Scale(basePoint, scale));

	/// <returns>
	/// <paramref name="obj"/> rotated by <paramref name="angle"/> degrees
	/// relatively to <paramref name="basePoint"/>.
	/// </returns>
	public static VectorObject Rotate(this VectorObject obj, Vector2 basePoint, float angle)
		=> ProcessInstructionsPoints(obj, p => p.Rotate(basePoint, angle));

	private static VectorObject ProcessInstructionsPoints(VectorObject obj, Func<Vector2, Vector2> processor)
	{
		foreach (var instruction in obj.EnumeratePathInstructions())
			for (var i = 0; i < instruction.Points.Length; i++)
				instruction.Points[i] = processor(instruction.Points[i]);

		return obj;
	}

	/// <summary>
	/// Enumerates all instructions of all paths in <paramref name="obj"/>.
	/// </summary>
	public static IEnumerable<VectorPathInstruction> EnumeratePathInstructions(this VectorObject obj)
		=> obj.Paths.SelectMany(p => p.Instructions);

	/// <summary>
	/// Skeletonizes all paths in <paramref name="obj"/> with the specified <paramref name="quality"/>
	/// using <see cref="Skeletonizer.Skeletonize(VectorPath, float)"/>.
	/// </summary>
	/// <returns>The same <paramref name="obj"/>.</returns>
	public static VectorObject SkeletonizePaths(this VectorObject obj, float quality = 1.0f)
	{
		foreach (var path in obj) path.Skeletonize(quality);
		return obj;
	}
}
