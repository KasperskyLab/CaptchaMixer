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

/// <inheritdoc cref="ICaptchaContext"/>
public class CaptchaContext : ICaptchaContext
{
	private readonly IDictionary<string, IVectorLayer> _vectorLayers;
	private readonly IDictionary<string, IRasterLayer> _rasterLayers;

	public CaptchaParameters Parameters { get; }
	public IVectorLayer GetVectorLayer(string name) => _vectorLayers?[name];
	public IRasterLayer GetRasterLayer(string name) => _rasterLayers[name];

	/// <param name="parameters"><inheritdoc cref="Parameters" path="/summary"/></param>
	/// <param name="vectorLayers">Vector layers.</param>
	/// <param name="rasterLayers">Raster layers.</param>
	/// <exception cref="ArgumentNullException"/>
	public CaptchaContext(
		CaptchaParameters parameters,
		IEnumerable<IVectorLayer> vectorLayers,
		IEnumerable<IRasterLayer> rasterLayers)
	{
		Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
		_vectorLayers = vectorLayers?.ToDictionary(l => l.Name, l => l);
		_rasterLayers = rasterLayers.ToDictionary(l => l.Name, l => l);
	}
}
