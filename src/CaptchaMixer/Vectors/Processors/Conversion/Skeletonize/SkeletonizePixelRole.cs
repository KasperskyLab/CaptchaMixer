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
/// Pixel role in process of skeletonization.
/// </summary>
internal enum SkeletonizePixelRole
{
	/// <summary>
	/// By far unknown.
	/// </summary>
	Unknown,
	
	/// <summary>
	/// No surrounding pixels exist.
	/// </summary>
	Alone,

	/// <summary>
	/// Pixel ends a line - it only has neighbours on one side.
	/// </summary>
	End,

	/// <summary>
	/// Pixel is part of a line - with neighbours in 2 different directions.
	/// </summary>
	Line,

	/// <summary>
	/// Pixel stands on lines intersection - it has neighbours in 3 or 4 directions.
	/// </summary>
	Node
}
