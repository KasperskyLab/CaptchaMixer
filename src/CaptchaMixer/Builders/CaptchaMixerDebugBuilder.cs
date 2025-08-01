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

using System.Reflection;

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// Debug captcha mixer builder. Wraps all root processors of all layers in proxies
/// which save the current image into PNG files.
/// </summary>
public class CaptchaMixerDebugBuilder : CaptchaMixerBuilder
{
	private readonly string _debugDir;
	private readonly int _debugScale;
	private readonly bool _clear;

	/// <inheritdoc cref="CaptchaMixerBuilder(int, int, CaptchaImageFormat)"/>
	/// <param name="debugDir">Debug PNG files directory.</param>
	/// <param name="debugScale">Images scale.</param>
	/// <param name="clear">Remove all debug images in <paramref name="debugDir"/> before new captcha generation.</param>
	public CaptchaMixerDebugBuilder(
		int width,
		int height,
		string debugDir,
		int debugScale = 10,
		bool clear = true,
		CaptchaImageFormat encode = CaptchaImageFormat.Png)
		: base(width, height, encode)
	{
		Directory.CreateDirectory(debugDir);
		_debugDir = debugDir;
		_debugScale = debugScale;
		_clear = clear;
	}

	/// <inheritdoc cref="CaptchaMixerDebugBuilder(int, int, string, int, bool, CaptchaImageFormat)"/>
	/// <summary>Creates a builder and applies a template to it.</summary>
	/// <typeparam name="T">Template type.</typeparam>
	public static CaptchaMixer FromTemplate<T>(
		int width,
		int height,
		string debugDir,
		int debugScale = 10,
		bool clear = true,
		CaptchaImageFormat encode = CaptchaImageFormat.Png)
		where T : ICaptchaMixerTemplate, new()
		=> new CaptchaMixerDebugBuilder(width, height, debugDir, debugScale, clear, encode).ApplyTemplate<T>().Build();

	private string GetDebugFilePathBase(
		string layerType,
		int layerIndex,
		string layerName,
		int processorIndex,
		object processor)
		=> Path.Combine(_debugDir, $"{layerType}{layerIndex:00}_{layerName}_{processorIndex:00}_{processor.GetType().Name}");

	private Dictionary<string, List<T>> Wrap<T>(
		string layerType,
		IDictionary<string, List<T>> layers,
		Func<T, string, int, T> wrapper)
		=> layers
			.Select((kv, i) => (kv, layerIndex: i))
			.ToDictionary(
				tuple => tuple.kv.Key,
				tuple => tuple.kv.Value
					.Select((processor, processorIndex) =>
					{
						if (processor.GetType().GetCustomAttribute<DebugBuilderSkipAttribute>() != null) return processor;
						var filePathBase = GetDebugFilePathBase(layerType, tuple.layerIndex, tuple.kv.Key, processorIndex, processor);
						return wrapper(processor, filePathBase, _debugScale);
					})
					.ToList());

	protected override CaptchaMixer BuildImpl(
		IDictionary<string, List<IVectorProcessor>> vectorLayers,
		IDictionary<string, List<IRasterProcessor>> rasterLayers)
	{
		if (_clear)
			foreach (var layerType in new[] { "0_Vector", "1_Raster" })
				Directory.EnumerateFiles(_debugDir, $"{layerType}??_*_??_*.png").ToList().ForEach(File.Delete);

		return base.BuildImpl(
			Wrap("0_Vector", vectorLayers, (p, f, s) => p.WithPngExport(f, false, s)),
			Wrap("1_Raster", rasterLayers, (p, f, s) => p.WithPngExport(f, false, s)));
	}
}
