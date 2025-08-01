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

using Nuke.Common.Tooling;

using System.ComponentModel;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
	public static Configuration Debug = new() { Value = nameof(Debug) };
	public static Configuration Release = new() { Value = nameof(Release) };

	public static implicit operator string(Configuration configuration) => configuration.Value;
}
