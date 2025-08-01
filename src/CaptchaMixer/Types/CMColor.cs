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

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// RGBA color handler.
/// </summary>
public struct CMColor
{
	#region Preset colors

	/// <inheritdoc cref="Color.Empty"/>
	public static CMColor Empty => Color.Empty;
	/// <inheritdoc cref="Color.Transparent"/>
	public static CMColor Transparent => Color.Transparent;
	/// <inheritdoc cref="Color.AliceBlue"/>
	public static CMColor AliceBlue => Color.AliceBlue;
	/// <inheritdoc cref="Color.AntiqueWhite"/>
	public static CMColor AntiqueWhite => Color.AntiqueWhite;
	/// <inheritdoc cref="Color.Aqua"/>
	public static CMColor Aqua => Color.Aqua;
	/// <inheritdoc cref="Color.Aquamarine"/>
	public static CMColor Aquamarine => Color.Aquamarine;
	/// <inheritdoc cref="Color.Azure"/>
	public static CMColor Azure => Color.Azure;
	/// <inheritdoc cref="Color.Beige"/>
	public static CMColor Beige => Color.Beige;
	/// <inheritdoc cref="Color.Bisque"/>
	public static CMColor Bisque => Color.Bisque;
	/// <inheritdoc cref="Color.Black"/>
	public static CMColor Black => Color.Black;
	/// <inheritdoc cref="Color.BlanchedAlmond"/>
	public static CMColor BlanchedAlmond => Color.BlanchedAlmond;
	/// <inheritdoc cref="Color.Blue"/>
	public static CMColor Blue => Color.Blue;
	/// <inheritdoc cref="Color.BlueViolet"/>
	public static CMColor BlueViolet => Color.BlueViolet;
	/// <inheritdoc cref="Color.Brown"/>
	public static CMColor Brown => Color.Brown;
	/// <inheritdoc cref="Color.BurlyWood"/>
	public static CMColor BurlyWood => Color.BurlyWood;
	/// <inheritdoc cref="Color.CadetBlue"/>
	public static CMColor CadetBlue => Color.CadetBlue;
	/// <inheritdoc cref="Color.Chartreuse"/>
	public static CMColor Chartreuse => Color.Chartreuse;
	/// <inheritdoc cref="Color.Chocolate"/>
	public static CMColor Chocolate => Color.Chocolate;
	/// <inheritdoc cref="Color.Coral"/>
	public static CMColor Coral => Color.Coral;
	/// <inheritdoc cref="Color.CornflowerBlue"/>
	public static CMColor CornflowerBlue => Color.CornflowerBlue;
	/// <inheritdoc cref="Color.Cornsilk"/>
	public static CMColor Cornsilk => Color.Cornsilk;
	/// <inheritdoc cref="Color.Crimson"/>
	public static CMColor Crimson => Color.Crimson;
	/// <inheritdoc cref="Color.Cyan"/>
	public static CMColor Cyan => Color.Cyan;
	/// <inheritdoc cref="Color.DarkBlue"/>
	public static CMColor DarkBlue => Color.DarkBlue;
	/// <inheritdoc cref="Color.DarkCyan"/>
	public static CMColor DarkCyan => Color.DarkCyan;
	/// <inheritdoc cref="Color.DarkGoldenrod"/>
	public static CMColor DarkGoldenrod => Color.DarkGoldenrod;
	/// <inheritdoc cref="Color.DarkGray"/>
	public static CMColor DarkGray => Color.DarkGray;
	/// <inheritdoc cref="Color.DarkGreen"/>
	public static CMColor DarkGreen => Color.DarkGreen;
	/// <inheritdoc cref="Color.DarkKhaki"/>
	public static CMColor DarkKhaki => Color.DarkKhaki;
	/// <inheritdoc cref="Color.DarkMagenta"/>
	public static CMColor DarkMagenta => Color.DarkMagenta;
	/// <inheritdoc cref="Color.DarkOliveGreen"/>
	public static CMColor DarkOliveGreen => Color.DarkOliveGreen;
	/// <inheritdoc cref="Color.DarkOrange"/>
	public static CMColor DarkOrange => Color.DarkOrange;
	/// <inheritdoc cref="Color.DarkOrchid"/>
	public static CMColor DarkOrchid => Color.DarkOrchid;
	/// <inheritdoc cref="Color.DarkRed"/>
	public static CMColor DarkRed => Color.DarkRed;
	/// <inheritdoc cref="Color.DarkSalmon"/>
	public static CMColor DarkSalmon => Color.DarkSalmon;
	/// <inheritdoc cref="Color.DarkSeaGreen"/>
	public static CMColor DarkSeaGreen => Color.DarkSeaGreen;
	/// <inheritdoc cref="Color.DarkSlateBlue"/>
	public static CMColor DarkSlateBlue => Color.DarkSlateBlue;
	/// <inheritdoc cref="Color.DarkSlateGray"/>
	public static CMColor DarkSlateGray => Color.DarkSlateGray;
	/// <inheritdoc cref="Color.DarkTurquoise"/>
	public static CMColor DarkTurquoise => Color.DarkTurquoise;
	/// <inheritdoc cref="Color.DarkViolet"/>
	public static CMColor DarkViolet => Color.DarkViolet;
	/// <inheritdoc cref="Color.DeepPink"/>
	public static CMColor DeepPink => Color.DeepPink;
	/// <inheritdoc cref="Color.DeepSkyBlue"/>
	public static CMColor DeepSkyBlue => Color.DeepSkyBlue;
	/// <inheritdoc cref="Color.DimGray"/>
	public static CMColor DimGray => Color.DimGray;
	/// <inheritdoc cref="Color.DodgerBlue"/>
	public static CMColor DodgerBlue => Color.DodgerBlue;
	/// <inheritdoc cref="Color.Firebrick"/>
	public static CMColor Firebrick => Color.Firebrick;
	/// <inheritdoc cref="Color.FloralWhite"/>
	public static CMColor FloralWhite => Color.FloralWhite;
	/// <inheritdoc cref="Color.ForestGreen"/>
	public static CMColor ForestGreen => Color.ForestGreen;
	/// <inheritdoc cref="Color.Fuchsia"/>
	public static CMColor Fuchsia => Color.Fuchsia;
	/// <inheritdoc cref="Color.Gainsboro"/>
	public static CMColor Gainsboro => Color.Gainsboro;
	/// <inheritdoc cref="Color.GhostWhite"/>
	public static CMColor GhostWhite => Color.GhostWhite;
	/// <inheritdoc cref="Color.Gold"/>
	public static CMColor Gold => Color.Gold;
	/// <inheritdoc cref="Color.Goldenrod"/>
	public static CMColor Goldenrod => Color.Goldenrod;
	/// <inheritdoc cref="Color.Gray"/>
	public static CMColor Gray => Color.Gray;
	/// <inheritdoc cref="Color.Green"/>
	public static CMColor Green => Color.Green;
	/// <inheritdoc cref="Color.GreenYellow"/>
	public static CMColor GreenYellow => Color.GreenYellow;
	/// <inheritdoc cref="Color.Honeydew"/>
	public static CMColor Honeydew => Color.Honeydew;
	/// <inheritdoc cref="Color.HotPink"/>
	public static CMColor HotPink => Color.HotPink;
	/// <inheritdoc cref="Color.IndianRed"/>
	public static CMColor IndianRed => Color.IndianRed;
	/// <inheritdoc cref="Color.Indigo"/>
	public static CMColor Indigo => Color.Indigo;
	/// <inheritdoc cref="Color.Ivory"/>
	public static CMColor Ivory => Color.Ivory;
	/// <inheritdoc cref="Color.Khaki"/>
	public static CMColor Khaki => Color.Khaki;
	/// <inheritdoc cref="Color.Lavender"/>
	public static CMColor Lavender => Color.Lavender;
	/// <inheritdoc cref="Color.LavenderBlush"/>
	public static CMColor LavenderBlush => Color.LavenderBlush;
	/// <inheritdoc cref="Color.LawnGreen"/>
	public static CMColor LawnGreen => Color.LawnGreen;
	/// <inheritdoc cref="Color.LemonChiffon"/>
	public static CMColor LemonChiffon => Color.LemonChiffon;
	/// <inheritdoc cref="Color.LightBlue"/>
	public static CMColor LightBlue => Color.LightBlue;
	/// <inheritdoc cref="Color.LightCoral"/>
	public static CMColor LightCoral => Color.LightCoral;
	/// <inheritdoc cref="Color.LightCyan"/>
	public static CMColor LightCyan => Color.LightCyan;
	/// <inheritdoc cref="Color.LightGoldenrodYellow"/>
	public static CMColor LightGoldenrodYellow => Color.LightGoldenrodYellow;
	/// <inheritdoc cref="Color.LightGreen"/>
	public static CMColor LightGreen => Color.LightGreen;
	/// <inheritdoc cref="Color.LightGray"/>
	public static CMColor LightGray => Color.LightGray;
	/// <inheritdoc cref="Color.LightPink"/>
	public static CMColor LightPink => Color.LightPink;
	/// <inheritdoc cref="Color.LightSalmon"/>
	public static CMColor LightSalmon => Color.LightSalmon;
	/// <inheritdoc cref="Color.LightSeaGreen"/>
	public static CMColor LightSeaGreen => Color.LightSeaGreen;
	/// <inheritdoc cref="Color.LightSkyBlue"/>
	public static CMColor LightSkyBlue => Color.LightSkyBlue;
	/// <inheritdoc cref="Color.LightSlateGray"/>
	public static CMColor LightSlateGray => Color.LightSlateGray;
	/// <inheritdoc cref="Color.LightSteelBlue"/>
	public static CMColor LightSteelBlue => Color.LightSteelBlue;
	/// <inheritdoc cref="Color.LightYellow"/>
	public static CMColor LightYellow => Color.LightYellow;
	/// <inheritdoc cref="Color.Lime"/>
	public static CMColor Lime => Color.Lime;
	/// <inheritdoc cref="Color.LimeGreen"/>
	public static CMColor LimeGreen => Color.LimeGreen;
	/// <inheritdoc cref="Color.Linen"/>
	public static CMColor Linen => Color.Linen;
	/// <inheritdoc cref="Color.Magenta"/>
	public static CMColor Magenta => Color.Magenta;
	/// <inheritdoc cref="Color.Maroon"/>
	public static CMColor Maroon => Color.Maroon;
	/// <inheritdoc cref="Color.MediumAquamarine"/>
	public static CMColor MediumAquamarine => Color.MediumAquamarine;
	/// <inheritdoc cref="Color.MediumBlue"/>
	public static CMColor MediumBlue => Color.MediumBlue;
	/// <inheritdoc cref="Color.MediumOrchid"/>
	public static CMColor MediumOrchid => Color.MediumOrchid;
	/// <inheritdoc cref="Color.MediumPurple"/>
	public static CMColor MediumPurple => Color.MediumPurple;
	/// <inheritdoc cref="Color.MediumSeaGreen"/>
	public static CMColor MediumSeaGreen => Color.MediumSeaGreen;
	/// <inheritdoc cref="Color.MediumSlateBlue"/>
	public static CMColor MediumSlateBlue => Color.MediumSlateBlue;
	/// <inheritdoc cref="Color.MediumSpringGreen"/>
	public static CMColor MediumSpringGreen => Color.MediumSpringGreen;
	/// <inheritdoc cref="Color.MediumTurquoise"/>
	public static CMColor MediumTurquoise => Color.MediumTurquoise;
	/// <inheritdoc cref="Color.MediumVioletRed"/>
	public static CMColor MediumVioletRed => Color.MediumVioletRed;
	/// <inheritdoc cref="Color.MidnightBlue"/>
	public static CMColor MidnightBlue => Color.MidnightBlue;
	/// <inheritdoc cref="Color.MintCream"/>
	public static CMColor MintCream => Color.MintCream;
	/// <inheritdoc cref="Color.MistyRose"/>
	public static CMColor MistyRose => Color.MistyRose;
	/// <inheritdoc cref="Color.Moccasin"/>
	public static CMColor Moccasin => Color.Moccasin;
	/// <inheritdoc cref="Color.NavajoWhite"/>
	public static CMColor NavajoWhite => Color.NavajoWhite;
	/// <inheritdoc cref="Color.Navy"/>
	public static CMColor Navy => Color.Navy;
	/// <inheritdoc cref="Color.OldLace"/>
	public static CMColor OldLace => Color.OldLace;
	/// <inheritdoc cref="Color.Olive"/>
	public static CMColor Olive => Color.Olive;
	/// <inheritdoc cref="Color.OliveDrab"/>
	public static CMColor OliveDrab => Color.OliveDrab;
	/// <inheritdoc cref="Color.Orange"/>
	public static CMColor Orange => Color.Orange;
	/// <inheritdoc cref="Color.OrangeRed"/>
	public static CMColor OrangeRed => Color.OrangeRed;
	/// <inheritdoc cref="Color.Orchid"/>
	public static CMColor Orchid => Color.Orchid;
	/// <inheritdoc cref="Color.PaleGoldenrod"/>
	public static CMColor PaleGoldenrod => Color.PaleGoldenrod;
	/// <inheritdoc cref="Color.PaleGreen"/>
	public static CMColor PaleGreen => Color.PaleGreen;
	/// <inheritdoc cref="Color.PaleTurquoise"/>
	public static CMColor PaleTurquoise => Color.PaleTurquoise;
	/// <inheritdoc cref="Color.PaleVioletRed"/>
	public static CMColor PaleVioletRed => Color.PaleVioletRed;
	/// <inheritdoc cref="Color.PapayaWhip"/>
	public static CMColor PapayaWhip => Color.PapayaWhip;
	/// <inheritdoc cref="Color.PeachPuff"/>
	public static CMColor PeachPuff => Color.PeachPuff;
	/// <inheritdoc cref="Color.Peru"/>
	public static CMColor Peru => Color.Peru;
	/// <inheritdoc cref="Color.Pink"/>
	public static CMColor Pink => Color.Pink;
	/// <inheritdoc cref="Color.Plum"/>
	public static CMColor Plum => Color.Plum;
	/// <inheritdoc cref="Color.PowderBlue"/>
	public static CMColor PowderBlue => Color.PowderBlue;
	/// <inheritdoc cref="Color.Purple"/>
	public static CMColor Purple => Color.Purple;
	/// <inheritdoc cref="Color.Red"/>
	public static CMColor Red => Color.Red;
	/// <inheritdoc cref="Color.RosyBrown"/>
	public static CMColor RosyBrown => Color.RosyBrown;
	/// <inheritdoc cref="Color.RoyalBlue"/>
	public static CMColor RoyalBlue => Color.RoyalBlue;
	/// <inheritdoc cref="Color.SaddleBrown"/>
	public static CMColor SaddleBrown => Color.SaddleBrown;
	/// <inheritdoc cref="Color.Salmon"/>
	public static CMColor Salmon => Color.Salmon;
	/// <inheritdoc cref="Color.SandyBrown"/>
	public static CMColor SandyBrown => Color.SandyBrown;
	/// <inheritdoc cref="Color.SeaGreen"/>
	public static CMColor SeaGreen => Color.SeaGreen;
	/// <inheritdoc cref="Color.SeaShell"/>
	public static CMColor SeaShell => Color.SeaShell;
	/// <inheritdoc cref="Color.Sienna"/>
	public static CMColor Sienna => Color.Sienna;
	/// <inheritdoc cref="Color.Silver"/>
	public static CMColor Silver => Color.Silver;
	/// <inheritdoc cref="Color.SkyBlue"/>
	public static CMColor SkyBlue => Color.SkyBlue;
	/// <inheritdoc cref="Color.SlateBlue"/>
	public static CMColor SlateBlue => Color.SlateBlue;
	/// <inheritdoc cref="Color.SlateGray"/>
	public static CMColor SlateGray => Color.SlateGray;
	/// <inheritdoc cref="Color.Snow"/>
	public static CMColor Snow => Color.Snow;
	/// <inheritdoc cref="Color.SpringGreen"/>
	public static CMColor SpringGreen => Color.SpringGreen;
	/// <inheritdoc cref="Color.SteelBlue"/>
	public static CMColor SteelBlue => Color.SteelBlue;
	/// <inheritdoc cref="Color.Tan"/>
	public static CMColor Tan => Color.Tan;
	/// <inheritdoc cref="Color.Teal"/>
	public static CMColor Teal => Color.Teal;
	/// <inheritdoc cref="Color.Thistle"/>
	public static CMColor Thistle => Color.Thistle;
	/// <inheritdoc cref="Color.Tomato"/>
	public static CMColor Tomato => Color.Tomato;
	/// <inheritdoc cref="Color.Turquoise"/>
	public static CMColor Turquoise => Color.Turquoise;
	/// <inheritdoc cref="Color.Violet"/>
	public static CMColor Violet => Color.Violet;
	/// <inheritdoc cref="Color.Wheat"/>
	public static CMColor Wheat => Color.Wheat;
	/// <inheritdoc cref="Color.White"/>
	public static CMColor White => Color.White;
	/// <inheritdoc cref="Color.WhiteSmoke"/>
	public static CMColor WhiteSmoke => Color.WhiteSmoke;
	/// <inheritdoc cref="Color.Yellow"/>
	public static CMColor Yellow => Color.Yellow;
	/// <inheritdoc cref="Color.YellowGreen"/>
	public static CMColor YellowGreen => Color.YellowGreen;

