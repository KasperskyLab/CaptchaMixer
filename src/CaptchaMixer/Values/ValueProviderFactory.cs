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

public static class ValueProviderFactory
{
	private static readonly bool[] BoolValues = { true, false };

	#region Random

	/// <returns>
	/// Provider of random values from <paramref name="values"/>.
	/// </returns>
	public static ValueProvider<TValue> RandomOf<TValue>(
		params TValue[] values)
		=> new RandomOfValueProvider<TValue>(values);

	/// <returns>
	/// Provider of random values from <paramref name="tuples"/> with the specified weights.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<TValue> WeightedRandomOf<TValue>(
		params (TValue Value, int Weight)[] tuples)
		=> new RandomOfValueProvider<TValue>(CreateRepeatingSequence(tuples));

	/// <inheritdoc cref="RandomFloat(float)"/>
	public static ValueProvider<int> RandomInt(
		int to)
		=> new RandomRangeValueProvider<int>(0, to, (_, t) => t >= 0, (f, t, r) => r.Next(0, to));

	/// <inheritdoc cref="RandomFloat(float, float)"/>
	public static ValueProvider<int> RandomInt(
		int from,
		int to)
		=> new RandomRangeValueProvider<int>(from, to, (f, t) => f <= t, (f, t, r) => r.Next(f, t));

	/// <inheritdoc cref="RandomMirroredFloat(float)"/>
	public static ValueProvider<int> RandomMirroredInt(
		int length)
		=> new RandomRangeValueProvider<int>(-length, length, (_, t) => t >= 0, (f, t, r) => r.Next(f, t));

	/// <returns>
	/// Provider of random values in range [0; <paramref name="to"/>).
	/// </returns>
	public static ValueProvider<float> RandomFloat(
		float to)
		=> new RandomRangeValueProvider<float>(0, to, (_, t) => t >= 0, (f, t, r) => (float)(r.NextDouble() * t));

	/// <returns>
	/// Provider of random values in range [<paramref name="from"/>; <paramref name="to"/>).
	/// </returns>
	public static ValueProvider<float> RandomFloat(
		float from,
		float to)
		=> new RandomRangeValueProvider<float>(from, to, (f, t) => f <= t, (f, t, r) => (float)(r.NextDouble() * (t - f) + f));

	/// <returns>
	/// Provider of random values in range [-<paramref name="length"/>; <paramref name="length"/>).
	/// </returns>
	public static ValueProvider<float> RandomMirroredFloat(
		float length)
		=> new RandomRangeValueProvider<float>(-length, length, (_, t) => t >= 0, (f, t, r) => (float)(r.NextDouble() * (t - f) + f));

	private static readonly Lazy<ValueProvider<bool>> _randomBoolLazy
		= new(() => RandomOf(true, false));

	/// <returns>
	/// Provider of random boolean values.
	/// </returns>
	public static ValueProvider<bool> RandomBool()
		=> _randomBoolLazy.Value;

	private static readonly Lazy<ValueProvider<CMColor>> _randomColorLazy
		= new(() => new ColorProvider());

	/// <returns>
	/// Provider of completely random colors.
	/// </returns>
	public static ValueProvider<CMColor> RandomColor()
		=> _randomColorLazy.Value;

	/// <returns>
	/// Provider of random colors with specified <paramref name="brightness"/>
	/// and <paramref name="alpha"/>.
	/// </returns>
	public static ValueProvider<CMColor> RandomColor(
		ValueProvider<float> brightness,
		ValueProvider<int> alpha)
		=> new ColorProvider
		{
			Brightness = brightness,
			Alpha = alpha
		};

	/// <returns>
	/// Provider of random colors with specified <paramref name="brightness"/>, <paramref name="hue"/>,
	/// <paramref name="saturation"/> and <paramref name="alpha"/>.
	/// </returns>
	public static ValueProvider<CMColor> RandomColor(
		ValueProvider<float> brightness,
		ValueProvider<int> hue,
		ValueProvider<float> saturation,
		ValueProvider<int> alpha)
		=> new ColorProvider
		{
			Brightness = brightness,
			Hue = hue,
			Saturation = saturation,
			Alpha = alpha
		};

	private static readonly Lazy<ValueProvider<CMColor>> _randomGrayscaleColorLazy
		= new(() => new GrayscaleColorProvider());

	/// <returns>
	/// Provider of random grayscale colors.
	/// </returns>
	public static ValueProvider<CMColor> RandomGrayscaleColor()
		=> _randomGrayscaleColorLazy.Value;

	/// <returns>
	/// Provider of random grayscale colors with specified <paramref name="brightness"/> and <paramref name="alpha"/>.
	/// </returns>
	public static ValueProvider<CMColor> RandomGrayscaleColor(ValueProvider<float> brightness, ValueProvider<int> alpha)
		=> new GrayscaleColorProvider
		{
			Brightness = brightness,
			Alpha = alpha
		};

	private static readonly Lazy<ValueProvider<int>> _randomHueLazy
		= new(() => RandomInt(CMColor.HuesCount));

	/// <returns>
	/// Provider of random color hues.
	/// </returns>
	public static ValueProvider<int> RandomHue()
		=> _randomHueLazy.Value;

	/// <returns>
	/// Provider of random well-known cross-platform fonts with specified <paramref name="style"/>.
	/// </returns>
	public static ValueProvider<FontInfo> RandomWellKnownFont(FontStyle style = FontStyle.Normal)
		=> new RandomOfValueProvider<FontInfo>(WellKnownFontFamilies.All.Select(f => FontInfo.FromFamily(f, style)));

	/// <returns>
	/// Provider of random well-known cross-platform thick-looking fonts with specified <paramref name="style"/>.
	/// </returns>
	public static ValueProvider<FontInfo> RandomWellKnownThickFont(FontStyle style = FontStyle.Normal)
		=> new RandomOfValueProvider<FontInfo>(WellKnownFontFamilies.Thick.Select(f => FontInfo.FromFamily(f, style)));

	/// <returns>
	/// Provider of random well-known cross-platform thin-looking fonts with specified <paramref name="style"/>.
	/// </returns>
	public static ValueProvider<FontInfo> RandomWellKnownThinFont(FontStyle style = FontStyle.Normal)
		=> new RandomOfValueProvider<FontInfo>(WellKnownFontFamilies.Thin.Select(f => FontInfo.FromFamily(f, style)));

	#endregion

	#region Carousel

	/// <inheritdoc cref="Carousel{TValue}(IEnumerable{TValue}, int)"/>
	public static ValueProvider<TValue> Carousel<TValue>(
		params TValue[] values)
		=> new CarouselValueProvider<TValue>(values);

	/// <inheritdoc cref="CarouselValueProvider{T}.CarouselValueProvider(IEnumerable{T}, int)" path="/param[@name='start']"/>
	/// <returns>
	/// <see cref="CarouselValueProvider{T}"/> from <paramref name="values"/>.
	/// </returns>
	public static ValueProvider<TValue> Carousel<TValue>(
		IEnumerable<TValue> values,
		int start = 0)
		=> new CarouselValueProvider<TValue>(values, start);

	/// <inheritdoc cref="RepeatingCarousel{TValue}(IEnumerable{ValueTuple{TValue, int}}, int)"/>
	public static ValueProvider<TValue> RepeatingCarousel<TValue>(
		params (TValue Value, int Repeats)[] tuples)
		=> new CarouselValueProvider<TValue>(CreateRepeatingSequence(tuples));

	/// <inheritdoc cref="CarouselValueProvider{T}.CarouselValueProvider(IEnumerable{T}, int)" path="/param[@name='start']"/>
	/// <returns>
	/// <see cref="CarouselValueProvider{T}"/> from <paramref name="tuples"/>
	/// with each element repeated the specified number of times.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<TValue> RepeatingCarousel<TValue>(
		IEnumerable<(TValue Value, int Repeats)> tuples,
		int start = 0)
		=> new CarouselValueProvider<TValue>(CreateRepeatingSequence(tuples), start);

	/// <inheritdoc cref="FloatRangeCarousel(float, float, float, int)"/>
	public static ValueProvider<int> IntRangeCarousel(
		int from,
		int to,
		int step = 1,
		int start = 0)
		=> new CarouselValueProvider<int>(CreateIntRangeSequence(from, to, step), start);

	/// <inheritdoc cref="CarouselValueProvider{T}.CarouselValueProvider(IEnumerable{T}, int)" path="/param[@name='start']"/>
	/// <returns>
	/// <see cref="CarouselValueProvider{T}"/> of values extracted from range
	/// [<paramref name="from"/>; <paramref name="to"/>] with specified <paramref name="step"/>.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<float> FloatRangeCarousel(
		float from,
		float to,
		float step = 1,
		int start = 0)
		=> new CarouselValueProvider<float>(CreateFloatRangeSequence(from, to, step), start);

	/// <param name="start">
	/// The first returned boolean value.
	/// </param>
	/// <returns>
	/// <see cref="CarouselValueProvider{T}"/> of <see cref="bool"/> values.
	/// </returns>
	public static ValueProvider<bool> BoolCarousel(bool start = true)
		=> new CarouselValueProvider<bool>(BoolValues, start ? 0 : 1);

	#endregion

	#region BackForth

	/// <inheritdoc cref="BackForth{TValue}(IEnumerable{TValue}, int, bool, bool)"/>
	public static ValueProvider<TValue> BackForth<TValue>(
		params TValue[] values)
		=> new BackForthValueProvider<TValue>(values);

	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='start']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='startReversed']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='repeatEnds']"/>
	/// <returns>
	/// <see cref="BackForthValueProvider{T}"/> from <paramref name="values"/>.
	/// </returns>
	public static ValueProvider<TValue> BackForth<TValue>(
		IEnumerable<TValue> values,
		int start = 0,
		bool startReversed = false,
		bool repeatEnds = false)
		=> new BackForthValueProvider<TValue>(values, start, startReversed, repeatEnds);

	/// <inheritdoc cref="RepeatingBackForth{TValue}(IEnumerable{ValueTuple{TValue, int}}, int, bool, bool)"/>
	public static ValueProvider<TValue> RepeatingBackForth<TValue>(
		params (TValue Value, int Repeats)[] tuples)
		=> new BackForthValueProvider<TValue>(CreateRepeatingSequence(tuples));

	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='start']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='startReversed']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='repeatEnds']"/>
	/// <returns>
	/// <see cref="BackForthValueProvider{T}"/> from <paramref name="tuples"/>
	/// with each element repeated the specified number of times.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<TValue> RepeatingBackForth<TValue>(
		IEnumerable<(TValue Value, int Repeats)> tuples,
		int start = 0,
		bool startReversed = false,
		bool repeatEnds = false)
		=> new BackForthValueProvider<TValue>(CreateRepeatingSequence(tuples), start, startReversed, repeatEnds);

	/// <inheritdoc cref="FloatRangeBackForth(float, float, float, int, bool, bool)"/>
	public static ValueProvider<int> IntRangeBackForth(
		int from,
		int to,
		int step = 1,
		int start = 0,
		bool startReversed = false,
		bool repeatEnds = false)
		=> new BackForthValueProvider<int>(CreateIntRangeSequence(from, to, step), start, startReversed, repeatEnds);

	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='start']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='startReversed']"/>
	/// <inheritdoc cref="BackForthValueProvider{T}.BackForthValueProvider(IEnumerable{T}, int, bool, bool)" path="/param[@name='repeatEnds']"/>
	/// <returns>
	/// <see cref="BackForthValueProvider{T}"/> of values extracted from range
	/// [<paramref name="from"/>; <paramref name="to"/>] with specified <paramref name="step"/>.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<float> FloatRangeBackForth(
		float from,
		float to,
		float step = 1,
		int start = 0,
		bool startReversed = false,
		bool repeatEnds = false)
		=> new BackForthValueProvider<float>(CreateFloatRangeSequence(from, to, step), start, startReversed, repeatEnds);

	#endregion

	#region Shuffle

	/// <inheritdoc cref="Shuffle{TValue}(IEnumerable{TValue}, bool)"/>
	public static ValueProvider<TValue> Shuffle<TValue>(
		params TValue[] values)
		=> new ShuffleValueProvider<TValue>(values);

	/// <inheritdoc cref="ShuffleValueProvider{T}.ShuffleValueProvider(IEnumerable{T}, bool)" path="/param[@name='reshuffle']"/>
	/// <returns>
	/// <see cref="ShuffleValueProvider{T}"/> from <paramref name="values"/>.
	/// </returns>
	public static ValueProvider<TValue> Shuffle<TValue>(
		IEnumerable<TValue> values,
		bool reshuffle = true)
		=> new ShuffleValueProvider<TValue>(values, reshuffle);

	/// <inheritdoc cref="RepeatingShuffle{TValue}(IEnumerable{ValueTuple{TValue, int}}, bool)"/>
	public static ValueProvider<TValue> RepeatingShuffle<TValue>(
		params (TValue Value, int Repeats)[] tuples)
		=> new ShuffleValueProvider<TValue>(CreateRepeatingSequence(tuples));

	/// <inheritdoc cref="ShuffleValueProvider{T}.ShuffleValueProvider(IEnumerable{T}, bool)" path="/param[@name='reshuffle']"/>
	/// <returns>
	/// <see cref="ShuffleValueProvider{T}"/> from <paramref name="tuples"/>
	/// with each element repeated the specified number of times.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<TValue> RepeatingShuffle<TValue>(
		IEnumerable<(TValue Value, int Repeats)> tuples,
		bool reshuffle = true)
		=> new ShuffleValueProvider<TValue>(CreateRepeatingSequence(tuples), reshuffle);

	/// <inheritdoc cref="FloatRangeShuffle(float, float, float, bool)"/>
	public static ValueProvider<int> IntRangeShuffle(
		int from,
		int to,
		int step = 1,
		bool reshuffle = true)
		=> new ShuffleValueProvider<int>(CreateIntRangeSequence(from, to, step), reshuffle);

	/// <inheritdoc cref="ShuffleValueProvider{T}.ShuffleValueProvider(IEnumerable{T}, bool)" path="/param[@name='reshuffle']"/>
	/// <returns>
	/// <see cref="ShuffleValueProvider{T}"/> of values extracted from range
	/// [<paramref name="from"/>; <paramref name="to"/>] with specified <paramref name="step"/>.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static ValueProvider<float> FloatRangeShuffle(
		float from,
		float to,
		float step = 1,
		bool reshuffle = true)
		=> new ShuffleValueProvider<float>(CreateFloatRangeSequence(from, to, step), reshuffle);

	/// <inheritdoc cref="ShuffleValueProvider{T}.ShuffleValueProvider(IEnumerable{T}, bool)" path="/param[@name='reshuffle']"/>
	/// <returns>
	/// <see cref="ShuffleValueProvider{T}"/> of boolean values.
	/// </returns>
	public static ValueProvider<bool> BoolShuffle(
		bool reshuffle = true)
		=> new ShuffleValueProvider<bool>(BoolValues, reshuffle);

	#endregion

	#region Helpers

	/// <returns>
	/// Flat sequence in which every value from <paramref name="tuples"/> is repeated the
	/// specified number of times.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	private static IEnumerable<TValue> CreateRepeatingSequence<TValue>(IEnumerable<(TValue Value, int Repeats)> tuples)
		=> tuples.SelectMany(t => Enumerable.Repeat(t.Value, t.Repeats));

	/// <inheritdoc cref="CreateFloatRangeSequence(float, float, float)"/>
	private static List<int> CreateIntRangeSequence(int from, int to, int step)
	{
		ArgumentOutOfRangeException.ThrowIfZero(step, nameof(step));
		if (step > 0) ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(from, to, nameof(from));
		else ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(from, to, nameof(from));

		var list = new List<int>();
		var next = from;
		while ((step > 0 && next <= to) || (step < 0 && next >= to))
		{
			list.Add(next);
			next += step;
		}

		return list;
	}

	/// <returns>
	/// List of numbers from range [<paramref name="from"/>; <paramref name="to"/>] extracted
	/// with <paramref name="step"/>.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException"/>
	private static List<float> CreateFloatRangeSequence(float from, float to, float step)
	{
		ArgumentOutOfRangeException.ThrowIfZero(step, nameof(step));
		if (step > 0) ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(from, to, nameof(from));
		else ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(from, to, nameof(from));

		var list = new List<float>();
		var next = from;
		while ((step > 0 && next <= to) || (step < 0 && next >= to))
		{
			list.Add(next);
			next += step;
		}

		return list;
	}

	#endregion
}
