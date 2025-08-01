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

using static Kaspersky.CaptchaMixer.ValueProviderFactory;

/// <summary>
/// <para>
/// Scales points within an ellipse defined by <see cref="Area"/> relative to its center.
/// </para>
/// <para>
/// <see cref="Positions"/> and <see cref="Scales"/> must provide <see cref="Count"/> of value pairs, at least 2.<br/>
/// These pairs are then used to create a list of intervals each of which has:<br/>
/// - Start and end positions from 0 to 1;<br/>
/// - Start and end scaling factors.
/// </para>
/// <para>
/// If the distance between a point and the ellipse's center, normalized relative to ellipse's radius, falls within one of
/// intervals then a point is scaled according to the proximity to interval's edges. The closer a point is to the
/// beginning of the interval, the closer its scaling factor will be to the interval start scale factor. Respectively,
/// the closer a point is to the end of the interval, the closer its scaling will be to the scale factor of the end of
/// the interval.
/// </para>
/// </summary>
/// <remarks>
/// This processor provides a variety of effects, including lens distortions:<br />
/// - barrel distortion example: <see cref="Count"/> = 2, <see cref="Positions"/> = [0, 1], <see cref="Scales"/> = [1, 0.7].<br />
/// - pincushion distortion example: <see cref="Count"/> = 2, <see cref="Positions"/> = [0, 1], <see cref="Scales"/> = [1, 1.3].<br />
/// Using three stops and the scale factor of 1 for both ends enables distorting objects inside the ellipse without goind beyond borders.<br/>
/// Scale factors are not required to increment - this allows some pretty wild things.
/// </remarks>
public class PointsRadialDistortion : OneByOnePathInstructionsProcessor
{
	/// <summary>
	/// Distortion ellipse area. Set to <see langword="null"/> to use the entire layer.
	/// </summary>
	public ValueProvider<RectangleF> Area { get; set; } = null;

	/// <summary>
	/// Distortion positions count, must be at least 2.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 3;

	/// <summary>
	/// Distortion positions from 0 to 1.
	/// Must provide <see cref="Count"/> of steadily increasing values.
	/// </summary>
	public ValueProvider<float> Positions { get; set; } = Carousel(0.0f, 0.5f, 1.0f);

	/// <summary>
	/// Scale factors on <see cref="Positions"/>.
	/// Must provide <see cref="Count"/> values.
	/// </summary>
	public ValueProvider<float> Scales { get; set; } = Carousel(1.0f, 1.5f, 1.0f);

	public PointsRadialDistortion() { }

	/// <summary>
	/// Creates a simple distortion with scale factors for positions 0 and 1.
	/// </summary>
	/// <param name="scale0">Scale factor for position 0.</param>
	/// <param name="scale1">Scale factor for position 1.</param>
	/// <param name="area"><inheritdoc cref="Area" path="/summary"/></param>
	public PointsRadialDistortion(float scale0, float scale1, ValueProvider<RectangleF> area = null)
		: this(
			2,
			Carousel(0f, 1f),
			Carousel(scale0, scale1),
			area)
	{ }

	/// <param name="stops">Positions and their scale factors.</param>
	public PointsRadialDistortion(params (float position, float scale)[] stops)
		: this(
			stops.Length,
			Carousel(stops.Select(s => s.position)),
			Carousel(stops.Select(s => s.scale)))
	{ }

	/// <param name="area"><inheritdoc cref="Area" path="/summary"/></param>
	/// <param name="stops"><inheritdoc cref="PointsRadialDistortion(ValueTuple{float, float}[])" path="/param[@name='stops']"/></param>
	public PointsRadialDistortion(
		ValueProvider<RectangleF> area,
		params (float position, float scale)[] stops)
		: this(
			stops.Length,
			Carousel(stops.Select(s => s.position)),
			Carousel(stops.Select(s => s.scale)),
			area)
	{ }

	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <param name="positions"><inheritdoc cref="Positions" path="/summary"/></param>
	/// <param name="scales"><inheritdoc cref="Scales" path="/summary"/></param>
	/// <param name="area"><inheritdoc cref="Area" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public PointsRadialDistortion(
		ValueProvider<int> count,
		ValueProvider<float> positions,
		ValueProvider<float> scales,
		ValueProvider<RectangleF> area = null)
	{
		Count = count ?? throw new ArgumentNullException(nameof(count));
		Positions = positions ?? throw new ArgumentNullException(nameof(positions));
		Scales = scales ?? throw new ArgumentNullException(nameof(scales));
		Area = area;
	}

	protected override IEnumerable<VectorPathInstruction> Process(
		VectorPathInstruction instruction,
		Vector2 position,
		ICaptchaContext context)
	{
		int count = Count;
		if (count < 2)
			throw new InvalidOperationException($"{nameof(Count)} provider must return value >= 2");

		var positions = Positions.Take(count).ToArray();
		for (var i = 1; i < count; i++)
			if (positions[i] <= positions[i - 1])
				throw new InvalidOperationException($"{nameof(Positions)} provider must return increasing values");

		var scales = Scales.Take(count).ToArray();

		var intervals = Enumerable
			.Range(1, count - 1)
			.Select(i => new
			{
				Position1 = positions[i - 1],
				Position2 = positions[i],
				Size = positions[i] - positions[i - 1],
				Scale1 = scales[i - 1],
				Scale2 = scales[i],
				ScalesDiff = scales[i] - scales[i - 1]
			})
			.ToArray();

		var area = Area.RectOrCaptchaSize(context);
		var rx = area.Width / 2;
		var ry = area.Height / 2;
		var center = area.GetAnchorPoint(RectAnchor.Center);
		var axis = new Vector2(center.X + 1, center.Y);

		for (var i = 0; i < instruction.Points.Length; i++)
		{
			var point = instruction.Points[i];

			// it's much cheaper to check falling into a rectangle then into an ellipse
			if (!area.Contains(point)) continue;

			// distance to ellipse's center
			var distance = VectorMath.LineLength(center, point);
			if (distance == 0) continue;

			// calc the radius of ellipse in an edge point which lies on the same line
			// https://math.stackexchange.com/a/432907
			var angle = VectorMath.AngleBetweenRad(point, center, axis);
			var sin = Math.Sin(angle);
			var cos = Math.Cos(angle);
			var radius = rx * ry / (float)Math.Sqrt(rx * rx * sin * sin + ry * ry * cos * cos);

			// now check if the point falls into an ellipse, not just into its bounds area
			if (radius < distance) continue;

			// position of the point normalized relative to ellipse radius
			var radiusPosition = distance / radius;

			// interval which the point falls into
			var interval = intervals.FirstOrDefault(i => radiusPosition >= i.Position1 && radiusPosition <= i.Position2);
			if (interval == default) continue;

			// position of the point normalized relative to interval size
			var intervalPosition = (radiusPosition - interval.Position1) / interval.Size;

			// finally scale it
			var scale = interval.Scale1 + interval.ScalesDiff * intervalPosition;
			instruction.Points[i] = point.Scale(center, scale);
		}

		yield return instruction;
	}
}
