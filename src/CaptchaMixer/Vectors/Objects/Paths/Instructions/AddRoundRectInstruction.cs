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
/// Draws a rectangle with rounded corners.
/// </summary>
/// <remarks>
/// <inheritdoc cref="ContourInstruction" path="/remarks"/>
/// </remarks>
public class AddRoundRectInstruction : AddRectInstruction
{
	/// <summary>
	/// X-axis corner rounding radius.
	/// </summary>
	public float RadiusX { get; set; }

	/// <summary>
	/// Y-axis corner rounding radius.
	/// </summary>
	public float RadiusY { get; set; }

	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)"/>
	/// <param name="radius">Corner rounding radius.</param>
	public AddRoundRectInstruction(
		float left,
		float top,
		float right,
		float bottom,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		: this(new(left, top), new(right, bottom), radius, radius, direction) { }

	/// <inheritdoc cref="AddRectInstruction(float, float, float, float, PathDirection)"/>
	/// <param name="radiusX"><inheritdoc cref="RadiusX" path="/summary"/></param>
	/// <param name="radiusY"><inheritdoc cref="RadiusY" path="/summary"/></param>
	public AddRoundRectInstruction(
		float left,
		float top,
		float right,
		float bottom,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		: this(new(left, top), new(right, bottom), radiusX, radiusY, direction) { }

	/// <inheritdoc cref="AddRectInstruction(Vector2, Vector2, PathDirection)"/>
	/// <param name="radius">Corner rounding radius.</param>
	public AddRoundRectInstruction(
		Vector2 leftTop,
		Vector2 rightBottom,
		float radius,
		PathDirection direction = PathDirection.Clockwise)
		: this(leftTop, rightBottom, radius, radius, direction) { }

	/// <inheritdoc cref="AddRectInstruction(Vector2, Vector2, PathDirection)"/>
	/// <param name="radiusX"><inheritdoc cref="RadiusX" path="/summary"/></param>
	/// <param name="radiusY"><inheritdoc cref="RadiusY" path="/summary"/></param>
	public AddRoundRectInstruction(
		Vector2 leftTop,
		Vector2 rightBottom,
		float radiusX,
		float radiusY,
		PathDirection direction = PathDirection.Clockwise)
		: base(leftTop, rightBottom, direction)
	{
		RadiusX = radiusX;
		RadiusY = radiusY;
	}

	public override VectorPathInstruction Clone()
		=> new AddRoundRectInstruction(LeftTop, RightBottom, RadiusX, RadiusY, Direction);
}