	#endregion

	#region Operators

	public static bool operator ==(CMColor color1, CMColor color2)
		=> color1.Equals(color2);

	public static bool operator !=(CMColor color1, CMColor color2)
		=> !color1.Equals(color2);

	public static implicit operator Color(CMColor color)
		=> Color.FromArgb(color.A, color.R, color.G, color.B);

	public static implicit operator CMColor(Color color)
		=> new(color.R, color.G, color.B, color.A);

	#endregion

	#region HueColors

	/// <summary>
	/// Red hue, represents color #ff0000.
	/// </summary>
	public const int HueRed = 0;

	/// <summary>
	/// Yellow hue, represents color #ffff00.
	/// </summary>
	public const int HueYellow = 255;

	/// <summary>
	/// Green hue, represents color #00ff00.
	/// </summary>
	public const int HueGreen = 510;

	/// <summary>
	/// Cyan hue, represents color #00ffff.
	/// </summary>
	public const int HueCyan = 765;

	/// <summary>
	/// Blue hue, represents color #0000ff.
	/// </summary>
	public const int HueBlue = 1020;

	/// <summary>
	/// Magenta hue, represents color #ff00ff.
	/// </summary>
	public const int HueMagenta = 1275;

	/// <summary>
	/// Total count of hues.
	/// </summary>
	public const int HuesCount = 1530;

