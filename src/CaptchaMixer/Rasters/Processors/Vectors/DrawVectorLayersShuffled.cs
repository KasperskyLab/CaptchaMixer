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
/// Draws vector objects from multiple vector layers in random order
/// while preserving paint settings for each layer.
/// </summary>
public class DrawVectorLayersShuffled : SKBitmapRasterProcessor
{
	/// <summary>
	/// Vector layers' draw information.
	/// </summary>
	public VectorLayerDrawInfo[] Infos { get; }

	/// <param name="infos"><inheritdoc cref="Infos" path="/summary"/></param>
	/// <exception cref="ArgumentException"/>
	public DrawVectorLayersShuffled(params VectorLayerDrawInfo[] infos)
	{
		if (infos.Length == 0)
			throw new ArgumentException("At least one vector layer required", nameof(infos));

		Infos = infos;
	}

	protected override void Process(SKBitmap bitmap, ICaptchaContext context)
	{
		var tuples = Infos
			.ToDictionary(
				i => context.GetVectorLayer(i.Name),
				i => (fill: (FillPaintInfo)i.Fill, stroke: (StrokePaintInfo)i.Stroke))
			.SelectMany(kv => kv
				.Key
				.Objects
				.Select(o => (obj: o, kv.Value.fill, kv.Value.stroke)))
			.ToArray()
			.Shuffle();

		using var canvas = new SKCanvas(bitmap);
		foreach (var (obj, fill, stroke) in tuples)
			canvas.Draw(obj, fill, stroke);
	}
}
