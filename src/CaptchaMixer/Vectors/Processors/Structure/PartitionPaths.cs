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
/// Walks through paths' instructions and generates a random double number on each step.
/// If the number appeared to be &lt;= <see cref="Probability"/> then extracts the next
/// <see cref="Count"/> instructions into a separate contour by inserting a new
/// <see cref="MoveToInstruction"/>.
/// </summary>
/// <remarks>
/// All <see cref="CloseInstruction"/>s will be removed from paths because they lose
/// their meaning in cut paths and would only make the result inconsistent.<br/>
/// A new cut-to-pieces path is still a single path. So in order to process its parts
/// one by one, you will have to use <see cref="SplitPaths"/> first.
/// </remarks>
public class PartitionPaths : OneByOnePathsProcessor
{
	private readonly Random _random = new();

	/// <summary>
	/// Probability of extracting the next <see cref="Count"/> instructions into a contour.
	/// </summary>
	public ValueProvider<float> Probability { get; set; } = 1;

	/// <summary>
	/// Count of instructions to extract into a contour.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 3;

	public PartitionPaths() { }

	/// <param name="probability"><inheritdoc cref="Probability" path="/summary"/></param>
	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public PartitionPaths(
		ValueProvider<float> probability,
		ValueProvider<int> count)
	{
		Probability = probability ?? throw new ArgumentNullException(nameof(probability));
		Count = count ?? throw new ArgumentNullException(nameof(count));
	}

	protected override IEnumerable<VectorPath> Process(VectorPath path, ICaptchaContext context)
	{
		var newInstructions = new List<VectorPathInstruction>();

		foreach (var contour in path.EnumerateContours(false))
		{
			// first detect the positions of MoveTo in a contour

			// contours acquired via paths split may or may not start with a MoveTo.
			// anyway there is no sense to process it.
			var i = contour[0] is MoveToInstruction ? 1 : 0;
			var hasMoveTo = i == 1;

			// sometimes there is simply nothing to cut
			if (contour.Count - i == 1)
			{
				newInstructions.AddRange(contour);
				continue;
			}

			// indexes to insert MoveTo at
			var inserts = new List<int>();

			while (i < contour.Count)
			{
				float probability = Probability;

				if (_random.NextDouble() > probability)
				{
					i++;
					continue;
				}

				int count = Count;

				// if we're at the start of a contour which has started with a MoveTo then we don't
				// need another one. this index may also have been added in case if the contour was
				// removed on the previous iteration.
				if (!(i == 1 && hasMoveTo) && inserts.IndexOf(i) == -1)
					inserts.Add(i);

				// if we have a count which doesn't fit in the remainder then just leave it as is.
				if (i + count >= contour.Count) break;

				inserts.Add(i + count);
				i += count;
			}

			for (i = inserts.Count - 1; i >= 0; i--)
			{
				var index = inserts[i];
				// contour may have not started with MoveTo
				var position = index > 0 ? contour[inserts[i] - 1].GetEndPoint() : new Vector2(0, 0);
				contour.Insert(index, new MoveToInstruction(position));
			}

			newInstructions.AddRange(contour);
		}

		path.Instructions = newInstructions;
		yield return path;
	}
}
