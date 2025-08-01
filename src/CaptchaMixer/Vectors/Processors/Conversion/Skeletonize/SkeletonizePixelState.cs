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
/// Pixel state during skeletonization.
/// </summary>
internal enum SkeletonizePixelState
{
	/// <summary>
	/// Unknown by far. A call to <see cref="SkeletonizePixel.Update"/> is needed.
	/// </summary>
	Unknown,

	/// <summary>
	/// Pixel is initially empty.
	/// </summary>
	Empty,

	/// <summary>
	/// Pixel must not be removed right now. This state may later switch to
	/// <see cref="Removable"/> if some of its neighbours would be removed.
	/// </summary>
	Unremovable,

	/// <summary>
	/// Pixel may be removed.
	/// </summary>
	Removable,

	/// <summary>
	/// Pixel has been removed.
	/// </summary>
	Removed,

	/// <summary>
	/// Pixel is finalized. It's now a part of the skeletonization result
	/// and this won't change.
	/// </summary>
	Final
}
