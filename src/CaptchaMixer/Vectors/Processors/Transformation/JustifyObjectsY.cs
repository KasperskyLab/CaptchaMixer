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
/// Justifies objects along Y axis.
/// </summary>
/// <remarks>
/// If the total height ob objects exceeds area height then the result may be inadequate.
/// </remarks>
public class JustifyObjectsY : JustifyObjects
{
	public JustifyObjectsY()
		: base() { }

	/// <inheritdoc cref="JustifyObjects(ValueProvider{float})"/>
	public JustifyObjectsY(
		ValueProvider<float> padding)
		: base(padding) { }

	/// <inheritdoc cref="JustifyObjects(ValueProvider{float},ValueProvider{float})"/>
	public JustifyObjectsY(
		ValueProvider<float> paddingStart,
		ValueProvider<float> paddingEnd)
		: base(paddingStart, paddingEnd) { }

	protected override int GetContextLength(ICaptchaContext context)
		=> context.Parameters.Size.Height;

	protected override float GetBoundsLength(RectangleF bounds)
		=> bounds.Height;

	protected override float GetBoundsStart(RectangleF bounds)
		=> bounds.Top;

	protected override void MoveObject(VectorObject obj, float distance)
		=> obj.MoveY(distance);
}
