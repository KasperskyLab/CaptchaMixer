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

namespace Kaspersky.CaptchaMixer.UnitTests.Utils;

public class ArrayExtensionsTests
{
	[Test]
	public void ArrayExtensions_Shuffle_Test()
	{
		var src = Enumerable.Range(0, 20).ToArray();
		var arr = new int[src.Length];
		Array.Copy(src, arr, src.Length);
		arr.Shuffle();
		arr.Should().BeEquivalentTo(src);
		arr.Should().NotContainInConsecutiveOrder(src);
	}
}
