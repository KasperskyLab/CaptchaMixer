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
/// Base class for transformers which use a base point.
/// </summary>
public abstract class BasePointObjectsTransformer : OneByOneVectorObjectsProcessor
{
	/// <summary>
	/// Type of base point.
	/// </summary>
	public ValueProvider<BasePointType> BasePoint { get; set; }

	/// <summary>
	/// <see cref="BasePoint"/> deviation along X axis.
	/// </summary>
	public ValueProvider<float> BasePointOffsetX { get; set; } = 0;

	/// <summary>
	/// <see cref="BasePoint"/> deviation along Y axis.
	/// </summary>
	public ValueProvider<float> BasePointOffsetY { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="BasePointOffsetX"/> and <see cref="BasePointOffsetY"/>.
	/// </summary>
	public ValueProvider<float> BasePointOffset
	{
		set
		{
			BasePointOffsetX = value;
			BasePointOffsetY = value;
		}
	}

	protected BasePointObjectsTransformer(ValueProvider<BasePointType> basePoint)
		=> BasePoint = basePoint ?? throw new ArgumentNullException(nameof(basePoint));

	protected override IEnumerable<VectorObject> Process(VectorObject obj, ICaptchaContext context)
	{
		BasePointType basePointType = BasePoint;
		Vector2 basePoint;

		if (basePointType.TryGetLayerAnchor(out var anchor))
			basePoint = context.Parameters.Size.ToRectangleF().GetAnchorPoint(anchor);
		else if (basePointType.TryGetObjectAnchor(out anchor))
			basePoint = obj.GetBounds().GetAnchorPoint(anchor);
		else
			throw new InvalidOperationException($"Unsupported {nameof(BasePoint)} type: {basePointType}");

		basePoint.X += BasePointOffsetX;
		basePoint.Y += BasePointOffsetY;

		yield return Transform(obj, basePoint);
	}

	protected abstract VectorObject Transform(VectorObject obj, Vector2 basePoint);
}
