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

/// <inheritdoc cref="IRasterLayer"/>
public class RasterLayer : IRasterLayer
{
	public string Name { get; }
	public Size Size { get; set; }
	public byte[] Data { get; set; }

	/// <param name="name"><inheritdoc cref="Name" path="/summary"/></param>
	/// <param name="width">Layer width.</param>
	/// <param name="height">Layer height.</param>
	public RasterLayer(string name, int width, int height)
		: this(name, new Size(width, height)) { }

	/// <param name="name"><inheritdoc cref="Name" path="/summary"/></param>
	/// <param name="size"><inheritdoc cref="Size" path="/summary"/></param>
	public RasterLayer(string name, Size size)
	{
		Name = name;
		Size = size;
		Data = new byte[size.Width * size.Height * 4];
	}
}
