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
/// Constants and collections of cross-platforms well-known fonts.
/// </summary>
public static class WellKnownFontFamilies
{
	public const string Arial = "arial";
	public const string ArialBlack = "arial black";
	public const string ComicSansMs = "comic sans ms";
	public const string CourierNew = "courier new";
	public const string Georgia = "georgia";
	public const string LucidaConsole = "lucida console";
	public const string LucidaSansUnicode = "lucida sans unicode";
	public const string PalatinoLinotype = "palatino linotype";
	public const string Tahoma = "tahoma";
	public const string TimesNewRoman = "times new roman";
	public const string TrebuchetMs = "trebuchet ms";
	public const string Verdana = "verdana";

	/// <summary>
	/// Full set of well-known fonts.
	/// </summary>
	public static readonly string[] All = new[]
	{
		Arial,
		ArialBlack,
		ComicSansMs,
		CourierNew,
		Georgia,
		LucidaConsole,
		LucidaSansUnicode,
		PalatinoLinotype,
		Tahoma,
		TimesNewRoman,
		TrebuchetMs,
		Verdana
	};

	/// <summary>
	/// Visually thick well-known fonts.
	/// </summary>
	public static readonly string[] Thick = new[]
	{
		Arial,
		ComicSansMs,
		LucidaConsole,
		LucidaSansUnicode,
		Tahoma,
		TrebuchetMs,
		Verdana
	};

	/// <summary>
	/// Visually thin well-known fonts.
	/// </summary>
	public static readonly string[] Thin = new[]
	{
		ArialBlack,
		CourierNew,
		Georgia,
		PalatinoLinotype,
		TimesNewRoman
	};
}
