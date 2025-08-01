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
/// Builds captcha mixer instances.
/// </summary>
public class CaptchaMixerBuilder
{
	private const string MasterLayerName = "master";

	private readonly Dictionary<string, List<IVectorProcessor>> _vectorLayers = new();
	private readonly Dictionary<string, List<IRasterProcessor>> _rasterLayers = new();
	private readonly IRasterProcessor _encoder;

	/// <summary>
	/// Image width.
	/// </summary>
	public int Width { get; }

	/// <summary>
	/// Image height.
	/// </summary>
	public int Height { get; }

	/// <summary>
	/// Size built from <see cref="Width"/> and <see cref="Height"/>.
	/// </summary>
	public Size Size { get; }

	/// <param name="width"><inheritdoc cref="Width" path="/summary"/></param>
	/// <param name="height"><inheritdoc cref="Height" path="/summary"/></param>
	/// <param name="encode"><inheritdoc cref="CaptchaImageFormat" path="/summary"/></param>
	/// <exception cref="ArgumentException"/>
	public CaptchaMixerBuilder(
		int width,
		int height,
		CaptchaImageFormat encode = CaptchaImageFormat.Png)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width, nameof(width));
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height, nameof(height));

		Width = width;
		Height = height;
		Size = new Size(width, height);

		_encoder = encode != CaptchaImageFormat.None
			? new EncodeRasterData(encode)
			: null;
	}

	/// <inheritdoc cref="CaptchaMixerBuilder(int, int, CaptchaImageFormat)"/>
	/// <typeparam name="T">Template type.</typeparam>
	/// <returns>A new captcha mixer from template.</returns>
	public static CaptchaMixer FromTemplate<T>(
		int width,
		int height,
		CaptchaImageFormat encode = CaptchaImageFormat.Png)
		where T : ICaptchaMixerTemplate, new()
		=> new CaptchaMixerBuilder(width, height, encode).ApplyTemplate<T>().Build();

	/// <summary>
	/// Applies a template to this builder.
	/// </summary>
	/// <typeparam name="T">Template type.</typeparam>
	/// <param name="configurator">Template configurator.</param>
	/// <returns>The same builder.</returns>
	public CaptchaMixerBuilder ApplyTemplate<T>(Action<T> configurator = null)
		where T : ICaptchaMixerTemplate, new()
	{
		var template = new T();
		configurator?.Invoke(template);
		template.ApplyTo(this);
		return this;
	}

	private static List<T> GetProcessorsList<T>(string layerName, Dictionary<string, List<T>> dict)
	{
		ArgumentNullException.ThrowIfNull(layerName, nameof(layerName));

		if (dict.TryGetValue(layerName, out var list)) return list;

		list = new List<T>();
		dict[layerName] = list;
		return list;
	}

	private CaptchaMixerBuilder AddProcessors<T>(
		string layerName,
		T[] processors,
		Dictionary<string, List<T>> dict)
	{
		if (processors.Any(p => p == null))
			throw new ArgumentNullException(nameof(processors), "Processors array cannot contain null values");

		GetProcessorsList(layerName, dict).AddRange(processors);
		return this;
	}

	private CaptchaMixerBuilder AddRasterProcessorsImpl(string layerName, IRasterProcessor[] processors)
	{
		if (processors.Any(p => p is EncodeRasterData))
			throw new InvalidOperationException($"{nameof(EncodeRasterData)} processor cannot be added manually, use constructor 'encode' argument instead");

		return AddProcessors(layerName, processors, _rasterLayers);
	}

	/// <summary>
	/// Adds processors to the specified vector layer.
	/// </summary>
	/// <exception cref="ArgumentNullException"/>
	public CaptchaMixerBuilder AddVectorProcessors(string layerName, params IVectorProcessor[] processors)
		=> AddProcessors(layerName, processors, _vectorLayers);

	/// <inheritdoc cref="AddVectorProcessors(string, IVectorProcessor[])"/>
	public CaptchaMixerBuilder AddVectorProcessors(string layerName, IEnumerable<IVectorProcessor> processors)
		=> AddProcessors(layerName, processors.ToArray(), _vectorLayers);

	/// <summary>
	/// Adds processors to the specified raster layer.
	/// </summary>
	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="InvalidOperationException"/>
	public CaptchaMixerBuilder AddRasterProcessors(string layerName, params IRasterProcessor[] processors)
		=> AddRasterProcessorsImpl(layerName, processors);

	/// <inheritdoc cref="AddRasterProcessors(string, IRasterProcessor[])"/>
	public CaptchaMixerBuilder AddRasterProcessors(string layerName, IEnumerable<IRasterProcessor> processors)
		=> AddRasterProcessorsImpl(layerName, processors.ToArray());

	/// <summary>
	/// Adds processors to the master raster layer.
	/// </summary>
	/// <exception cref="InvalidOperationException"/>
	public CaptchaMixerBuilder AddMasterProcessors(params IRasterProcessor[] processors)
		=> AddRasterProcessors(MasterLayerName, processors);

	/// <inheritdoc cref="AddMasterProcessors(IRasterProcessor[])"/>
	public CaptchaMixerBuilder AddMasterProcessors(IEnumerable<IRasterProcessor> processors)
		=> AddRasterProcessors(MasterLayerName, processors);

	/// <returns>A new captcha mixer.</returns>
	/// <exception cref="ArgumentException"/>
	public CaptchaMixer Build()
	{
		if (!_rasterLayers.ContainsKey(MasterLayerName))
			throw new ArgumentException("Master raster layer required");

		var rasterLayers = _rasterLayers;

		if (_encoder != null)
		{
			// for the builder to be reusable we shall only append the encoder here.
			rasterLayers = new(_rasterLayers);
			var masterProcessors = new List<IRasterProcessor>(rasterLayers[MasterLayerName]) { _encoder };
			rasterLayers[MasterLayerName] = masterProcessors;
		}

		return BuildImpl(_vectorLayers, rasterLayers);
	}

	protected virtual CaptchaMixer BuildImpl(
		IDictionary<string, List<IVectorProcessor>> vectorLayers,
		IDictionary<string, List<IRasterProcessor>> rasterLayers)
	{
		var vectorProcessors = vectorLayers.ToDictionary(t => t.Key, t => (IVectorProcessor)new VectorProcessorsSequence(t.Value));
		var rasterProcessors = rasterLayers.ToDictionary(t => t.Key, t => (IRasterProcessor)new RasterProcessorsSequence(t.Value));
		return new(Width, Height, vectorProcessors, rasterProcessors, MasterLayerName);
	}
}
