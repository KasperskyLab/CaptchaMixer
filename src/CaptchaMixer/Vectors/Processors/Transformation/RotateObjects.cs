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
/// Rotates objects relative to <see cref="BasePointObjectsTransformer.BasePoint"/> on
/// <see cref="Angle"/> degrees clockwise.
/// </summary>
public class RotateObjects : BasePointObjectsTransformer
{
	/// <summary>
	/// Rotation angle in degrees.
	/// </summary>
	public ValueProvider<float> Angle { get; set; } = RandomMirroredFloat(90);

	public RotateObjects()
		: base(BasePointType.ObjectCenter) { }

	/// <param name="angle"><inheritdoc cref="Angle" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public RotateObjects(
		ValueProvider<float> angle,
		ValueProvider<BasePointType> basePoint = null)
		: base(basePoint ?? BasePointType.ObjectCenter)
		=> Angle = angle ?? throw new ArgumentNullException(nameof(angle));

	protected override VectorObject Transform(VectorObject obj, Vector2 basePoint)
		=> obj.Rotate(basePoint, Angle);
}
