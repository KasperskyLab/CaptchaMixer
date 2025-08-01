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
/// Moves objects for their <see cref="Anchor"/> point to match the
/// <see cref="BasePointObjectsTransformer.BasePoint"/>.
/// </summary>
public class SnapObjects : BasePointObjectsTransformer
{
	/// <summary>
	/// Object's bounds anchor point.
	/// </summary>
	public ValueProvider<RectAnchor> Anchor { get; set; }

	public SnapObjects()
		: this(RectAnchor.LeftTop) { }

	/// <param name="anchor"><inheritdoc cref="Anchor" path="/summary"/></param>
	/// <param name="basePoint"><inheritdoc cref="BasePointObjectsTransformer.BasePoint" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public SnapObjects(
		ValueProvider<RectAnchor> anchor,
		ValueProvider<BasePointType> basePoint = null)
		: base(basePoint ?? BasePointType.LayerLeftTop)
		=> Anchor = anchor ?? throw new ArgumentNullException(nameof(anchor));

	protected override VectorObject Transform(VectorObject obj, Vector2 basePoint)
	{
		var (moveX, moveY) = obj.GetBounds().CalcMoveShift(Anchor, basePoint);
		return obj.Move(moveX, moveY);
	}
}