	private static readonly (byte r, byte g, byte b)[] HueColors = new (byte, byte, byte)[HuesCount];

	private static void FillHueColors()
	{
		#region Explanation

		/*
		
		hue smoothly shifts between these colors:
		(255, 0, 0) - red
		(255, 255, 0) - yellow
		(0, 255, 0) - green
		(0, 255, 255) - light blue
		(0, 0, 255) - blue
		(255, 0, 255) - violet
		(255, 0, 0) - red

		each transition takes 256 steps.
		as a result we have 6 transitions in total = 255 * 6 = 1530 values.
		outside of [0, 1530) range we will consider the function to be periodical.

		each color may be represented as a simple function based on abs, min and max.

		*/

		#endregion

		for (var hue = 0; hue < HuesCount; hue++)
		{
			var r = (byte)(Math.Abs(hue - 255 * 3) - 255).Clamp(0, 255);
			var g = (byte)(-Math.Abs(hue - 255 * 2) + 255 * 2).Clamp(0, 255);
			var b = (byte)(-Math.Abs(hue - 255 * 4) + 255 * 2).Clamp(0, 255);
			HueColors[hue] = (r, g, b);
		}
	}

	/// <returns>
	/// Color by hue from 0 to 1530.
	/// </returns>
	private static (byte r, byte g, byte b) HueToRgb(int hue)
	{
		if (hue < 0) hue += 1530 * (Math.Abs(hue / 1530) + 1);
		hue %= 1530;
		return HueColors[hue];
	}

