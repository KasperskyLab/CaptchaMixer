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

using System.Runtime.InteropServices;

namespace Kaspersky.CaptchaMixer;

public static class SKExtensions
{
	#region SKBitmap

	/// <summary>
	/// Attaches <paramref name="bitmap"/> to raw <paramref name="data"/> which
	/// describes a raster image with specified <paramref name="size"/>.
	/// </summary>
	public static void AttachToBytes(this SKBitmap bitmap, byte[] data, Size size)
	{
		// pin the managed array so that the GC doesn't move it
		var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

		// install the pixels with the color type of the pixel data
		var info = new SKImageInfo(size.Width, size.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
		bitmap.InstallPixels(info, gcHandle.AddrOfPinnedObject(), info.RowBytes, delegate { gcHandle.Free(); }, null);
	}

	/// <summary>
	/// Exports <paramref name="bitmap"/> into a raster file at
	/// <paramref name="filePath"/> of specified <paramref name="format"/>.
	/// </summary>
	public static void Export(this SKBitmap bitmap, string filePath, CaptchaImageFormat format = CaptchaImageFormat.Png)
	{
		using var fs = new FileStream(filePath, FileMode.Create);
		bitmap.Encode(fs, format.ToSKEncodedImageFormat(), 100);
	}

	/// <returns>
	/// <paramref name="bitmap"/>, encoded by <paramref name="format"/>.
	/// </returns>
	public static byte[] Encode(this SKBitmap bitmap, CaptchaImageFormat format = CaptchaImageFormat.Png)
	{
		using var ms = new MemoryStream();
		bitmap.Encode(ms, format.ToSKEncodedImageFormat(), 100);
		return ms.ToArray();
	}

	#endregion

	#region SKPath

	/// <returns>
	/// <paramref name="path"/> converted to <see cref="VectorPath"/>.
	/// </returns>
	/// <exception cref="NotSupportedException"/>
	public static VectorPath ToVectorPath(this SKPath path)
	{
		var result = new VectorPath();
		var instructions = result.Instructions;
		var points = new SKPoint[4];

		var iterator = path.CreateIterator(false);

		try
		{
			while (true)
			{
				var verb = iterator.Next(points);
				if (verb == SKPathVerb.Done) break;

				switch (verb)
				{
					case SKPathVerb.Move:
						instructions.Add(new MoveToInstruction(points[0].ToVector2()));
						break;
					case SKPathVerb.Line:
						instructions.Add(new LineToInstruction(points[1].ToVector2()));
						break;
					case SKPathVerb.Cubic:
						instructions.Add(new CubicToInstruction(points[1].ToVector2(), points[2].ToVector2(), points[3].ToVector2()));
						break;
					case SKPathVerb.Quad:
						instructions.Add(new QuadToInstruction(points[1].ToVector2(), points[2].ToVector2()));
						break;
					case SKPathVerb.Conic:
						instructions.Add(new RationalToInstruction(1, points[1].ToVector2(), iterator.ConicWeight(), points[2].ToVector2(), 1));
						break;
					case SKPathVerb.Close:
						instructions.Add(new CloseInstruction());
						break;
					default:
						throw new NotSupportedException($"{nameof(SKPathVerb)}.{verb} cannot be converted to {nameof(VectorPathInstruction)}");
				}
			}
		}
		finally
		{
			iterator.Dispose();
		}

		return result;
	}

	#endregion

	#region SKPoint

	/// <returns>
	/// <paramref name="point"/> converted to <see cref="SKPoint"/>.
	/// </returns>
	public static Vector2 ToVector2(this SKPoint point)
		=> new(point.X, point.Y);

	#endregion

	#region SKPaint

	/// <returns>
	/// <paramref name="paint"/> converted to <see cref="SKPaint"/>
	/// for rectangle <paramref name="rect"/>.
	/// </returns>
	public static SKPaint ToSKPaint(this PaintInfo paint, RectangleF rect)
		=> paint.ToSKPaint(() => rect);

	/// <returns>
	/// <paramref name="paint"/> converted to <see cref="SKPaint"/>
	/// for rectangle of <paramref name="obj"/> bounds.
	/// </returns>
	public static SKPaint ToSKPaint(this PaintInfo paint, VectorObject obj)
		=> paint.ToSKPaint(() => obj.GetBounds());

	/// <returns>
	/// <paramref name="paint"/> converted to <see cref="SKPaint"/>
	/// for rectangle returned by <paramref name="rect"/> factory.
	/// </returns>
	private static SKPaint ToSKPaint(this PaintInfo paint, Func<RectangleF> rect)
	{
		var result = new SKPaint { IsAntialias = paint.Antialiasing };
		var rectLazy = new Lazy<RectangleF>(rect);

		switch (paint)
		{
			case StrokePaintInfo stroke:
				result.Style = SKPaintStyle.Stroke;
				result.StrokeWidth = stroke.Width;
				result.StrokeCap = ((StrokeCap)stroke.Cap).ToSKStrokeCap();
				result.StrokeJoin = ((StrokeJoin)stroke.Join).ToSKStrokeJoin();
				result.PathEffect = SKPathEffect.CreateDash(stroke.Dash.Take(stroke.DashPairs * 2).ToArray(), 0);
				break;
			case FillPaintInfo _:
				result.Style = SKPaintStyle.Fill;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(paint));
		}

		PaintColor color = paint.Color;
		switch (color)
		{
			case SolidPaintColor solid:
				result.Color = ((CMColor)solid.Color).ToSKColor();
				break;
			case LinearGradientPaintColor linear:
				int count = linear.Count;
				result.Shader = SKShader.CreateLinearGradient(
					rectLazy.Value.GetAnchorPoint(linear.StartAnchor).ToSKPoint(),
					rectLazy.Value.GetAnchorPoint(linear.EndAnchor).ToSKPoint(),
					linear.Colors.Take(count).Select(c => c.ToSKColor()).ToArray(),
					linear.Positions.Take(count).ToArray(),
					linear.TileMode.GetNext().ToSKShaderTileMode());
				break;
			case RadialGradientPaintColor radial:
				count = radial.Count;
				result.Shader = SKShader.CreateRadialGradient(
					rectLazy.Value.GetAnchorPoint(radial.StartAnchor).ToSKPoint(),
					radial.Radius,
					radial.Colors.Take(count).Select(c => c.ToSKColor()).ToArray(),
					radial.Positions.Take(count).ToArray(),
					radial.TileMode.GetNext().ToSKShaderTileMode());
				break;
		}

		return result;
	}

