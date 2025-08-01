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
/// Creates paths consisting of sequences of straight line segments.
/// </summary>
public class PointsLinePaths : PathCreator
{
	public PointsLinePaths()
		: base() { }

	/// <inheritdoc cref="PathCreator(ValueProvider{int}, ValueProvider{int})" />
	public PointsLinePaths(
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
			var prev = point;
			point = points;
			if (point.Equals(prev)) continue;
			if (!beyonds && !area.Contains(point)) continue;
			path.Instructions.Add(new LineToInstruction(point));
		}

		return path;
	}
}
