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
/// A complete captcha image.
/// </summary>
public class CaptchaImage
{
	/// <summary>
	/// Image binary data.
	/// </summary>
	public byte[] Data { get; }

	/// <summary>
	/// Image size.
	/// </summary>
	public Size Size { get; }

	/// <param name="data"><inheritdoc cref="Data" path="/summary"/></param>
	/// <param name="width">Image width.</param>
	/// <param name="height">Image height.</param>
	public CaptchaImage(byte[] data, int width, int height)
		: this(data, new Size(width, height)) { }

	/// <param name="data"><inheritdoc cref="Data" path="/summary"/></param>
	/// <param name="size"><inheritdoc cref="Size" path="/summary"/></param>
	public CaptchaImage(byte[] data, Size size)
	{
		Data = data;
		Size = size;
	}
}
