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
/// Captcha generation parameters.
/// </summary>
public class CaptchaParameters
{
	/// <summary>
	/// Captcha answer.
	/// </summary>
	public string Answer { get; }

	/// <summary>
	/// Image size.
	/// </summary>
	public Size Size { get; }

	/// <param name="answer"><inheritdoc cref="Answer" path="/summary"/></param>
	/// <param name="width">Image width.</param>
	/// <param name="height">Image height.</param>
	public CaptchaParameters(string answer, int width, int height)
		: this(answer, new Size(width, height)) { }

	/// <param name="answer"><inheritdoc cref="Answer" path="/summary"/></param>
	/// <param name="size"><inheritdoc cref="Size" path="/summary"/></param>
	public CaptchaParameters(string answer, Size size)
	{
		Answer = answer;
		Size = size;
	}
}
