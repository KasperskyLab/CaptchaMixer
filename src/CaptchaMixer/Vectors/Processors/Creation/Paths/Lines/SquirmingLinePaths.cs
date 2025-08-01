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
/// Creates paths consisting of sequences of straight line segments
/// where the next point is calculated by angle and radius relative to
/// the current pen position.
/// </summary>
public class SquirmingLinePaths : PathCreator
{
	/// <summary>
	/// Length of the next line.
	/// </summary>
	public ValueProvider<float> Radiuses { get; set; } = RandomFloat(5, 10);

	/// <summary>
	/// Angle of the next line.
	/// </summary>
	public ValueProvider<float> Angles { get; set; } = RandomMirroredFloat(90);

	public SquirmingLinePaths()
		: base() { }

	/// <inheritdoc cref="PathCreator(ValueProvider{int}, ValueProvider{int})" />
	public SquirmingLinePaths(
		ValueProvider<int> paths,
		ValueProvider<int> parts)
		: base(paths, parts) { }

	protected override VectorPath CreatePath(
		RectangleF area,
		ValueProvider<Vector2> points,
		int parts,
		bool beyonds)
	{
		Vector2 point = points;
		var path = new VectorPath(new MoveToInstruction(point));

		for (var i = 0; i < parts; i++)
		{
			float radius = Radiuses;
			if (radius == 0) continue;

			float angle = MathUtils.DegToRad(Angles);
			point = new Vector2(point.X + radius * (float)Math.Cos(angle), point.Y + radius * (float)Math.Sin(angle));
			if (!beyonds && !area.Contains(point)) continue;

			path.Instructions.Add(new LineToInstruction(point));
		}

		return path;
	}
}
