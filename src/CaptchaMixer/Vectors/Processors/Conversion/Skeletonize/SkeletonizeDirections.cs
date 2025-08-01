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
/// Direction relative to a pixel for skeletonization algorithm.
/// </summary>
internal static class SkeletonizeDirections
{
	internal const int LeftTop = 0;
	internal const int Top = 1;
	internal const int RightTop = 2;
	internal const int Right = 3;
	internal const int RightBottom = 4;
	internal const int Bottom = 5;
	internal const int LeftBottom = 6;
	internal const int Left = 7;
}
