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
/// Value provider proxy which repeats each value returned by the source provider
/// multiple times.
/// </summary>
public class ValueProviderRepeater<T> : ValueProvider<T>
{
	private readonly ValueProvider<T> _provider;
	private readonly int _repeats;

	private T _value;
	private int _counter = -1;

	/// <param name="provider">Source value provider.</param>
	/// <param name="repeats">Repeats count.</param>
	/// <exception cref="ArgumentException"/>
	/// <exception cref="ArgumentNullException"/>
	public ValueProviderRepeater(ValueProvider<T> provider, int repeats)
	{
		if (repeats < 1)
			throw new ArgumentException($"Value of '{nameof(repeats)}' must be >= 1", nameof(repeats));

		_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		_repeats = repeats;
	}

	public override T GetNext()
	{
		_counter++;

		if (_counter % _repeats == 0)
			_value = _provider;

		return _value;
	}
}
