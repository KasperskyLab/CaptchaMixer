# Getting started

Right off the bat. Let's render some text-image CAPTCHA.

Create a new console application with top-level statements enabled, then add the `CaptchaMixer` NuGet package and paste this code into `Program.cs`:

```csharp
using Kaspersky.CaptchaMixer;

using static Kaspersky.CaptchaMixer.ValueProviderFactory;

var captcha =
    // don't forget to replace this with normal builder for production
    new CaptchaMixerDebugBuilder(340, 80, "debug-builder", 2)
    .AddVectorProcessors(
        "lines",
        new RandomLines(RandomInt(50, 80), 2),
        new SplitObjects())
    .AddVectorProcessors(
        "chars",
        new CaptchaChars(RandomWellKnownThickFont()),
        new SnapObjects(),
        new MoveObjects(0, 10),
        new ResizeObjects(null, 60, BasePointType.ObjectLeftTop),
        new JustifyObjectsX(10))
    .AddMasterProcessors(
        new Fill(CMColor.Black),
        new DrawVectorLayer(
            "lines",
            new StrokePaintInfo(
                RandomColor(0.4f, 255),
                RandomFloat(0.5f, 2))),
        new DrawVectorLayer(
            "chars",
            new StrokePaintInfo(
                RandomColor(0.6f, 255),
                2)))
    .Build()
    .CreateCaptcha("MYCAPTCHA");

File.WriteAllBytes("test.png", captcha.Data);
```

Run the program. If everything's fine, you'll find a file `test.png` next to the your program's executable:

![test.png](getting-started\test.png "test.png")

Not bad already!

To understand how this has been generated let's view the contents of `debug-builder` directory.
There we see debug PNG files whose names contain layer index and name, processor index and name.

First we see that on the vector layer `lines` some lines have appeared.

![0_Vector00_lines_00_RandomLines_1.png](getting-started\0_Vector00_lines_00_RandomLines_1.png)

Then those lines which all used to compose a single vector object are split into many objects.
Debug renderer indicates this with different boundary lines: gray dashed lines denote objects, blue smaller-dashed lines denote individual paths within objects.

We've split those lines into different objects because we want them to have different colors during rendering and `DrawVectorLayer` raster processor evaluates color on per-object basis.

![0_Vector00_lines_01_SplitObjects_1.png](getting-started\0_Vector00_lines_01_SplitObjects_1.png)

Then we proceed to the second vector layer `chars`.
The set of processors within that layer is something quite common when rendering text-image CAPTCHAs.
The first processor `CaptchaChars` vectorizes answer characters but does not arrange them in any way, so by default they reside somewhere around the left-top corner.
So next processors do the arrangement.

Initial characters somewhere left-topped:

![0_Vector01_chars_00_CaptchaChars_1.png](getting-started\0_Vector01_chars_00_CaptchaChars_1.png)

Snap to the left-top corner or the image:

![0_Vector01_chars_01_SnapObjects_1.png](getting-started\0_Vector01_chars_01_SnapObjects_1.png)

Move along Y axis to 10px:

![0_Vector01_chars_02_MoveObjects_1.png](getting-started\0_Vector01_chars_02_MoveObjects_1.png)

Resize proportionally to 60px height so they appear vertically centered:

![0_Vector01_chars_03_ResizeObjects_1.png](getting-started\0_Vector01_chars_03_ResizeObjects_1.png)

And finally distribute along X axis with the same 10px padding we've used for Y axis:

![0_Vector01_chars_04_JustifyObjectsX_1.png](getting-started\0_Vector01_chars_04_JustifyObjectsX_1.png)

Notice how the dots differ. Green dots are normal contour points, while orange are Bezier curves control points.

Next are raster processors of the master layer. First of all the `Fill` processor does its job:

![1_Raster00_master_00_Fill_1.png](getting-started\1_Raster00_master_00_Fill_1.png)

Then the noise lines are drawn:

![1_Raster00_master_01_DrawVectorLayer_1.png](getting-started\1_Raster00_master_01_DrawVectorLayer_1.png)

And finally the CAPTCHA characters:

![1_Raster00_master_02_DrawVectorLayer_1.png](getting-started\1_Raster00_master_02_DrawVectorLayer_1.png)