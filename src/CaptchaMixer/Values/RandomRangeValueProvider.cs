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
/// Returns random values from a specified range.
/// </summary>
public class RandomRangeValueProvider<T> : RangeBasedValueProvider<T>
{
	private readonly Random _random = new();

	/// <param name="from">Range start.</param>
	/// <param name="to">Range end.</param>
	/// <param name="validator">Range validation function for <paramref name="from"/> and <paramref name="to"/>.</param>
	/// <param name="evaluator">Value generation function using <see cref="Random"/>.</param>
	/// <exception cref="ArgumentException"/>
	public RandomRangeValueProvider(
		T from,
		T to,
		Func<T, T, bool> validator,
		Func<T, T, Random, T> evaluator)
		: base(from, to, validator, (f, t) => default)
		=> _evaluator = (f, t) => evaluator(f, t, _random);
}
