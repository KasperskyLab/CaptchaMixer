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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Objects.Paths;

public class VectorPathExtensionsTests
{
	[Test]
	public void VectorPathExtensions_AddInstructions_Test()
	{
		var path = new VectorPath()
			.MoveTo(0, 0)
			.MoveTo(new(1, 0))
			.Close()
			.LineTo(3, 0)
			.LineTo(new(4, 0))
			.QuadTo(5, 0, 5, 1)
			.QuadTo(new(6, 0), new(6, 1))
			.CubicTo(7, 0, 7, 1, 7, 2)
			.CubicTo(new(8, 0), new(8, 1), new(8, 2))
			.RationalTo(90, 9, 0, 91, 9, 1, 92)
			.RationalTo(100, new(10, 0, 101), new(10, 1, 102))
			.RationalTo(110, new(11, 0), 111, new(11, 1), 112)
			.AddRect(12, 0, 12, 1, PathDirection.Clockwise)
			.AddRect(new(13, 0), new(13, 1), PathDirection.CounterClockwise)
			.AddRoundRect(14, 0, 14, 1, 140, 141, PathDirection.Clockwise)
			.AddRoundRect(15, 0, 15, 1, 150, PathDirection.CounterClockwise)
			.AddRoundRect(new(16, 0), new(16, 1), 160, 161, PathDirection.Clockwise)
			.AddRoundRect(new(17, 0), new(17, 1), 170, PathDirection.CounterClockwise)
			.AddOval(18, 0, 18, 1, PathDirection.Clockwise)
			.AddOval(new Vector2(19, 0), new Vector2(19, 1), PathDirection.CounterClockwise)
			.AddOval(new Vector2(0, 0), 200, 201, PathDirection.Clockwise)
			.AddOval(new Vector2(0, 0), 210, PathDirection.CounterClockwise);

		var moveTo = path.Instructions[0].Should().BeOfType<MoveToInstruction>().Which;
		moveTo.Points.Should().HaveCount(1).And.ContainInConsecutiveOrder(new Vector2(0, 0));

		moveTo = path.Instructions[1].Should().BeOfType<MoveToInstruction>().Which;
		moveTo.Points.Should().HaveCount(1).And.ContainInConsecutiveOrder(new Vector2(1, 0));

		var close = path.Instructions[2].Should().BeOfType<CloseInstruction>().Which;
		close.Points.Should().HaveCount(0);

		var lineTo = path.Instructions[3].Should().BeOfType<LineToInstruction>().Which;
		lineTo.Points.Should().HaveCount(1).And.ContainInConsecutiveOrder(new Vector2(3, 0));

		lineTo = path.Instructions[4].Should().BeOfType<LineToInstruction>().Which;
		lineTo.Points.Should().HaveCount(1).And.ContainInConsecutiveOrder(new Vector2(4, 0));

		var quadTo = path.Instructions[5].Should().BeOfType<QuadToInstruction>().Which;
		quadTo.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(5, 0), new Vector2(5, 1));

