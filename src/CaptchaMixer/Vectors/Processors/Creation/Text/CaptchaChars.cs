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
/// Creates a vector object for each char of <see cref="CaptchaParameters.Answer"/> from
/// captcha context.
/// </summary>
/// <remarks>
/// Resultant objects are placed somewhere aroung the lop left corner. Then you'll
/// need to scale and distribute them over the layer using other processors.
/// </remarks>
public class CaptchaChars : SKFontTextCreator
{
	public static readonly MemoryCacheWrapper<(char chr, FontInfo font), VectorObject> Cache = new(86400, 10000, 10);

	/// <summary>
	/// Characters font.
	/// </summary>
	public ValueProvider<FontInfo> Font { get; set; } = FontInfo.FromFamily("Courier New", FontStyle.Bold);

	public CaptchaChars() { }

	/// <param name="font"><inheritdoc cref="Font" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public CaptchaChars(ValueProvider<FontInfo> font)
		=> Font = font ?? throw new ArgumentNullException(nameof(font));

	public override void Process(IVectorLayer layer, ICaptchaContext context)
	{
		foreach (var chr in context.Parameters.Answer)
		{
			var obj = Cache
				.GetOrCreate(
					(chr, Font),
					t => CreateTextVectorObject(t.chr.ToString(), t.font))
				.Clone();

			layer.Objects.Add(obj);
		}
	}
}
