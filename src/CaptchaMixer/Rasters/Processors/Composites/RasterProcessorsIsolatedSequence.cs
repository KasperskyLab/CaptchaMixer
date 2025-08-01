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
/// Raster processors sequence which operates on an empty temporary layer. After all processors
/// finish work, their temporary layer gets applied to the initially processed layer.
/// </summary>
/// <remarks>
/// This basically implements sub-layer logic which simplifies templates programming. If a sub-layer
/// is only applied onto one layer then you don't need to describe it as a whole separate layer and
/// then use a raster mixer processor to combine them - just use an isolator.<br />
/// If you use any constructor without specification of layers mixing method then the
/// <see cref="DrawRasterLayer"/> is used by default.
/// </remarks>
public class RasterProcessorsIsolatedSequence : RasterProcessorsSequence
{
	private static readonly Func<byte[], RasterLayerMixer> DefaultMixerFactory = d => new DrawRasterLayer(d);

	private readonly Func<byte[], RasterLayerMixer> _mixerFactory;

	/// <param name="processors">Processors sequence.</param>
	public RasterProcessorsIsolatedSequence(
		params IRasterProcessor[] processors)
		: this(null, processors as IEnumerable<IRasterProcessor>) { }

	/// <inheritdoc cref="RasterProcessorsIsolatedSequence(IRasterProcessor[])"/>
	public RasterProcessorsIsolatedSequence(
		IEnumerable<IRasterProcessor> processors)
		: this(null, processors) { }

	/// <param name="mixerFactory">Layers mixer factory.</param>
	/// <inheritdoc cref="RasterProcessorsIsolatedSequence(IRasterProcessor[])"/>
	public RasterProcessorsIsolatedSequence(
		Func<byte[], RasterLayerMixer> mixerFactory,
		params IRasterProcessor[] processors)
		: this(mixerFactory, processors as IEnumerable<IRasterProcessor>) { }

	/// <inheritdoc cref="RasterProcessorsIsolatedSequence(Func{byte[], RasterLayerMixer}, IRasterProcessor[])"/>
	public RasterProcessorsIsolatedSequence(
		Func<byte[], RasterLayerMixer> mixerFactory,
		IEnumerable<IRasterProcessor> processors)
		: base(processors)
		=> _mixerFactory = mixerFactory ?? DefaultMixerFactory;

	public override void Process(IRasterLayer layer, ICaptchaContext context)
	{
		var subLayer = new RasterLayer($"{layer.Name}_sub", context.Parameters.Size);
		base.Process(subLayer, context);
		_mixerFactory(subLayer.Data).Process(layer, context);
	}
}
