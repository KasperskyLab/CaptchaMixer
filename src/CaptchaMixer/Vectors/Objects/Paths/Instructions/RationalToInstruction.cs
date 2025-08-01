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
/// Draws a Bezier rational quadratic curve.
/// </summary>
public class RationalToInstruction : CurveInstruction
{
	/// <summary>
	/// Start point weight.
	/// </summary>
	public float StartWeight { get; set; }

	/// <summary>
	/// Control point.
	/// </summary>
	public Vector2 Control
	{
		get => Points[0];
		set => Points[0] = value;
	}

	/// <summary>
	/// Control point weight.
	/// </summary>
	public float ControlWeight { get; set; }

	/// <summary>
	/// End point.
	/// </summary>
	public Vector2 End
	{
		get => Points[1];
		set => Points[1] = value;
	}

	/// <summary>
	/// End point weight.
	/// </summary>
	public float EndWeight { get; set; }

	/// <param name="startWeight"><inheritdoc cref="StartWeight" path="/summary"/></param>
	/// <param name="controlX">Control point X coordinate.</param>
	/// <param name="controlY">Control point Y coordinate.</param>
	/// <param name="controlWeight"><inheritdoc cref="ControlWeight" path="/summary"/></param>
	/// <param name="endX">End point X coordinate.</param>
	/// <param name="endY">End point Y coordinate.</param>
	/// <param name="endWeight"><inheritdoc cref="EndWeight" path="/summary"/></param>
	public RationalToInstruction(
		float startWeight,
		float controlX,
		float controlY,
		float controlWeight,
		float endX,
		float endY,
		float endWeight)
		: this(startWeight, new(controlX, controlY), controlWeight, new(endX, endY), endWeight) { }

	/// <param name="startWeight"><inheritdoc cref="StartWeight" path="/summary"/></param>
	/// <param name="control">Control point and its weight (in Z coordinate).</param>
	/// <param name="end">End point and its weight (in Z coordinate).</param>
	public RationalToInstruction(
		float startWeight,
		Vector3 control,
		Vector3 end)
		: this(startWeight, control.X, control.Y, control.Z, end.X, end.Y, end.Z) { }

	/// <param name="startWeight"><inheritdoc cref="StartWeight" path="/summary"/></param>
	/// <param name="control"><inheritdoc cref="Control" path="/summary"/></param>
	/// <param name="controlWeight"><inheritdoc cref="ControlWeight" path="/summary"/></param>
	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	/// <param name="endWeight"><inheritdoc cref="EndWeight" path="/summary"/></param>
	public RationalToInstruction(
		float startWeight,
		Vector2 control,
		float controlWeight,
		Vector2 end,
		float endWeight)
		: base(PointType.Control, PointType.Contour)
	{
		StartWeight = startWeight;
		Control = control;
		ControlWeight = controlWeight;
		End = end;
		EndWeight = endWeight;
	}

	public override VectorPathInstruction Clone()
		=> new RationalToInstruction(StartWeight, Control, ControlWeight, End, EndWeight);

	public override RectangleF GetBounds(Vector2 position)
	{
		Span<Vector2> points = stackalloc Vector2[BoundsCalcPointsCount];
		VectorMath.CurveRationalEvaluate(
			new CurveQuad3(
				new(position, StartWeight),
				new(Control, ControlWeight),
				new(End, EndWeight)),
			points);
		return points.GetPointsBounds();
	}
}