	#endregion

	static CMColor()
	{
		FillHueColors();
	}

	private const float BrightnessR = 0.299f;
	private const float BrightnessG = 0.587f;
	private const float BrightnessB = 0.114f;

	// color components
	private byte _r = 0;
	private byte _g = 0;
	private byte _b = 0;
	private byte _a = 0;

	// external byte array attachment
	private bool _isAttached = false;
	private byte[] _data = null;
	private int _indexR = 0;
	private int _indexG = 0;
	private int _indexB = 0;
	private int _indexA = 0;

	/// <summary>
	/// Creates color from components.
	/// </summary>
	public CMColor(int r, int g, int b, int a = 255)
	{
		_r = (byte)r.Clamp(0, 255);
		_g = (byte)g.Clamp(0, 255);
		_b = (byte)b.Clamp(0, 255);
		_a = (byte)a.Clamp(0, 255);
	}

	/// <summary>
	/// Creates color from components of another color.
	/// </summary>
	public CMColor(Color color)
		: this(color.R, color.G, color.B, color.A) { }

	/// <summary>
	/// Creates color from components of another color.
	/// </summary>
	public CMColor(CMColor color)
		: this(color.R, color.G, color.B, color.A) { }

	/// <summary>
	/// Creates color from hue and alpha component.
	/// </summary>
	public CMColor(int hue, byte a = 255)
	{
		(R, G, B) = HueToRgb(hue);
		A = a;
	}

