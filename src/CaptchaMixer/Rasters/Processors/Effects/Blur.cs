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
/// Blue effect.
/// </summary>
public class Blur : SKBitmapRasterProcessor
{
	/// <summary>
	/// X axis blur sigma.
	/// </summary>
	public ValueProvider<float> SigmaX { get; set; } = 1;

	/// <summary>
	/// Y axis blur sigma.
	/// </summary>
	public ValueProvider<float> SigmaY { get; set; } = 1;

	/// <summary>
	/// Both axes blur sigma.
	/// </summary>
	public ValueProvider<float> Sigma
	{
		set
		{
			SigmaX = value;
			SigmaY = value;
		}
	}

	public Blur() { }

	/// <param name="sigma"><inheritdoc cref="Sigma" path="/summary"/></param>
	public Blur(ValueProvider<float> sigma)
		: this(sigma, sigma) { }

	/// <param name="sigmaX"><inheritdoc cref="SigmaX" path="/summary"/></param>
	/// <param name="sigmaY"><inheritdoc cref="SigmaY" path="/summary"/></param>
	public Blur(ValueProvider<float> sigmaX, ValueProvider<float> sigmaY)
	{
		SigmaX = sigmaX;
		SigmaY = sigmaY;
	}

	protected override void Process(SKBitmap bitmap, ICaptchaContext context)
	{
		float sigmaX = SigmaX;
		float sigmaY = SigmaY;

		using var canvas = new SKCanvas(bitmap);
		var paint = new SKPaint { ImageFilter = SKImageFilter.CreateBlur(sigmaX, sigmaY) };
		var size = context.Parameters.Size;
		var rect = new SKRect(0, 0, size.Width, size.Height);
		canvas.DrawBitmap(bitmap, rect, paint);
	}
}
