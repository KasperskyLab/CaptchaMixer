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
/// Draws a rectangle.
/// </summary>
/// <remarks>
/// <inheritdoc cref="ContourInstruction" path="/remarks"/>
/// </remarks>
public class AddRectInstruction : ContourInstruction
{
	/// <summary>
	/// Left-top corner.
	/// </summary>
	public Vector2 LeftTop
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <summary>
	/// Right-bottom corner.
	/// </summary>
	public Vector2 RightBottom
	{
		get => Points[1];
		set => Points[1] = value;
	}

	/// <param name="left">Left border X-axis coordinate.</param>
	/// <param name="top">Top border Y-axis coordinate.</param>
	/// <param name="right">Right border X-axis coordinate.</param>
	/// <param name="bottom">Bottom border Y-axis coordinate.</param>
	/// <param name="direction"><inheritdoc cref="PathDirection" path="/summary"/></param>
	public AddRectInstruction(
		float left,
		float top,
		float right,
		float bottom,
		PathDirection direction = PathDirection.Clockwise)
		: this(new(left, top), new(right, bottom), direction) { }

	/// <param name="leftTop"><inheritdoc cref="LeftTop" path="/summary"/></param>
	/// <param name="rightBottom"><inheritdoc cref="RightBottom" path="/summary"/></param>
	/// <param name="direction"><inheritdoc cref="PathDirection" path="/summary"/></param>
	public AddRectInstruction(
		Vector2 leftTop,
		Vector2 rightBottom,
		PathDirection direction = PathDirection.Clockwise)
		: base(2, direction)
	{
		LeftTop = leftTop;
		RightBottom = rightBottom;
	}

	public override VectorPathInstruction Clone()
		=> new AddRectInstruction(LeftTop, RightBottom, Direction);
}
