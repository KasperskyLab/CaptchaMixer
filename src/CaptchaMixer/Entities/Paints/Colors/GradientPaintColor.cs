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
/// Gradient paint color base class.
/// </summary>
public abstract class GradientPaintColor : PaintColor
{
	/// <summary>
	/// Gradient steps count.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 3;

	/// <summary>
	/// Gradient colors providers. Must sequentially provide <see cref="Count"/> colors.
	/// </summary>
	public ValueProvider<CMColor> Colors { get; set; } = Carousel(CMColor.White, new CMColor(0, 57, 166), new CMColor(213, 43, 30));

	/// <summary>
	/// Gradient color positions from 0 to 1.
	/// </summary>
	public ValueProvider<float> Positions { get; set; } = Carousel(0f, 0.5f, 1f);

	/// <inheritdoc cref="GradientTileMode"/>
	public ValueProvider<GradientTileMode> TileMode { get; set; } = GradientTileMode.Repeat;

	/// <summary>
	/// Rectangle anchor point where the gradient starts.
	/// </summary>
	public ValueProvider<RectAnchor> StartAnchor { get; set; } = RectAnchor.LeftTop;
}
