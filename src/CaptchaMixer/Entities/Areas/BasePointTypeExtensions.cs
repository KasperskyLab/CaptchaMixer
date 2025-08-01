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

public static class BasePointTypeExtensions
{
	/// <summary>
	/// Calculates a layer anchor point for <paramref name="basePointType"/> if it points
	/// to a layer point and not to an object point.
	/// </summary>
	public static bool TryGetLayerAnchor(this BasePointType basePointType, out RectAnchor anchor)
	{
		switch (basePointType)
		{
			case BasePointType.LayerCenter:
				anchor = RectAnchor.Center;
				return true;
			case BasePointType.LayerLeftTop:
				anchor = RectAnchor.LeftTop;
				return true;
			case BasePointType.LayerCenterTop:
				anchor = RectAnchor.CenterTop;
				return true;
			case BasePointType.LayerRightTop:
				anchor = RectAnchor.RightTop;
				return true;
			case BasePointType.LayerRightCenter:
				anchor = RectAnchor.RightCenter;
				return true;
			case BasePointType.LayerRightBottom:
				anchor = RectAnchor.RightBottom;
				return true;
			case BasePointType.LayerCenterBottom:
				anchor = RectAnchor.CenterBottom;
				return true;
			case BasePointType.LayerLeftBottom:
				anchor = RectAnchor.LeftBottom;
				return true;
			case BasePointType.LayerLeftCenter:
				anchor = RectAnchor.LeftCenter;
				return true;
			default:
				anchor = 0;
				return false;
		};
	}

	/// <summary>
	/// Calculates an object anchor point for <paramref name="basePointType"/> if it points
	/// to an object point and not to a layer point.
	/// </summary>
	public static bool TryGetObjectAnchor(this BasePointType basePointType, out RectAnchor anchor)
	{
		switch (basePointType)
		{
			case BasePointType.ObjectCenter:
				anchor = RectAnchor.Center;
				return true;
			case BasePointType.ObjectLeftTop:
				anchor = RectAnchor.LeftTop;
				return true;
			case BasePointType.ObjectCenterTop:
				anchor = RectAnchor.CenterTop;
				return true;
			case BasePointType.ObjectRightTop:
				anchor = RectAnchor.RightTop;
				return true;
			case BasePointType.ObjectRightCenter:
				anchor = RectAnchor.RightCenter;
				return true;
			case BasePointType.ObjectRightBottom:
				anchor = RectAnchor.RightBottom;
				return true;
			case BasePointType.ObjectCenterBottom:
				anchor = RectAnchor.CenterBottom;
				return true;
			case BasePointType.ObjectLeftBottom:
				anchor = RectAnchor.LeftBottom;
				return true;
			case BasePointType.ObjectLeftCenter:
				anchor = RectAnchor.LeftCenter;
				return true;
			default:
				anchor = 0;
				return false;
		};
	}
}