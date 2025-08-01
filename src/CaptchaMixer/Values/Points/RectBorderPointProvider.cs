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
/// Generates random points placed on the boundaries of a rectangle.
/// </summary>
public class RectBorderPointProvider : ValueProvider<Vector2>
{
	/// <summary>
	/// Rectangle provider.
	/// </summary>
	public ValueProvider<RectangleF> Rect { get; set; }

	/// <summary>
	/// Rectangle border to generate the points on.
	/// </summary>
	public ValueProvider<RectBorder> Border { get; set; }

	/// <param name="rect"><inheritdoc cref="Rect" path="/summary"/></param>
	/// <param name="border"><inheritdoc cref="Border" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public RectBorderPointProvider(
		ValueProvider<RectangleF> rect,
		ValueProvider<RectBorder> border)
	{
		Rect = rect ?? throw new ArgumentNullException(nameof(rect));
		Border = border ?? throw new ArgumentNullException(nameof(border));
	}

	/// <param name="rect"><inheritdoc cref="Rect" path="/summary"/></param>
	/// <param name="border"><inheritdoc cref="Border" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public RectBorderPointProvider(
		ValueProvider<Size> rect,
		ValueProvider<RectBorder> border)
		: this(rect.Convert(GeometryExtensions.ToRectangleF), border) { }

	public override Vector2 GetNext()
	{
		RectangleF rect = Rect;
		RectBorder border = Border;
		return border switch
		{
			RectBorder.Left => new(rect.Left, RandomFloat(rect.Top, rect.Bottom)),
			RectBorder.Top => new(RandomFloat(rect.Left, rect.Right), rect.Top),
			RectBorder.Right => new(rect.Right, RandomFloat(rect.Top, rect.Bottom)),
			RectBorder.Bottom => new(RandomFloat(rect.Left, rect.Right), rect.Bottom),
			_ => throw new ArgumentOutOfRangeException(nameof(border))
		};
	}
}
