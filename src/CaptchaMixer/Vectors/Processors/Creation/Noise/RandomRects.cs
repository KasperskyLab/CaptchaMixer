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
/// Creates paths consisting of random rectangles.
/// </summary>
public class RandomRects : AreaBasedVectorObjectsCreator
{
	/// <summary>
	/// Count of rectangles.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 2;

	/// <summary>
	/// Rectangle width.
	/// </summary>
	public ValueProvider<float> Width { get; set; } = 5;

	/// <summary>
	/// Rectangle height.
	/// </summary>
	public ValueProvider<float> Height { get; set; } = 5;

	/// <summary>
	/// Sets <see cref="Width"/> and <see cref="Height"/>.
	/// </summary>
	public ValueProvider<float> Side
	{
		set
		{
			Width = value;
			Height = value;
		}
	}

	public RandomRects() { }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="side"><inheritdoc cref="Side" path="/summary"/></param>
	public RandomRects(
		ValueProvider<int> count,
		ValueProvider<float> side)
		: this(count, side, side) { }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	/// <param name="height"><inheritdoc cref="Height" path="/summary"/></param>
	public RandomRects(
		ValueProvider<int> count,
		ValueProvider<float> width,
		ValueProvider<float> height)
	{
		Count = count;
		Width = width;
		Height = height;
	}

	public override void Process(RectangleF area, IVectorLayer layer, ICaptchaContext context)
	{
		var vectorObject = new VectorObject();

		int count = Count;

		for (var i = 0; vectorObject.Paths.Count < count; i++)
		{
			float halfWidth = Width / 2;
			float halfHeight = Height / 2;
			var randomAreaX = RandomFloat(area.Left, area.Right);
			var randomAreaY = RandomFloat(area.Top, area.Bottom);
			var center = new Vector2(randomAreaX, randomAreaY);
			var lt = new Vector2(center.X - halfWidth, center.Y - halfHeight);
			var rb = new Vector2(center.X + halfWidth, center.Y + halfHeight);
			var path = new VectorPath(new[] { new AddRectInstruction(lt, rb) });
			vectorObject.Paths.Add(path);
		}

		layer.Objects.Add(vectorObject);
	}
}
