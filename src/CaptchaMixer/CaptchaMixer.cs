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

/// <inheritdoc cref="ICaptchaMixer"/>
public class CaptchaMixer : ICaptchaMixer
{
	private readonly Size _size;
	private readonly IDictionary<string, IVectorProcessor> _vectorProcessors;
	private readonly IDictionary<string, IRasterProcessor> _rasterProcessors;
	private readonly string _masterLayer;

	/// <param name="width">Image width.</param>
	/// <param name="height">Image height.</param>
	/// <param name="vectorProcessors">Vector processors.</param>
	/// <param name="rasterProcessors">Raster processors.</param>
	/// <param name="masterLayer">Master raster layer name.</param>
	/// <exception cref="ArgumentOutOfRangeException"/>
	/// <exception cref="ArgumentNullException"/>
	public CaptchaMixer(
		int width,
		int height,
		IDictionary<string, IVectorProcessor> vectorProcessors,
		IDictionary<string, IRasterProcessor> rasterProcessors,
		string masterLayer)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width, nameof(width));
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height, nameof(height));

		ArgumentNullException.ThrowIfNull(rasterProcessors, nameof(rasterProcessors));
		ArgumentOutOfRangeException.ThrowIfZero(rasterProcessors.Count, nameof(rasterProcessors));

		ArgumentNullException.ThrowIfNull(masterLayer, nameof(masterLayer));
		if (!rasterProcessors.ContainsKey(masterLayer))
			throw new ArgumentException($"Master raster layer '{_masterLayer}' required");

		_size = new(width, height);
		_vectorProcessors = vectorProcessors;
		_rasterProcessors = rasterProcessors;
		_masterLayer = masterLayer;
	}

	public CaptchaImage CreateCaptcha(string answer)
	{
		var vectorLayers = _vectorProcessors?.ToDictionary(t => t.Key, t => new VectorLayer(t.Key));
		var rasterLayers = _rasterProcessors.ToDictionary(t => t.Key, t => new RasterLayer(t.Key, _size));

		var context = new CaptchaContext(new CaptchaParameters(answer, _size), vectorLayers?.Values, rasterLayers.Values);

		if (_vectorProcessors != null)
			foreach (var kv in _vectorProcessors)
				kv.Value.Process(vectorLayers[kv.Key], context);

		foreach (var kv in _rasterProcessors)
			kv.Value.Process(rasterLayers[kv.Key], context);

		return new(rasterLayers[_masterLayer].Data, _size);
	}
}
