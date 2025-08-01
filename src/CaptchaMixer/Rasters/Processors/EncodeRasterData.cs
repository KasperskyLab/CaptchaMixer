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
/// Encodes raw raster data thus preparing it for export. No further raster
/// manipulations can be performed, this processor must be the last in sequence.
/// </summary>
[DebugBuilderSkip]
public class EncodeRasterData : IRasterProcessor
{
	/// <summary>
	/// Image format.
	/// </summary>
	private readonly CaptchaImageFormat _format;

	/// <param name="format">
	/// <inheritdoc cref="_format" path="/summary"/>
	/// </param>
	public EncodeRasterData(CaptchaImageFormat format)
		=> _format = format;

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		if (_format == CaptchaImageFormat.None) return;
		layer.Encode(_format);
	}
}
