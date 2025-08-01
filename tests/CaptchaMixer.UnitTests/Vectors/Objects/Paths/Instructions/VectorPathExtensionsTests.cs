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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Objects.Paths.Instructions;

public class VectorPathInstructionExtensionsTests
{
	[Test]
	public void VectorPathInstructionExtensions_Reverse_Test()
	{
		var moveTo = new MoveToInstruction(10, 10).Reverse(0, 0);
		moveTo.Point.Should().Be(new Vector2(0, 0));

		var lineTo = new LineToInstruction(10, 10).Reverse(0, 0);
		lineTo.End.Should().Be(new Vector2(0, 0));

		var close = new CloseInstruction();
		close.Reverse(0, 0).Should().Be(close);

		var quadTo = new QuadToInstruction(10, 10, 20, 0).Reverse(0, 0);
		quadTo.Control.Should().Be(new Vector2(10, 10));
		quadTo.End.Should().Be(new Vector2(0, 0));

		var rationalTo = new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).Reverse(0, 0);
		rationalTo.StartWeight.Should().Be(3);
		rationalTo.Control.Should().Be(new Vector2(10, 10));
		rationalTo.ControlWeight.Should().Be(2);
		rationalTo.End.Should().Be(new Vector2(0, 0));
		rationalTo.EndWeight.Should().Be(1);

		var cubicTo = new CubicToInstruction(0, 10, 10, 10, 10, 0).Reverse(0, 0);
		cubicTo.Control1.Should().Be(new Vector2(10, 10));
		cubicTo.Control2.Should().Be(new Vector2(0, 10));
		cubicTo.End.Should().Be(new Vector2(0, 0));

		var addRect = new AddRectInstruction(10, 10, 20, 20).Reverse(0, 0);
		addRect.LeftTop.Should().Be(new Vector2(10, 10));
		addRect.RightBottom.Should().Be(new Vector2(20, 20));

		var addRoundRect = new AddRoundRectInstruction(10, 10, 20, 20, 2).Reverse(0, 0);
		addRoundRect.LeftTop.Should().Be(new Vector2(10, 10));
		addRoundRect.RightBottom.Should().Be(new Vector2(20, 20));

