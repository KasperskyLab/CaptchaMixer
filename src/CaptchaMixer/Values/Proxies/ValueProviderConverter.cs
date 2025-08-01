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
/// Value provider proxy which takes a value from another provider and converts it
/// using the specified function.
/// </summary>
public class ValueProviderConverter<TFrom, TTo> : ValueProvider<TTo>
{
	private readonly ValueProvider<TFrom> _provider;
	private readonly Func<TFrom, TTo> _converter;

	/// <param name="provider">Source values provider.</param>
	/// <param name="converter">Convert function.</param>
	/// <exception cref="ArgumentNullException"/>
	public ValueProviderConverter(
		ValueProvider<TFrom> provider,
		Func<TFrom, TTo> converter)
	{
		_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		_converter = converter ?? throw new ArgumentNullException(nameof(converter));
	}

	public override TTo GetNext()
		=> _converter(_provider.GetNext());
}
