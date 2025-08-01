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

public static partial class GeometryExtensions
{
	/// <returns>
	/// <paramref name="point"/> converted to <see cref="SKPoint"/>.
	/// </returns>
	public static SKPoint ToSKPoint(this Vector2 point)
		=> new(point.X, point.Y);

	/// <returns>
	/// <paramref name="vector"/> converted to <see cref="Vector2"/>
	/// by throwing <see cref="Vector3.Z"/> off.
	/// </returns>
	public static Vector2 TruncateZ(this ref Vector3 vector)
		=> new(vector.X, vector.Y);
}
