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
/// Creates a horizontal segmentation (segments from left to right) with
/// Bezier curves as separators.
/// </summary>
public class CurveSegmentationX : CurveSegmentationCreator
{
	public CurveSegmentationX() { }

	/// <inheritdoc cref="AreaSegmentationCreator(ValueProvider{int})"/>
	public CurveSegmentationX(ValueProvider<int> segments)
		: base(segments) { }

	protected override (Vector2 point1, Vector2 point2) GetLine1(RectangleF area)
		=> (area.GetLeftTop(), area.GetRightTop());

	protected override (Vector2 point1, Vector2 point2) GetLine2(RectangleF area)
		=> (area.GetLeftBottom(), area.GetRightBottom());
}
