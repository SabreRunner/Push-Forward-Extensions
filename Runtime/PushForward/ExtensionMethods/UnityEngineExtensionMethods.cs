/*
	UnityEngineExtensionMethods

	Description: A collection of useful methods for Unity Engine classes to enhance functionality.
	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2017-03-31
*/

namespace UnityEngine
{
	#region using
	using System.Text;
	using System;
	using System.Collections.Generic;
	#endregion

	/// <summary>Helper methods for Unity Engine objects and behaviours</summary>
	static class UnityEngineExtensionMethods
	{
		#region primitives
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}
		#endregion // primitives

		#region basic properties
		/// <summary>Activates the game object</summary>
		/// <remarks>Checks for null</remarks>
		public static void Activate(this GameObject obj)
		{
			if (obj != null)
			{ obj.SetActive(true); }
		}
		/// <summary>Deactivates the game object</summary>
		/// <remarks>Checks for null</remarks>
		public static void Deactivate(this GameObject obj)
		{
			if (obj != null)
			{ obj.SetActive(false); }
		}
		/// <summary>Sets the active state of the underlying game object</summary>
		/// <remarks>Checks for null</remarks>
		public static void SetActive(this Component component, bool state)
		{
			if (component != null)
			{ component.gameObject.SetActive(state); }
		}
		/// <summary>Activates the component's underlying game object</summary>
		/// <remarks>Checks for null</remarks>
		public static void Activate(this Component component)
		{
			if (component != null)
			{ component.SetActive(true); }
		}
		/// <summary>Deactivates the component's underlying game object</summary>
		/// <remarks>Checks for null</remarks>
		public static void Deactivate(this Component component)
		{
			if (component != null)
			{ component.SetActive(false); }
		}
		/// <summary>Adds the given component type to the underlying GameObject.</summary>
		/// <returns>A reference to the new Component</returns>
		public static Component AddComponent(this Component component, Type type)
		{ return component.gameObject.AddComponent(type); }
		public static RectTransform GetRectTransform(this GameObject gameObject)
		{ return (RectTransform)gameObject.transform; }
		/// <summary>Gets the component's RectTransform if available. Null, otherwise.</summary>
		public static RectTransform GetRectTransform(this Component component)
		{ return component.gameObject.GetRectTransform(); }
		#endregion         // basic properties

		#region children methods
		/// <summary>Get all the children objects of the given object.</summary>
		public static GameObject[] GetChildObjects(this GameObject gameObject)
		{
			GameObject[] objects = new GameObject[gameObject.transform.childCount];
			for (int objectIndex = 0; objectIndex < gameObject.transform.childCount; objectIndex++)
			{ objects[objectIndex] = gameObject.transform.GetChild(objectIndex).gameObject; }
			return objects;
		}

		public static GameObject FindChildByName(this GameObject gameObject, string name)
		{
			// if bad parameters, abort
			if (gameObject == null)
			{ throw new ArgumentException("GameObject can't be null.", "gameObject"); }
			if (string.IsNullOrEmpty(name))
			{ throw new ArgumentException("Name can't be null or empty", "name"); }

			// if this is the desired child, return
			if (gameObject.name == name)
			{ return gameObject; }

			// look in current children
			Transform childTransform = gameObject.transform.Find(name);

			// if found, return it.
			if (childTransform != null)
			{ return childTransform.gameObject; }

			// if not, iterate over children
			for (int childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++)
			{
				// if found in child, return it
				GameObject childGameObject = gameObject.transform.GetChild(childIndex).gameObject.FindChildByName(name);
				if (childGameObject != null)
				{ return childGameObject; }
			}

			// if nothing found, return null
			return null;
		}
		/// <summary>Searches the object tree for the object specified by the given path.</summary>
		/// <remarks>It is assumed that the given object is the first in the path.</remarks>
		/// <param name="obj">The object to start searching from, it will be the first in the path.</param>
		/// <param name="path">The path to an object to search for ('\\' delimited)</param>
		/// <returns>The object found or null if none was found (the path will be modified to contain only the objects found)</returns>

		public static GameObject FindChildByPath(this GameObject obj, ref string path)
		{
			// get all the possible names
			string[] objectNames = path.Split('\\');
			// prepare to find object
			GameObject foundObject = null;
			// iteration variable
			GameObject child = obj;
			// build the path string
			StringBuilder pathBuilder = new StringBuilder();

			// if path has one step, check it current object matches
			if (objectNames.Length == 1)
			{
				if (objectNames[0] == obj.name)
				{
					path = objectNames[0];
					return obj;
				}
				return null;
			}

			// go over the list of names and make sure the current object exists and its name matches the one in the path
			for (int index = 0; index < objectNames.Length && child != null && objectNames[index] == child.name; index++)
			{
				// save the object
				foundObject = child;
				// update the path
				pathBuilder.Append(objectNames[index]).Append('\\');
				// search for the next one
				Transform transform =
					child.transform != null && index + 1 < objectNames.Length ? child.transform.Find(objectNames[index + 1]) : null;
				child = transform == null ? null : transform.gameObject;
			}

			pathBuilder.Remove(pathBuilder.Length - 1, 1);
			path = pathBuilder.ToString();
			// return whatever was found
			return foundObject;
		}
		/// <summary>Searches the object tree for the object specified by the given path.</summary>
		/// <remarks>It is assumed that the given object is the first in the path.</remarks>
		/// <param name="component">The component of the object to start searching from, it will be the first in the path.</param>
		/// <param name="path">The path to an object to search for</param>
		/// <returns>The object found or null if none was found (The path will be modified to contain only the objects found)</returns>
		public static GameObject FindChildByPath(this Component component, ref string path)
		{ return component.gameObject.FindChildByPath(ref path); }
		/// <summary>Gets all components of type T in the object's children and not in the object itself</summary>
		/// <typeparam name="T">The type of Component to get.</typeparam>
		/// <returns>An array of Components of type T. Order is not guaranteed.</returns>

