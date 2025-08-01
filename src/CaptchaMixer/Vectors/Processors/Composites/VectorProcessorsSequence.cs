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
/// Raster processors sequence.
/// </summary>
public class VectorProcessorsSequence : VectorProcessorsComposite
{
	public VectorProcessorsSequence(params IVectorProcessor[] processors)
		: base(processors) { }

	public VectorProcessorsSequence(IEnumerable<IVectorProcessor> processors)
		: base(processors) { }

	public override void Process(IVectorLayer layer, ICaptchaContext context)
		=> _processors.ForEach(p => p.Process(layer, context));
}
