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
/// Base class for complex path creators.
/// </summary>
public abstract class PathCreator : AreaBasedVectorObjectsCreator
{
	/// <summary>
	/// Paths count.
	/// </summary>
	public ValueProvider<int> Paths { get; set; } = 10;

	/// <summary>
	/// Provider of points for paths building.
	/// </summary>
	public ValueProvider<Vector2> Points { get; set; } = null;

	/// <summary>
	/// Count of instructions in a path.
	/// </summary>
	public ValueProvider<int> Parts { get; set; } = 3;

	/// <summary>
	/// Is it fine to go beyond the <see cref="AreaBasedVectorObjectsCreator.Area"/>.
	/// </summary>
	public ValueProvider<bool> Beyonds { get; set; } = true;

	/// <summary>
	/// Close the paths.
	/// </summary>
	public ValueProvider<bool> Close { get; set; } = false;

	protected PathCreator() { }

	/// <param name="paths"><inheritdoc cref="Paths" path="/summary"/></param>
	/// <param name="parts"><inheritdoc cref="Parts" path="/summary"/></param>
	/// <exception cref="ArgumentNullException"/>
	protected PathCreator(
		ValueProvider<int> paths,
		ValueProvider<int> parts)
	{
		Paths = paths ?? throw new ArgumentNullException(nameof(paths));
		Parts = parts ?? throw new ArgumentNullException(nameof(parts));
	}

	public override void Process(
		RectangleF area,
		IVectorLayer layer,
		ICaptchaContext context)
	{
		var obj = new VectorObject();

		int paths = Paths;
		var points = Points ?? new RectRandomPointProvider(area);
		for (var i = 0; i < paths; i++)
		{
			var path = CreatePath(area, points, Parts, Beyonds);
			if (!path.IsMeaningful()) continue;
			if ((bool)Close) path.Instructions.Add(new CloseInstruction());
			obj.Paths.Add(path);
		}

		layer.Objects.Add(obj);
	}

	protected abstract VectorPath CreatePath(
		RectangleF area,
		ValueProvider<Vector2> points,
		int parts,
		bool beyonds);
}
