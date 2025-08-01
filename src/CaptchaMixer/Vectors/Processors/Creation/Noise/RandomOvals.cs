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
/// Creates paths consisting of random ovals.
/// </summary>
public class RandomOvals : AreaBasedVectorObjectsCreator
{
	/// <summary>
	/// Paths count.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 2;

	/// <summary>
	/// Ovals X axis raduis.
	/// </summary>
	public ValueProvider<float> RadiusX { get; set; } = 5;

	/// <summary>
	/// Ovals Y axis raduis.
	/// </summary>
	public ValueProvider<float> RadiusY { get; set; } = 5;

	/// <summary>
	/// Sets <see cref="RadiusX"/> and <see cref="RadiusY"/>.
	/// </summary>
	public ValueProvider<float> Radius
	{
		set
		{
			RadiusX = value;
			RadiusY = value;
		}
	}

	public RandomOvals() { }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="radius"><inheritdoc cref="Radius" path="/summary"/></param>
	public RandomOvals(
		ValueProvider<int> count,
		ValueProvider<float> radius)
		: this(count, radius, radius) { }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="radiusX"><inheritdoc cref="RadiusX" path="/summary"/></param>
	/// <param name="radiusY"><inheritdoc cref="RadiusY" path="/summary"/></param>
	public RandomOvals(
		ValueProvider<int> count,
		ValueProvider<float> radiusX,
		ValueProvider<float> radiusY)
	{
		Count = count;
		RadiusX = radiusX;
		RadiusY = radiusY;
	}

	public override void Process(RectangleF area, IVectorLayer layer, ICaptchaContext context)
	{
		var vectorObject = new VectorObject();

		int count = Count;

		for (var i = 0; vectorObject.Paths.Count < count; i++)
		{
			float radiusX = RadiusX;
			float radiusY = RadiusY;
			var randomAreaX = RandomFloat(area.Left, area.Right);
			var randomAreaY = RandomFloat(area.Top, area.Bottom);
			var center = new Vector2(randomAreaX, randomAreaY);
			var path = new VectorPath(new[] { new AddOvalInstruction(center, radiusX, radiusY) });
			vectorObject.Paths.Add(path);
		}

		layer.Objects.Add(vectorObject);
	}
}
