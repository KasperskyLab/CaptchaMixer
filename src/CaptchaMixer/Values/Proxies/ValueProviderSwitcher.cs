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
/// Value provider proxy which returns the same value from the source provider over and over
/// again until <see cref="NextValue"/> gets called.
/// </summary>
/// <remarks>
/// This class implements <see cref="IVectorProcessor"/> and <see cref="IRasterProcessor"/>,
/// switching to the next value on
/// <see cref="IVectorProcessor.Process(IVectorLayer, ICaptchaContext)"/> and
/// <see cref="IRasterProcessor.Process(IRasterLayer, ICaptchaContext)"/> methods calls.
/// Thus such providers can be used as parts of captcha generation pipelines as a way to
/// control the moments of switching values during captcha generation.
/// </remarks>
[DebugBuilderSkip]
public class ValueProviderSwitcher<T> : ValueProvider<T>, IVectorProcessor, IRasterProcessor
{
	private readonly ValueProvider<T> _provider;
	private T _value;
	private bool _valueAcquired = false;

	/// <param name="provider">Value provider.</param>
	/// <exception cref="ArgumentNullException"/>
	public ValueProviderSwitcher(ValueProvider<T> provider)
	{
		_provider = provider ?? throw new ArgumentNullException(nameof(provider));
	}

	public override T GetNext()
	{
		if (!_valueAcquired) NextValue();
		return _value;
	}

	public void NextValue()
	{
		_value = _provider.GetNext();
		_valueAcquired = true;
	}

	public void Process(IVectorLayer layer, ICaptchaContext context)
		=> NextValue();

	public void Process(IRasterLayer layer, ICaptchaContext context)
		=> NextValue();
}
