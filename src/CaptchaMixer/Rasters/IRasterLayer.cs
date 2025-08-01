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
/// Raster layer.
/// </summary>
public interface IRasterLayer
{
	/// <summary>
	/// Layer name.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Layer size.
	/// </summary>
	Size Size { get; set; }

	/// <summary>
	/// Pixels binary data.
	/// </summary>
	byte[] Data { get; set; }
}