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
/// Base class for processors which justify objects over the layer.
/// </summary>
public abstract class JustifyObjects : IVectorProcessor
{
	/// <summary>
	/// Area start padding.
	/// </summary>
	public ValueProvider<float> PaddingStart { get; set; } = 0;

	/// <summary>
	/// Area end offset.
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

	protected JustifyObjects() { }

	/// <param name="padding"><inheritdoc cref="Padding" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected JustifyObjects(
		ValueProvider<float> padding)
		: this(padding, padding) { }

	/// <param name="paddingStart"><inheritdoc cref="PaddingStart" path="/summary"/></param>
	/// <param name="paddingEnd"><inheritdoc cref="PaddingEnd" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected JustifyObjects(
		ValueProvider<float> paddingStart,
		ValueProvider<float> paddingEnd)
	{
		PaddingStart = paddingStart ?? throw new ArgumentNullException(nameof(paddingStart));
		PaddingEnd = paddingEnd ?? throw new ArgumentNullException(nameof(paddingEnd));
	}

	public void Process(IVectorLayer layer, ICaptchaContext context)
	{
		var objects = layer.Objects;

		if (objects.Count == 0)
			return;

		float paddingStart = PaddingStart;
		float paddingEnd = PaddingEnd;

		var areaStart = paddingStart;

		if (objects.Count == 1)
		{
			MoveObject(objects[0], areaStart - objects[0].GetBounds().Left);
			return;
		}

		var areaEnd = GetContextLength(context) - paddingEnd;
		var areaLength = areaEnd - areaStart;
		var bounds = objects.Select(o => o.GetBounds()).ToList();
		// an object may have zero width and/or height but we can't take that as is
		var space = (areaLength - bounds.Sum(b => Math.Max(GetBoundsLength(b), 1))) / (objects.Count - 1);
		float position = areaStart;
		for (var i = 0; i < objects.Count; i++)
		{
			MoveObject(objects[i], position - GetBoundsStart(bounds[i]));
			position += Math.Max(GetBoundsLength(bounds[i]), 1) + space;
		}
	}

	protected abstract int GetContextLength(ICaptchaContext context);
	protected abstract float GetBoundsLength(RectangleF bounds);
	protected abstract float GetBoundsStart(RectangleF bounds);
	protected abstract void MoveObject(VectorObject obj, float distance);
}
