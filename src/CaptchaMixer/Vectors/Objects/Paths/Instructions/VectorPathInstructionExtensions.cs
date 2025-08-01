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

public static class VectorPathInstructionExtensions
{
	/// <returns>
	/// <paramref name="instruction"/> cloned via <see cref="VectorPathInstruction.Clone"/> and
	/// cast to type <typeparamref name="T"/>.
	/// </returns>
	public static T CastClone<T>(this T instruction) where T : VectorPathInstruction
		=> (T)instruction.Clone();

	/// <returns>
	/// <paramref name="rect"/> converted to <see cref="SKRect"/>.
	/// </returns>
	public static SKRect ToSKRect(this AddRectInstruction rect)
		=> new(rect.LeftTop.X, rect.LeftTop.Y, rect.RightBottom.X, rect.RightBottom.Y);

	/// <returns>
	/// <paramref name="direction"/> converted to <see cref="SKPathDirection"/>.
	/// </returns>
	public static SKPathDirection ToSKPathDirection(this PathDirection direction)
		=> direction == PathDirection.Clockwise ? SKPathDirection.Clockwise : SKPathDirection.CounterClockwise;

	/// <returns>
	/// <paramref name="instruction"/> reversed so that the <paramref name="end"/> point is now
	/// the instruction's end point, or unchanged <paramref name="instruction"/> is it's
	/// not reversible.
	/// </returns>
	public static T Reverse<T>(this T instruction, Vector2 end)
		where T : VectorPathInstruction
		=> instruction switch
		{
			CubicToInstruction cubic => new CubicToInstruction(cubic.Control2, cubic.Control1, end) as T,
			QuadToInstruction quad => new QuadToInstruction(quad.Control, end) as T,
			RationalToInstruction rational => new RationalToInstruction(rational.EndWeight, rational.Control, rational.ControlWeight, end, rational.StartWeight) as T,
			LineToInstruction _ => new LineToInstruction(end) as T,
			MoveToInstruction _ => new MoveToInstruction(end) as T,
			_ => instruction,
		};

	/// <returns>
	/// <paramref name="instruction"/> reversed so that the end point described by
	/// <paramref name="endX"/> and <paramref name="endY"/> is now the instruction's
	/// end point, or unchanged <paramref name="instruction"/> is it's not reversible.
	/// </returns>
	public static T Reverse<T>(this T instruction, float endX, float endY)
		where T : VectorPathInstruction
		=> instruction.Reverse(new(endX, endY));

	/// <remarks>
	/// End point depends on type of <paramref name="instruction"/>.<br/>
	/// For straight lines and Bezier curves their end points are returned.<br/>
	/// For <see cref="MoveToInstruction"/> - the target pen move point.<br/>
	/// For contour instructions - the point used by SkiaSharp to start and end drawing:<br/>
	/// - <see cref="AddOvalInstruction"/>: the rightmost point;<br/>
	/// - <see cref="AddRoundRectInstruction"/>: the point on the left border where the
	/// lower-left rounded corner starts;<br/>
	/// - <see cref="AddRectInstruction"/>: left top corner.<br/>
	/// For any other instruction a <see cref="NotSupportedException"/> is thrown.
	/// </remarks>
	/// <returns>
	/// End point of <paramref name="instruction"/>.
	/// </returns>
	/// <exception cref="NotSupportedException"/>
	public static Vector2 GetEndPoint(this VectorPathInstruction instruction)
	{
		switch (instruction)
		{
			case CubicToInstruction cubic:
				return cubic.End;
			case QuadToInstruction quad:
				return quad.End;
			case RationalToInstruction rational:
				return rational.End;
			case LineToInstruction line:
				return line.End;
			case MoveToInstruction move:
				return move.Point;
			case AddOvalInstruction oval:
				var bounds = oval.GetBounds();
				return bounds.GetAnchorPoint(RectAnchor.RightCenter);
			case AddRoundRectInstruction roundRect:
				bounds = roundRect.GetBounds();
				return new(bounds.Left, bounds.Bottom - roundRect.RadiusY);
			case AddRectInstruction rect:
				return rect.LeftTop;
			default:
				throw new NotSupportedException($"Cannot evaluate end point of {instruction.GetType().Name}");
		}
	}

