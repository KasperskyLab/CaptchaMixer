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
/// Scales objects.
/// </summary>
public class ScaleObjects : BasePointObjectsTransformer
{
	/// <summary>
	/// Scale factor along X axis.
	/// </summary>
	public ValueProvider<float> ScaleX { get; set; } = 1;

	/// <summary>
	/// Scale factor along Y axis.
	/// </summary>
	public ValueProvider<float> ScaleY { get; set; } = 1;

	/// <summary>
	/// Sets <see cref="ScaleX"/> and <see cref="ScaleY"/>.
	/// </summary>
	public ValueProvider<float> Scale
	{
		set
		{
			ScaleX = value;
			ScaleY = value;
		}
	}

	public ScaleObjects()
		: base(BasePointType.ObjectCenter) { }

	/// <param name="scale"><inheritdoc cref="Scale" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public ScaleObjects(
		ValueProvider<float> scale,
		ValueProvider<BasePointType> basePoint = null)
		: this(scale, scale, basePoint) { }

	/// <param name="scaleX"><inheritdoc cref="ScaleX" path="/summary"/></param>
	/// <param name="scaleY"><inheritdoc cref="ScaleY" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public ScaleObjects(
		ValueProvider<float> scaleX,
		ValueProvider<float> scaleY,
		ValueProvider<BasePointType> basePoint)
		: base(basePoint ?? BasePointType.ObjectCenter)
	{
		ScaleX = scaleX ?? throw new ArgumentNullException(nameof(scaleX));
		ScaleY = scaleY ?? throw new ArgumentNullException(nameof(scaleY));
	}

	protected override VectorObject Transform(VectorObject obj, Vector2 basePoint)
		=> obj.Scale(basePoint, ScaleX, ScaleY);
}