		public static T[] GetComponentsOnlyInChildren<T>(this GameObject gameObject) where T : Component
		{
			System.Collections.Generic.List<T> list = new List<T>();

			for (int childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++)
			{ list.AddRange(gameObject.transform.GetChild(childIndex).GetComponents<T>()); }

			return list.ToArray();
		}
		/// <summary>Gets all components of type T in the object's children and not in the object itself</summary>
		/// <typeparam name="T">The type of Component to get.</typeparam>
		/// <returns>An array of Components of type T. Order is not guaranteed.</returns>
		public static T[] GetComponentsOnlyInChildren<T>(this Component component) where T : Component
		{ return component.gameObject.GetComponentsOnlyInChildren<T>(); }

		public static void DestroyAllChildren(this Transform transform)
		{
			if (Application.isPlaying)
			{
				while (transform.transform.childCount > 0)
				{
					Transform child = transform.transform.GetChild(0);
					Object.DestroyImmediate(child.gameObject);
				}
			}
			else
			{
				List<Transform> removeThese = new List<Transform>();
				// if we have more key code objects
				for (int childIndex = 0; childIndex < transform.transform.childCount; childIndex++)
				{
					// save them for removal
					Transform unnecessaryTransform = transform.transform.GetChild(childIndex);
					removeThese.Add(unnecessaryTransform);
				}
#if UNITY_EDITOR
				// destroy unnecessary objects
				removeThese.DoForEach(tform => { UnityEditor.EditorApplication.delayCall += () => Object.DestroyImmediate(tform.gameObject); });
#endif
			}
		}
		public static void DestroyAllChildren(this GameObject gameObject)
		{ gameObject.transform.DestroyAllChildren(); }
		#endregion

		#region parent methods
		/// <summary>Finds the index of this game object within its parent's transform</summary>
		/// <returns>The index of the game object or -1 if errored</returns>
		public static int FindIndexInParent(this GameObject gameObject)
		{
			// if no parent, this is an error
			if (gameObject.transform.parent == null)
			{ return -1; }

			// get parent
			Transform parent = gameObject.transform.parent;

			// iterate over child references until found this one.
			for (int childIndex = 0; childIndex < parent.transform.childCount; childIndex++)
			{
				if (object.ReferenceEquals(parent.GetChild(childIndex).gameObject, gameObject))
				{ return childIndex; }
			}

			return -1;
		}

		/// <summary>Finds the index of this component's game object within its parent's transform</summary>
		/// <returns>The index of the game object or -1 if errored</returns>
		public static int FindIndexInParent(this Component component)
		{
			return component.gameObject.FindIndexInParent();
		}

		/// <summary>Gets the '\\' delimited full path of the given object from the root onwards</summary>
		public static string GetHierarchyPath(this GameObject gameObject)
		{
			StringBuilder pathBuilder = new StringBuilder(gameObject.name);

			for (Transform parent = gameObject.transform.parent; parent != null; parent = parent.gameObject.transform.parent)
			{
				pathBuilder.Insert(0, '\\');
				pathBuilder.Insert(0, parent.gameObject.name);
			}

			return pathBuilder.ToString();
		}
		#endregion // parent methods

		#region transforms
		/// <summary>Copy the values of the RectTransform.</summary>
		/// <param name="source">The source transform.</param>
		/// <param name="destination">The destination transform.</param>
		public static void CopyTo(this RectTransform source, RectTransform destination)
		{
			destination.anchorMax = source.anchorMax;
			destination.anchorMin = source.anchorMin;
			destination.pivot = source.pivot;
			destination.offsetMax = source.offsetMax;
			destination.offsetMin = source.offsetMin;
		}

		/// <summary>Copy the values of the Transform.</summary>
		/// <param name="source">The source transform.</param>
		/// <param name="destination">The destination transform.</param>
		public static void CopyTo(this Transform source, Transform destination)
		{
			destination.position = source.position + Vector3.zero;
			destination.localScale = source.localScale + Vector3.zero;
			destination.rotation.Set(source.rotation.x, source.rotation.y, source.rotation.z, source.rotation.w);
		}
		#endregion
	}
}
