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

namespace Kaspersky.CaptchaMixer.UnitTests.Types;

public class CMColorTests
{
	private static readonly byte[] Alphas = new byte[] { 0, 127, 255 };

	private static void AssertComponents(CMColor color, byte r, byte g, byte b, byte a)
		=> color.Should().Be(new CMColor(r, g, b, a));

	private static void AssertComponentsWrite(CMColor color, byte r, byte g, byte b, byte a)
	{
		color.R = r;
		color.G = g;
		color.B = b;
		color.A = a;
		AssertComponents(color, r, g, b, a);
	}

	[Test]
	public void CMColor_Init_Empty_Test()
	{
		var color = new CMColor();
		AssertComponents(color, 0, 0, 0, 0);
		AssertComponentsWrite(color, 10, 11, 12, 13);
	}

	[Test]
	public void CMColor_Init_CMColor_Test()
	{
		var color = new CMColor(new CMColor(10, 11, 12, 13));
		AssertComponents(color, 10, 11, 12, 13);
		AssertComponentsWrite(color, 20, 21, 22, 23);
	}

	[Test]
	public void CMColor_Init_Color_Test()
	{
		var color = new CMColor(Color.FromArgb(13, 10, 11, 12));
		AssertComponents(color, 10, 11, 12, 13);
		AssertComponentsWrite(color, 20, 21, 22, 23);
	}

	[Test]
	public void CMColor_Init_Components_Test()
	{
		var color = new CMColor(10, 11, 12, 13);
		AssertComponents(color, 10, 11, 12, 13);
		AssertComponentsWrite(color, 20, 21, 22, 23);
	}

	[Test]
	[TestCase(-1, 255, 0, 1)]
	[TestCase(0, 255, 0, 0)]
	[TestCase(1, 255, 1, 0)]
	[TestCase(254, 255, 254, 0)]
	[TestCase(255, 255, 255, 0)]
	[TestCase(256, 254, 255, 0)]
	[TestCase(509, 1, 255, 0)]
	[TestCase(510, 0, 255, 0)]
	[TestCase(511, 0, 255, 1)]
	[TestCase(764, 0, 255, 254)]
	[TestCase(765, 0, 255, 255)]
	[TestCase(766, 0, 254, 255)]
	[TestCase(1019, 0, 1, 255)]
	[TestCase(1020, 0, 0, 255)]
	[TestCase(1021, 1, 0, 255)]
	[TestCase(1274, 254, 0, 255)]
	[TestCase(1275, 255, 0, 255)]
	[TestCase(1276, 255, 0, 254)]
	[TestCase(1529, 255, 0, 1)]
	[TestCase(1530, 255, 0, 0)]
	[TestCase(1531, 255, 1, 0)]
	public void CMColor_Init_Hue_Test(int hue, byte r, byte g, byte b)
	{
		var color = new CMColor(hue, 13);
		AssertComponents(color, r, g, b, 13);
		AssertComponentsWrite(color, 20, 21, 22, 23);
	}

	[Test]
	public void CMColor_Init_Attached_Test()
	{
		var data = new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 };

		var color = new CMColor(data, 0);
		AssertComponents(color, 10, 11, 12, 13);

