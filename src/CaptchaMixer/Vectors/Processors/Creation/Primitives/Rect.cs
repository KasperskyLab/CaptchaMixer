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
/// Creates a single rectangle sized by <see cref="AreaBasedVectorObjectsCreator.Area"/>.
/// </summary>
public class Rect : AreaBasedVectorObjectsCreator
{
	public override void Process(RectangleF area, IVectorLayer layer, ICaptchaContext context)
	{
		var path = new VectorPath(new AddRectInstruction(area.Left, area.Top, area.Right, area.Bottom));
		layer.Objects.Add(new(path));
	}
}