		quadTo = path.Instructions[6].Should().BeOfType<QuadToInstruction>().Which;
		quadTo.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(6, 0), new Vector2(6, 1));

		var cubicTo = path.Instructions[7].Should().BeOfType<CubicToInstruction>().Which;
		cubicTo.Points.Should().HaveCount(3).And.ContainInConsecutiveOrder(new Vector2(7, 0), new Vector2(7, 1));

		cubicTo = path.Instructions[8].Should().BeOfType<CubicToInstruction>().Which;
		cubicTo.Points.Should().HaveCount(3).And.ContainInConsecutiveOrder(new Vector2(8, 0), new Vector2(8, 1));

		var rationalTo = path.Instructions[9].Should().BeOfType<RationalToInstruction>().Which;
		rationalTo.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(9, 0), new Vector2(9, 1));
		rationalTo.StartWeight.Should().Be(90);
		rationalTo.ControlWeight.Should().Be(91);
		rationalTo.EndWeight.Should().Be(92);

		rationalTo = path.Instructions[10].Should().BeOfType<RationalToInstruction>().Which;
		rationalTo.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(10, 0), new Vector2(10, 1));
		rationalTo.StartWeight.Should().Be(100);
		rationalTo.ControlWeight.Should().Be(101);
		rationalTo.EndWeight.Should().Be(102);

		rationalTo = path.Instructions[11].Should().BeOfType<RationalToInstruction>().Which;
		rationalTo.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(11, 0), new Vector2(11, 1));
		rationalTo.StartWeight.Should().Be(110);
		rationalTo.ControlWeight.Should().Be(111);
		rationalTo.EndWeight.Should().Be(112);

		var addRect = path.Instructions[12].Should().BeOfType<AddRectInstruction>().Which;
		addRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(12, 0), new Vector2(12, 1));
		addRect.Direction.Should().Be(PathDirection.Clockwise);

		addRect = path.Instructions[13].Should().BeOfType<AddRectInstruction>().Which;
		addRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(13, 0), new Vector2(13, 1));
		addRect.Direction.Should().Be(PathDirection.CounterClockwise);

		var addRoundRect = path.Instructions[14].Should().BeOfType<AddRoundRectInstruction>().Which;
		addRoundRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(14, 0), new Vector2(14, 1));
		addRoundRect.Direction.Should().Be(PathDirection.Clockwise);
		addRoundRect.RadiusX.Should().Be(140);
		addRoundRect.RadiusY.Should().Be(141);

		addRoundRect = path.Instructions[15].Should().BeOfType<AddRoundRectInstruction>().Which;
		addRoundRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(15, 0), new Vector2(15, 1));
		addRoundRect.Direction.Should().Be(PathDirection.CounterClockwise);
		addRoundRect.RadiusX.Should().Be(150);
		addRoundRect.RadiusY.Should().Be(150);

		addRoundRect = path.Instructions[16].Should().BeOfType<AddRoundRectInstruction>().Which;
		addRoundRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(16, 0), new Vector2(16, 1));
		addRoundRect.Direction.Should().Be(PathDirection.Clockwise);
		addRoundRect.RadiusX.Should().Be(160);
		addRoundRect.RadiusY.Should().Be(161);

		addRoundRect = path.Instructions[17].Should().BeOfType<AddRoundRectInstruction>().Which;
		addRoundRect.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(17, 0), new Vector2(17, 1));
		addRoundRect.Direction.Should().Be(PathDirection.CounterClockwise);
		addRoundRect.RadiusX.Should().Be(170);
		addRoundRect.RadiusY.Should().Be(170);

		var addOval = path.Instructions[18].Should().BeOfType<AddOvalInstruction>().Which;
		addOval.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(18, 0), new Vector2(18, 1));
		addOval.Direction.Should().Be(PathDirection.Clockwise);

		addOval = path.Instructions[19].Should().BeOfType<AddOvalInstruction>().Which;
		addOval.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(19, 0), new Vector2(19, 1));
		addOval.Direction.Should().Be(PathDirection.CounterClockwise);

		addOval = path.Instructions[20].Should().BeOfType<AddOvalInstruction>().Which;
		addOval.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(-200, -201), new Vector2(200, 201));
		addOval.Direction.Should().Be(PathDirection.Clockwise);

		addOval = path.Instructions[21].Should().BeOfType<AddOvalInstruction>().Which;
		addOval.Points.Should().HaveCount(2).And.ContainInConsecutiveOrder(new Vector2(-210, -210), new Vector2(210, 210));
		addOval.Direction.Should().Be(PathDirection.CounterClockwise);
	}

	[Test]
	public void VectorPathExtensions_ToSKPath_Test()
	{
		var skPath = new VectorPath()
			.MoveTo(0, 0)
			.Close()
			.LineTo(2, 0)
			.QuadTo(3, 0, 3, 1)
			.CubicTo(4, 0, 4, 1, 4, 2)
			.RationalTo(50, new(5, 0), 51, new(5, 1), 52)
			.RationalTo(1, new(6, 0), 61, new(6, 1), 1)
			.AddRect(7, 0, 7, 1, PathDirection.Clockwise)
			.AddRoundRect(8, 0, 8, 1, 80, 81, PathDirection.CounterClockwise)
			.AddOval(9, 0, 9, 1, PathDirection.Clockwise)
			.ToSKPath();

		var iterator = skPath.CreateIterator(false);
		var items = new List<(SKPathVerb Verb, SKPoint[] Points)>();

		try
		{
			while (true)
			{
				var points = new SKPoint[4];
				var verb = iterator.Next(points);
				if (verb == SKPathVerb.Done) break;
				items.Add((verb, points));
			}
		}
		finally
		{
			iterator.Dispose();
		}

		items.Should().HaveCount(40);

		items[00].Verb.Should().Be(SKPathVerb.Move);
		items[00].Points.Should().ContainInConsecutiveOrder(new(0, 0), new(0, 0), new(0, 0), new(0, 0));

		items[01].Verb.Should().Be(SKPathVerb.Close);
		items[01].Points.Should().ContainInConsecutiveOrder(new(0, 0), new(0, 0), new(0, 0), new(0, 0));

		items[02].Verb.Should().Be(SKPathVerb.Move);
		items[02].Points.Should().ContainInConsecutiveOrder(new(0, 0), new(0, 0), new(0, 0), new(0, 0));

		items[03].Verb.Should().Be(SKPathVerb.Line);
		items[03].Points.Should().ContainInConsecutiveOrder(new(0, 0), new(2, 0), new(0, 0), new(0, 0));

		items[04].Verb.Should().Be(SKPathVerb.Quad);
		items[04].Points.Should().ContainInConsecutiveOrder(new(2, 0), new(3, 0), new(3, 1), new(0, 0));

		items[05].Verb.Should().Be(SKPathVerb.Cubic);
		items[05].Points.Should().ContainInConsecutiveOrder(new(3, 1), new(4, 0), new(4, 1), new(4, 2));

		// next we have a long sequence of lines which is a result of VectorMath.CurveRationalAngleSplit.
		// validating all of that would be too fragile since it depends on minLinearAngle passed to angle
		// split method and this angle may later be changed. so we'll just validate first and end points.
		items[06].Verb.Should().Be(SKPathVerb.Line);
		items[06].Points[0].Should().Be(new SKPoint(4, 2));

		int i;
		var found = false;
		for (i = 7; i < items.Count; i++)
		{
			if (items[i].Verb != SKPathVerb.Conic) continue;
			found = true;
			break;
		}

		found.Should().BeTrue();

		items[i - 01].Points[1].Should().Be(new SKPoint(5, 1));

		items[i - 00].Points.Should().ContainInConsecutiveOrder(new(5, 1), new(6, 0), new(6, 1), new(0, 0));

		items[i + 01].Verb.Should().Be(SKPathVerb.Move);
		items[i + 01].Points.Should().ContainInConsecutiveOrder(new(7, 0), new(0, 0), new(0, 0), new(0, 0));
		items[i + 02].Verb.Should().Be(SKPathVerb.Line);
		items[i + 02].Points.Should().ContainInConsecutiveOrder(new(7, 0), new(7, 0), new(0, 0), new(0, 0));
		items[i + 03].Verb.Should().Be(SKPathVerb.Line);
		items[i + 03].Points.Should().ContainInConsecutiveOrder(new(7, 0), new(7, 1), new(0, 0), new(0, 0));
		items[i + 04].Verb.Should().Be(SKPathVerb.Line);
		items[i + 04].Points.Should().ContainInConsecutiveOrder(new(7, 1), new(7, 1), new(0, 0), new(0, 0));
		items[i + 05].Verb.Should().Be(SKPathVerb.Line);
		items[i + 05].Points.Should().ContainInConsecutiveOrder(new(7, 1), new(7, 0), new(0, 0), new(0, 0));
		items[i + 06].Verb.Should().Be(SKPathVerb.Close);
		items[i + 06].Points.Should().ContainInConsecutiveOrder(new(7, 0), new(0, 0), new(0, 0), new(0, 0));

		items[i + 07].Verb.Should().Be(SKPathVerb.Move);
		items[i + 07].Points.Should().ContainInConsecutiveOrder(new(8, 0), new(0, 0), new(0, 0), new(0, 0));
		items[i + 08].Verb.Should().Be(SKPathVerb.Line);
		items[i + 08].Points.Should().ContainInConsecutiveOrder(new(8, 0), new(8, 1), new(0, 0), new(0, 0));
		items[i + 09].Verb.Should().Be(SKPathVerb.Line);
		items[i + 09].Points.Should().ContainInConsecutiveOrder(new(8, 1), new(8, 1), new(0, 0), new(0, 0));
		items[i + 10].Verb.Should().Be(SKPathVerb.Line);
		items[i + 10].Points.Should().ContainInConsecutiveOrder(new(8, 1), new(8, 0), new(0, 0), new(0, 0));
		items[i + 11].Verb.Should().Be(SKPathVerb.Close);
		items[i + 11].Points.Should().ContainInConsecutiveOrder(new(8, 0), new(0, 0), new(0, 0), new(0, 0));

		items[i + 12].Verb.Should().Be(SKPathVerb.Move);
		items[i + 12].Points.Should().ContainInConsecutiveOrder(new(9, 0.5f), new(0, 0), new(0, 0), new(0, 0));
		items[i + 13].Verb.Should().Be(SKPathVerb.Conic);
		items[i + 13].Points.Should().ContainInConsecutiveOrder(new(9, 0.5f), new(9, 1), new(9, 1), new(0, 0));
		items[i + 14].Verb.Should().Be(SKPathVerb.Conic);
		items[i + 14].Points.Should().ContainInConsecutiveOrder(new(9, 1), new(9, 1), new(9, 0.5f), new(0, 0));
		items[i + 15].Verb.Should().Be(SKPathVerb.Conic);
		items[i + 15].Points.Should().ContainInConsecutiveOrder(new(9, 0.5f), new(9, 0), new(9, 0), new(0, 0));
		items[i + 16].Verb.Should().Be(SKPathVerb.Conic);
		items[i + 16].Points.Should().ContainInConsecutiveOrder(new(9, 0), new(9, 0), new(9, 0.5f), new(0, 0));
		items[i + 17].Verb.Should().Be(SKPathVerb.Close);
		items[i + 17].Points.Should().ContainInConsecutiveOrder(new(9, 0.5f), new(0, 0), new(0, 0), new(0, 0));
	}

	[Test]
	public void VectorPathExtensions_IsMeaningful_Test()
	{
		new VectorPath().IsMeaningful().Should().BeFalse();
		new VectorPath().MoveTo(10, 10).Close().IsMeaningful().Should().BeFalse();
		new VectorPath().LineTo(10, 10).IsMeaningful().Should().BeTrue();
		new VectorPath().QuadTo(10, 10, 20, 20).IsMeaningful().Should().BeTrue();
		new VectorPath().CubicTo(10, 10, 20, 20, 30, 30).IsMeaningful().Should().BeTrue();
		new VectorPath().RationalTo(0, new(10, 10), 1, new(20, 20), 2).IsMeaningful().Should().BeTrue();
		new VectorPath().AddRect(10, 10, 20, 20).IsMeaningful().Should().BeTrue();
		new VectorPath().AddRoundRect(10, 10, 20, 20, 1).IsMeaningful().Should().BeTrue();
		new VectorPath().AddOval(10, 10, 20, 20).IsMeaningful().Should().BeTrue();
	}

	[Test]
	public void VectorPathExtensions_EnumerateContours_Test()
	{
		new VectorPath().EnumerateContours().Should().HaveCount(0);
		new VectorPath().MoveTo(10, 10).EnumerateContours().Should().HaveCount(0);
		new VectorPath().LineTo(10, 10).EnumerateContours().Should().HaveCount(1);

		new VectorPath()
			.LineTo(10, 10)
			.Close()
			.LineTo(10, 0)
			.EnumerateContours()
			.Should()
			.HaveCount(2);

		new VectorPath()
			.LineTo(10, 10)
			.AddRect(20, 20, 30, 30)
			.LineTo(10, 0)
			.EnumerateContours()
			.Should()
			.HaveCount(3);
	}

	[Test]
	public void VectorPathExtensions_MoveX_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).MoveX(10);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(20, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 20));
	}

	[Test]
	public void VectorPathExtensions_MoveY_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).MoveY(10);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 20));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(20, 30));
	}

	[Test]
	public void VectorPathExtensions_Move_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).Move(10, 10);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(20, 20));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	public void VectorPathExtensions_ScaleX_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).ScaleX(new(10, 10), 2);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 20));
	}

	[Test]
	public void VectorPathExtensions_ScaleY_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).ScaleY(new(10, 10), 2);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(20, 30));
	}

	[Test]
	public void VectorPathExtensions_ScaleXY_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).Scale(new(10, 10), 2, 2);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	public void VectorPathExtensions_Scale_Test()
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).Scale(new(10, 10), 2);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		path.Instructions[1].As<LineToInstruction>().End.Should().Be(new Vector2(30, 30));
	}

	[Test]
	[TestCase(0, 20, 20)]
	[TestCase(90, 0, 20)]
	[TestCase(-90, 20, 0)]
	[TestCase(360, 20, 20)]
	[TestCase(-360, 20, 20)]
	[TestCase(450, 0, 20)]
	[TestCase(-450, 20, 0)]
	public void VectorPathExtensions_Rotate_Test(float angle, float lineToX, float lineToY)
	{
		var path = new VectorPath().MoveTo(10, 10).LineTo(20, 20).Rotate(new(10, 10), angle);
		path.Instructions[0].As<MoveToInstruction>().Point.Should().Be(new Vector2(10, 10));
		var lineToEnd = path.Instructions[1].As<LineToInstruction>().End;
		lineToEnd.X.Should().BeApproximately(lineToX, 0.00001f);
		lineToEnd.Y.Should().BeApproximately(lineToY, 0.00001f);
	}

	[Test]
	public void VectorPathExtensions_Straighten_Test()
	{
		var path = new VectorPath()
			.LineTo(10, 10)
			.LineTo(20, 0)
			.QuadTo(30, 10, 40, 0)
			.RationalTo(1, new(50, 10), 1, new(60, 0), 1)
			.CubicTo(60, 10, 70, 10, 70, 0)
			.LineTo(80, 20)
			.LineTo(90, 0)
			.QuadTo(100, 20, 110, 0)
			.RationalTo(1, new(120, 20), 1, new(130, 0), 1)
			.CubicTo(130, 20, 140, 20, 140, 0)
			.Straighten(0.5f, 10);

		// two line segments - middle point moved
		path.Instructions[0].Points.Should().ContainInConsecutiveOrder(new Vector2(10, 5));
		path.Instructions[1].Points.Should().ContainInConsecutiveOrder(new Vector2(20, 0));
		// quad curves - single control point moved
		path.Instructions[2].Points.Should().ContainInConsecutiveOrder(new Vector2(30, 5), new Vector2(40, 0));
		path.Instructions[3].Points.Should().ContainInConsecutiveOrder(new Vector2(50, 5), new Vector2(60, 0));
		// cubic curve - both control points moved
		path.Instructions[4].Points.Should().ContainInConsecutiveOrder(new Vector2(60, 5), new Vector2(70, 5), new Vector2(70, 0));
		// same instrution types but with attractable points beyond max distance - not moved
		path.Instructions[5].Points.Should().ContainInConsecutiveOrder(new Vector2(80, 20));
		path.Instructions[6].Points.Should().ContainInConsecutiveOrder(new Vector2(90, 0));
		path.Instructions[7].Points.Should().ContainInConsecutiveOrder(new Vector2(100, 20), new Vector2(110, 0));
		path.Instructions[8].Points.Should().ContainInConsecutiveOrder(new Vector2(120, 20), new Vector2(130, 0));
		path.Instructions[9].Points.Should().ContainInConsecutiveOrder(new Vector2(130, 20), new Vector2(140, 20), new Vector2(140, 0));
	}

	[Test]
	public void VectorPathExtensions_Primitivize_Test()
	{
		var path = new VectorPath()
			// the following instructions shall all be replaced with a single line
			.LineTo(10, 1)
			.LineTo(20, 5)
			.LineTo(30, 10)
			.LineTo(40, 5)
			.LineTo(50, 1)
			.LineTo(60, 0)
			.QuadTo(70, 100, 80, 1)
			.RationalTo(1, new(90, -100), 1, new(100, 1), -1)
			.CubicTo(100, 50, 110, -50, 110, 0)
			// here we jump too far away by Y
			.LineTo(110, 100)
			// one more jump, this time by X.
			// this shall result in one line for a jump and one more for all other instructions.
			.LineTo(130, 100)
			.LineTo(140, 101)
			.LineTo(150, 105)
			.QuadTo(160, 200, 170, 110)
			.RationalTo(1, new(180, 0), 1, new(190, 105), 1)
			.CubicTo(190, 200, 200, 0, 200, 100)
			.Primitivize(10);

		path.Instructions.Should().HaveCount(3);
		path.Instructions.Should().AllBeOfType<LineToInstruction>();
		path.Instructions[0].Points[0].Should().Be(new Vector2(110, 0));
		path.Instructions[1].Points[0].Should().Be(new Vector2(110, 100));
		path.Instructions[2].Points[0].Should().Be(new Vector2(200, 100));
	}
}