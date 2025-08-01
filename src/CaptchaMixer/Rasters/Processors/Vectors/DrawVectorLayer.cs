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
/// Draws a vector layer.
/// </summary>
public class DrawVectorLayer : SKBitmapRasterProcessor
{
	/// <inheritdoc cref="VectorLayerDrawInfo"/>
	public VectorLayerDrawInfo Info { get; }

	private DrawVectorLayer(VectorLayerDrawInfo info)
		=> Info = info ?? throw new ArgumentNullException(nameof(info));

	/// <inheritdoc cref="VectorLayerDrawInfo(ValueProvider{string}, ValueProvider{FillPaintInfo}, ValueProvider{StrokePaintInfo})"/>
	public DrawVectorLayer(
		ValueProvider<string> name,
		ValueProvider<FillPaintInfo> fill)
		: this(new(name, fill)) { }

	/// <inheritdoc cref="VectorLayerDrawInfo(ValueProvider{string}, ValueProvider{FillPaintInfo}, ValueProvider{StrokePaintInfo})"/>
	public DrawVectorLayer(
		ValueProvider<string> name,
		ValueProvider<StrokePaintInfo> stroke)
		: this(new(name, stroke)) { }

	/// <inheritdoc cref="VectorLayerDrawInfo(ValueProvider{string}, ValueProvider{FillPaintInfo}, ValueProvider{StrokePaintInfo})"/>
	public DrawVectorLayer(
		ValueProvider<string> name,
		ValueProvider<FillPaintInfo> fill,
		ValueProvider<StrokePaintInfo> stroke)
		: this(new(name, fill, stroke)) { }

	protected override void Process(SKBitmap bitmap, ICaptchaContext context)
	{
		using var canvas = new SKCanvas(bitmap);
		context
			.GetVectorLayer(Info.Name)
			.Objects
			.ForEach(o => canvas.Draw(o, Info.Fill, Info.Stroke));
	}
}
