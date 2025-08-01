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
/// Base class for raster processors which use <see cref="SKBitmap"/> for
/// drawing on a layer.
/// </summary>
public abstract class SKBitmapRasterProcessor : IRasterProcessor
{
	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		using var bitmap = layer.ToSKBitmap();
		Process(bitmap, context);
	}

	protected abstract void Process(SKBitmap bitmap, ICaptchaContext context);
}
