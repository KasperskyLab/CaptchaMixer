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
/// Draws a Bezier cubic curve.
/// </summary>
public class CubicToInstruction : CurveInstruction
{
	/// <summary>
	/// First control point.
	/// </summary>
	public Vector2 Control1
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <summary>
	/// Second control point.
	/// </summary>
	public Vector2 Control2
	{
		get => Points[1];
		set => Points[1] = value;
	}

	/// <summary>
	/// End point.
	/// </summary>
	public Vector2 End
	{
		get => Points[2];
		set => Points[2] = value;
	}

	/// <param name="control1X">First control point X coordinate.</param>
	/// <param name="control1Y">First control point Y coordinate.</param>
	/// <param name="control2X">Second control point X coordinate.</param>
	/// <param name="control2Y">Second control point Y coordinate.</param>
	/// <param name="endX">End point X coordinate.</param>
	/// <param name="endY">End point Y coordinate.</param>
	public CubicToInstruction(
		float control1X,
		float control1Y,
		float control2X,
		float control2Y,
		float endX,
		float endY)
		: this(new(control1X, control1Y), new(control2X, control2Y), new(endX, endY)) { }

	/// <param name="control1"><inheritdoc cref="Control1" path="/summary"/></param>
	/// <param name="control2"><inheritdoc cref="Control2" path="/summary"/></param>
	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	public CubicToInstruction(
		Vector2 control1,
		Vector2 control2,
		Vector2 end)
		: base(PointType.Control, PointType.Control, PointType.Contour)
	{
		Control1 = control1;
		Control2 = control2;
		End = end;
	}

	public override VectorPathInstruction Clone()
		=> new CubicToInstruction(Control1, Control2, End);

	public override RectangleF GetBounds(Vector2 position)
	{
		Span<Vector2> points = stackalloc Vector2[BoundsCalcPointsCount];
		VectorMath.CurveCubicEvaluate(new CurveCubic2(position, Control1, Control2, End), points);
		return points.GetPointsBounds();
	}
}
