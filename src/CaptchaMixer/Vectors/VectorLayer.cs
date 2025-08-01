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

/// <inheritdoc cref="IVectorLayer"/>
public class VectorLayer : IVectorLayer
{
	public string Name { get; }
	public List<VectorObject> Objects { get; set; }

	/// <param name="name"><inheritdoc cref="Name" path="/summary"/></param>
	/// <param name="objects"><inheritdoc cref="Objects" path="/summary"/></param>
	public VectorLayer(string name, params VectorObject[] objects)
		: this(name, objects as IEnumerable<VectorObject>) { }

	/// <param name="name"><inheritdoc cref="Name" path="/summary"/></param>
	/// <param name="objects"><inheritdoc cref="Objects" path="/summary"/></param>
	public VectorLayer(string name, IEnumerable<VectorObject> objects = null)
	{
		Name = name;
		Objects = objects?.ToList() ?? new();
	}

	public IVectorLayer Clone()
		=> new VectorLayer(Name, Objects.Select(o => o.Clone()));
}
