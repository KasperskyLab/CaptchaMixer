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
/// Base class for pattern generators.
/// </summary>
public abstract class PatternCreator : AreaBasedVectorObjectsCreator
{
	/// <summary>
	/// Maximal count of pattern elements.
	/// </summary>
	public ValueProvider<int> MaxCount { get; set; } = 1000;

	public override void Process(RectangleF area, IVectorLayer layer, ICaptchaContext context)
	{
		var pattern = CreatePattern(area, MaxCount);
		layer.Objects.Add(new(pattern));
	}

	protected abstract VectorPath CreatePattern(RectangleF area, int maxCount);
}
