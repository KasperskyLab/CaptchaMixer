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
/// Vector processors composite which calls other processors for a temporary layer which
/// only contains a single object from the processed layer. When all processing is finished,
/// objects of the original layer are replaced with resultant objects.
/// </summary>
/// <remarks>
/// Objects processing isolation is useful in combination with structural processors such as
/// <see cref="SplitObjects"/> and <see cref="MergeObjects"/>.
/// Many processors operate on layer's objects rather than on their subcomponents (paths or
/// contours). In order to operate with those lower-level components you will first have to
/// decompose the objects. However by applying <see cref="SplitObjects"/> onto a layer, you
/// end up with a flat collection of once-were-paths-objects with no way to restore their
/// original objects/paths grouping.
/// In such case per-object processing isolation would let you split every object separately,
/// process its subcomponents, and then merge the object back.
/// </remarks>
public class VectorProcessorsPerObjectSequence : VectorProcessorsSequence
{
	public VectorProcessorsPerObjectSequence(params IVectorProcessor[] processors)
		: base(processors) { }

	public VectorProcessorsPerObjectSequence(IEnumerable<IVectorProcessor> processors)
		: base(processors) { }

	public override void Process(IVectorLayer layer, ICaptchaContext context)
	{
		var objects = new List<VectorObject>();

		for (var i = 0; i < layer.Objects.Count; i++)
		{
			var obj = layer.Objects[i];
			var subLayer = new VectorLayer($"{layer.Name}_sub_{i}");
			subLayer.Objects.Add(obj);
			base.Process(subLayer, context);
			objects.AddRange(subLayer.Objects);
		}

		layer.Objects.ReplaceItems(objects);
	}
}
