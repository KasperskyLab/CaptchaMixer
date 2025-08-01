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
/// A proxy for provider of providers which returns values returned by one of the providers until
/// <see cref="NextProvider"/> gets called.
/// </summary>
/// <remarks>
/// <para>
/// This class is somewhat similar to <see cref="ValueProviderSwitcher{T}"/>, but it's not just
/// switching a value - it's also switching the value provider.
/// </para>
/// <para>
/// Using a provider of providers gives an opportunity to conviniently control provider selection:
/// you can use simple static shorthands such as <see cref="RandomOf{TValue}(TValue[])"/>.
/// </para>
/// <para>
/// This class implements <see cref="IVectorProcessor"/> and <see cref="IRasterProcessor"/>,
/// switching to the next value on
/// <see cref="IVectorProcessor.Process(IVectorLayer, ICaptchaContext)"/> and
/// <see cref="IRasterProcessor.Process(IRasterLayer, ICaptchaContext)"/> methods calls.
/// Thus such providers can be used as parts of captcha generation pipelines as a way to
/// control the moments of switching values during captcha generation.
/// </para>
/// </remarks>
[DebugBuilderSkip]
public class ValueProviderSelector<T> : ValueProvider<T>, IVectorProcessor, IRasterProcessor
{
	private readonly ValueProviderSwitcher<ValueProvider<T>> _switcher;
	private ValueProvider<T> _provider;

	/// <param name="providers">Provider of value providers.</param>
	/// <exception cref="ArgumentNullException"/>
	public ValueProviderSelector(ValueProvider<ValueProvider<T>> providers)
	{
		ArgumentNullException.ThrowIfNull(providers, nameof(providers));
		_switcher = new ValueProviderSwitcher<ValueProvider<T>>(providers);
		NextProvider();
	}

	public void NextProvider()
	{
		_switcher.NextValue();
		_provider = _switcher.GetNext();
	}

	public override T GetNext()
		=> _provider.GetNext();

	public void Process(IVectorLayer layer, ICaptchaContext context)
		=> NextProvider();

	public void Process(IRasterLayer layer, ICaptchaContext context)
		=> NextProvider();
}
