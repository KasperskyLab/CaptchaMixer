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
/// Saves raster layer data into PNG files before and/or after processing.
/// </summary>
public class PngExportingRasterProcessorProxy : IRasterProcessor
{
	private readonly IRasterProcessor _processor;
	private readonly string _filePathBase;
	private readonly bool _saveBefore;
	private readonly int _scale;

	/// <param name="processor">Wrapped raster processor.</param>
	/// <param name="filePathBase">PNG files path and name base.</param>
	/// <param name="saveBefore">Save an image before processing.</param>
	/// <param name="scale">Image scale factor.</param>
	public PngExportingRasterProcessorProxy(
		IRasterProcessor processor,
		string filePathBase,
		bool saveBefore = true,
		int scale = 10)
	{
		_processor = processor;
		_filePathBase = filePathBase;
		_saveBefore = saveBefore;
		_scale = scale;
	}

	private void Export(IRasterLayer layer, ICaptchaContext context, string filePath)
	{
		var size = context.Parameters.Size;
		using var original = layer.ToSKBitmap();
		using var scaled = new SKBitmap(new SKImageInfo(size.Width * _scale, size.Height * _scale, SKColorType.Rgba8888));
		using var canvas = new SKCanvas(scaled);
		canvas.DrawBitmap(original, scaled.Info.Rect);
		scaled.Export(filePath);
	}

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		if (_saveBefore) Export(layer, context, _filePathBase + "_0.png");
		_processor.Process(layer, context);
		Export(layer, context, _filePathBase + "_1.png");
	}
}
