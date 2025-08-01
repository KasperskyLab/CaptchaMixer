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
/// Values provider base class.
/// </summary>
public abstract class ValueProvider<T>
{
	public abstract T GetNext();

	public static implicit operator T(ValueProvider<T> provider) => provider.GetNext();

	public static implicit operator ValueProvider<T>(T value) => new ConstValueProvider<T>(value);
}
