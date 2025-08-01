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
/// Returns values from a collection first in direct, then in reversed order.
/// </summary>
public class BackForthValueProvider<T> : CarouselValueProvider<T>
{
	private bool _direct;
	private readonly bool _repeatEnds;

	/// <param name="values">Values.</param>
	/// <param name="start">Index of the first returned element.</param>
	/// <param name="startReversed">Start iterating in reversed order.</param>
	/// <param name="repeatEnds">Repeat start and end elements on direction switch.</param>
	public BackForthValueProvider(
		IEnumerable<T> values,
		int start = 0,
		bool startReversed = false,
		bool repeatEnds = false)
		: base(values, start)
	{
		_direct = !startReversed;
		_repeatEnds = repeatEnds;
	}

	public override T GetNext()
	{
		if (_values.Length == 1)
			return _values[0];

		if (_direct && _index >= _values.Length)
		{
			_direct = false;
			_index = _values.Length - (_repeatEnds ? 1 : 2);
		}
		else if (!_direct && _index < 0)
		{
			_direct = true;
			_index = _repeatEnds ? 0 : 1;
		}

		var result = _values[_index];
		_index += _direct ? 1 : -1;
		return result;
	}
}
