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
public class CurveRationalGranulateBenchmarks
{
	[Benchmark(Baseline = true)]
	public void CurveRationalGranulate_Vector3()
	{
		VectorMath.CurveRationalGranulate(
			new CurveQuad3(
				new Vector3(10, 10, 0.8f),
				new Vector3(20, 20, 0.6f),
				new Vector3(30, 10, 1f)),
			4);
	}
}
