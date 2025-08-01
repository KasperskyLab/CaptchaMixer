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
/// 3-dimensional Bezier curve.
/// </summary>
public struct CurveQuad3 : IEquatable<CurveQuad3>
{
	/// <summary>
	/// Curve start point.
	/// </summary>
	public Vector3 Start;

	/// <summary>
	/// Curve control point.
	/// </summary>
	public Vector3 Control;

	/// <summary>
	/// Curve end point.
	/// </summary>
	public Vector3 End;

	/// <param name="start"><inheritdoc cref="Start" path="/summary"/></param>
	/// <param name="control"><inheritdoc cref="Control" path="/summary"/></param>
	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	public CurveQuad3(Vector3 start, Vector3 control, Vector3 end)
	{
		Start = start;
		Control = control;
		End = end;
	}

	/// <param name="points">Array of 3 points.</param>
	/// <exception cref="ArgumentException"/>
	public CurveQuad3(Vector3[] points)
	{
		if (points.Length != 3)
			throw new ArgumentException("Expected array of 3 elements", nameof(points));

		Start = points[0];
		Control = points[1];
		End = points[2];
	}

	/// <returns>
	/// Curve as a point array.
	/// </returns>
	public readonly Vector3[] ToArray()
		=> new[] { Start, Control, End };

	/// <summary>
	/// Copies points into <paramref name="span"/>.
	/// </summary>
	public readonly void CopyTo(Span<Vector3> span)
	{
		span[0] = Start;
		span[1] = Control;
		span[2] = End;
	}

	#region IEquatable and Object methods

	public readonly bool Equals(CurveQuad3 other)
		=> Start.Equals(other.Start) &&
		Control.Equals(other.Control) &&
		End.Equals(other.End);

	public override readonly bool Equals(object obj)
		=> obj is CurveQuad3 other && Equals(other);

	public override readonly int GetHashCode()
		=> HashCode.Combine(Start, Control, End);

	public static bool operator ==(CurveQuad3 left, CurveQuad3 right)
		=> left.Equals(right);

	public static bool operator !=(CurveQuad3 left, CurveQuad3 right)
		=> !(left == right);

	#endregion
}
