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
/// Draws a straight line.
/// </summary>
public class LineToInstruction : VectorPathInstruction
{
	/// <summary>
	/// End point.
	/// </summary>
	public Vector2 End
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <param name="endX">End point X coordinate.</param>
	/// <param name="endY">End point Y coordinate.</param>
	public LineToInstruction(float endX, float endY)
		: this(new(endX, endY)) { }

	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	public LineToInstruction(Vector2 end)
		: base(1)
		=> End = end;

	public override VectorPathInstruction Clone()
		=> new LineToInstruction(End);

	public override RectangleF GetBounds(Vector2 position)
		=> new[] { position, End }.GetPointsBounds();
}
