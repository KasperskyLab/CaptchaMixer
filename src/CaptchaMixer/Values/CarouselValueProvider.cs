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
/// Returns values from a collection sequentially.
/// Jumps back to the first array element after returning the last element.
/// </summary>
public class CarouselValueProvider<T> : ArrayBasedValueProvider<T>
{
	protected int _index;

	/// <param name="values">Values.</param>
	/// <param name="start">Index of the first returned element.</param>
	/// <exception cref="ArgumentException"/>
	public CarouselValueProvider(IEnumerable<T> values, int start = 0)
		: base(values)
	{
		if (start < 0 || start >= _values.Length)
			throw new ArgumentException($"Start index must be greater than 0 and less than {_values.Length}", nameof(start));

		_index = start;
	}

	public override T GetNext()
	{
		if (_values.Length == 1)
			return _values[0];

		if (_index >= _values.Length) _index = 0;
		return _values[_index++];
	}
}
