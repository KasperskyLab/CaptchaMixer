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
/// Draws an oval in a rectangle area.
/// </summary>
/// <remarks>
/// <inheritdoc cref="ContourInstruction" path="/remarks"/>
/// </remarks>
public class AddOvalInstruction : AddRectInstruction
{
	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)"/>
	public AddOvalInstruction(
		float left,
		float top,
		float right,
		float bottom,
		PathDirection direction = PathDirection.Clockwise)
		: base(new(left, top), new(right, bottom), direction) { }

	/// <inheritdoc cref="AddRectInstruction(Vector2, Vector2, PathDirection)"/>
	public AddOvalInstruction(
		Vector2 leftTop,
		Vector2 rightBottom,
		PathDirection direction = PathDirection.Clockwise)
		: base(leftTop, rightBottom, direction) { }

	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)"/>
	/// <param name="center">Oval center.</param>
	/// <param name="radius">Oval radius along X and Y axes.</param>
	public AddOvalInstruction(
		Vector2 center,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		: this(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius, direction) { }

	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)"/>
	/// <param name="center">Oval center.</param>
	/// <param name="radiusX">Oval radius along X axis.</param>
	/// <param name="radiusY">Oval radius along Y axis.</param>
	public AddOvalInstruction(
		Vector2 center,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		: this(center.X - radiusX, center.Y - radiusY, center.X + radiusX, center.Y + radiusY, direction) { }

	public override VectorPathInstruction Clone()
		=> new AddOvalInstruction(LeftTop, RightBottom, Direction);
}
