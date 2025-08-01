# Templates

The `ICaptchaMixerTemplate` interface defines a single `ApplyTo` method which applies changes onto a `CaptchaMixerBuilder` instance.

A template in general is not limited to defining complete generation pipelines only - it may as well define just parts of it, allowing full-scale templates to reuse the common pipeline parts.

```csharp
class BackgroundNoisePartialTemplate : ICaptchaMixerTemplate
{
    public const string Name = "background-noise";

    public void ApplyTo(CaptchaMixerBuilder builder)
    {
        builder.AddVectorProcessors(
            Name,
            new VectorProcessorsSelector(
                RandomOf<IVectorProcessor>(
                    new RandomLines(),
                    new RandomCurves(),
                    new RandomOvals())));
    }
}

class CaptchaCharsPartialTemplate : ICaptchaMixerTemplate
{
    public const string Name = "captcha-chars";

    public void ApplyTo(CaptchaMixerBuilder builder)
    {
        const int offset = 10;
        builder.AddVectorProcessors(
            Name,
            new CaptchaChars(RandomWellKnownThickFont()),
            new SnapObjects(),
            new MoveObjects(0, offset),
            new ResizeObjects(null, builder.Height - offset * 2, BasePointType.ObjectLeftTop),
            new JustifyObjectsX(offset));
    }
}

class FullScaleTemplate : ICaptchaMixerTemplate
{
    public void ApplyTo(CaptchaMixerBuilder builder)
    {
        builder
            .ApplyTemplate<BackgroundNoisePartialTemplate>()
            .ApplyTemplate<CaptchaCharsPartialTemplate>()
            .AddMasterProcessors(
                new Fill(CMColor.White),
                new DrawVectorLayer(
                    BackgroundNoisePartialTemplate.Name,
                    new StrokePaintInfo(CMColor.Blue, 1)),
                new DrawVectorLayer(
                    CaptchaCharsPartialTemplate.Name,
                    new StrokePaintInfo(CMColor.Red, 1)));
    }
}
```