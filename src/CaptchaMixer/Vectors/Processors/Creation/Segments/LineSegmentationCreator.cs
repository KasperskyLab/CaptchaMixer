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
/// Base class for creators of segmentations separated by straight lines.
/// </summary>
public abstract class LineSegmentationCreator : AreaSegmentationCreator
{
	protected LineSegmentationCreator() { }

	/// <inheritdoc cref="AreaSegmentationCreator(ValueProvider{int})"/>
	protected LineSegmentationCreator(ValueProvider<int> segments)
		: base(segments) { }

	protected override VectorPathInstruction GetSeparator(Vector2 from, Vector2 to)
		=> new LineToInstruction(to);
}
