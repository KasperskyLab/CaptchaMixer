# Value providers

In order to simplify supplying random or otherwise non-constant values the library widely uses it's own `ValueProvider` class and it's descendants.

## Factory

It is not intended to use those classes directly.
Instead use the numerous shorthand methods provided by the static `ValueProviderFactory` class:

```csharp
using static Kaspersky.CaptchaMixer.ValueProviderFactory;
```

This frees you from having to deal with value provider classes directly, for example:

```csharp
RandomOf(1, 5, 9); // one random number of these
RandomInt(4); // random int in range [0; 4)
RandomInt(-5, 4); // random int in range [-5; 4)
RandomFloat(2f); // random float in range [0; 2)
RandomFloat(-3f, 2f); // random float in range [-3; 2)
RandomMirroredFloat(2f); // random float in range [-2; 2)

WeightedRandomOf((2, 1), (4, 2), (7, 3)); // probabilities are 1/6 for 2, 2/6 for 4 and 3/6 for 7

Carousel(4, 2, 1); // provides: 4, 2, 1, 4, 2, 1 and so on
RepeatingCarousel((8, 2), (9, 1), (0, 3)); // provides: 8, 8, 9, 0, 0, 0, 8, 8, 9, 0, 0, 0 and so on
IntRangeCarousel(5, 7); // provides: 5, 6, 7, 5, 6, 7 and so on

BackForth(5, 3, 0); // provides: 5, 3, 0, 3, 5, 3, 0 and so on
RepeatingBackForth((6, 1), (4, 3), (2, 2)); // provides: 6, 4, 4, 4, 2, 2, 2, 4, 4, 4, 6 and so on
FloatRangeBackForth(3, 4, 0.5f); // provides: 3, 3.5, 4, 3.5, 3, 3.5, 4 and so on

Shuffle(1, 2, 3, 4); // provides in random order, then reshuffles and provides in new order
IntRangeShuffle(6, 9); // same as above, for numbers 6, 7, 8 and 9

RandomColor(0.5f, RandomHue(), 0.8f, 255); // 0.5 brightness, random hue, 0.8 saturation, no transparency
RandomGrayscaleColor(); // any grayscale color with no transparency

RandomWellKnownFont(); // any random font from built-in cross-platform families list
```

## Advanced usages

### Repeating values

Only switch to the next value after returning the current value several times.

```csharp
Carousel(4, 2, 1).Repeat(2); // 4, 4, 2, 2, 1, 1, 4, 4, 2, 2, 1, 1 and so on
```

### Converting values

Returns a value provider which converts the values returned by another provider.

```csharp
IntRangeCarousel(2, 4).Convert(i => (float)i / 2); // 1, 1.5, 2, 1, 1.5, 2 and so on
```

### Variating processors

One way to increase the automated recognition resistance level of your CAPTCHAs is to variate their look.

Use `VectorProcessorsSelector` or `RasterProcessorsSelector` to select a processor on each image generation.
These proxies use a `ValueProvider<TProcessor>` parameter, so you can use `ValueProviderFactory`'s shorthand methods to define the selection strategy.

```csharp
// noise type may vary between generations
captchaMixerBuilder
    .AddVectorProcessors(
        "some-noise",
        new VectorProcessorsSelector(
            RandomOf<IVectorProcessor>(
                new RandomLines(),
                new RandomCurves(),
                new RandomOvals())));
```

### Probability-based processor call

```csharp
// 25% of times all image pixels' colors will be inverted
captchaMixerBuilder
    .AddMasterProcessors(
        new InvertColors().WithProbability(0.25));
```

### Value switches

Normally a value provider returns a new value on each call.
But there are many situations when one needs to only shift the value in certain moments.

```csharp
var provider = Carousel(8, 4, 1).Switch();
provider.GetNext(); // returns 8
provider.GetNext(); // still returns 8
provider.NextValue();
provider.GetNext(); // now returns 4
provider.GetNext(); // still returns 4
provider.NextValue();
provider.GetNext(); // now returns 1
```

The `ValueProviderSwitcher<T>` - a proxy behind `Switch()` method - implements both `IVectorProcessor` and `IRasterProcessor` interfaces, thus allowing the value provider act as part of image generation pipelines.

```csharp
// we want to use a random font for characters, but only one for each captcha.
// unfortunately CaptchaChars evaluates font for each character, so we'll "freeze" it
// for entire pipeline.
var font = RandomWellKnownThinFont().Switch();

captchaMixerBuilder
    .AddVectorProcessors(
        "captcha-chars",
        font, // switches to next value only here
        new CaptchaChars(font));
```