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

public static class IRasterProcessorExtensions
{
	/// <inheritdoc cref="PngExportingRasterProcessorProxy(IRasterProcessor, string, bool, int)"/>
	/// <returns>
	/// <paramref name="processor"/> wrapped into a <see cref="PngExportingRasterProcessorProxy"/>.
	/// </returns>
	public static IRasterProcessor WithPngExport(
		this IRasterProcessor processor,
		string filePathBase,
		bool saveBefore = true,
		int scale = 10)
		=> new PngExportingRasterProcessorProxy(processor, filePathBase, saveBefore, scale);

	/// <inheritdoc cref="ProbabilityRasterProcessorProxy(IRasterProcessor, double)"/>
	/// <returns>
	/// <paramref name="processor"/> wrapped into a <see cref="ProbabilityRasterProcessorProxy"/>.
	/// </returns>
	public static IRasterProcessor WithProbability(
		this IRasterProcessor processor,
		double probability)
		=> new ProbabilityRasterProcessorProxy(processor, probability);
}
