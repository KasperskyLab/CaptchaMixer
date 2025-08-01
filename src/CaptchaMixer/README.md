# CaptchaMixer

CaptchaMixer is a CAPTCHA image generation engine for .NET.

CAPTCHA generation algorithm is described as a pipeline consisting of vector and raster processors.
Processors and vector/raster image layers may be mixed in various ways to compose unique and completely differently looking CAPTCHA images.

Key features:

- **Well-packed**: comes with dozens of vector and raster processors to start creating CAPTCHAs with
- **Simple visual randomization**: processors consume library's own `ValueProvider`s instead of plain values, and the static `ValueProviderFactory` helps even more by providing numerous shorthand methods
- **Extensibility**: add your own vector and raster processors by implementing trivial interfaces and/or by using various base classes and helper methods
- **Templates support**: take out generation pipeline parts or even complete pipelines into `ICaptchaMixerTemplate`s to easily reuse them
- **Development facilitation**: use the `CaptchaMixerDebugBuilder` to preview how your images are being built step by step
- **For math lovers**: the `VectorMath` static class with vector utility methods (Bezier curves, lines, points etc.)
- **Cross-platform**: thanks to [SkiaSharp](https://github.com/mono/SkiaSharp) under the hood
- **Well-documented**: most library's APIs are augmented by detailed code documentation

See [the project's page at GitHub](https://github.com/KasperskyLab/CaptchaMixer) for more information and usage documentation.