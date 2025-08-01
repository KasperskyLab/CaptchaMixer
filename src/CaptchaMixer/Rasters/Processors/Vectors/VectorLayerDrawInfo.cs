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
/// Vector layer drawing information.
/// </summary>
public class VectorLayerDrawInfo
{
	/// <summary>
	/// Vector layer name.
	/// </summary>
	public ValueProvider<string> Name { get; set; }

	/// <inheritdoc cref="FillPaintInfo"/>
	/// <remarks>
	/// <see cref="ValueProvider{T}.GetNext"/> of this fill paint provider will only be called once
	/// but the resultant provider's painting settings will be evaluated for each vector object.
	/// </remarks>
	public ValueProvider<FillPaintInfo> Fill { get; set; }

	/// <inheritdoc cref="StrokePaintInfo"/>
	/// <inheritdoc cref="Fill" path="/remarks"/>
	public ValueProvider<StrokePaintInfo> Stroke { get; set; }

	/// <inheritdoc cref="VectorLayerDrawInfo(ValueProvider{string}, ValueProvider{FillPaintInfo}, ValueProvider{StrokePaintInfo})"/>
	public VectorLayerDrawInfo(
		ValueProvider<string> name,
		ValueProvider<FillPaintInfo> fill)
		: this(name, fill, StrokePaintInfo.Disabled) { }

	/// <inheritdoc cref="VectorLayerDrawInfo(ValueProvider{string}, ValueProvider{FillPaintInfo}, ValueProvider{StrokePaintInfo})"/>
	public VectorLayerDrawInfo(
		ValueProvider<string> name,
		ValueProvider<StrokePaintInfo> stroke)
		: this(name, FillPaintInfo.Disabled, stroke) { }

	/// <param name="name">
	/// <inheritdoc cref="Name" path="/summary"/>
	/// </param>
	/// <param name="fill">
	/// <inheritdoc cref="Fill" path="/summary"/><br />
	/// <inheritdoc cref="Fill" path="/remarks"/>
	/// </param>
	/// <param name="stroke">
	/// <inheritdoc cref="Stroke" path="/summary"/><br />
	/// <inheritdoc cref="Stroke" path="/remarks"/>
	/// </param>
	/// <exception cref="ArgumentNullException"/>
	public VectorLayerDrawInfo(
		ValueProvider<string> name,
		ValueProvider<FillPaintInfo> fill,
		ValueProvider<StrokePaintInfo> stroke)
	{
		Name = name ?? throw new ArgumentNullException(nameof(name));
		Fill = fill ?? throw new ArgumentNullException(nameof(fill));
		Stroke = stroke ?? throw new ArgumentNullException(nameof(stroke));
	}
}
