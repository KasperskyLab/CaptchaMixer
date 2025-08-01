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
/// Creates a vector object for each skeletonized char of <see cref="CaptchaParameters.Answer"/>
/// from captcha context.
/// </summary>
/// <remarks>
/// <inheritdoc cref="Skeletonizer.Skeletonize(VectorPath, float)" path="/remarks"/>
/// <para>
/// Resultant objects are placed somewhere aroung the lop left corner. Then you'll
/// need to scale and distribute them over the layer using other processors.
/// </para>
/// </remarks>
public class SkeletonizedCaptchaChars : SKFontTextCreator
{
	public static readonly MemoryCacheWrapper<(char chr, FontInfo font, float quality), VectorObject> Cache = new(86400, 10000, 60);

	/// <summary>
	/// Characters font (before skeletonization).
	/// </summary>
	public ValueProvider<FontInfo> Font { get; set; } = FontInfo.FromFamily("Courier New", FontStyle.Bold);

	/// <summary>
	/// <inheritdoc cref="Skeletonizer.Skeletonize(VectorPath, float)" path="/param[@name='quality']"/>
	/// </summary>
	public ValueProvider<float> Quality { get; set; } = 10.0f;

	/// <summary>
	/// The number of decimals to which <see cref="Quality"/> is rounded before skeletonization.
	/// </summary>
	/// <remarks>
	/// Skeletonization is an expensive process, so once skeletonized characters are being cached.
	/// Quality is a part of the cache key. So if the quality value provider, for example,
	/// provides random float values, those values shall be rounded for caching to be efficient.
	/// </remarks>
	public int QualityRoundDecimals { get; set; } = 1;

	public SkeletonizedCaptchaChars() { }

	/// <param name="font"><inheritdoc cref="Font" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public SkeletonizedCaptchaChars(ValueProvider<FontInfo> font)
		=> Font = font ?? throw new ArgumentNullException(nameof(font));

	/// <param name="font"><inheritdoc cref="Font" path="/summary"/></param>
	/// <param name="quality"><inheritdoc cref="Quality" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public SkeletonizedCaptchaChars(ValueProvider<FontInfo> font, ValueProvider<float> quality)
		: this(font)
		=> Quality = quality ?? throw new ArgumentNullException(nameof(quality));

	public override void Process(IVectorLayer layer, ICaptchaContext context)
	{
		foreach (var chr in context.Parameters.Answer)
		{
			var quality = (float)Math.Round(Quality, QualityRoundDecimals);

			var obj = Cache
				.GetOrCreate(
					(chr, Font, quality),
					k => CreateTextVectorObject(k.chr.ToString(), k.font).SkeletonizePaths(k.quality))
				.Clone();

			layer.Objects.Add(obj);
		}
	}
}
