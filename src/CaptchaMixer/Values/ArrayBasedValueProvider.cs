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
/// Base class for array-based value providers.
/// </summary>
public abstract class ArrayBasedValueProvider<T> : ValueProvider<T>
{
	protected readonly T[] _values;

	/// <param name="values">Values array.</param>
	/// <exception cref="ArgumentException"/>
	protected ArrayBasedValueProvider(IEnumerable<T> values)
	{
		_values = values?.ToArray() ?? Array.Empty<T>();

		if (_values.Length == 0)
			throw new ArgumentException("Value sequence is empty", nameof(values));
	}
}