	/// <summary>
	/// Granulates <paramref name="instruction"/> by <paramref name="maxLength"/>.
	/// </summary>
	/// <param name="instruction">Instruction.</param>
	/// <param name="position">Pen position from which the instruction is drawn.</param>
	/// <param name="maxLength">Maximal segment length.</param>
	/// <returns>
	/// A sequence of granulated instructions or a sequence of only the original
	/// instruction if granulation is not possible.
	/// </returns>
	public static IEnumerable<VectorPathInstruction> Granulate(
		this VectorPathInstruction instruction,
		Vector2 position,
		float maxLength)
		=> instruction switch
		{
			LineToInstruction line => VectorMath
				.LineGranulate(position, line.End, maxLength)
				.Skip(1)
				.Select(c => new LineToInstruction(c)),
			CubicToInstruction cubic => VectorMath
				.CurveCubicGranulate(new CurveCubic2(position, cubic.Control1, cubic.Control2, cubic.End), maxLength)
				.Select(c => new CubicToInstruction(c.Control1, c.Control2, c.End)),
			QuadToInstruction quad => VectorMath
				.CurveQuadGranulate(new CurveQuad2(position, quad.Control, quad.End), maxLength)
				.Select(c => new QuadToInstruction(c.Control, c.End)),
			RationalToInstruction rational => VectorMath
				.CurveRationalGranulate(
					new CurveQuad3(
						new(position, rational.StartWeight),
						new(rational.Control, rational.ControlWeight),
						new(rational.End, rational.EndWeight)),
					maxLength)
				.Select(c => new RationalToInstruction(c.Start.Z, c.Control, c.End)),
			_ => new[] { instruction },
		};

	/// <inheritdoc cref="Granulate(VectorPathInstruction, Vector2, float)"/>
	/// <param name="positionX">Pen X axis position from which the instruction is drawn.</param>
	/// <param name="positionY">Pen Y axis position from which the instruction is drawn.</param>
	public static IEnumerable<VectorPathInstruction> Granulate(
		this VectorPathInstruction instruction,
		float positionX,
		float positionY,
		float maxLength)
		=> instruction.Granulate(new(positionX, positionY), maxLength);

