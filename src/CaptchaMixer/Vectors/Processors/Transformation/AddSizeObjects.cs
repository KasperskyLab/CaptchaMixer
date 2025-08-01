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
/// Increases/decreases objects size by given value.
/// </summary>
public class AddSizeObjects : BasePointObjectsTransformer
{
	/// <summary>
	/// How much to add to width.
	/// </summary>
	public ValueProvider<float> AddWidth { get; set; } = 0;

	/// <summary>
	/// How much to add to height.
	/// </summary>
	public ValueProvider<float> AddHeight { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="AddWidth"/> and <see cref="AddHeight"/>.
	/// </summary>
	public ValueProvider<float> Add
	{
		set
		{
			AddWidth = value;
			AddHeight = value;
		}
	}

	public AddSizeObjects()
		: base(BasePointType.ObjectCenter) { }

	/// <param name="add"><inheritdoc cref="Add" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public AddSizeObjects(
		ValueProvider<float> add,
		ValueProvider<BasePointType> basePoint = null)
		: this(add, add, basePoint) { }

	/// <param name="addWidth"><inheritdoc cref="AddWidth" path="/summary"/></param>
	/// <param name="addHeight"><inheritdoc cref="AddHeight" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public AddSizeObjects(
		ValueProvider<float> addWidth,
		ValueProvider<float> addHeight,
		ValueProvider<BasePointType> basePoint = null)
		: base(basePoint ?? BasePointType.ObjectCenter)
	{
		AddWidth = addWidth ?? throw new ArgumentNullException(nameof(addWidth));
		AddHeight = addHeight ?? throw new ArgumentNullException(nameof(addHeight));
	}

	protected override VectorObject Transform(VectorObject obj, Vector2 basePoint)
	{
		float addWidth = AddWidth;
		float addHeight = AddHeight;
		if (addWidth == 0 && addHeight == 0)
			return obj;

		var bounds = obj.GetBounds();
		var scaleX = (bounds.Width + addWidth) / bounds.Width;
		var scaleY = (bounds.Height + addHeight) / bounds.Height;
		return obj.Scale(basePoint, scaleX, scaleY);
	}
}
