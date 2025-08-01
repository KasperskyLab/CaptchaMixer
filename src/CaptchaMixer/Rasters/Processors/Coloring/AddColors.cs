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
/// Increases/decreases pixel color components.
/// </summary>
public class AddColors : IRasterProcessor
{
	/// <summary>
	/// Red component shift.
	/// </summary>
	public ValueProvider<int> AddR { get; set; } = RandomInt(-32, 32);

	/// <summary>
	/// Green component shift.
	/// </summary>
	public ValueProvider<int> AddG { get; set; } = RandomInt(-32, 32);

	/// <summary>
	/// Blue component shift.
	/// </summary>
	public ValueProvider<int> AddB { get; set; } = RandomInt(-32, 32);

	/// <summary>
	/// Alpha component shift.
	/// </summary>
	public ValueProvider<int> AddA { get; set; } = 0;

	/// <summary>
	/// Sets <see cref="AddR"/>, <see cref="AddG"/> and <see cref="AddB"/>.
	/// </summary>
	public ValueProvider<int> AddRGB
	{
		set
		{
			AddR = value;
			AddG = value;
			AddB = value;
		}
	}

	/// <summary>
	/// Sets <see cref="AddR"/>, <see cref="AddG"/> , <see cref="AddB"/> and <see cref="AddA"/>.
	/// </summary>
	public ValueProvider<int> Add
	{
		set
		{
			AddRGB = value;
			AddA = value;
		}
	}

	public AddColors() { }

	/// <param name="addRGB"><inheritdoc cref="AddRGB" path="/summary"/></param>
	public AddColors(ValueProvider<int> addRGB)
		: this(addRGB, addRGB, addRGB, 0) { }

	/// <param name="addRGB"><inheritdoc cref="AddRGB" path="/summary"/></param>
	/// <param name="addA"><inheritdoc cref="AddA" path="/summary"/></param>
	public AddColors(ValueProvider<int> addRGB, ValueProvider<int> addA)
		: this(addRGB, addRGB, addRGB, addA) { }

	/// <param name="addR"><inheritdoc cref="AddR" path="/summary"/></param>
	/// <param name="addG"><inheritdoc cref="AddG" path="/summary"/></param>
	/// <param name="addB"><inheritdoc cref="AddB" path="/summary"/></param>
	/// <param name="addA"><inheritdoc cref="AddA" path="/summary"/></param>
	public AddColors(
		ValueProvider<int> addR,
		ValueProvider<int> addG,
		ValueProvider<int> addB,
		ValueProvider<int> addA)
	{
		AddR = addR;
		AddG = addG;
		AddB = addB;
		AddA = addA;
	}

	public void Process(IRasterLayer layer, ICaptchaContext context)
	{
		var data = layer.Data;
		var length = data.Length;
		for (var i = 0; i < length; i += 4)
		{
			ModifyChannel(ref data[i], AddR);
			ModifyChannel(ref data[i + 1], AddG);
			ModifyChannel(ref data[i + 2], AddB);
			ModifyChannel(ref data[i + 3], AddA);
		}
	}

	private static byte ModifyChannel(ref byte value, ValueProvider<int> offset)
		=> value = (byte)(value + offset).Clamp(0, 255);
}
