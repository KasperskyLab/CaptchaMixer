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
/// Rounds coordinates of points to <see cref="Decimals"/>.
/// </summary>
public class RoundPoints : OneByOnePathInstructionsProcessor
{
	/// <summary>
	/// Points' coordinates decimals.
	/// </summary>
	public int Decimals { get; set; } = 0;

	public RoundPoints() { }

	/// <param name="decimals"><inheritdoc cref="Decimals" path="/summary"/></param>
	public RoundPoints(int decimals)
		=> Decimals = decimals;

	protected override IEnumerable<VectorPathInstruction> Process(
		VectorPathInstruction instruction,
		Vector2 position,
		ICaptchaContext context)
	{
		for (var i = 0; i < instruction.Points.Length; i++)
			RoundPoint(ref instruction.Points[i]);

		yield return instruction;
	}

	private void RoundPoint(ref Vector2 point)
	{
		point.X = (float)Math.Round(point.X, Decimals);
		point.Y = (float)Math.Round(point.Y, Decimals);
	}
}
