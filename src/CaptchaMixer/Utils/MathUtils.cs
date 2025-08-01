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

public static class MathUtils
{
	private static readonly float Pi = (float)Math.PI;

	/// <returns>
	/// <paramref name="deg"/> converted to radians.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DegToRad(this float deg)
		=> deg * Pi / 180;

	/// <returns>
	/// <paramref name="rad"/> converted to degrees.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float RadToDeg(this float rad)
		=> rad * 180 / Pi;

	/// <returns>
	/// - <paramref name="min"/> if <paramref name="n"/> &lt; <paramref name="min"/>;<br/>
	/// - <paramref name="max"/> if <paramref name="n"/> &gt; <paramref name="max"/>;<br/>
	/// - <paramref name="n"/> in other cases.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Clamp(this float n, float min, float max)
		=> n < min ? min : n > max ? max : n;

	/// <inheritdoc cref="Clamp(float, float, float)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Clamp(this int n, int min, int max)
		=> n < min ? min : n > max ? max : n;

	/// <returns>
	/// Min and max of three numbers.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void GetMinMax(out int min, out int max, int n1, int n2, int n3)
	{
		if (n1 > n2)
		{
			max = n1;
			min = n2;
		}
		else
		{
			max = n2;
			min = n1;
		}

		if (n3 > max)
		{
			max = n3;
		}
		else if (n3 < min)
		{
			min = n3;
		}
	}
}
