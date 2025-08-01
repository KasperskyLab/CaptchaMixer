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
/// Moves the pen to another point to continue drawing from there.
/// </summary>
public class MoveToInstruction : VectorPathInstruction
{
	/// <summary>
	/// Point to move the pen to.
	/// </summary>
	public Vector2 Point
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <param name="x">Pen move point X coordinate.</param>
	/// <param name="y">Pen move point Y coordinate.</param>
	public MoveToInstruction(float x, float y)
		: this(new(x, y)) { }

	/// <param name="point"><inheritdoc cref="Point" path="/summary"/></param>
	public MoveToInstruction(Vector2 point)
		: base(1)
		=> Point = point;

	public override VectorPathInstruction Clone()
		=> new MoveToInstruction(Point);
}
