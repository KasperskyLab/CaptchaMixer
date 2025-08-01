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
/// Horizontally distributes objects over the layer and changes their widths so that the spacing between
/// objects becomes equal while the proportion of their widths remains unchanged.
/// </summary>
public class PopulateObjectsX : PopulateObjects
{
	public PopulateObjectsX()
		: base() { }

	/// <inheritdoc cref="PopulateObjects(ValueProvider{float})"/>
	public PopulateObjectsX(
		ValueProvider<float> spacing)
		: base(spacing) { }

	/// <inheritdoc cref="PopulateObjects(ValueProvider{float},ValueProvider{float})"/>
	public PopulateObjectsX(
		ValueProvider<float> spacing,
		ValueProvider<float> padding)
		: base(spacing, padding) { }

	/// <inheritdoc cref="PopulateObjects(ValueProvider{float},ValueProvider{float},ValueProvider{float})"/>
	public PopulateObjectsX(
		ValueProvider<float> spacing,
		ValueProvider<float> paddingStart,
		ValueProvider<float> paddingEnd)
		: base(spacing, paddingStart, paddingEnd) { }

	protected override int GetContextLength(ICaptchaContext context)
		=> context.Parameters.Size.Width;

	protected override float GetBoundsLength(RectangleF bounds)
		=> bounds.Width;

	protected override float GetBoundsStart(RectangleF bounds)
		=> bounds.Left;

	protected override void MoveObject(VectorObject obj, float distance)
		=> obj.MoveX(distance);

	protected override void ScaleObject(VectorObject obj, Vector2 basePoint, float scale)
		=> obj.ScaleX(basePoint, scale);
}
