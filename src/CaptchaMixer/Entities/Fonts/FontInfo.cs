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
/// Font information.
/// </summary>
public class FontInfo : IEquatable<FontInfo>
{
	public static readonly FontInfo Arial = FromFamily(WellKnownFontFamilies.Arial);
	public static readonly FontInfo ArialBlack = FromFamily(WellKnownFontFamilies.ArialBlack);
	public static readonly FontInfo ComicSansMs = FromFamily(WellKnownFontFamilies.ComicSansMs);
	public static readonly FontInfo CourierNew = FromFamily(WellKnownFontFamilies.CourierNew);
	public static readonly FontInfo Georgia = FromFamily(WellKnownFontFamilies.Georgia);
	public static readonly FontInfo LucidaConsole = FromFamily(WellKnownFontFamilies.LucidaConsole);
	public static readonly FontInfo LucidaSansUnicode = FromFamily(WellKnownFontFamilies.LucidaSansUnicode);
	public static readonly FontInfo PalatinoLinotype = FromFamily(WellKnownFontFamilies.PalatinoLinotype);
	public static readonly FontInfo Tahoma = FromFamily(WellKnownFontFamilies.Tahoma);
	public static readonly FontInfo TimesNewRoman = FromFamily(WellKnownFontFamilies.TimesNewRoman);
	public static readonly FontInfo TrebuchetMs = FromFamily(WellKnownFontFamilies.TrebuchetMs);
	public static readonly FontInfo Verdana = FromFamily(WellKnownFontFamilies.Verdana);

	/// <summary>
	/// Font file path.
	/// </summary>
	public string FilePath { get; private set; }

	/// <summary>
	/// Font family name.
	/// </summary>
	public string FamilyName { get; private set; }

	/// <summary>
	/// Font style.
	/// </summary>
	public FontStyle Style { get; private set; }

	private FontInfo() { }

	/// <summary>
	/// Creates font information from font file path.
	/// </summary>
	public static FontInfo FromFile(string filePath)
		=> new() { FilePath = filePath };

	/// <summary>
	/// Creates font information from font family name and style.
	/// </summary>
	public static FontInfo FromFamily(string familyName, FontStyle style = FontStyle.Normal)
		=> new() { FamilyName = familyName, Style = style };

	#region Object and IEquatable

	public bool Equals(FontInfo other)
	{
		var filePathEquals = FilePath?.Equals(other.FilePath) ?? other.FilePath == null;
		var familyNameEquals = FamilyName?.Equals(other.FamilyName) ?? other.FamilyName == null;
		var styleEquals = Style.Equals(other.Style);
		return filePathEquals && familyNameEquals && styleEquals;
	}

	public override bool Equals(object obj)
		=> obj is FontInfo other && Equals(other);

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = FilePath?.GetHashCode() ?? 0;
			hashCode = (hashCode * 398) ^ (FamilyName?.GetHashCode() ?? 0);
			hashCode = (hashCode * 398) ^ Style.GetHashCode();
			return hashCode;
		}
	}

	#endregion
}
