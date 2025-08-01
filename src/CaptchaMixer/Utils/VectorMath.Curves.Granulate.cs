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

public static partial class VectorMath
{
	private const string CurveGranulateInvalidLengthExceptionMessage
		= "Failed to perform curve granulate: minimal length must be > 0";

	private class Curve2GranulationInfo
	{
		internal Vector2[] Curve { get; }
		internal Curve2GranulationInfo Parent { get; }
		internal float LinearLength { get; }

		internal Curve2GranulationInfo(Vector2[] curve, Curve2GranulationInfo parent)
		{
			Curve = curve;
			Parent = parent;
			LinearLength = LineLength(curve[0], curve[^1]);
		}
	}

	private class Curve3GranulationInfo
	{
		internal Vector3[] Curve { get; }
		internal Curve3GranulationInfo Parent { get; }
		internal float LinearLength { get; }

		internal Curve3GranulationInfo(Vector3[] curve, Curve3GranulationInfo parent)
		{
			Curve = curve;
			Parent = parent;
			LinearLength = LineLength(curve[0], curve[^1]);
		}
	}

	/// <summary>
	/// Splits a non-rational Bezier curve on such parts that the linear distance between start and end
	/// points of each sub-curve does not exceed <paramref name="maxLinearLength"/>.
	/// </summary>
	/// <remarks>
	/// An accurate calculation of the length of the Bezier curve for any cases except degenerate ones
	/// (when all control points are located on the segment connecting the start and end points, and
	/// the curve eventually reduces to a straight segment) is virtually impossible. And even calculating
	/// the length with relatively high accuracy is a heavy operation. The purpose of this method is to
	/// meet the needs of captcha generation, for which there is no sense in 100% accuracy, therefore
	/// accuracy is not guaranteed, although some measures are still taken to avoid the most gross
	/// miscalculations.
	/// </remarks>
	internal static IEnumerable<Vector2[]> CurveGranulate(Vector2[] curve, float maxLinearLength)
	{
		if (maxLinearLength <= 0)
			throw new ArgumentException(CurveGranulateInvalidLengthExceptionMessage, nameof(maxLinearLength));

		// this list is a set of tree leaves of resultant segments. when the curve is being split
		// on two sub-curves, the corresponding element is replaced by two new.
		var granulations = new List<Curve2GranulationInfo> { new(curve, null) };

		// start and end points may be placed very close to each other or even be equal, while the
		// control points may be very far from them. this creates a very long curve. therefore, to insure
		// against gross errors, we first break the curve into several parts anyway.
		// this does not guarantee success in all cases, but will cover most.
		for (var i = 0; i < 4; i++)
			for (var j = granulations.Count - 1; j >= 0; j--)
				Split(j);

		// since we have blindly split the curve, now we need to check where this made sense to do this
		// and merge back the sections that turned out to be too large.
		var merged = true;
		while (merged)
		{
			merged = false;

			for (var i = 0; i < granulations.Count - 1; i++)
			{
				var curr = granulations[i];
				var next = granulations[i + 1];

				// so if two new segments, as well as the original segment they've been derived from,
				// are small enough, then merge them back
				if (curr.Parent != next.Parent || // derived sub-curves only
					curr.Parent.LinearLength > maxLinearLength ||
					curr.LinearLength > maxLinearLength ||
					next.LinearLength > maxLinearLength)
					continue;

				granulations[i] = curr.Parent;
				granulations.RemoveAt(i + 1);
				merged = true;
			}
		}

		// now detail the segments with too big linear lengths
		var split = true;
		while (split)
		{
			split = false;

			for (var i = granulations.Count - 1; i >= 0; i--)
			{
				if (granulations[i].LinearLength <= maxLinearLength)
					continue;

				Split(i);
				split = true;
			}
		}

		return granulations.Select(g => g.Curve);

		void Split(int index)
		{
			var granulation = granulations[index];
			var (curve1, curve2) = CurveSplit(granulation.Curve, 0.5f);
			granulations.Insert(index, new(curve1, granulation));
			granulations[index + 1] = new(curve2, granulation);
		}
	}

	/// <summary>
	/// Splits a rational Bezier curve on such parts that the linear distance between start and end
	/// points of each sub-curve does not exceed <paramref name="maxLinearLength"/>.
	/// </summary>
	/// <remarks>
	/// <inheritdoc cref="CurveGranulate(Vector2[], float)" path="/remarks"/>
	/// </remarks>
	internal static IEnumerable<Vector3[]> CurveRationalGranulate(Vector3[] curve, float maxLinearLength)
	{
		if (maxLinearLength <= 0)
			throw new ArgumentException(CurveGranulateInvalidLengthExceptionMessage, nameof(maxLinearLength));

		// see algorithm explanation above in a method for non-rational curves

		var granulations = new List<Curve3GranulationInfo> { new(curve, null) };

		for (var i = 0; i < 4; i++)
			for (var j = granulations.Count - 1; j >= 0; j--)
				Split(j);

		var merged = true;
		while (merged)
		{
			merged = false;

			for (var i = 0; i < granulations.Count - 1; i++)
			{
				var curr = granulations[i];
				var next = granulations[i + 1];

				if (curr.Parent != next.Parent ||
					curr.Parent.LinearLength > maxLinearLength ||
					curr.LinearLength > maxLinearLength ||
					next.LinearLength > maxLinearLength)
					continue;

				granulations[i] = curr.Parent;
				granulations.RemoveAt(i + 1);
				merged = true;
			}
		}

		var split = true;
		while (split)
		{
			split = false;

			for (var i = granulations.Count - 1; i >= 0; i--)
			{
				if (granulations[i].LinearLength <= maxLinearLength)
					continue;

				Split(i);
				split = true;
			}
		}

		return granulations.Select(g => g.Curve);

		void Split(int index)
		{
			var granulation = granulations[index];
			var (curve1, curve2) = CurveRationalSplit(granulation.Curve, 0.5f);
			granulations.Insert(index, new(curve1, granulation));
			granulations[index + 1] = new(curve2, granulation);
		}
	}

	/// <summary>
	/// Splits a Bezier cubic curve on such parts that the linear distance between start and end
	/// points of each sub-curve does not exceed <paramref name="maxLinearLength"/>.
	/// </summary>
	public static CurveCubic2[] CurveCubicGranulate(CurveCubic2 curve, float maxLinearLength)
		=> CurveGranulate(curve.ToArray(), maxLinearLength)
			.Select(a => new CurveCubic2(a))
			.ToArray();

	/// <summary>
	/// Splits a Bezier quadratic curve on such parts that the linear distance between start and end
	/// points of each sub-curve does not exceed <paramref name="maxLinearLength"/>.
	/// </summary>
	public static CurveQuad2[] CurveQuadGranulate(CurveQuad2 curve, float maxLinearLength)
		=> CurveGranulate(curve.ToArray(), maxLinearLength)
			.Select(a => new CurveQuad2(a))
			.ToArray();

	/// <summary>
	/// Splits a Bezier rational curve on such parts that the linear distance between start and end
	/// points of each sub-curve does not exceed <paramref name="maxLinearLength"/>.
	/// </summary>
	public static CurveQuad3[] CurveRationalGranulate(CurveQuad3 curve, float maxLinearLength)
		=> CurveRationalGranulate(curve.ToArray(), maxLinearLength)
			.Select(a => new CurveQuad3(a))
			.ToArray();
}
