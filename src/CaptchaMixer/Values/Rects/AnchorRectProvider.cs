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
/// Generates rectangles from anchor point, width and height.
/// </summary>
public class AnchorRectProvider : ValueProvider<RectangleF>
{
	/// <summary>
	/// Type of <see cref="AnchorPoint"/>.
	/// </summary>
	public ValueProvider<RectAnchor> AnchorType { get; set; } = RectAnchor.LeftTop;

	/// <summary>
	/// Anchor point.
	/// </summary>
	public ValueProvider<Vector2> AnchorPoint { get; set; } = new Vector2(0, 0);

	/// <summary>
	/// Rectangle width.
	/// </summary>
	public ValueProvider<float> Width { get; set; } = RandomFloat(10, 50);

	/// <summary>
	/// Rectangle height.
	/// </summary>
	public ValueProvider<float> Height { get; set; } = RandomFloat(10, 50);

	/// <param name="anchorType"><inheritdoc cref="AnchorType" path="/summary"/></param>
	/// <param name="anchorPoint"><inheritdoc cref="AnchorPoint" path="/summary"/></param>
	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	/// <param name="height"><inheritdoc cref="Height" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public AnchorRectProvider(
		ValueProvider<RectAnchor> anchorType,
		ValueProvider<Vector2> anchorPoint,
		ValueProvider<float> width,
		ValueProvider<float> height)
	{
		AnchorType = anchorType ?? throw new ArgumentNullException(nameof(anchorType));
		AnchorPoint = anchorPoint ?? throw new ArgumentNullException(nameof(anchorPoint));
		Width = width ?? throw new ArgumentNullException(nameof(width));
		Height = height ?? throw new ArgumentNullException(nameof(height));
	}

	public override RectangleF GetNext()
	{
		RectAnchor anchor = AnchorType;
		Vector2 point = AnchorPoint;
		float width = Width;
		float height = Height;

		return anchor switch
		{
			RectAnchor.Center => new(point.X - width / 2, point.Y - height / 2, width, height),
			RectAnchor.LeftTop => new(point.X, point.Y, width, height),
			RectAnchor.CenterTop => new(point.X - width / 2, point.Y, width, height),
			RectAnchor.RightTop => new(point.X - width, point.Y, width, height),
			RectAnchor.RightCenter => new(point.X - width, point.Y - height / 2, width, height),
			RectAnchor.RightBottom => new(point.X - width, point.Y - height, width, height),
			RectAnchor.CenterBottom => new(point.X - width / 2, point.Y - height, width, height),
			RectAnchor.LeftBottom => new(point.X, point.Y - height, width, height),
			RectAnchor.LeftCenter => new(point.X, point.Y - height / 2, width, height),
			_ => throw new ArgumentOutOfRangeException(nameof(anchor)),
		};
	}
}