		var addOval = new AddRectInstruction(10, 10, 20, 20).Reverse(0, 0);
		addOval.LeftTop.Should().Be(new Vector2(10, 10));
		addOval.RightBottom.Should().Be(new Vector2(20, 20));
	}

	[Test]
	public void VectorPathInstructionExtensions_GetEndPoint_Test()
	{
		new MoveToInstruction(10, 10).GetEndPoint().Should().Be(new Vector2(10, 10));
		new LineToInstruction(10, 10).GetEndPoint().Should().Be(new Vector2(10, 10));
		var func = () => new CloseInstruction().GetEndPoint();
		func.Should().Throw<NotSupportedException>();
		new QuadToInstruction(10, 10, 20, 0).GetEndPoint().Should().Be(new Vector2(20, 0));
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).GetEndPoint().Should().Be(new Vector2(20, 0));
		new CubicToInstruction(0, 10, 10, 10, 10, 0).GetEndPoint().Should().Be(new Vector2(10, 0));
		new AddRectInstruction(10, 10, 20, 20).GetEndPoint().Should().Be(new Vector2(10, 10));
		new AddRoundRectInstruction(10, 10, 20, 20, 2).GetEndPoint().Should().Be(new Vector2(10, 18));
		new AddOvalInstruction(10, 10, 20, 20).GetEndPoint().Should().Be(new Vector2(20, 15));
	}

	[Test]
	public void VectorPathInstructionExtensions_Granulate_Test()
	{
		// instruction's Granulate extension calls specific granulation methods from VectorMath which are
		// covered by VectorMath tests. detailed testing would cost too much effort for basically no reason.
		// so here we only verify whether any granulation occurs.
		new MoveToInstruction(10, 0).Granulate(0, 0, 5).Should().AllBeOfType<MoveToInstruction>().And.HaveCount(1);
		new LineToInstruction(10, 0).Granulate(0, 0, 5).Should().AllBeOfType<LineToInstruction>().And.HaveCountGreaterThan(1);
		new CloseInstruction().Granulate(0, 0, 5).Should().AllBeOfType<CloseInstruction>().And.HaveCount(1);
		new QuadToInstruction(10, 10, 20, 0).Granulate(0, 0, 5).Should().AllBeOfType<QuadToInstruction>().And.HaveCountGreaterThan(1);
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).Granulate(0, 0, 5).Should().AllBeOfType<RationalToInstruction>().And.HaveCountGreaterThan(1);
		new CubicToInstruction(0, 10, 10, 10, 10, 0).Granulate(0, 0, 5).Should().AllBeOfType<CubicToInstruction>().And.HaveCountGreaterThan(1);
		// non-disassembled contour instructions are not granulated
		new AddRectInstruction(10, 10, 20, 20).Granulate(0, 0, 5).Should().AllBeOfType<AddRectInstruction>().And.HaveCount(1);
		new AddRoundRectInstruction(10, 10, 20, 20, 2).Granulate(0, 0, 5).Should().AllBeOfType<AddRoundRectInstruction>().And.HaveCount(1);
		new AddOvalInstruction(10, 10, 20, 20).Granulate(0, 0, 5).Should().AllBeOfType<AddOvalInstruction>().And.HaveCount(1);
	}

	[Test]
	public void VectorPathInstructionExtensions_Disassemble_Test()
	{
		// disassemblying only works for contour instructions, for others a single-item enumerable is returned
		new MoveToInstruction(10, 0).Disassemble().Should().AllBeOfType<MoveToInstruction>().And.HaveCount(1);
		new LineToInstruction(10, 0).Disassemble().Should().AllBeOfType<LineToInstruction>().And.HaveCount(1);
		new CloseInstruction().Disassemble().Should().AllBeOfType<CloseInstruction>().And.HaveCount(1);
		new QuadToInstruction(10, 10, 20, 0).Disassemble().Should().AllBeOfType<QuadToInstruction>().And.HaveCount(1);
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).Disassemble().Should().AllBeOfType<RationalToInstruction>().And.HaveCount(1);
		new CubicToInstruction(0, 10, 10, 10, 10, 0).Disassemble().Should().AllBeOfType<CubicToInstruction>().And.HaveCount(1);

		var disassembled = new AddRectInstruction(10, 10, 20, 20, PathDirection.Clockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(6);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 10));
		disassembled[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 10));
		disassembled[2].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 20));
		disassembled[3].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 20));
		disassembled[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
		disassembled[5].Should().BeOfType<CloseInstruction>();

		disassembled = new AddRectInstruction(10, 10, 20, 20, PathDirection.CounterClockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(6);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 10));
		disassembled[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 20));
		disassembled[2].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 20));
		disassembled[3].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 10));
		disassembled[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
		disassembled[5].Should().BeOfType<CloseInstruction>();

		disassembled = new AddRoundRectInstruction(10, 10, 20, 20, 2, PathDirection.Clockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(10);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 18));
		disassembled[1].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 12));
		disassembled[2].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(12, 10));
		disassembled[3].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(18, 10));
		disassembled[4].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(20, 12));
		disassembled[5].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 18));
		disassembled[6].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(18, 20));
		disassembled[7].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(12, 20));
		disassembled[8].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(10, 18));
		disassembled[9].Should().BeOfType<CloseInstruction>();

		disassembled = new AddRoundRectInstruction(10, 10, 20, 20, 2, PathDirection.CounterClockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(10);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 18));
		disassembled[1].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(12, 20));
		disassembled[2].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(18, 20));
		disassembled[3].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(20, 18));
		disassembled[4].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 12));
		disassembled[5].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(18, 10));
		disassembled[6].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(12, 10));
		disassembled[7].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(10, 12));
		disassembled[8].Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 18));
		disassembled[9].Should().BeOfType<CloseInstruction>();

		disassembled = new AddOvalInstruction(10, 10, 20, 20, PathDirection.Clockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(6);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(20, 15));
		disassembled[1].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(15, 20));
		disassembled[2].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(10, 15));
		disassembled[3].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(15, 10));
		disassembled[4].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(20, 15));
		disassembled[5].Should().BeOfType<CloseInstruction>();

		disassembled = new AddOvalInstruction(10, 10, 20, 20, PathDirection.CounterClockwise).Disassemble().ToList();
		disassembled.Should().HaveCount(6);
		disassembled[0].Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(20, 15));
		disassembled[1].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(15, 10));
		disassembled[2].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(10, 15));
		disassembled[3].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(15, 20));
		disassembled[4].Should().BeOfType<RationalToInstruction>().Which.End.Should().Be(new Vector2(20, 15));
		disassembled[5].Should().BeOfType<CloseInstruction>();
	}

	[Test]
	public void VectorPathInstructionExtensions_ToLine_Test()
	{
		new MoveToInstruction(10, 0).ToLine().Should().BeOfType<MoveToInstruction>().Which.Point.Should().Be(new Vector2(10, 0));
		new LineToInstruction(10, 0).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
		new CloseInstruction().ToLine().Should().BeOfType<CloseInstruction>();
		new QuadToInstruction(10, 10, 20, 0).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 0));
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 0));
		new CubicToInstruction(0, 10, 10, 10, 10, 0).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 0));
		new AddRectInstruction(10, 10, 20, 20).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 10));
		new AddRoundRectInstruction(10, 10, 20, 20, 2).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(10, 18));
		new AddOvalInstruction(10, 10, 20, 20).ToLine().Should().BeOfType<LineToInstruction>().Which.End.Should().Be(new Vector2(20, 15));
	}

	[Test]
	public void VectorPathInstructionExtensions_IsGraphical_Test()
	{
		new MoveToInstruction(10, 0).IsGraphical().Should().BeFalse();
		new LineToInstruction(10, 0).IsGraphical().Should().BeTrue();
		new CloseInstruction().IsGraphical().Should().BeFalse();
		new QuadToInstruction(10, 10, 20, 0).IsGraphical().Should().BeTrue();
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).IsGraphical().Should().BeTrue();
		new CubicToInstruction(0, 10, 10, 10, 10, 0).IsGraphical().Should().BeTrue();
		new AddRectInstruction(10, 10, 20, 20).IsGraphical().Should().BeTrue();
		new AddRoundRectInstruction(10, 10, 20, 20, 2).IsGraphical().Should().BeTrue();
		new AddOvalInstruction(10, 10, 20, 20).IsGraphical().Should().BeTrue();
	}

	[Test]
	public void VectorPathInstructionExtensions_EnumeratePoints_Test()
	{
		new MoveToInstruction(10, 0).EnumeratePoints().Should().HaveCount(1);
		new LineToInstruction(10, 0).EnumeratePoints().Should().HaveCount(1);
		new CloseInstruction().EnumeratePoints().Should().HaveCount(0);
		new QuadToInstruction(10, 10, 20, 0).EnumeratePoints().Should().HaveCount(2);
		new RationalToInstruction(1, new(10, 10), 2, new(20, 0), 3).EnumeratePoints().Should().HaveCount(2);
		new CubicToInstruction(0, 10, 10, 10, 10, 0).EnumeratePoints().Should().HaveCount(3);
		new AddRectInstruction(10, 10, 20, 20).EnumeratePoints().Should().HaveCount(2);
		new AddRoundRectInstruction(10, 10, 20, 20, 2).EnumeratePoints().Should().HaveCount(2);
		new AddOvalInstruction(10, 10, 20, 20).EnumeratePoints().Should().HaveCount(2);
	}
}