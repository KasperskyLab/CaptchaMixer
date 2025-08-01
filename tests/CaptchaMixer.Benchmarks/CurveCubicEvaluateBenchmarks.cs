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
public class CurveCubicEvaluateBenchmarks
{
	[Benchmark]
	public void CurveCubicEvaluate_Vector2_Span()
	{
		Span<Vector2> result = stackalloc Vector2[5];
		VectorMath.CurveCubicEvaluate(new CurveCubic2
		{
			Start = new Vector2(10, 10),
			Control1 = new Vector2(20, 20),
			Control2 = new Vector2(25, 25),
			End = new Vector2(30, 10)
		}, result);
	}
}
