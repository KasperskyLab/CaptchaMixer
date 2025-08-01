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
/// Iterates over vector instructions and returns tuples of the current pen position and next instruction.
/// </summary>
public sealed class VectorPathInstructionsIterator : IEnumerator<(Vector2 position, VectorPathInstruction instruction)>
{
	private readonly List<VectorPathInstruction> _instructions;

	private int _index;
	private (Vector2 position, VectorPathInstruction instruction) _current;
	private Vector2 _currentPosition;
	private Vector2 _currentContourStart;

	public VectorPathInstructionsIterator(IEnumerable<VectorPathInstruction> instructions)
		=> _instructions = instructions.ToList();

	public (Vector2 position, VectorPathInstruction instruction) Current
		=> _current;

	object IEnumerator.Current
		=> _current;

	public bool MoveNext()
	{
		if (_index >= _instructions.Count)
			return false;

		var instruction = _instructions[_index++];

		if (instruction is MoveToInstruction ||
			instruction is ContourInstruction)
			_currentContourStart = instruction.GetEndPoint();

		_current.position = _currentPosition;
		_current.instruction = instruction;

		_currentPosition = instruction is CloseInstruction
			? _currentContourStart
			: instruction.GetEndPoint();

		return true;
	}

	public void Reset()
	{
		_index = 0;
		_current = default;
		_currentPosition = new(0, 0);
		_currentContourStart = new(0, 0);
	}

	public void Dispose() { }

	public IEnumerable<(Vector2 position, VectorPathInstruction instruction)> Enumerate()
	{
		Reset();
		while (MoveNext()) yield return Current;
	}
}
