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

using Kaspersky.CaptchaMixer.Examples;

namespace Kaspersky.CaptchaMixer.UnitTests.Builders;

public class CaptchaMixerBuilderTests
{
	[Test]
	public void CaptchaMixerBuilder_Constructor_Default_Test()
	{
		const int width = 100;
		const int height = 30;

		var builder = new CaptchaMixerBuilder(width, height);
		builder.Width.Should().Be(width);
		builder.Height.Should().Be(height);
		builder.Size.Should().Be(new Size(width, height));

		var captcha = builder
			.AddMasterProcessors(new Fill(CMColor.White))
			.Build()
			.CreateCaptcha(null);

		// PNG header
		captcha.Data.Should().ContainInConsecutiveOrder(137, 80, 78, 71, 13, 10, 26, 10);
	}

	[Test]
	public void CaptchaMixerBuilder_Constructor_NonDefaultEncoder_Test()
	{
		var captcha = new CaptchaMixerBuilder(100, 30, CaptchaImageFormat.Jpeg)
			.AddMasterProcessors(new Fill(CMColor.White))
			.Build()
			.CreateCaptcha(null);

		// JPEG header
		captcha.Data.Should().ContainInConsecutiveOrder(255, 216, 255);
	}

	[Test]
	public void CaptchaMixerBuilder_Constructor_NoEncoder_Test()
	{
		var captcha = new CaptchaMixerBuilder(100, 30, CaptchaImageFormat.None)
			.AddMasterProcessors(new Fill(CMColor.White))
			.Build()
			.CreateCaptcha(null);

		// no format header, raw data
		captcha.Data.Should().ContainInConsecutiveOrder(255, 255, 255, 255);
	}

	[Test]
	[TestCase(0, 1)]
	[TestCase(-1, 1)]
	[TestCase(1, 0)]
	[TestCase(1, -1)]
	[TestCase(0, 0)]
	[TestCase(-1, -1)]
	public void CaptchaMixerBuilder_Constructor_InvalidSize_Test(int width, int height)
	{
		var action = () => new CaptchaMixerBuilder(width, height);
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Test]
	public void CaptchaMixerBuilder_Build_NoMasterLayer_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30, CaptchaImageFormat.None).Build();
		action.Should().Throw<ArgumentException>();
	}

	[Test]
	public void CaptchaMixerBuilder_Build_EmptyVectorLayer_Test()
	{
		new CaptchaMixerBuilder(100, 30)
			.AddVectorProcessors("empty-vector-layer")
			.AddMasterProcessors(new Fill(CMColor.White))
			.Build()
			.CreateCaptcha(null);
	}

	[Test]
	public void CaptchaMixerBuilder_Build_EmptyRasterLayer_Test()
	{
		new CaptchaMixerBuilder(100, 30)
			.AddRasterProcessors("empty-raster-layer")
			.AddMasterProcessors(new Fill(CMColor.White))
			.Build()
			.CreateCaptcha(null);
	}

	[Test]
	public void CaptchaMixerBuilder_Build_EmptyMasterLayer_Test()
	{
		new CaptchaMixerBuilder(100, 30)
			.AddMasterProcessors()
			.Build()
			.CreateCaptcha(null);
	}

	[Test]
	public void CaptchaMixerBuilder_AddProcessors_Incremental_Test()
	{
		// adding new processors to existing layers
		var captcha = new CaptchaMixerBuilder(2, 2, CaptchaImageFormat.None)
			.AddVectorProcessors(
				"vector-layer",
				new PointsLinePaths(1, 1)
				{
					Points = Carousel(new Vector2(0, 0), new Vector2(1, 0)),
					Close = false
				})
			.AddVectorProcessors(
				"vector-layer",
				new PointsLinePaths(1, 1)
				{
					Points = Carousel(new Vector2(1, 1), new Vector2(2, 1)),
					Close = false
				})
			.AddRasterProcessors(
				"raster-layer",
				new Fill(CMColor.White))
			.AddRasterProcessors(
				"raster-layer",
				new DrawVectorLayer(
					"vector-layer",
					new StrokePaintInfo(CMColor.Black) { Antialiasing = false }))
			.AddMasterProcessors(
				new DrawRasterLayer(
					"raster-layer"))
			.Build()
			.CreateCaptcha(null);

		// 2x2 area filled white
		// line (0, 0) - (1, 0) paints (0, 0) pixel black
		// line (1, 1) - (2, 1) paints (1, 1) pixel black
		captcha.Data.Should().ContainInConsecutiveOrder(
			0, 0, 0, 255,
			255, 255, 255, 255,
			255, 255, 255, 255,
			0, 0, 0, 255);
	}

	[Test]
	public void CaptchaMixerBuilder_AddVectorProcessors_NullLayerName_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddVectorProcessors(null, new Rect());

		action.Should().Throw<ArgumentNullException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddVectorProcessors_NullProcessor_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddRasterProcessors("vector-layer", null);

		action.Should().Throw<ArgumentNullException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddRasterProcessors_NullLayerName_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddRasterProcessors(null, new Fill(CMColor.White));

		action.Should().Throw<ArgumentNullException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddRasterProcessors_NullProcessor_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddRasterProcessors("raster-layer", null);

		action.Should().Throw<ArgumentNullException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddRasterProcessors_Encoder_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddRasterProcessors("raster-layer", new EncodeRasterData(CaptchaImageFormat.Png));

		action.Should().Throw<InvalidOperationException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddMasterProcessors_NullProcessor_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddMasterProcessors(null);

		action.Should().Throw<ArgumentNullException>();
	}

	[Test]
	public void CaptchaMixerBuilder_AddMasterProcessors_Encoder_Test()
	{
		var action = () => new CaptchaMixerBuilder(100, 30)
			.AddMasterProcessors(new EncodeRasterData(CaptchaImageFormat.Png));

		action.Should().Throw<InvalidOperationException>();
	}

	[Test]
	[Timeout(5000)]
	[Retry(3)]
	public void CaptchaMixerBuilder_FromTemplate_Test()
	{
		var mixer = CaptchaMixerBuilder.FromTemplate<CirclesAndCurves>(300, 80);
		Assert.That(mixer, Is.Not.Null);
		var captcha = mixer.CreateCaptcha("ABCDEFGH");
		File.WriteAllBytes($"_{nameof(CirclesAndCurves)}.png", captcha.Data);
	}
}
