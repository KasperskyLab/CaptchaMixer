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

public static class IVectorProcessorExtensions
{
	/// <inheritdoc cref="PngExportingVectorProcessorProxy(IVectorProcessor, string, bool, int)"/>
	/// <returns>
	/// <paramref name="processor"/> wrapped into a <see cref="PngExportingVectorProcessorProxy"/>.
	/// </returns>
	public static IVectorProcessor WithPngExport(
		this IVectorProcessor processor,
		string filePathBase,
		bool saveBefore = true,
		int scale = 10)
		=> new PngExportingVectorProcessorProxy(processor, filePathBase, saveBefore, scale);

	/// <inheritdoc cref="ProbabilityVectorProcessorProxy(IVectorProcessor, double)"/>
	/// <returns>
	/// <paramref name="processor"/> wrapped into a <see cref="ProbabilityVectorProcessorProxy"/>.
	/// </returns>
	public static IVectorProcessor WithProbability(
		this IVectorProcessor processor,
		double probability)
		=> new ProbabilityVectorProcessorProxy(processor, probability);
}
