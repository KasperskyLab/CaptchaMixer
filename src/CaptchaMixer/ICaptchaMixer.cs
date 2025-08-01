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
/// Captcha mixer (generator).
/// </summary>
public interface ICaptchaMixer
{
	/// <returns>
	/// Captcha image with the specified <paramref name="answer"/>.
	/// </returns>
	CaptchaImage CreateCaptcha(string answer);
}
