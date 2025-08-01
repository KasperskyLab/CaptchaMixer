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

## Philosophy

The library is not bundled with ready-to-use CAPTCHA templates.

**It is implied that you will create your own templates.**

There are a couple of text-image templates in `CaptchaMixer.Examples` project, but they are, well, examples.
We believe that for sake of better security CAPTCHA templates shall not be open-sourced.

See [security considerations](docs/security-considerations.md) for more details and recommendations.

## Examples of what you can achieve

**These are proprietary Kaspersky templates, they are not included.**

But that is what we've managed to do for our needs using stock processors only. So you can do something similar or even better.

![example-01.png](docs\examples\example-01.png)
![example-02.png](docs\examples\example-02.png)
![example-03.png](docs\examples\example-03.png)
![example-04.png](docs\examples\example-04.png)
![example-05.png](docs\examples\example-05.png)
![example-06.png](docs\examples\example-06.png)
![example-07.png](docs\examples\example-07.png)
![example-08.png](docs\examples\example-08.png)
![example-09.png](docs\examples\example-09.png)
![example-10.png](docs\examples\example-10.png)
![example-11.png](docs\examples\example-11.png)
![example-12.png](docs\examples\example-12.png)
![example-13.png](docs\examples\example-13.png)
![example-14.png](docs\examples\example-14.png)
![example-15.png](docs\examples\example-15.png)
![example-16.png](docs\examples\example-16.png)

## Further reading

- [Getting started](docs/getting-started.md) - creating your first CAPTCHA
- [CAPTCHA anatomy](docs/anatomy.md) - image generation pipeline and layers structure
- [Processors](docs/processors.md) - out-of-the-box processors and implementing your own
- [Value providers](docs/value-providers.md) - randomizing and otherwise controlling behavior of processors and image generation pipelines
- [Templates](docs/templates.md) - using templates
- [Performance and multithreading](docs/performance.md)
- [Security considerations](docs/security-considerations.md) - automated recognition precaution and general recommendations on using CAPTCHAs
- [Building](docs/build.md) - guide on building the solution for contributors