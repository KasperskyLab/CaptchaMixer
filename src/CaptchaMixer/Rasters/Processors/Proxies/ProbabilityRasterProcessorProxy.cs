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
/// Calls a raster processor with specified probability.
/// </summary>
public class ProbabilityRasterProcessorProxy : IRasterProcessor
{
	private readonly Random _random = new();
	private readonly IRasterProcessor _processor;
	private readonly double _probability;

	/// <param name="processor">The called processor.</param>
	/// <param name="probability">Processor call probability from 0 to 1.</param>
	public ProbabilityRasterProcessorProxy(IRasterProcessor processor, double probability)
	{
		_processor = processor;
		_probability = probability;
	}

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		if (_random.NextDouble() > _probability) return;
		_processor.Process(layer, context);
	}
}
