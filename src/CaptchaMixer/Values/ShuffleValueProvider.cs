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
/// Returns values from a collection whose elements have been shuffled.
/// </summary>
public class ShuffleValueProvider<T> : ArrayBasedValueProvider<T>
{
	private readonly bool _reshuffle;
	private int _index = 0;

	/// <param name="values">Values.</param>
	/// <param name="reshuffle">Reshuffle elements after each walkthrough.</param>
	public ShuffleValueProvider(IEnumerable<T> values, bool reshuffle = true)
		: base(values)
	{
		_reshuffle = reshuffle;
		_values.Shuffle();
	}

	public override T GetNext()
	{
		if (_values.Length == 1)
			return _values[0];

		if (_index >= _values.Length)
		{
			if (_reshuffle) _values.Shuffle();
			_index = 0;
		}

		return _values[_index++];
	}
}
