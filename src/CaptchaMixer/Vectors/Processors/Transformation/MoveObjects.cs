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
/// Moves objects.
/// </summary>
public class MoveObjects : OneByOneVectorObjectsProcessor
{
	/// <summary>
	/// Shift along X axis.
	/// </summary>
	public ValueProvider<float> MoveX { get; set; } = 0;

	/// <summary>
	/// Shift along Y axis.
	/// </summary>
	public ValueProvider<float> MoveY { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="MoveX"/> and <see cref="MoveY"/>.
	/// </summary>
	public ValueProvider<float> Move
	{
		set
		{
			MoveX = value;
			MoveY = value;
		}
	}

	public MoveObjects() { }

	/// <param name="move"><inheritdoc cref="Move" path="/summary"/></param>
	public MoveObjects(ValueProvider<float> move)
		: this(move, move) { }

	/// <param name="moveX"><inheritdoc cref="MoveX" path="/summary"/></param>
	/// <param name="moveY"><inheritdoc cref="MoveY" path="/summary"/></param>
	public MoveObjects(ValueProvider<float> moveX, ValueProvider<float> moveY)
	{
		MoveX = moveX ?? throw new ArgumentNullException(nameof(moveX));
		MoveY = moveY ?? throw new ArgumentNullException(nameof(moveY));
	}

	protected override IEnumerable<VectorObject> Process(VectorObject obj, ICaptchaContext context)
	{
		yield return obj.Move(MoveX, MoveY);
	}
}
