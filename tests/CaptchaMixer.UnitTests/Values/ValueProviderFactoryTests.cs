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

public class ValueProviderFactoryTests
{
	#region Random

	private const int RandomValuesCount = 100000;
	private const double RandomAccuracy = 0.5;

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomOf_Test()
	{
		var values = RandomOf(1, 2, 3).Take(RandomValuesCount).ToList();
		values.Should().Contain(new[] { 1, 2, 3 });
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_WeightedRandomOf_Test()
	{
		var values = WeightedRandomOf((1, 1), (2, 2), (3, 3)).Take(RandomValuesCount).ToList();
		var count1 = values.Count(v => v == 1);
		var count2 = values.Count(v => v == 2);
		var count3 = values.Count(v => v == 3);
		((double)count2 / count1).Should().BeApproximately(2, RandomAccuracy);
		((double)count3 / count1).Should().BeApproximately(3, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomInt_To_Test()
	{
		var values = RandomInt(4).Take(RandomValuesCount).ToList();
		values.Should().Contain(new[] { 0, 1, 2, 3 });
		values.Average().Should().BeApproximately(1.5, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomInt_FromTo_Test()
	{
		var values = RandomInt(1, 4).Take(RandomValuesCount).ToList();
		values.Should().Contain(new[] { 1, 2, 3 });
		values.Average().Should().BeApproximately(2, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomMirroredInt_Test()
	{
		var values = RandomMirroredInt(4).Take(RandomValuesCount).ToList();
		values.Should().Contain(new[] { -3, -2, -1, 0, 1, 2, 3 });
		values.Average().Should().BeApproximately(-0.5, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomFloat_To_Test()
	{
		var values = RandomFloat(4).Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(v => v.Should().BeGreaterThanOrEqualTo(0).And.BeLessThan(4));
		((double)values.Average()).Should().BeApproximately(2, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomFloat_FromTo_Test()
	{
		var values = RandomFloat(1, 4).Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(v => v.Should().BeGreaterThanOrEqualTo(1).And.BeLessThan(4));
		((double)values.Average()).Should().BeApproximately(2.5, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomMirroredFloat_Test()
	{
		var values = RandomMirroredFloat(4).Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(v => v.Should().BeGreaterThanOrEqualTo(-4).And.BeLessThan(4));
		((double)values.Average()).Should().BeApproximately(0, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomBool_Test()
	{
		var values = RandomBool().Take(RandomValuesCount).ToList();
		var countTrue = values.Count(v => v == true);
		var countFalse = values.Count(v => v == false);
		((double)countTrue / countFalse).Should().BeApproximately(1, RandomAccuracy);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomColor_Test()
	{
		var values = RandomColor().Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(c => c.A.Should().Be(255));
		values.Min(v => v.R).Should().Be(0);
		values.Min(v => v.G).Should().Be(0);
		values.Min(v => v.B).Should().Be(0);
		values.Max(v => v.R).Should().Be(255);
		values.Max(v => v.G).Should().Be(255);
		values.Max(v => v.B).Should().Be(255);
		values.Average(v => v.R).Should().BeApproximately(127.5, 2);
		values.Average(v => v.G).Should().BeApproximately(127.5, 2);
		values.Average(v => v.B).Should().BeApproximately(127.5, 2);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomGrayscaleColor_Test()
	{
		var values = RandomGrayscaleColor().Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(c => c.R.Should().Be(c.G));
		values.Should().AllSatisfy(c => c.R.Should().Be(c.B));
		values.Should().AllSatisfy(c => c.A.Should().Be(255));
		values.Min(v => v.R).Should().Be(0);
		values.Min(v => v.G).Should().Be(0);
		values.Min(v => v.B).Should().Be(0);
		values.Max(v => v.R).Should().Be(255);
		values.Max(v => v.G).Should().Be(255);
		values.Max(v => v.B).Should().Be(255);
		values.Average(v => v.R).Should().BeApproximately(127.5, 2);
		values.Average(v => v.G).Should().BeApproximately(127.5, 2);
		values.Average(v => v.B).Should().BeApproximately(127.5, 2);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomHue_Test()
	{
		var values = RandomHue().Take(RandomValuesCount).ToList();
		values.Should().AllSatisfy(h => h.Should().BeGreaterThanOrEqualTo(0).And.BeLessThan(CMColor.HuesCount));
		values.Average().Should().BeApproximately((CMColor.HuesCount - 1) / 2.0, 10);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomWellKnownFont_Test()
	{
		var values = RandomWellKnownFont().Take(RandomValuesCount).ToList();
		values.Select(v => v.FamilyName).Should().Contain(WellKnownFontFamilies.All);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomWellKnownThickFont_Test()
	{
		var values = RandomWellKnownThickFont().Take(RandomValuesCount).ToList();
		values.Select(v => v.FamilyName).Should().Contain(WellKnownFontFamilies.Thick);
	}

	[Test]
	[Retry(3)]
	public void Values_ValueProviderFactory_RandomWellKnownThinFont_Test()
	{
		var values = RandomWellKnownThinFont().Take(RandomValuesCount).ToList();
		values.Select(v => v.FamilyName).Should().Contain(WellKnownFontFamilies.Thin);
	}

	#endregion

	#region Carousel

	[Test]
	[TestCase(0, new[] { 1, 2, 3, 1, 2, 3 })]
	[TestCase(1, new[] { 2, 3, 1, 2, 3, 1 })]
	public void Values_ValueProviderFactory_Carousel_Test(
		int start,
		int[] expected)
	{
		var values = Carousel(new[] { 1, 2, 3 }, start).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0, new[] { 1, 2, 2, 3, 3, 3, 1, 2, 2, 3, 3, 3 })]
	[TestCase(2, new[] { 2, 3, 3, 3, 1, 2, 2, 3, 3, 3 })]
	public void Values_ValueProviderFactory_RepeatingCarousel_Test(
		int start,
		int[] expected)
	{
		var values = RepeatingCarousel(new[] { (1, 1), (2, 2), (3, 3) }, start).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0, 3, 1, 0, new[] { 0, 1, 2, 3, 0, 1, 2, 3 })]
	[TestCase(0, 3, 1, 1, new[] { 1, 2, 3, 0, 1, 2, 3, 0 })]
	[TestCase(0, 3, 2, 0, new[] { 0, 2, 0, 2 })]
	[TestCase(0, 3, 2, 1, new[] { 2, 0, 2, 0 })]
	public void Values_ValueProviderFactory_IntRangeCarousel_Test(
		int from,
		int to,
		int step,
		int start,
		int[] expected)
	{
		var values = IntRangeCarousel(from, to, step, start).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0f, 3f, 1f, 0, new[] { 0f, 1f, 2f, 3f, 0f, 1f, 2f, 3f })]
	[TestCase(0f, 3f, 1f, 1, new[] { 1f, 2f, 3f, 0f, 1f, 2f, 3f, 0f })]
	[TestCase(0f, 3f, 2f, 0, new[] { 0f, 2f, 0f, 2f })]
	[TestCase(0f, 3f, 2f, 1, new[] { 2f, 0f, 2f, 0f })]
	[TestCase(0f, 2f, 0.6f, 0, new[] { 0f, 0.6f, 1.2f, 1.8f, 0f, 0.6f, 1.2f, 1.8f })]
	public void Values_ValueProviderFactory_FloatRangeCarousel_Test(
		float from,
		float to,
		float step,
		int start,
		float[] expected)
	{
		var provider = FloatRangeCarousel(from, to, step, start);
		foreach (var value in expected)
			provider.GetNext().Should().BeApproximately(value, 0.0001f);
	}

	[Test]
	[TestCase(true, new[] { true, false, true, false })]
	[TestCase(false, new[] { false, true, false, true })]
	public void Values_ValueProviderFactory_BoolCarousel_Test(
		bool start,
		bool[] expected)
	{
		var values = BoolCarousel(start).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	#endregion

	#region BackForth

	[Test]
	[TestCase(0, false, false, new[] { 1, 2, 3, 2, 1, 2, 3 })]
	[TestCase(1, false, false, new[] { 2, 3, 2, 1, 2, 3 })]
	[TestCase(1, true, false, new[] { 2, 1, 2, 3, 2, 1 })]
	[TestCase(0, false, true, new[] { 1, 2, 3, 3, 2, 1, 1, 2 })]
	[TestCase(1, true, true, new[] { 2, 1, 1, 2, 3, 3, 2 })]
	public void Values_ValueProviderFactory_BackForth_Test(
		int start,
		bool startReversed,
		bool repeatEnds,
		int[] expected)
	{
		var values = BackForth(new[] { 1, 2, 3 }, start, startReversed, repeatEnds).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0, false, false, new[] { 1, 2, 2, 3, 3, 3, 3, 3, 2, 2, 1, 2, 2, 3, 3, 3, 3, 3, 2 })]
	[TestCase(1, false, false, new[] { 2, 2, 3, 3, 3, 3, 3, 2, 2, 1, 2, 2, 3, 3, 3, 3, 3, 2 })]
	[TestCase(1, true, false, new[] { 2, 1, 2, 2, 3, 3, 3, 3, 3, 2, 2, 1 })]
	[TestCase(0, false, true, new[] { 1, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 1, 1, 2 })]
	[TestCase(1, true, true, new[] { 2, 1, 1, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 1, 1 })]
	public void Values_ValueProviderFactory_RepeatingBackForth_Test(
		int start,
		bool startReversed,
		bool repeatEnds,
		int[] expected)
	{
		var values = RepeatingBackForth(new[] { (1, 1), (2, 2), (3, 3) }, start, startReversed, repeatEnds).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0, 3, 1, 0, new[] { 0, 1, 2, 3, 2, 1, 0 })]
	[TestCase(0, 3, 1, 1, new[] { 1, 2, 3, 2, 1, 0, 1 })]
	[TestCase(0, 3, 2, 0, new[] { 0, 2, 0, 2 })]
	[TestCase(0, 3, 2, 1, new[] { 2, 0, 2, 0 })]
	public void Values_ValueProviderFactory_IntRangeBackForth_Test(
		int from,
		int to,
		int step,
		int start,
		int[] expected)
	{
		var values = IntRangeBackForth(from, to, step, start).Take(expected.Length).ToList();
		values.Should().ContainInConsecutiveOrder(expected);
	}

	[Test]
	[TestCase(0f, 3f, 1f, 0, new[] { 0f, 1f, 2f, 3f, 2f, 1f, 0f })]
	[TestCase(0f, 3f, 1f, 1, new[] { 1f, 2f, 3f, 2f, 1f, 0f, 1f })]
	[TestCase(0f, 3f, 2f, 0, new[] { 0f, 2f, 0f, 2f })]
	[TestCase(0f, 3f, 2f, 1, new[] { 2f, 0f, 2f, 0f })]
	[TestCase(0f, 2f, 0.6f, 0, new[] { 0f, 0.6f, 1.2f, 1.8f, 1.2f, 0.6f, 0f, 0.6f })]
	public void Values_ValueProviderFactory_FloatRangeBackForth_Test(
		float from,
		float to,
		float step,
		int start,
		float[] expected)
	{
		var provider = FloatRangeBackForth(from, to, step, start);
		foreach (var value in expected)
			provider.GetNext().Should().BeApproximately(value, 0.0001f);
	}

	#endregion

	#region Shuffle

	[Test]
	public void Values_ValueProviderFactory_Shuffle_Test()
	{
		var values = Shuffle(new[] { 1, 2, 3 }).Take(3).ToList();
		values.Should().Contain(new[] { 1, 2, 3 });
	}

	[Test]
	public void Values_ValueProviderFactory_RepeatingShuffle_Test()
	{
		var values = RepeatingShuffle(new[] { (1, 1), (2, 2), (3, 3) }).Take(6).ToList();
		values.Count(v => v == 1).Should().Be(1);
		values.Count(v => v == 2).Should().Be(2);
		values.Count(v => v == 3).Should().Be(3);
	}

	[Test]
	public void Values_ValueProviderFactory_IntRangeShuffle_Test()
	{
		var values = IntRangeShuffle(0, 3, 1).Take(4).ToList();
		values.Should().Contain(new[] { 0, 1, 2, 3 });
	}

	[Test]
	public void Values_ValueProviderFactory_FloatRangeShuffle_Test()
	{
		var values = FloatRangeShuffle(0, 2, 0.5f).Take(5).ToList();
		values.Should().Contain(new[] { 0f, 0.5f, 1f, 1.5f, 2f });
	}

	[Test]
	public void Values_ValueProviderFactory_BoolShuffle_Test()
	{
		var values = BoolShuffle().Take(2).ToList();
		values.Should().Contain(new[] { true, false });
	}

	#endregion
}