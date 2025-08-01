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

public static class FontInfoExtensions
{
	private static readonly SKFontManager FontManager = SKFontManager.CreateDefault();

	/// <summary>
	/// Converts <paramref name="font"/> into <see cref="SKTypeface"/>.
	/// </summary>
	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="ArgumentException"/>
	public static SKTypeface ToSKTypeface(this FontInfo font)
	{
		ArgumentNullException.ThrowIfNull(font, nameof(font));

		if (font.FamilyName != null)
			return FontManager.MatchFamily(font.FamilyName, font.Style.ToSKFontStyle());
		else if (font.FilePath != null)
			return FontManager.CreateTypeface(font.FilePath);
		else
			throw new ArgumentException("Invalid font info", nameof(font));
	}

	/// <summary>
	/// Converts <paramref name="style"/> into <see cref="SKFontStyle"/>.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"/>
	public static SKFontStyle ToSKFontStyle(this FontStyle style)
		=> style switch
		{
			FontStyle.Normal => SKFontStyle.Normal,
			FontStyle.Bold => SKFontStyle.Bold,
			FontStyle.Italic => SKFontStyle.Italic,
			FontStyle.BoldItalic => SKFontStyle.BoldItalic,
			_ => throw new ArgumentOutOfRangeException(nameof(style)),
		};
}
