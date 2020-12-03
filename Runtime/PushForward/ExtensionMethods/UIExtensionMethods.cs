/*
	UIExtensionMethos

	Description: A collection of useful methods for UI management to enhance functionality.
	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-07-19
*/

namespace UnityEngine
{
	#region using
	using System.Text;
	using System;
	using System.Collections.Generic;
	using UnityEngine.EventSystems;
	#endregion

	/// <summary>Helper methods for Unity Engine objects and behaviours</summary>
	static class UIExtensionMethods
	{
		public static bool IsPointerOverUIObject()
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}
	}
}
