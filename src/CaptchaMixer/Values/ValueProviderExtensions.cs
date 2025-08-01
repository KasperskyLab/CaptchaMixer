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

public static class ValueProviderExtensions
{
	/// <returns>
	/// <paramref name="provider"/> wrapped into a <see cref="ValueProviderRepeater{T}"/> with
	/// <paramref name="repeats"/> count.
	/// </returns>
	public static ValueProvider<T> Repeat<T>(this ValueProvider<T> provider, int repeats)
		=> new ValueProviderRepeater<T>(provider, repeats);

	/// <returns>
	/// <paramref name="provider"/> wrapped into a <see cref="ValueProviderSwitcher{T}"/>.
	/// </returns>
	public static ValueProviderSwitcher<T> Switch<T>(this ValueProvider<T> provider)
		=> new(provider);

	/// <returns>
	/// <paramref name="provider"/> wrapped into a <see cref="ValueProviderConverter{TFrom, TTo}"/>
	/// with the specified <paramref name="converter"/> function.<br/>
	/// If <paramref name="provider"/> is a <see cref="ConstValueProvider{TFrom}"/> then the convertion
	/// is performed in-place, and a new <see cref="ConstValueProvider{TTo}"/> is returned.
	/// </returns>
	public static ValueProvider<TTo> Convert<TFrom, TTo>(this ValueProvider<TFrom> provider, Func<TFrom, TTo> converter)
	{
		if (provider is ConstValueProvider<TFrom>)
			return new ConstValueProvider<TTo>(converter(provider.GetNext()));

		return new ValueProviderConverter<TFrom, TTo>(provider, converter);
	}

	/// <returns>
	/// <paramref name="count"/> values acquired from <paramref name="provider"/>.
	/// </returns>
	public static IEnumerable<T> Take<T>(this ValueProvider<T> provider, int count)
	{
		for (int i = 0; i < count; i++)
			yield return provider.GetNext();
	}

	/// <returns>
	/// Rectangle provided by <paramref name="provider"/> it it's not <see langword="null"/>,
	/// otherwise - a rectangle of <see cref="CaptchaParameters.Size"/> from <paramref name="context"/>.
	/// </returns>
	public static RectangleF RectOrCaptchaSize(this ValueProvider<RectangleF> provider, ICaptchaContext context)
		=> provider ?? context.Parameters.Size.ToRectangleF();
}