	/// <summary>
	/// Creates color whose components are attached to an external byte array.
	/// </summary>
	/// <param name="data">Byte array.</param>
	/// <param name="index">Array index of red component.</param>
	/// <remarks>
	/// Any color changes will be automatically saved into the attached byte array.
	/// </remarks>
	public CMColor(byte[] data, int index)
		=> Attach(data, index);

	/// <summary>
	/// Creates color whose components are attached to raw data of a raster layer.
	/// </summary>
	/// <param name="layer">Raster layer.</param>
	/// <param name="x">Pixel X coordinate.</param>
	/// <param name="y">Pixel Y coordinate.</param>
	/// <remarks>
	/// Any color changes will be automatically saved into the raster layer's data.
	/// </remarks>
	public CMColor(IRasterLayer layer, int x, int y)
		=> Attach(layer.Data, y * layer.Size.Width * 4 + x * 4);

	/// <summary>
	/// Red component.
	/// </summary>
	public byte R
	{
		readonly get => _r;
		set => SetComponent(ref _r, _indexR, value);
	}

	/// <summary>
	/// Green component.
	/// </summary>
	public byte G
	{
		readonly get => _g;
		set => SetComponent(ref _g, _indexG, value);
	}

	/// <summary>
	/// Blue component.
	/// </summary>
	public byte B
	{
		readonly get => _b;
		set => SetComponent(ref _b, _indexB, value);
	}

