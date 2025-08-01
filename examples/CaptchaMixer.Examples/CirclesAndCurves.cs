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

namespace Kaspersky.CaptchaMixer.Examples;

/// <summary>
/// A relatively simple example:<br/>
/// - White background<br/>
/// - Random black circles with high transparency<br/>
/// - Captcha characters of different twice-repeating colors<br/>
/// - Random colored curves
/// </summary>
public class CirclesAndCurves : ICaptchaMixerTemplate
{
	public void ApplyTo(CaptchaMixerBuilder builder)
	{
		const string VectorCircles = "circles";
		const string VectorChars = "chars";
		const string VectorCurves = "curves";

		// store sizes to use them for various calculations - this lets the
		// template adaptive for different captcha image sizes.
		var width = builder.Width;
		var height = builder.Height;

		// this area will be used to create noise in background and foreground.
		// it's a rectangle bigger than the image for noise to overflow in all directions.
		var noiseArea = new RectangleF(-width, -height, width * 2, height * 2);

		// this color will be used for both captcha letters and foreground noise.
		var color =
			// 0.6 brightness, random hue and relatively high saturation.
			RandomColor(0.6f, RandomHue(), RandomFloat(0.5f, 1), 255)
			// repeat this color twice, so neighbouring captcha letters will have the
			// same color - this will make them less distinguishable. two consequent noise
			// curves will also be painted using the same color - not a big deal.
			.Repeat(2);

		// most processors added to the layers have constructors accepting the most useful
		// arguments. but for sake of example all properties here are set explicitly.
		builder
			.AddVectorProcessors(
				VectorCircles,
				// throw some random ovals on this layer.
				new RandomOvals
				{
					// the bigger captcha image - the more noise objects needed.
					Count = RandomInt(width * height / 100, width * height / 30),
					// create not just ovals, but circles. radius is evaluated twice
					// per oval - for X and Y, so a circle is done by repeating it.
					Radius = RandomFloat(1, 20).Repeat(2),
					// rectangle for circles coordinates generation.
					Area = noiseArea
				},
				// RandomOvals creates a single vector object with multiple ovals inside.
				// rendering on raster layer evaluates painting parameters per object, so
				// in order to paint each circle differently, we need them separated.
				new SplitObjects())
			.AddVectorProcessors(
				VectorChars,
				// creates a vector object for each captcha answer character.
				new CaptchaChars
				{
					// well-known fonts constants contain cross-platform font families,
					// so may be safely universally used. the font is evaluated for each
					// character, so their fonts will vary.
					Font = RandomWellKnownFont(FontStyle.Bold)
				},
				// vectorized captcha chars are all initially placed somewhere around the
				// origin (0, 0) point. so they always require some arrangement strategy.
				// first we snap them. we do not use any constructor arguments, so the
				// processor uses its default values which are: snap the object's left-top
				// corner to layer's left-top corner.
				new SnapObjects
				{
					// we also add an Y offset from base point (layer left-top) to move all
					// characters below the top boundary of image.
					BasePointOffsetY = height * 0.26f
				},
				// captcha characters initially have some default size which is usually way
				// smaller then the image needs, so resize them.
				new ResizeObjects
				{
					// setting only height means proportional resizing based on height.
					// the height is 0.48 of image height because we've earlier snapped the objects
					// with 0.26 Y offset. 1 - 0.26 * 2 = 0.48, so our chars are now centered by Y.
					Height = height * 0.48f,
					// the base point for resize is the object's left-top corner.
					BasePoint = BasePointType.ObjectLeftTop
				},
				// distributes objects along X axis so they fill the entire width
				new JustifyObjectsX
				{
					// we will shake the letters up a little bit later, so padding from both ends
					// helps us prevent letters from moving outside of the image box.
					Padding = width * 0.05f
				},
				// some random rotation.
				new RotateObjects
				{
					Angle = RandomMirroredFloat(30)
				},
				// some random scaling.
				new ScaleObjects
				{
					Scale = RandomFloat(1f, 1.4f)
				},
				// some random position shift.
				new MoveObjects
				{
					MoveX = RandomMirroredFloat(width * 0.02f),
					MoveY = RandomMirroredFloat(height * 0.07f)
				})
			.AddVectorProcessors(
				VectorCurves,
				// totally random Bezier curves.
				new RandomCurves
				{
					Count = RandomInt(3, 8),
					Points = RandomInt(3, 6),
					Area = noiseArea
				},
				// split for the same reason we've splitted ovals above - to variate painting style.
				new SplitObjects())
			.AddMasterProcessors(
				// fills the entire image with some color.
				new Fill
				{
					// not using constructor for particularly fill/draw processors forces us to
					// explicitly define the paint color which is cumbersome. don't do it this way
					// unless you need something more complex then just a solid color.
					Color = new SolidPaintColor(CMColor.White)
				},
				// draw ovals noise.
				new DrawVectorLayer(
					VectorCircles,
					new StrokePaintInfo
					{
						// paint random circles in background with black color of random transparency.
						Color = new SolidPaintColor(RandomGrayscaleColor(0, RandomInt(35, 70))),
						// stroke width is also randomized.
						Width = RandomFloat(1, 3)
					}),
				// draw captcha characters.
				new DrawVectorLayer(
					VectorChars,
					new FillPaintInfo
					{
						Color = new SolidPaintColor(color)
					}),
				// draw random curves.
				new DrawVectorLayer(
					VectorCurves,
					new StrokePaintInfo
					{
						Color = new SolidPaintColor(color),
						Width = RandomFloat(1, 2)
					}));
	}
}
