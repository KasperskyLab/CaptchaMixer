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
/// Base class for processors which create objects from text.
/// </summary>
public abstract class SKFontTextCreator : IVectorProcessor
{
	public abstract void Process(IVectorLayer layer, ICaptchaContext context);

	protected static VectorObject CreateTextVectorObject(string text, FontInfo fontInfo)
	{
		if (text == null || text.Length == 0)
			throw new ArgumentException($"Value of '{text}' must be a non-empty string", nameof(text));

		var result = new VectorObject();

		var typeface = fontInfo.ToSKTypeface();
		var font = new SKFont(typeface);
		var glyphsCount = font.CountGlyphs(text);
		var glyphs = new Span<ushort>(new ushort[glyphsCount]);
		var glyphPositions = new Span<SKPoint>(new SKPoint[glyphsCount]);
		font.GetGlyphs(text, glyphs);
		font.GetGlyphPositions(glyphs, glyphPositions);

		for (var i = 0; i < glyphsCount; i++)
		{
			var glyphPath = font.GetGlyphPath(glyphs[i]);
			result.Paths.Add(glyphPath.ToVectorPath());
		}

		return result;
	}
}
