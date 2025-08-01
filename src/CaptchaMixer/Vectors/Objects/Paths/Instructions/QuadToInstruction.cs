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
/// Draws a Bezier quadratic curve.
/// </summary>
public class QuadToInstruction : CurveInstruction
{
	/// <summary>
	/// Control point.
	/// </summary>
	public Vector2 Control
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <summary>
	/// End point.
	/// </summary>
	public Vector2 End
	{
		get => Points[1];
		set => Points[1] = value;
	}

	/// <param name="controlX">Control point X coordinate.</param>
	/// <param name="controlY">Control point Y coordinate.</param>
	/// <param name="endX">End point X coordinate.</param>
	/// <param name="endY">End point Y coordinate.</param>
	public QuadToInstruction(
		float controlX,
		float controlY,
		float endX,
		float endY)
		: this(new(controlX, controlY), new(endX, endY)) { }

	/// <param name="control"><inheritdoc cref="Control" path="/summary"/></param>
	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	public QuadToInstruction(Vector2 control, Vector2 end)
		: base(PointType.Control, PointType.Contour)
	{
		Control = control;
		End = end;
	}

	public override VectorPathInstruction Clone()
		=> new QuadToInstruction(Control, End);

	public override RectangleF GetBounds(Vector2 position)
	{
		Span<Vector2> points = stackalloc Vector2[BoundsCalcPointsCount];
		VectorMath.CurveQuadEvaluate(new CurveQuad2(position, Control, End), points);
		return points.GetPointsBounds();
	}
}
