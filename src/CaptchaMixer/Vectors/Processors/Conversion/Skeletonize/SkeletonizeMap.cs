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

using Pixel = Kaspersky.CaptchaMixer.SkeletonizePixel;

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// Pixels map for skeletonization algorithm.
/// </summary>
internal class SkeletonizeMap : IEnumerable<Pixel>
{
	private readonly int _width;
	private readonly int _height;
	private readonly Pixel[] _pixels;
	private readonly Dictionary<(int x, int y), Pixel> _beyonds = new();

	/// <returns>Pixel state by its coordinates.</returns>
	internal Pixel this[int x, int y]
		=> _pixels[y * _width + x];

	internal SkeletonizeMap(RasterLayer layer)
	{
		_width = layer.Size.Width;
		_height = layer.Size.Height;

		_pixels = new Pixel[_width * _height];
		var data = layer.Data;
		var length = data.Length;

		for (var i = 0; i < length; i += 4)
		{
			var index = i / 4;
			var x = index % _width;
			var y = index / _width;
			_pixels[index] = new Pixel(x, y, data[i] > 0);
		}

		for (var i = 0; i < _pixels.Length; i++)
		{
			var x = i % _width;
			var y = i / _width;
			_pixels[i].Init(
				GetPixelOrBeyond(x - 1, y - 1),
				GetPixelOrBeyond(x, y - 1),
				GetPixelOrBeyond(x + 1, y - 1),
				GetPixelOrBeyond(x + 1, y),
				GetPixelOrBeyond(x + 1, y + 1),
				GetPixelOrBeyond(x, y + 1),
				GetPixelOrBeyond(x - 1, y + 1),
				GetPixelOrBeyond(x - 1, y));
		}
	}

	private Pixel GetPixelOrBeyond(int x, int y)
		=> x < 0 || y < 0 || x >= _width || y >= _height
			? GetBeyond(x, y)
			: this[x, y];

	private Pixel GetBeyond(int x, int y)
	{
		var key = (x, y);
		if (_beyonds.TryGetValue(key, out var pixel)) return pixel;
		pixel = new Pixel(x, y, false);
		_beyonds[key] = pixel;
		return pixel;
	}

	IEnumerator<Pixel> IEnumerable<Pixel>.GetEnumerator()
		=> ((IEnumerable<Pixel>)_pixels).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> _pixels.GetEnumerator();
}
