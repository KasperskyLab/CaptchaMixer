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
/// Base class for processors which "populate" objects over the layer.
/// </summary>
public abstract class PopulateObjects : IVectorProcessor
{
	/// <summary>
	/// Distance between objects.
	/// </summary>
	public ValueProvider<float> Spacing { get; set; } = 0;

	/// <summary>
	/// Offset from area start.
	/// </summary>
	public ValueProvider<float> PaddingStart { get; set; } = 0;

	/// <summary>
	/// Offset from area end.
	/// </summary>
	public ValueProvider<float> PaddingEnd { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="PaddingStart"/> and <see cref="PaddingEnd"/>.
	/// </summary>
	public ValueProvider<float> Padding
	{
		set
		{
			PaddingStart = value;
			PaddingEnd = value;
		}
	}

	protected PopulateObjects() { }

	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected PopulateObjects(ValueProvider<float> spacing)
		=> Spacing = spacing ?? throw new ArgumentNullException(nameof(spacing));

	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <param name="padding"><inheritdoc cref="Padding" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected PopulateObjects(
		ValueProvider<float> spacing,
		ValueProvider<float> padding)
		: this(spacing, padding, padding) { }

	/// <param name="spacing"><inheritdoc cref="Spacing" path="/summary"/></param>
	/// <param name="paddingStart"><inheritdoc cref="PaddingStart" path="/summary"/></param>
	/// <param name="paddingEnd"><inheritdoc cref="PaddingEnd" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected PopulateObjects(
		ValueProvider<float> spacing,
		ValueProvider<float> paddingStart,
		ValueProvider<float> paddingEnd)
		: this(spacing)
	{
		PaddingStart = paddingStart ?? throw new ArgumentNullException(nameof(paddingStart));
		PaddingEnd = paddingEnd ?? throw new ArgumentNullException(nameof(paddingEnd));
	}

	public void Process(IVectorLayer layer, ICaptchaContext context)
	{
		var objects = layer.Objects;

		if (objects.Count == 0)
			return;

		float spacing = Spacing;
		float paddingStart = PaddingStart;
		float paddingEnd = PaddingEnd;

		var areaStart = paddingStart;
		var areaEnd = GetContextLength(context) - paddingEnd;
		var areaLength = areaEnd - areaStart;
		var contentLength = areaLength - spacing * (objects.Count - 1);
		var bounds = objects.Select(o => o.GetBounds()).ToList();
		// an object may have zero width and/or height but we can't take that as is
		var lengths = bounds.Select(b => Math.Max(GetBoundsLength(b), 1)).ToList();
		var sumLength = lengths.Sum();
		float position = areaStart;
		for (var i = 0; i < objects.Count; i++)
		{
			var newLength = lengths[i] * contentLength / sumLength;
			var scale = newLength / lengths[i];
			ScaleObject(objects[i], bounds[i].GetAnchorPoint(RectAnchor.LeftTop), scale);
			MoveObject(objects[i], position - GetBoundsStart(bounds[i]));
			position += newLength + spacing;
		}
	}

	protected abstract int GetContextLength(ICaptchaContext context);
	protected abstract float GetBoundsLength(RectangleF bounds);
	protected abstract float GetBoundsStart(RectangleF bounds);
	protected abstract void MoveObject(VectorObject obj, float distance);
	protected abstract void ScaleObject(VectorObject obj, Vector2 basePoint, float scale);
}
