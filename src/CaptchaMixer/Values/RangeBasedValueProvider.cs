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
/// Returns values from range.
/// </summary>
public class RangeBasedValueProvider<T> : ValueProvider<T>
{
	protected readonly T _from;
	protected readonly T _to;
	protected Func<T, T, T> _evaluator;

	/// <param name="from">Range start.</param>
	/// <param name="to">Range end.</param>
	/// <param name="validator">Range validation function for <paramref name="from"/> and <paramref name="to"/>.</param>
	/// <param name="evaluator">Value generation function.</param>
	/// <exception cref="ArgumentException"/>
	public RangeBasedValueProvider(
		T from,
		T to,
		Func<T, T, bool> validator,
		Func<T, T, T> evaluator)
	{
		if (!validator(from, to))
			throw new ArgumentException($"Invalid range: {from}, {to}");

		_from = from;
		_to = to;
		_evaluator = evaluator;
	}

	public override T GetNext()
		=> _evaluator(_from, _to);
}
