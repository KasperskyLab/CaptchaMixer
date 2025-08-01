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
/// Base class for Bezier curve instructions.
/// </summary>
public abstract class CurveInstruction : VectorPathInstruction
{
	/// <summary>
	/// Count of curve points to evaluate when calculating curve's bounds.
	/// </summary>
	/// <remarks>
	/// There is no simple way to calculate a Bezier curve's bounds.
	/// 20 is an experimentally derived more or less optimal value.
	/// Increasing this value would raise accuracy but lower performance.
	/// </remarks>
	protected const int BoundsCalcPointsCount = 20;

	protected CurveInstruction(params PointType[] pointTypes)
		: base(pointTypes) { }
}
