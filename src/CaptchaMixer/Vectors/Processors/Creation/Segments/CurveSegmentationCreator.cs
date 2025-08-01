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
/// Base class for creators of segmentations separted by Bezier curves.
/// </summary>
public abstract class CurveSegmentationCreator : AreaSegmentationCreator
{
	/// <summary>
	/// Distance between the first base lines snap point and first control point.
	/// </summary>
	public ValueProvider<float> Radius1 { get; set; } = 10;

	/// <summary>
	/// Distance between the second base lines snap point and second control point.
	/// </summary>
	public ValueProvider<float> Radius2 { get; set; } = 10;

	/// <summary>
	/// Sets <see cref="Radius1"/> and <see cref="Radius2"/>.
	/// </summary>
	public ValueProvider<float> Radius
	{
		set
		{
			Radius1 = value;
			Radius2 = value;
		}
	}

	/// <summary>
	/// Tilt angle between the line from first to second snap points
	/// and the line from first snap to first control point.
	/// </summary>
	public ValueProvider<float> Angle1 { get; set; } = 10;

	/// <summary>
	/// Tilt angle between the line from first to second snap points
	/// and the line from second snap to second control point.
	/// </summary>
	public ValueProvider<float> Angle2 { get; set; } = 10;

	/// <summary>
	/// Sets <see cref="Angle1"/> and <see cref="Angle2"/>.
	/// </summary>
	public ValueProvider<float> Angle
	{
		set
		{
			Angle1 = value;
			Angle2 = value;
		}
	}

	protected CurveSegmentationCreator() { }

	/// <inheritdoc cref="AreaSegmentationCreator(ValueProvider{int})"/>
	protected CurveSegmentationCreator(ValueProvider<int> segments)
		: base(segments) { }

	protected override VectorPathInstruction GetSeparator(Vector2 from, Vector2 to)
		=> new CubicToInstruction(
			GetControlPoint(from, to, Radius1, Angle1),
			GetControlPoint(to, from, Radius2, Angle2),
			to);

	private static Vector2 GetControlPoint(
		Vector2 from,
		Vector2 to,
		ValueProvider<float> radius,
		ValueProvider<float> angle)
	{
		var dx = to.X - from.X;
		var dy = to.Y - from.Y;
		var l = (float)Math.Sqrt(dx * dx + dy * dy);
		var ratio = radius / l;
		var point = new Vector2(from.X + ratio * dx, from.Y + ratio * dy);
		return VectorMath.Rotate(point, from, angle);
	}
}