		color.R = 20;
		data.Should().ContainInConsecutiveOrder(20, 11, 12, 13, 14, 15, 16, 17);
		color.G = 21;
		data.Should().ContainInConsecutiveOrder(20, 21, 12, 13, 14, 15, 16, 17);
		color.B = 22;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 13, 14, 15, 16, 17);
		color.A = 23;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 14, 15, 16, 17);
		AssertComponents(color, 20, 21, 22, 23);

		color = new CMColor(data, 4);
		AssertComponents(color, 14, 15, 16, 17);

		color.R = 24;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 15, 16, 17);
		color.G = 25;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 16, 17);
		color.B = 26;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 26, 17);
		color.A = 27;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 26, 27);
		AssertComponents(color, 24, 25, 26, 27);
	}

	[Test]
	public void CMColor_Attach_Test()
	{
		var data = new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 };

		var color = new CMColor();

		color.Attach(data, 0);
		AssertComponents(color, 10, 11, 12, 13);

		color.R = 20;
		data.Should().ContainInConsecutiveOrder(20, 11, 12, 13, 14, 15, 16, 17);
		color.G = 21;
		data.Should().ContainInConsecutiveOrder(20, 21, 12, 13, 14, 15, 16, 17);
		color.B = 22;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 13, 14, 15, 16, 17);
		color.A = 23;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 14, 15, 16, 17);
		AssertComponents(color, 20, 21, 22, 23);

		color.Attach(data, 4);
		AssertComponents(color, 14, 15, 16, 17);

		color.R = 24;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 15, 16, 17);
		color.G = 25;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 16, 17);
		color.B = 26;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 26, 17);
		color.A = 27;
		data.Should().ContainInConsecutiveOrder(20, 21, 22, 23, 24, 25, 26, 27);
		AssertComponents(color, 24, 25, 26, 27);
	}

	[Test]
	[TestCase(0, 0, 0, 0)]
	[TestCase(0, 0, 127, 0.057f)]
	[TestCase(0, 0, 255, 0.115f)]
	[TestCase(0, 127, 0, 0.292f)]
	[TestCase(0, 127, 127, 0.349f)]
	[TestCase(0, 127, 255, 0.406f)]
	[TestCase(0, 255, 0, 0.587f)]
	[TestCase(0, 255, 127, 0.644f)]
	[TestCase(0, 255, 255, 0.701f)]
	[TestCase(127, 0, 0, 0.149f)]
	[TestCase(127, 0, 127, 0.206f)]
	[TestCase(127, 0, 255, 0.263f)]
	[TestCase(127, 127, 0, 0.441f)]
	[TestCase(127, 127, 127, 0.498f)]
	[TestCase(127, 127, 255, 0.555f)]
	[TestCase(127, 255, 0, 0.736f)]
	[TestCase(127, 255, 127, 0.793f)]
	[TestCase(127, 255, 255, 0.85f)]
	[TestCase(255, 0, 0, 0.299f)]
	[TestCase(255, 0, 127, 0.356f)]
	[TestCase(255, 0, 255, 0.413f)]
	[TestCase(255, 127, 0, 0.591f)]
	[TestCase(255, 127, 127, 0.648f)]
	[TestCase(255, 127, 255, 0.705f)]
	[TestCase(255, 255, 0, 0.886f)]
	[TestCase(255, 255, 127, 0.943f)]
	[TestCase(255, 255, 255, 1)]
	public void CMColor_GetBrightness_Test(byte r, byte g, byte b, float v)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(r, g, b, a);
			color.GetBrightness().Should().BeApproximately(v, 0.01f);
		}
	}

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0.1f, 26, 26, 26)]
	[TestCase(0, 0, 0, 0.25f, 64, 64, 64)]
	[TestCase(0, 0, 0, 0.75f, 191, 191, 191)]
	[TestCase(0, 0, 0, 1, 255, 255, 255)]
	[TestCase(0, 0, 255, 0, 0, 0, 0)]
	[TestCase(0, 0, 255, 0.1f, 0, 0, 224)]
	[TestCase(0, 0, 255, 0.25f, 39, 39, 255)]
	[TestCase(0, 0, 255, 0.75f, 183, 183, 255)]
	[TestCase(0, 0, 255, 1, 255, 255, 255)]
	[TestCase(0, 0, 255, 0.114f, 0, 0, 255)]
	[TestCase(0, 255, 0, 0, 0, 0, 0)]
	[TestCase(0, 255, 0, 0.1f, 0, 43, 0)]
	[TestCase(0, 255, 0, 0.25f, 0, 109, 0)]
	[TestCase(0, 255, 0, 0.75f, 101, 255, 101)]
	[TestCase(0, 255, 0, 1, 255, 255, 255)]
	[TestCase(0, 255, 0, 0.587f, 0, 255, 0)]
	[TestCase(255, 0, 0, 0, 0, 0, 0)]
	[TestCase(255, 0, 0, 0.1f, 85, 0, 0)]
	[TestCase(255, 0, 0, 0.25f, 213, 0, 0)]
	[TestCase(255, 0, 0, 0.75f, 255, 164, 164)]
	[TestCase(255, 0, 0, 1, 255, 255, 255)]
	[TestCase(255, 0, 0, 0.299f, 255, 0, 0)]
	public void CMColor_SetBrightness_Test(
		byte rs, byte gs, byte bs,
		float v,
		byte rr, byte gr, byte br)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(rs, gs, bs, a);
			color.SetBrightness(v);
			AssertComponents(color, rr, gr, br, a);
			color.GetBrightness().Should().BeApproximately(v, 0.01f);
		}
	}

	[Test]
	[TestCase(0, 0, 0, 0)]
	[TestCase(0, 0, 127, 0.5f)]
	[TestCase(0, 0, 255, 1)]
	[TestCase(0, 127, 0, 0.5f)]
	[TestCase(0, 127, 127, 0.5f)]
	[TestCase(0, 127, 255, 1)]
	[TestCase(0, 255, 0, 1)]
	[TestCase(0, 255, 127, 1f)]
	[TestCase(0, 255, 255, 1f)]
	[TestCase(127, 0, 0, 0.5f)]
	[TestCase(127, 0, 127, 0.5f)]
	[TestCase(127, 0, 255, 1)]
	[TestCase(127, 127, 0, 0.5f)]
	[TestCase(127, 127, 127, 0)]
	[TestCase(127, 127, 255, 0.5f)]
	[TestCase(127, 255, 0, 1)]
	[TestCase(127, 255, 127, 0.5f)]
	[TestCase(127, 255, 255, 0.5f)]
	[TestCase(255, 0, 0, 1)]
	[TestCase(255, 0, 127, 1)]
	[TestCase(255, 0, 255, 1)]
	[TestCase(255, 127, 0, 1)]
	[TestCase(255, 127, 127, 0.5f)]
	[TestCase(255, 127, 255, 0.5f)]
	[TestCase(255, 255, 0, 1)]
	[TestCase(255, 255, 127, 0.5f)]
	[TestCase(255, 255, 255, 0)]
	public void CMColor_GetSaturation_Test(byte r, byte g, byte b, float v)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(r, g, b, a);
			color.GetSaturation().Should().BeApproximately(v, 0.01f);
		}
	}

	[Test]
	[TestCase(0, 0, 0, 0, 0, 0, 0)]
	[TestCase(0, 0, 0, 0.5f, 128, 0, 0)]
	[TestCase(0, 0, 0, 1, 255, 0, 0)]
	[TestCase(0, 0, 127, 0, 14, 14, 14)]
	[TestCase(0, 0, 127, 0.5f, 0, 0, 128)]
	[TestCase(0, 0, 127, 1, 0, 0, 255)]
	[TestCase(0, 0, 255, 0, 29, 29, 29)]
	[TestCase(0, 0, 255, 0.5f, 14, 14, 142)]
	[TestCase(0, 0, 255, 1, 0, 0, 255)]
	[TestCase(0, 127, 0, 0, 75, 75, 75)]
	[TestCase(0, 127, 0, 0.5f, 0, 128, 0)]
	[TestCase(0, 127, 0, 1, 0, 255, 0)]
	[TestCase(0, 127, 127, 0, 89, 89, 89)]
	[TestCase(0, 127, 127, 0.5f, 0, 128, 128)]
	[TestCase(0, 127, 127, 1, 0, 255, 255)]
	[TestCase(0, 127, 255, 0, 104, 104, 104)]
	[TestCase(0, 127, 255, 0.5f, 52, 116, 180)]
	[TestCase(0, 127, 255, 1, 0, 127, 255)]
	[TestCase(0, 255, 0, 0, 150, 150, 150)]
	[TestCase(0, 255, 0, 0.5f, 75, 202, 75)]
	[TestCase(0, 255, 0, 1, 0, 255, 0)]
	[TestCase(0, 255, 127, 0, 164, 164, 164)]
	[TestCase(0, 255, 127, 0.5f, 82, 210, 146)]
	[TestCase(0, 255, 127, 1, 0, 255, 127)]
	[TestCase(0, 255, 255, 0, 179, 179, 179)]
	[TestCase(0, 255, 255, 0.5f, 90, 217, 217)]
	[TestCase(0, 255, 255, 1, 0, 255, 255)]
	[TestCase(127, 0, 0, 0, 38, 38, 38)]
	[TestCase(127, 0, 0, 0.5f, 128, 0, 0)]
	[TestCase(127, 0, 0, 1, 255, 0, 0)]
	[TestCase(127, 0, 127, 0, 52, 52, 52)]
	[TestCase(127, 0, 127, 0.5f, 128, 0, 128)]
	[TestCase(127, 0, 127, 1, 255, 0, 255)]
	[TestCase(127, 0, 255, 0, 67, 67, 67)]
	[TestCase(127, 0, 255, 0.5f, 97, 34, 161)]
	[TestCase(127, 0, 255, 1, 127, 0, 255)]
	[TestCase(127, 127, 0, 0, 113, 113, 113)]
	[TestCase(127, 127, 0, 0.5f, 128, 128, 0)]
	[TestCase(127, 127, 0, 1, 255, 255, 0)]
	[TestCase(127, 127, 127, 0, 127, 127, 127)]
	[TestCase(127, 127, 127, 0.5f, 191, 64, 64)]
	[TestCase(127, 127, 127, 1, 255, 0, 0)]
	[TestCase(127, 127, 255, 0, 142, 142, 142)]
	[TestCase(127, 127, 255, 0.5f, 127, 127, 255)]
	[TestCase(127, 127, 255, 1, 0, 0, 255)]
	[TestCase(127, 255, 0, 0, 188, 188, 188)]
	[TestCase(127, 255, 0, 0.5f, 158, 222, 94)]
	[TestCase(127, 255, 0, 1, 127, 255, 0)]
	[TestCase(127, 255, 127, 0, 202, 202, 202)]
	[TestCase(127, 255, 127, 0.5f, 127, 255, 127)]
	[TestCase(127, 255, 127, 1, 0, 255, 0)]
	[TestCase(127, 255, 255, 0, 217, 217, 217)]
	[TestCase(127, 255, 255, 0.5f, 127, 255, 255)]
	[TestCase(127, 255, 255, 1, 0, 255, 255)]
	[TestCase(255, 0, 0, 0, 76, 76, 76)]
	[TestCase(255, 0, 0, 0.5f, 166, 38, 38)]
	[TestCase(255, 0, 0, 1, 255, 0, 0)]
	[TestCase(255, 0, 127, 0, 91, 91, 91)]
	[TestCase(255, 0, 127, 0.5f, 173, 46, 109)]
	[TestCase(255, 0, 127, 1, 255, 0, 127)]
	[TestCase(255, 0, 255, 0, 105, 105, 105)]
	[TestCase(255, 0, 255, 0.5f, 180, 52, 180)]
	[TestCase(255, 0, 255, 1, 255, 0, 255)]
	[TestCase(255, 127, 0, 0, 151, 151, 151)]
	[TestCase(255, 127, 0, 0.5f, 203, 139, 76)]
	[TestCase(255, 127, 0, 1, 255, 127, 0)]
	[TestCase(255, 127, 127, 0, 165, 165, 165)]
	[TestCase(255, 127, 127, 0.5f, 255, 127, 127)]
	[TestCase(255, 127, 127, 1, 255, 0, 0)]
	[TestCase(255, 127, 255, 0, 180, 180, 180)]
	[TestCase(255, 127, 255, 0.5f, 255, 127, 255)]
	[TestCase(255, 127, 255, 1, 255, 0, 255)]
	[TestCase(255, 255, 0, 0, 226, 226, 226)]
	[TestCase(255, 255, 0, 0.5f, 240, 240, 113)]
	[TestCase(255, 255, 0, 1, 255, 255, 0)]
	[TestCase(255, 255, 127, 0, 240, 240, 240)]
	[TestCase(255, 255, 127, 0.5f, 255, 255, 127)]
	[TestCase(255, 255, 127, 1, 255, 255, 0)]
	[TestCase(255, 255, 255, 0, 255, 255, 255)]
	[TestCase(255, 255, 255, 0.5f, 255, 128, 128)]
	[TestCase(255, 255, 255, 1, 255, 0, 0)]
	public void CMColor_SetSaturation_Test(
		byte rs, byte gs, byte bs,
		float v,
		byte rr, byte gr, byte br)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(rs, gs, bs, a);
			color.SetSaturation(v);
			AssertComponents(color, rr, gr, br, a);
			color.GetSaturation().Should().BeApproximately(v, 0.01f);
		}
	}

	[Test]
	[TestCase(0, 0, 0, 0)]
	[TestCase(0, 0, 127, 1020)]
	[TestCase(0, 0, 255, 1020)]
	[TestCase(0, 127, 0, 510)]
	[TestCase(0, 127, 127, 765)]
	[TestCase(0, 127, 255, 893)]
	[TestCase(0, 255, 0, 510)]
	[TestCase(0, 255, 127, 637)]
	[TestCase(0, 255, 255, 765)]
	[TestCase(127, 0, 0, 0)]
	[TestCase(127, 0, 127, 1275)]
	[TestCase(127, 0, 255, 1147)]
	[TestCase(127, 127, 0, 255)]
	[TestCase(127, 127, 127, 0)]
	[TestCase(127, 127, 255, 1020)]
	[TestCase(127, 255, 0, 383)]
	[TestCase(127, 255, 127, 510)]
	[TestCase(127, 255, 255, 765)]
	[TestCase(255, 0, 0, 0)]
	[TestCase(255, 0, 127, 1403)]
	[TestCase(255, 0, 255, 1275)]
	[TestCase(255, 127, 0, 127)]
	[TestCase(255, 127, 127, 0)]
	[TestCase(255, 127, 255, 1275)]
	[TestCase(255, 255, 0, 255)]
	[TestCase(255, 255, 127, 255)]
	[TestCase(255, 255, 255, 0)]
	public void CMColor_GetHue_Test(byte r, byte g, byte b, int v)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(r, g, b, a);
			color.GetHue().Should().Be(v);
		}
	}

	[Test]
	public void CMColor_SetHue_Test(
		[Values(0, 127, 255)] int r,
		[Values(0, 127, 255)] int g,
		[Values(0, 127, 255)] int b,
		[Values(0, 255, 510, 765, 1020, 1275)] int v)
	{
		foreach (var a in Alphas)
		{
			var color = new CMColor(r, g, b, a);
			var brightness = color.GetBrightness();
			color.SetHue(v);

			var expected = r != g || g != b || r != b ? v : 0;
			color.GetHue().Should().BeCloseTo(expected, 10);

			color.GetBrightness().Should().BeApproximately(brightness, 0.01f);
		}
	}
}
