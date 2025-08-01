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

namespace Kaspersky.CaptchaMixer.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class RationalToInstructionBenchmarks
{
	private RationalToInstruction? _instruction;

	[GlobalSetup]
	public void Setup()
	{
		_instruction = new RationalToInstruction(1, new Vector3(10, 25, 0.8f), new Vector3(50, 30, 0.7f));
	}

	[Benchmark(Baseline = true)]
	public void GetBounds_Span()
	{
		_instruction!.GetBounds(new Vector2(100, 7));
	}
}