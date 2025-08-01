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
/// Creates a grid of ovals.
/// </summary>
public class OvalsGrid : RectsGrid
{
	public OvalsGrid() : base() { }

	/// <inheritdoc cref="RectsGrid(ValueProvider{float}, ValueProvider{float}, ValueProvider{float})"/>
	public OvalsGrid(
		ValueProvider<float> width,
		ValueProvider<float> height,
		ValueProvider<float> spacing)
		: base(width, height, spacing) { }

	protected override VectorPath CreatePattern(RectangleF area, int maxCount)
	{
		var path = base.CreatePattern(area, maxCount);

		for (var i = 0; i < path.Instructions.Count; i++)
		{
			var rect = (AddRectInstruction)path.Instructions[i];
			path.Instructions[i] = new AddOvalInstruction(rect.LeftTop, rect.RightBottom, rect.Direction);
		}

		return path;
	}
}