	/// <summary>
	/// Alpha component.
	/// </summary>
	public byte A
	{
		readonly get => _a;
		set => SetComponent(ref _a, _indexA, value);
	}

	private readonly void SetComponent(ref byte field, int index, byte value)
	{
		if (field == value) return;
		field = value;
		if (_isAttached) _data[index] = value;
	}

	/// <summary>
	/// Attaches this structure to byte array.
	/// </summary>
	/// <param name="data"><inheritdoc cref="CMColor(byte[], int)" path="/param[@name='data']"/></param>
	/// <param name="index"><inheritdoc cref="CMColor(byte[], int)" path="/param[@name='index']"/></param>
	public void Attach(byte[] data, int index)
	{
		_isAttached = true;
		_data = data;
		_indexR = index;
		_indexG = index + 1;
		_indexB = index + 2;
		_indexA = index + 3;
		_r = _data[_indexR];
		_g = _data[_indexG];
		_b = _data[_indexB];
		_a = _data[_indexA];
	}

	#region Object methods

	public override readonly bool Equals(object obj)
		=> obj is CMColor color &&
			R == color.R &&
			G == color.G &&
			B == color.B &&
			A == color.A;

	public override readonly int GetHashCode()
		=> HashCode.Combine(R, G, B, A);

	public override readonly string ToString()
		=> $"{nameof(CMColor)} [R={R}, G={G}, B={B}, A={A}]";