	/// <summary>
	/// Converts a single contour instruction into a sequence of lower-level simple instructions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Contour instructions are the ones inherited from <see cref="ContourInstruction"/> -
	/// <see cref="AddRectInstruction"/>, <see cref="AddOvalInstruction"/> and <see cref="AddRoundRectInstruction"/>.
	/// </para>
	/// <para>
	/// Some transformations (including rotation) don't work correctly for those instructions
	/// because transformations process <see cref="VectorPathInstruction.Points"/> and contour
	/// instructions do not hold all actual points which would be used for rendering.
	/// So for those transformations to work we need to split these instructions into sets of
	/// lower-level instructions which are completely normally transformable.
	/// But if such transformations are not needed then disassembling shall not be performed,
	/// because contour instructions are otherwise processed and rendered faster.
	/// </para>
	/// </remarks>
	/// <returns>
	/// A sequence of non-contour instructions that eventually draw the same path, or a sequence
	/// of just <paramref name="instruction"/> if it's not a contour.
	/// </returns>
	public static IEnumerable<VectorPathInstruction> Disassemble(this VectorPathInstruction instruction)
	{
		const float ovalControlWeight = 0.7071f; // ~cos(45°)

		switch (instruction)
		{
			case AddOvalInstruction oval:

				var bounds = oval.GetBounds();
				var leftTop = bounds.GetAnchorPoint(RectAnchor.LeftTop);
				var rightTop = bounds.GetAnchorPoint(RectAnchor.RightTop);
				var rightBottom = bounds.GetAnchorPoint(RectAnchor.RightBottom);
				var leftBottom = bounds.GetAnchorPoint(RectAnchor.LeftBottom);
				var rightCenter = bounds.GetAnchorPoint(RectAnchor.RightCenter);
				var leftCenter = bounds.GetAnchorPoint(RectAnchor.LeftCenter);
				var centerTop = bounds.GetAnchorPoint(RectAnchor.CenterTop);
				var centerBottom = bounds.GetAnchorPoint(RectAnchor.CenterBottom);

				yield return new MoveToInstruction(rightCenter);

				if (oval.Direction == PathDirection.Clockwise)
				{
					yield return new RationalToInstruction(1, rightBottom, ovalControlWeight, centerBottom, 1);
					yield return new RationalToInstruction(1, leftBottom, ovalControlWeight, leftCenter, 1);
					yield return new RationalToInstruction(1, leftTop, ovalControlWeight, centerTop, 1);
					yield return new RationalToInstruction(1, rightTop, ovalControlWeight, rightCenter, 1);
				}
				else
				{
					yield return new RationalToInstruction(1, rightTop, ovalControlWeight, centerTop, 1);
					yield return new RationalToInstruction(1, leftTop, ovalControlWeight, leftCenter, 1);
					yield return new RationalToInstruction(1, leftBottom, ovalControlWeight, centerBottom, 1);
					yield return new RationalToInstruction(1, rightBottom, ovalControlWeight, rightCenter, 1);
				}

				yield return new CloseInstruction();

				break;

			case AddRoundRectInstruction roundRect:

				bounds = roundRect.GetBounds();
				var left = bounds.Left;
				var top = bounds.Top;
				var right = bounds.Right;
				var bottom = bounds.Bottom;
				var radiusX = roundRect.RadiusX;
				var radiusY = roundRect.RadiusY;

				yield return new MoveToInstruction(left, bottom - radiusY);

				if (roundRect.Direction == PathDirection.Clockwise)
				{
					yield return new LineToInstruction(new(left, top + radiusY));
					yield return new RationalToInstruction(1, new(left, top), ovalControlWeight, new(left + radiusX, top), 1);
					yield return new LineToInstruction(new(right - radiusX, top));
					yield return new RationalToInstruction(1, new(right, top), ovalControlWeight, new(right, top + radiusY), 1);
					yield return new LineToInstruction(new(right, bottom - radiusY));
					yield return new RationalToInstruction(1, new(right, bottom), ovalControlWeight, new(right - radiusX, bottom), 1);
					yield return new LineToInstruction(new(left + radiusX, bottom));
					yield return new RationalToInstruction(1, new(left, bottom), ovalControlWeight, new(left, bottom - radiusY), 1);
				}
				else
				{
					yield return new RationalToInstruction(1, new(left, bottom), ovalControlWeight, new(left + radiusX, bottom), 1);
					yield return new LineToInstruction(new(right - radiusX, bottom));
					yield return new RationalToInstruction(1, new(right, bottom), ovalControlWeight, new(right, bottom - radiusY), 1);
					yield return new LineToInstruction(new(right, top + radiusY));
					yield return new RationalToInstruction(1, new(right, top), ovalControlWeight, new(right - radiusX, top), 1);
					yield return new LineToInstruction(new(left + radiusX, top));
					yield return new RationalToInstruction(1, new(left, top), ovalControlWeight, new(left, top + radiusY), 1);
					yield return new LineToInstruction(new(left, bottom - radiusY));
				}

				yield return new CloseInstruction();

				break;

			case AddRectInstruction rect:

				bounds = rect.GetBounds();
				leftTop = bounds.GetAnchorPoint(RectAnchor.LeftTop);
				rightTop = bounds.GetAnchorPoint(RectAnchor.RightTop);
				rightBottom = bounds.GetAnchorPoint(RectAnchor.RightBottom);
				leftBottom = bounds.GetAnchorPoint(RectAnchor.LeftBottom);

				yield return new MoveToInstruction(leftTop);

				if (rect.Direction == PathDirection.Clockwise)
				{
					yield return new LineToInstruction(rightTop);
					yield return new LineToInstruction(rightBottom);
					yield return new LineToInstruction(leftBottom);
					yield return new LineToInstruction(leftTop);
				}
				else
				{
					yield return new LineToInstruction(leftBottom);
					yield return new LineToInstruction(rightBottom);
					yield return new LineToInstruction(rightTop);
					yield return new LineToInstruction(leftTop);
				}

				yield return new CloseInstruction();

				break;

			default:
				yield return instruction;
				break;
		}
	}

	/// <summary>
	/// Creates a <see cref="LineToInstruction"/> based on the end point of <paramref name="instruction"/>.
	/// </summary>
	/// <remarks>
	/// The instruction's end point is acquired via <see cref="GetEndPoint(VectorPathInstruction)"/>.
	/// </remarks>
	/// <returns>
	/// A new <see cref="LineToInstruction"/> based on the <paramref name="instruction"/>'s end point if
	/// it's a Bezier curve or contour, or the <paramref name="instruction"/> itself in all other cases.
	/// </returns>
	public static VectorPathInstruction ToLine(this VectorPathInstruction instruction)
		=> instruction switch
		{
			CurveInstruction or ContourInstruction => new LineToInstruction(instruction.GetEndPoint()),
			_ => instruction,
		};

	/// <returns>
	/// <see langword="true"/> if <paramref name="instruction"/> causes anything to be actually drawn.
	/// </returns>
	public static bool IsGraphical(this VectorPathInstruction instruction)
		=> instruction is LineToInstruction ||
			instruction is CurveInstruction ||
			instruction is ContourInstruction;

	/// <returns>
	/// <paramref name="instruction"/>'s points and their types as a tuple sequence.
	/// </returns>
	public static IEnumerable<(Vector2 point, PointType type)> EnumeratePoints(this VectorPathInstruction instruction)
	{
		for (var i = 0; i < instruction.Points.Length; i++)
			yield return (instruction.Points[i], instruction.PointTypes[i]);
	}
}
