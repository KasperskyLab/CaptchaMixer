# Performance and multithreading

## Performance

### General information

The library currently uses only single-threaded CPU calculations while executing it's generation pipeline.

During a single-threaded performance test the heaviest image templates we had took on average ~20-25ms to generate on a pretty old (10+ years) mediocre-powered machine (Intel i7-4790 3.60 GHz, 24GB of DDR3 1600 MHz RAM).

`SkiaSharp` - the rendering engine behind the scenes - may involve hardware acceleration, but is only used for 2D graphics rendering.
Benchmarks and code profiling showed that SkiaSharp is almost never a significant bottleneck of this system.
However, improper generation pipeline design may easily flood SkiaSharp with unneccessary work.
The library contains no any _smart_ logic to figure out and eliminate consequences of suboptimal pipeline programming. So don't mess to much.

### Vector processing

Most vector data operations - such as generating simple new objects, moving, rotating, resizing, altering composition, distorting, etc. - are farely lightweight and fast.

But certain things are way heavier and usage of some vector instructions may complicate even otherwise simple operations.

#### Skeletonizer

First of all, one of the heaviest things is the _path skeletonizing algorithm_, which is utilized by the `SkeletonizedCaptchaChars` processor but also available for arbitrary usage via the `Skeletonizer` static class.
Skeletonizer has been developed from scratch for a very specific purpose - transforming vectorized font characters from a prepared-for-fill-painting default form into a prepared-for-stroke-painting form, such that the characters are literally drawn with single lines (like when you write them with a pen by hand) instead of being outlined.

Skeletonization process involves actual rendering of the vectors, then analyzing and step-by-step cleaning the raster image until only lines of 1 pixel width remain, and then transforming those lines into vector path instructions.
The process is so heavy that the `SkeletonizedCaptchaChars` processor involves caching to avoid rerunning skeletonization when possible.

#### Rational Bezier curves

SkiaSharp, despite being a super cool and efficient library, lacks full-scale support for so-called rational Bezier curves - it only supports one specific case of such curves - a _conic_ curve, which is not always sufficient.
So the support for rational curves has been implemented as part of our library.
In order to paint those curves with SkiaSharp, we transform them into sequences of straight lines small enough for the human eye to be unable to catch the difference in most cases.

The math behind those transformations is in many cases quite complex and involves a lot of memory operations.
While we did benchmark those things and have gone through multiple iterations of optimization, it's still far from being lightweight.

Rational curves are utilized by the library itself when it needs to disassemble (convert into multiple simpler instructions) an `AddOvalInstruction`.
So if, for example, you have created a massive grid with the `OvalsGrid` processor and then used the `Disassemble` processor - you may face a serious performance drop.

Rule of thumb: if you can avoid rational curves - avoid them.

#### Granulation

Granulation is a process of splitting vector instructions into smaller instructions without changing the path trajectory.
It is very useful for purposes of CAPTCHA generation since it allows you to apply distortion to vector graphics.
But keep in mind that granulation, apart from being memory-intensive (many-many new objects created), also requires heavy math for splitting Bezier curves - especially rational curves (hello again).

So do not overuse it and do not use too small linear segment length limits.

### Raster processing

Since the library uses single-threaded CPU calculations, raster processing is usually a plain cycle over all pixels of the image.
While cycling over them is by itself not expensive, as soon as coloring operations using built-in `CMColor` start taking place - everything changes.

`CMColor` is _kind of a smart thing_ to work with colors in context of CAPTCHA image generation.
It allows you for example to change the color's hue while trying to preserve its brightness and saturation which makes your images more resistant to grayscale convertion - one of many techniques used during automated recognition.
But smart things come at a price of performance.

Built-in raster processors, when possible, do not use `CMColor`.
Among others most only utilize its non-heavy functions.

The one which stands out is the `HueShift` processor.
It's sole purpose is to perform exactly the described smart hue shift for the entire image.
However, in most cases, you can avoid using it - instead perform the same hue shift on colors beforehand and then use the colors to paint your vectors.

## Multithreading

It would be quite fair to say that **pretty much nothing of the core classes is concurrent or even thread-safe**.

So use one `CaptchaMixer` from a single thread only.