	#endregion

	/// <summary>
	/// Copies color components from another color.
	/// </summary>
	public void CopyFrom(CMColor color)
	{
		R = color.R;
		G = color.G;
		B = color.B;
		A = color.A;
	}

	/// <summary>
	/// Set a color between current color and another <paramref name="color"/>.<br/>
	/// <paramref name="position"/> = 0 points to the current color.<br/>
	/// <paramref name="position"/> = 1 points to <paramref name="color"/>.<br/>
	/// All components including alpha are affected.
	/// </summary>
	public void SetBetween(CMColor color, float position)
		=> SetBetween(color.R, color.G, color.B, color.A, position);

	/// <summary>
	/// Set a color between current color and another color descibed by components.<br/>
	/// <paramref name="position"/> = 0 points to the current color.<br/>
	/// <paramref name="position"/> = 1 points to another color.<br/>
	/// All components including alpha are affected.
	/// </summary>
	public void SetBetween(byte r, byte g, byte b, byte a, float position)
	{
		R = (byte)Math.Round(_r + (r - _r) * position);
		G = (byte)Math.Round(_g + (g - _g) * position);
		B = (byte)Math.Round(_b + (b - _b) * position);
		A = (byte)Math.Round(_a + (a - _a) * position);
	}

	/// <returns>
	/// Color brightness from 0 to 1.
	/// </returns>
	public readonly float GetBrightness()
		=> _r * BrightnessR / 255 + _g * BrightnessG / 255 + _b * BrightnessB / 255;

	/// <summary>
	/// Sets a color with the same hue but different brightness.
	/// Alpha component remains unchanged.
	/// </summary>
	/// <remarks>
	/// Depending on the current color this method may be lossy.
	/// For example, (255, 0, 0) is pure red color with brightness = 0.299.
	/// Raising its brightness to 0.5 will result in color (255, 73, 73).
	/// If we then lower brightness back to 0.299, the result would be (153, 44, 44)
	/// which differs from original pure red.
	/// </remarks>
	public void SetBrightness(float brightness)
	{
		var source = GetBrightness();
		var target = brightness.Clamp(0, 1);
		if (target == source) return;

		if (target > source)
			SetBetween(255, 255, 255, A, (target - source) / (1 - source));
		else
			SetBetween(0, 0, 0, A, (source - target) / source);
	}

	/// <returns>
	/// Color saturation from 0 to 1.
	/// </returns>
	public readonly float GetSaturation()
	{
		MathUtils.GetMinMax(out var min, out var max, _r, _g, _b);
		return (max - min) / 255f;
	}

	/// <summary>
	/// Sets a color with approximately same hue but different saturation.
	/// Alpha component remains unchanged.
	/// </summary>
	public void SetSaturation(float saturation)
	{
		var source = GetSaturation();
		var target = saturation.Clamp(0, 1);
		if (target == source) return;

		if (target < source)
		{
			var brightness = (byte)Math.Round(GetBrightness() * 255);
			SetBetween(brightness, brightness, brightness, A, (source - target) / source);
			return;
		}

		if (_r == _g && _g == _b)
		{
			// here we have grayscale color, its saturation cannot be changed.
			// but we anyway need to set it to something more saturated, so we need
			// any saturated counterpart color. mspaint uses blue color in such cases
			// but in fact the choice doesn't matter - all options are equally bad.
			SetBetween(255, 0, 0, _a, (target - source) / (1 - source));
			return;
		}

		var r = _r;
		var g = _g;
		var b = _b;
		var a = _a;

		// find the closest color with maximal saturation. it shall have at least one
		// component = 255 and at least one = 0.

		MathUtils.GetMinMax(out var min, out var max, r, g, b);

		// lower one or two components to 0.
		// raise one or two other components to 1.
		// now if there is any component in between 0 and 255 then raise it so that
		// its ratio to max component remains the same.

		if (r == min) r = 0;
		else if (r == max) r = 255;
		else r = (byte)Math.Round((float)r * 255 / max);

		if (g == min) g = 0;
		else if (g == max) g = 255;
		else g = (byte)Math.Round((float)g * 255 / max);

		if (b == min) b = 0;
		else if (b == max) b = 255;
		else b = (byte)Math.Round((float)b * 255 / max);

		SetBetween(r, g, b, a, (target - source) / (1 - source));
	}

