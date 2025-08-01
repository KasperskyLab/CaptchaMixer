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
/// Saves vector layer data into PNG files before and/or after processing.
/// </summary>
public class PngExportingVectorProcessorProxy : IVectorProcessor
{
	private readonly IVectorProcessor _processor;
	private readonly string _filePathBase;
	private readonly bool _saveBefore;
	private readonly int _scale;
	private readonly ScaleObjects _scaleTransformer;

	private static readonly SKPaint PaintBackground = new()
	{
		Style = SKPaintStyle.Fill,
		Color = new SKColor(0, 0, 0),
		IsAntialias = true
	};

	private static readonly float[] ObjectBoundsDashIntervals = new[] { 5f, 2f };

	private static readonly SKPaint PaintObjectBounds = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = new SKColor(255, 255, 255, 64),
		PathEffect = SKPathEffect.CreateDash(ObjectBoundsDashIntervals, 0),
		IsAntialias = true
	};

	private static readonly float[] PathBoundsDashIntervals = new[] { 3f, 1f };

	private static readonly SKPaint PaintPathBounds = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = new SKColor(128, 160, 255, 64),
		PathEffect = SKPathEffect.CreateDash(PathBoundsDashIntervals, 0),
		IsAntialias = true
	};

	private static readonly SKPaint PaintVectors = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = new SKColor(255, 0, 0, 160),
		IsAntialias = true
	};

	private static readonly SKPaint PaintFill = new()
	{
		Style = SKPaintStyle.Fill,
		Color = new SKColor(255, 0, 0, 20),
		IsAntialias = true
	};

	private static readonly SKPaint PaintControlPoints = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = new SKColor(255, 128, 0),
		IsAntialias = true,
		StrokeWidth = 2
	};

	private static readonly SKPaint PaintContourPoints = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = new SKColor(0, 255, 0),
		IsAntialias = true,
		StrokeWidth = 3
	};

	/// <param name="processor">Wrapped vector processor.</param>
	/// <param name="filePathBase">PNG files path and name base.</param>
	/// <param name="saveBefore">Save an image before processing.</param>
	/// <param name="scale">Image scale factor.</param>
	public PngExportingVectorProcessorProxy(
		IVectorProcessor processor,
		string filePathBase,
		bool saveBefore = true,
		int scale = 10)
	{
		_processor = processor;
		_filePathBase = filePathBase;
		_saveBefore = saveBefore;
		_scale = scale;

		_scaleTransformer = new ScaleObjects(scale)
		{
			BasePoint = BasePointType.LayerLeftTop,
			BasePointOffset = 0
		};
	}

	private void Export(IVectorLayer layer, ICaptchaContext context, string filePath)
	{
		layer = layer.Clone();
		_scaleTransformer.Process(layer, context);
		var size = context.Parameters.Size;

		using var bitmap = new SKBitmap(size.Width * _scale, size.Height * _scale);
		using var canvas = new SKCanvas(bitmap);

		canvas.DrawRect(bitmap.Info.Rect, PaintBackground);

		foreach (var obj in layer.Objects)
		{
			canvas.DrawRect(obj.GetBounds().ToSKRect(), PaintObjectBounds);

			if (obj.Paths.Count > 1)
				foreach (var path in obj)
					canvas.DrawRect(path.GetBounds().ToSKRect(), PaintPathBounds);

			foreach (var path in obj)
				canvas.DrawPath(path.ToSKPath(), PaintFill);

			foreach (var path in obj)
				canvas.DrawPath(path.ToSKPath(), PaintVectors);

			var points = obj
				.EnumeratePathInstructions()
				.SelectMany(i => i
					.Points
					.Select((p, index) => (point: p.ToSKPoint(), type: i.PointTypes[index])))
				.ToArray();

			canvas.DrawPoints(
				SKPointMode.Points,
				points
					.Where(t => t.type == PointType.Control)
					.Select(t => t.point)
					.ToArray(),
				PaintControlPoints);

			canvas.DrawPoints(
				SKPointMode.Points,
				points
					.Where(t => t.type == PointType.Contour)
					.Select(t => t.point)
					.ToArray(),
				PaintContourPoints);
		}

		bitmap.Export(filePath);
	}

	public void Process(IVectorLayer layer, ICaptchaContext context)
	{
		if (_saveBefore) Export(layer, context, _filePathBase + "_0.png");
		_processor.Process(layer, context);
		Export(layer, context, _filePathBase + "_1.png");
	}
}
