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
/// Changes objects' sizes.<br/>
/// If <see cref="Width"/> and <see cref="Height"/> are defined then size is set exactly.<br/>
/// If one of dimensions is <see langword="null"/> then it's set value depends on the value of
/// <see cref="Proportional"/>.
/// </summary>
public class ResizeObjects : BasePointObjectsTransformer
{
	/// <summary>
	/// Object width provider. Shall return an exact width value or <see langword="null"/>
	/// in order to use <see cref="Proportional"/> for calculating width.
	/// </summary>
	public ValueProvider<float?> Width { get; set; } = new ConstValueProvider<float?>(null);

	/// <summary>
	/// Object height provider. Shall return an exact height value or <see langword="null"/>
	/// in order to use <see cref="Proportional"/> for calculating height.
	/// </summary>
	public ValueProvider<float?> Height { get; set; } = new ConstValueProvider<float?>(null);

	/// <summary>
	/// Perform proportional resize if one of dimension providers (<see cref="Width"/> or
	/// <see cref="Height"/>> returns <see langword="null"/>.
	/// </summary>
	public ValueProvider<bool> Proportional { get; set; } = true;

	public ResizeObjects()
		: this(null, null, BasePointType.ObjectCenter)
	{ }

	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	/// <param name="height"><inheritdoc cref="Height" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	public ResizeObjects(
		ValueProvider<float?> width,
		ValueProvider<float?> height,
		ValueProvider<BasePointType> basePoint = null)
		: base(basePoint ?? BasePointType.ObjectCenter)
	{
		if (width != null) Width = width;
		if (height != null) Height = height;
	}

	protected override VectorObject Transform(VectorObject obj, Vector2 basePoint)
	{
		float? width = Width;
		float? height = Height;
		if (width == null && height == null)
			return obj;

		var bounds = obj.GetBounds();
		float scaleX, scaleY;

		if (width == null)
		{
			scaleY = bounds.Height != 0 ? height.Value / bounds.Height : 0;
			scaleX = Proportional ? scaleY : 1;
		}
		else if (height == null)
		{
			scaleX = bounds.Width != 0 ? width.Value / bounds.Width : 0;
			scaleY = Proportional ? scaleX : 1;
		}
		else
		{
			scaleX = bounds.Width != 0 ? width.Value / bounds.Width : 0;
			scaleY = bounds.Height != 0 ? height.Value / bounds.Height : 0;
		}

		return obj.Scale(basePoint, scaleX, scaleY);
	}
}
