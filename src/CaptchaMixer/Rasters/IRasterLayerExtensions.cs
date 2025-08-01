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

public static class IRasterLayerExtensions
{
	/// <summary>
	/// Converts <paramref name="layer"/> into <see cref="SKBitmap"/>.
	/// </summary>
	public static SKBitmap ToSKBitmap(this IRasterLayer layer)
	{
		var bitmap = new SKBitmap();
		bitmap.AttachToBytes(layer.Data, layer.Size);
		return bitmap;
	}

	/// <summary>
	/// Replaces raw raster data with format-encoded data.
	/// </summary>
	public static void Encode(
		this IRasterLayer layer,
		CaptchaImageFormat format = CaptchaImageFormat.Png)
	{
		using var bitmap = layer.ToSKBitmap();
		layer.Data = bitmap.Encode(format);
	}

	/// <summary>
	/// Pixels processing delegate.
	/// </summary>
	/// <param name="color">Pixel color.</param>
	/// <param name="x">Pixel X coordinate.</param>
	/// <param name="y">Pixel Y coordinate.</param>
	public delegate void PixelProcessor(ref CMColor color, int x, int y);

	/// <summary>
	/// Process layer pixels.
	/// </summary>
	/// <remarks>
	/// This method using <see cref="PixelProcessor"/> is 15-20% faster than the
	/// Action-based <see cref="ProcessPixels(IRasterLayer, Action{CMColor, int, int})"/>.
	/// </remarks>
	public static void ProcessPixels(
		this IRasterLayer layer,
		PixelProcessor processor)
	{
		var data = layer.Data;
		var width = layer.Size.Width;
		var color = new CMColor();
		var length = data.Length;
		for (var i = 0; i < length; i += 4)
		{
			color.Attach(data, i);
			processor(ref color, i / 4 % width, i / 4 / width);
		}
	}

	/// <summary>
	/// Process layer pixels.
	/// </summary>
	/// <remarks>
	/// This method using <see cref="Action{T1, T2, T3}"/> is 15-20% slower than the
	/// delegate-based <see cref="ProcessPixels(IRasterLayer, PixelProcessor)"/>.
	/// </remarks>
	public static void ProcessPixels(
		this IRasterLayer layer,
		Action<CMColor, int, int> processor)
	{
		var data = layer.Data;
		var width = layer.Size.Width;
		var color = new CMColor();
		var length = data.Length;
		for (var i = 0; i < length; i += 4)
		{
			color.Attach(data, i);
			processor(color, i / 4 % width, i / 4 / width);
		}
	}

	/// <returns>
	/// Color of pixel (<paramref name="x"/>, <paramref name="y"/>) of <paramref name="layer"/>.
	/// </returns>
	/// <remarks>
	/// Do not use this method if you need to process all raster's pixels, use extension methods
	/// <see cref="ProcessPixels(IRasterLayer, PixelProcessor)"/> or
	/// <see cref="ProcessPixels(IRasterLayer, Action{CMColor, int, int})"/> instead.
	/// Pixel by pixel reading with this method is less performant.
	/// </remarks>
	public static CMColor GetPixel(this IRasterLayer layer, int x, int y)
		=> new(layer, x, y);
}
