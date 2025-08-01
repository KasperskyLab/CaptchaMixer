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
/// Splits objects into multiple objects - one object for each
/// path of the original objects.
/// </summary>
public class SplitObjects : OneByOneVectorObjectsProcessor
{
	protected override IEnumerable<VectorObject> Process(VectorObject obj, ICaptchaContext context)
	{
		if (obj.Paths.Count <= 1)
		{
			yield return obj;
			yield break;
		}

		foreach (var path in obj)
			yield return new VectorObject(path);
	}
}
