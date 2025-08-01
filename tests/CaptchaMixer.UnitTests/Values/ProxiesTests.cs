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

namespace Kaspersky.CaptchaMixer.UnitTests.Values;

public class ProxiesTests
{
	[Test]
	public void Values_Proxies_ValueProviderConverter_Test()
	{
		var provider = IntRangeCarousel(0, 3);
		var values = new ValueProviderConverter<int, int>(provider, i => i + 1).Take(3);
		values.Should().ContainInConsecutiveOrder(new[] { 1, 2, 3 });
	}

	[Test]
	public void Values_Proxies_ValueProviderRepeater_Test()
	{
		var provider = IntRangeCarousel(0, 3);
		var values = new ValueProviderRepeater<int>(provider, 2).Take(10);
		values.Should().ContainInConsecutiveOrder(new[] { 0, 0, 1, 1, 2, 2, 3, 3, 0, 0 });
	}

	[Test]
	public void Values_Proxies_ValueProviderSelector_Test()
	{
		var provider1 = IntRangeCarousel(0, 3);
		var provider2 = IntRangeCarousel(4, 7);
		var selector = new ValueProviderSelector<int>(Carousel(provider1, provider2));
		selector.Take(5).Should().ContainInConsecutiveOrder(new[] { 0, 1, 2, 3, 0 });
		selector.NextProvider();
		selector.Take(5).Should().ContainInConsecutiveOrder(new[] { 4, 5, 6, 7, 4 });
		selector.NextProvider();
		selector.Take(5).Should().ContainInConsecutiveOrder(new[] { 1, 2, 3, 0, 1 });
	}

	[Test]
	public void Values_Proxies_ValueProviderSwitcher_Test()
	{
		var provider = IntRangeCarousel(0, 3);
		var switcher = new ValueProviderSwitcher<int>(provider);
		switcher.Take(10).Should().AllSatisfy(v => v.Should().Be(0));
		switcher.NextValue();
		switcher.Take(10).Should().AllSatisfy(v => v.Should().Be(1));
		switcher.NextValue();
		switcher.Take(10).Should().AllSatisfy(v => v.Should().Be(2));
		switcher.NextValue();
		switcher.Take(10).Should().AllSatisfy(v => v.Should().Be(3));
		switcher.NextValue();
		switcher.Take(10).Should().AllSatisfy(v => v.Should().Be(0));
	}
}