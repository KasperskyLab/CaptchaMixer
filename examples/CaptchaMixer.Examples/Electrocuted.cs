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
/// Strongly shaken-up colorful captcha characters.
/// </summary>
public class Electrocuted : ICaptchaMixerTemplate
{
	public void ApplyTo(CaptchaMixerBuilder builder)
	{
		/*
		 * Here we're doing something way more advanced that in the simple example.
		 * We're going to create several vector layers for captcha charaters,
		 * each next layer will contain more and more messed up version of them.
		 * Then we will render those layers from the most to the least messed up
		 * with less and less transparent color. So the shapes of characters will
		 * still mostly be recongnizable for a human, but (hopefully) challenging
		 * for automated recognition.
		 */

		const string VectorChars = "chars";
		const string VectorCharsElectrocuted1 = "chars-electrocuted-1";
		const string VectorCharsElectrocuted2 = "chars-electrocuted-2";
		const string VectorCharsElectrocuted3 = "chars-electrocuted-3";
		const string VectorCharsElectrocuted4 = "chars-electrocuted-4";
		const string VectorCharsMessedUp = "chars-messed-up";

		// store sizes to use them for various calculations - this lets the
		// template be adaptive for different captcha image sizes.
		var width = builder.Width;
		var height = builder.Height;

		// each layer will contain one vector object per character. but the layers are
		// rendered one by one with color evaluated per object, so if we want each
		// character from different layers to have the same color hue, we need color
		// providers that share the color hue sequence.
		// so we first prepare the hue sequence by directly generating random values.
		var hues = RandomHue().Take(10).ToArray();

		// now we create the colors all of which utilize the hue sequence as a base for
		// carousel values provider. carousel moves over the sequence from start to end
		// and then returns to the beginning. we've only generated 10 hue values which
		// effectively limits us to 10 different colors. if captcha text would be longer
		// then the colors would start repeating.
		// also notice the colors having different alpha values.
		var color1 = RandomColor(0.6f, Carousel(hues), 1f, 230);
		var color2 = RandomColor(0.6f, Carousel(hues), 1f, 120);
		var color3 = RandomColor(0.6f, Carousel(hues), 1f, 90);
		var color4 = RandomColor(0.6f, Carousel(hues), 1f, 60);
		var color5 = RandomColor(0.6f, Carousel(hues), 1f, 30);

		// since this example is not just basic, here we're no longer using explicit property
		// names, instead using constructor arguments where possible to shorten the code.
		// this shows how compact a normal template shall be.
		builder
			.AddVectorProcessors(
				VectorChars,
				// for the most part here we do almost the same we were doing in the basic example.
				new CaptchaChars(RandomWellKnownFont(FontStyle.Bold)),
				new SnapObjects { BasePointOffsetY = height * 0.26f },
				new ResizeObjects(null, height * 0.48f, BasePointType.ObjectLeftTop),
				new JustifyObjectsX(width * 0.05f),
				new RotateObjects(RandomMirroredFloat(30)),
				new ScaleObjects(RandomFloat(1f, 1.4f)),
				new MoveObjects(RandomMirroredFloat(width * 0.02f), RandomMirroredFloat(height * 0.07f)),
				// now that's the difference. first we granulate the paths. this makes them
				// consist of smaller segments with linear length not more that 2 pixels.
				new GranulatePaths(2),
				// then we transform all those now-small-segments into straight lines.
				// this way it'll be better for distortions that we will apply later on.
				new ToLines())
			// now the distorted layers.
			// first copy the contents from non-distorted characters layer.
			// then move points randomly. since our paths consist of small sections after granulation,
			// even after such distortion they more or less follow the original shapes.
			// each next layer moves points in bigger coordinate range.
			.AddVectorProcessors(
				VectorCharsElectrocuted1,
				new CopyVectorLayer(VectorChars),
				new MovePoints(RandomMirroredFloat(1)))
			.AddVectorProcessors(
				VectorCharsElectrocuted2,
				new CopyVectorLayer(VectorChars),
				new MovePoints(RandomMirroredFloat(3)))
			.AddVectorProcessors(
				VectorCharsElectrocuted3,
				new CopyVectorLayer(VectorChars),
				new MovePoints(RandomMirroredFloat(6)))
			.AddVectorProcessors(
				VectorCharsElectrocuted4,
				new CopyVectorLayer(VectorChars),
				new MovePoints(RandomMirroredFloat(10)))
			.AddVectorProcessors(
				VectorCharsMessedUp,
				new CopyVectorLayer(VectorChars),
				new MovePoints(RandomMirroredFloat(2)),
				// the last layer is special, we use it to create a real mess in the background.
				// this mess would be barely recognizable but the points themselves in fact would
				// still persist some level of connection to original shapes.
				// to achieve this we use the "straightening" processor, but we use it intentionally
				// incorrectly. as the processor's documentation states, "ratio" values below zero
				// lead to de-straightening instead of straightening. in reality it's even worse -
				// even not-so-big negative values may not just de-straighten the paths, but
				// completely mess them up - and do it in a quite interesting way.
				new StraightenPaths(RandomFloat(-2.5f, -1.5f)))
			.AddMasterProcessors(
				new Fill(CMColor.Black),
				// draw vector layers using corresponding colors
				new DrawVectorLayer(VectorCharsMessedUp, new StrokePaintInfo(color5)),
				new DrawVectorLayer(VectorCharsElectrocuted4, new StrokePaintInfo(color4)),
				new DrawVectorLayer(VectorCharsElectrocuted3, new StrokePaintInfo(color3)),
				new DrawVectorLayer(VectorCharsElectrocuted2, new StrokePaintInfo(color2)),
				new DrawVectorLayer(VectorCharsElectrocuted1, new StrokePaintInfo(color1)),
				// recognition shall sometimes suffer a little bit.
				new AddColors(RandomInt(20)).WithProbability(0.3),
				// also invert colors sometimes - why not?
				new InvertColors().WithProbability(0.1));
	}
}
