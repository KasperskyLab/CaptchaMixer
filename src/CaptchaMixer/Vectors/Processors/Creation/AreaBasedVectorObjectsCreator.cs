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
/// Base class for processors which create objects within the specified area.
/// </summary>
public abstract class AreaBasedVectorObjectsCreator : IVectorProcessor
{
	/// <summary>
	/// Objects generation area. If <see langword="null"/> then the entire layer
	/// is used.
	/// </summary>
	public ValueProvider<RectangleF> Area { get; set; } = null;

	public void Process(IVectorLayer layer, ICaptchaContext context)
		=> Process(Area.RectOrCaptchaSize(context), layer, context);

	public abstract void Process(
		RectangleF area,
		IVectorLayer layer,
		ICaptchaContext context);
}
