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
/// Merges all paths within each object into a single path.
/// </summary>
/// <remarks>
/// This processor ensures that the new path would be visually identical to the set
/// of original paths by inserting <see cref="MoveToInstruction"/>s when needed.
/// </remarks>
public class MergePaths : OneByOneVectorObjectsProcessor
{
	protected override IEnumerable<VectorObject> Process(VectorObject obj, ICaptchaContext context)
	{
		var merged = new VectorPath();
		var mergedInstructions = merged.Instructions;

		foreach (var path in obj)
		{
			if (!path.IsMeaningful())
				continue;

			var pathInstructions = path.Instructions;
			var firstInstruction = pathInstructions[0];

			if (mergedInstructions.Count > 0 &&
				firstInstruction is not MoveToInstruction &&
				firstInstruction is not ContourInstruction)
			{
				// if the first instruction does not move the pen then the first contour starts
				// drawing from (0, 0). this shall be preserved to avoid contours ditortion.
				mergedInstructions.Add(new MoveToInstruction(0, 0));
			}

			mergedInstructions.AddRange(pathInstructions);
		}

		obj.Paths.ReplaceItems(merged);
		yield return obj;
	}
}
