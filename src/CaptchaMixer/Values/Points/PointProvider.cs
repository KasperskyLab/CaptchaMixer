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
/// Generates points from their X and Y coordinates.
/// </summary>
public class PointProvider : ValueProvider<Vector2>
{
	/// <summary>
	/// X coordinate provider.
	/// </summary>
	public ValueProvider<float> X { get; set; }

	/// <summary>
	/// Y coordinate provider.
	/// </summary>
	public ValueProvider<float> Y { get; set; }

	public PointProvider(
		ValueProvider<float> x,
		ValueProvider<float> y)
	{
		X = x ?? throw new ArgumentNullException(nameof(x));
		Y = y ?? throw new ArgumentNullException(nameof(y));
	}

	public override Vector2 GetNext()
		=> new(X, Y);
}
