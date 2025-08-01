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
/// If the number appeared to be &lt;= <see cref="Probability"/> then replaces the next
/// <see cref="Count"/> graphical instructions with a single <see cref="MoveToInstruction"/>
/// to the point of the first non-removed instruction.
/// </summary>
/// <remarks>
/// Unlike <see cref="DamagePaths"/>, <see cref="ChopPaths"/> only removes the visible parts of
/// paths which leads to controlled discontinuity. <see cref="DamagePaths"/> removes any
/// instructions which may lead to actual damage if compositional instructions (like
/// <see cref="MoveToInstruction"/> or <see cref="CloseInstruction"/>) get removed.
/// </remarks>
public class ChopPaths : OneByOnePathsProcessor
{
	private readonly Random _random = new();

	/// <summary>
	/// Probability of removing the next <see cref="Count"/> graphical instructions.
	/// </summary>
	public ValueProvider<float> Probability { get; set; } = 0.1f;

	/// <summary>
	/// Count of graphical instructions to remove.
	/// </summary>
	public ValueProvider<int> Count { get; set; } = 3;

	public ChopPaths() { }

	/// <param name="probability"><inheritdoc cref="Probability" path="/summary"/></param>
	/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	public ChopPaths(
		ValueProvider<float> probability,
		ValueProvider<int> count)
	{
		Probability = probability ?? throw new ArgumentNullException(nameof(probability));
		Count = count ?? throw new ArgumentNullException(nameof(count));
	}

	protected override IEnumerable<VectorPath> Process(VectorPath path, ICaptchaContext context)
	{
		var tuples = path.ToList();
		var newPath = new VectorPath();

		var i = 0;
		while (i < tuples.Count)
		{
			float probability = Probability;
			var (_, instruction) = tuples[i];

			if (_random.NextDouble() <= probability)
			{
				int count = Count;
				int skipped = 0;

				while (instruction.IsGraphical())
				{
					skipped++;
					i++;
					if (skipped >= count || i >= tuples.Count) break;
					(_, instruction) = tuples[i];
				}

				if (skipped > 0 && i < tuples.Count)
					newPath.Instructions.Add(new MoveToInstruction(tuples[i].position));

				continue;
			}

			newPath.Instructions.Add(tuples[i++].instruction);
		}

		yield return newPath;
	}
}
