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
/// Creates paths consisting of random straight lines.
/// </summary>
public class RandomLines : AreaBasedVectorObjectsCreator
{
	/// <summary>
	/// Paths count.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 2;

	/// <summary>
	/// Count of instructions in each path.
	/// </summary>
	public ValueProvider<int> Points { get; set; } = 5;

	/// <summary>
	/// Close the paths.
	/// </summary>
	public ValueProvider<bool> Close { get; set; } = false;

	public RandomLines() { }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="points"><inheritdoc cref="Points" path="/summary"/></param>
	public RandomLines(
		ValueProvider<int> count,
		ValueProvider<int> points)
	{
		Count = count;
		Points = points;
	}

	public override void Process(RectangleF area, IVectorLayer layer, ICaptchaContext context)
	{
		var vectorObject = new VectorObject();

		var randomAreaX = RandomFloat(area.Left, area.Right);
		var randomAreaY = RandomFloat(area.Top, area.Bottom);
		int count = Count;

		for (var i = 0; vectorObject.Paths.Count < count; i++)
		{
			float points = Points;
			bool close = Close;

			var path = new VectorPath();
			path.Instructions.Add(new MoveToInstruction(GetRandomAreaPoint()));

			for (var j = 1; j < points; j++)
				path.Instructions.Add(new LineToInstruction(GetRandomAreaPoint()));

			if (close)
				path.Instructions.Add(new CloseInstruction());

			vectorObject.Paths.Add(path);
		}

		layer.Objects.Add(vectorObject);

		Vector2 GetRandomAreaPoint() => new(randomAreaX, randomAreaY);
	}
}
