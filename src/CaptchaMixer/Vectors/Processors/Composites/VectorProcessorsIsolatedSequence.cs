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
/// Vector processors sequence which operates on an empty temporary layer. After all processors
/// finish work, objects from the temporary layer are added to the initially processed layer.
/// </summary>
/// <remarks>
/// This basically implements sub-layer logic which simplifies templates programming. If you need
/// to perform some small operations you don't need a separate layer and combine layers later on -
/// just use an isolator.
/// </remarks>
public class VectorProcessorsIsolatedSequence : VectorProcessorsSequence
{
	public VectorProcessorsIsolatedSequence(params IVectorProcessor[] processors)
		: base(processors) { }

	public VectorProcessorsIsolatedSequence(IEnumerable<IVectorProcessor> processors)
		: base(processors) { }

	public override void Process(IVectorLayer layer, ICaptchaContext context)
	{
		var subLayer = new VectorLayer($"{layer.Name}_sub");
		base.Process(subLayer, context);
		layer.Objects.AddRange(subLayer.Objects);
	}
}