	/// <returns>
	/// Approximate color hue from 0 to 1530.
	/// </returns>
	public readonly int GetHue()
	{
		if (_r == _g && _g == _b)
			// that's a lie but what can we do. use red like we do in SetSaturation.
			return 0;

		// normalize components against [0; 255] range
		MathUtils.GetMinMax(out var min, out var max, _r, _g, _b);
		var diff = max - min;
		var r = Normalize(_r);
		var g = Normalize(_g);
		var b = Normalize(_b);

		// now at least one component is 255 and at least another one is 0.
		// so a corresponding hue must exist.
		return b == 0
			? g - r + 255
			: r == 0
				? b - g + 255 * 3
				: r - b + 255 * 5;

		byte Normalize(byte component)
			=> (byte)Math.Round(255f * (component - min) / diff);
	}

	/// <summary>
	/// Sets color with different hue with same brightness and approximately same saturation.
	/// Alpha component remains unchanged.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Preserving brightness is more important for captcha generation then preserving saturation.<br />
	/// This method attempts to preserve saturation but in most cases it will significantly change.<br/>
	/// Color components contribution to brightness is different, so preserving both brightness and
	/// saturation is impossible when altering hue.
	/// </para>
	/// <para>
	/// For example, take pure blue color (0, 0, 255). Its brightness is 0.114, saturation is 1.<br/>
	/// Shifting its hue to red (hue = 0) with preserving brightness would result in (97, 0, 0) color.
	/// But this new color's saturation is 0.38, not 1.<br/>
	/// On the other side to preserve saturation we would have to return (255, 0, 0), but then we have
	/// brightness 0.299 instead of original 0.114.
	/// </para>
	/// <para>
	/// It's not a problem of computational complexity but a limitation resulting from using component
	/// weights for brightness calculation.<br />
	/// For pure blue color discussed above, there is simply no red analogue with the same combination
	/// of brightness and saturation.
	/// </para>
	/// </remarks>
	public void SetHue(int hue)
	{
		#region Explanation

		/*

		this method's mission is to change the color's hue while doing best to preserve other characteristics.
		since SetSaturation cannot change saturation while preserving brightness (it's just impossible),
		brightness shall anyway be set last.

		but why shall we also set brightness before saturation?

		imagine changing hue of dark blue color (0, 0, 127) to red (hue = 0). brightness = 0.057, saturation = 0.5.
		HueToRgb for 0 gives us color (255, 0, 0).
		setting its saturation to 0.5 (the saturation of dark blue we work on) changes color to (165, 38, 38).
		further setting brightness to 0.057 brings us to (31, 7, 7) which is weird: we were shifting the color with
		only blue component lit and ended up with all three instead of just red.

		the reason behind this is composed of mulitple factors:
		1. HueToRgb returns colors with saturation = 1 (at least one component is 255 and at least one is 0).
		2. lowering their saturation always results in colors with all non-zero components.
		3. SetBrightness uses SetBetween relative to white or black color, so it shifts all components
		   proportionally, so non-zero components in most cases remain non-zero.

		in order to smoothen this distortion we need to lower SetSaturation's room for maneuver - and that's
		where an extra brightness change comes in play. this is more useful for blue colors since they have overall
		lower brightness levels.

		*/

		#endregion

		var saturation = GetSaturation();
		var brightness = GetBrightness();
		(R, G, B) = HueToRgb(hue);
		SetBrightness(brightness);
		SetSaturation(saturation);
		SetBrightness(brightness);
	}

	/// <summary>
	/// Sets color to grayscale while preserving its brightness.
	/// </summary>
	public void SetGrayscale()
	{
		var brightness = GetBrightness();
		var comp = (byte)Math.Round(brightness * 255);
		R = comp;
		G = comp;
		B = comp;
	}
}