	/// <returns>
	/// <paramref name="mode"/> converted to <see cref="SKShaderTileMode"/>.
	/// </returns>
	public static SKShaderTileMode ToSKShaderTileMode(this GradientTileMode mode)
		=> mode switch
		{
			GradientTileMode.Repeat => SKShaderTileMode.Repeat,
			GradientTileMode.Mirror => SKShaderTileMode.Mirror,
			GradientTileMode.Clamp => SKShaderTileMode.Clamp,
			_ => throw new ArgumentOutOfRangeException(nameof(mode)),
		};

	/// <returns>
	/// <paramref name="cap"/> converted to <see cref="SKStrokeCap"/>.
	/// </returns>
	public static SKStrokeCap ToSKStrokeCap(this StrokeCap cap)
		=> cap switch
		{
			StrokeCap.Square => SKStrokeCap.Square,
			StrokeCap.Round => SKStrokeCap.Round,
			StrokeCap.Butt => SKStrokeCap.Butt,
			_ => throw new ArgumentOutOfRangeException(nameof(cap)),
		};

	/// <returns>
	/// <paramref name="join"/> converted to <see cref="SKStrokeJoin"/>.
	/// </returns>
	public static SKStrokeJoin ToSKStrokeJoin(this StrokeJoin join)
		=> join switch
		{
			StrokeJoin.Miter => SKStrokeJoin.Miter,
			StrokeJoin.Round => SKStrokeJoin.Round,
			StrokeJoin.Bevel => SKStrokeJoin.Bevel,
			_ => throw new ArgumentOutOfRangeException(nameof(join)),
		};

	#endregion

	#region SKColor

	/// <returns>
	/// <paramref name="color"/> converted to <see cref="CMColor"/>.
	/// </returns>
	public static CMColor ToCMColor(this SKColor color)
		=> new(color.Red, color.Green, color.Blue, color.Alpha);

	#endregion

	#region SKCanvas

	/// <summary>
	/// Draws the vector <paramref name="obj"/> on <paramref name="canvas"/> using specified
	/// <paramref name="fill"/> and <paramref name="stroke"/>.
	/// </summary>
	public static void Draw(
		this SKCanvas canvas,
		VectorObject obj,
		FillPaintInfo fill,
		StrokePaintInfo stroke)
	{
		var fillEnabled = fill.Enabled;
		var strokeEnabled = stroke.Enabled;
		if (!fillEnabled && !strokeEnabled) return;

		var paintFill = fillEnabled ? fill.ToSKPaint(obj) : null;
		var paintStroke = strokeEnabled ? stroke.ToSKPaint(obj) : null;

		foreach (var path in obj)
		{
			var skPath = path.ToSKPath();
			if (fillEnabled) canvas.DrawPath(skPath, paintFill);
			if (strokeEnabled) canvas.DrawPath(skPath, paintStroke);
		}
	}

	#endregion
}
