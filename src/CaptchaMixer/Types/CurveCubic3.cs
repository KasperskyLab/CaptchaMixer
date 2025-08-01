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
/// 3-dimensional cubic Bezier curve.
/// </summary>
public struct CurveCubic3 : IEquatable<CurveCubic3>
{
	/// <summary>
	/// Curve start point.
	/// </summary>
	public Vector3 Start;

	/// <summary>
	/// Curve first control point.
	/// </summary>
	public Vector3 Control1;

	/// <summary>
	/// Curve second control point.
	/// </summary>
	public Vector3 Control2;

	/// <summary>
	/// Curve end point.
	/// </summary>
	public Vector3 End;

	/// <param name="start"><inheritdoc cref="Start" path="/summary"/></param>
	/// <param name="control1"><inheritdoc cref="Control1" path="/summary"/></param>
	/// <param name="control2"><inheritdoc cref="Control2" path="/summary"/></param>
	/// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
	public CurveCubic3(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
	{
		Start = start;
		Control1 = control1;
		Control2 = control2;
		End = end;
	}

	/// <param name="points">Array of 4 points.</param>
	/// <exception cref="ArgumentException"/>
	public CurveCubic3(Vector3[] points)
	{
		if (points.Length != 4)
			throw new ArgumentException("Expected array of 4 elements", nameof(points));

		Start = points[0];
		Control1 = points[1];
		Control2 = points[2];
		End = points[3];
	}

	/// <returns>
	/// Curve as a point array.
	/// </returns>
	public readonly Vector3[] ToArray()
		=> new[] { Start, Control1, Control2, End };

	/// <summary>
	/// Copies points into <paramref name="span"/>.
	/// </summary>
	public readonly void CopyTo(Span<Vector3> span)
	{
		span[0] = Start;
		span[1] = Control1;
		span[2] = Control2;
		span[3] = End;
	}

	#region IEquatable and Object methods

	public readonly bool Equals(CurveCubic3 other)
		=> Start.Equals(other.Start) &&
		Control1.Equals(other.Control1) &&
		Control2.Equals(other.Control2) &&
		End.Equals(other.End);

	public override readonly bool Equals(object obj)
		=> obj is CurveCubic3 other && Equals(other);

	public override readonly int GetHashCode()
		=> HashCode.Combine(Start, Control1, Control2, End);

	public static bool operator ==(CurveCubic3 left, CurveCubic3 right)
		=> left.Equals(right);

	public static bool operator !=(CurveCubic3 left, CurveCubic3 right)
		=> !(left == right);

	#endregion
}
