/*
 * Engine Extension Methods
 *
 * Description: Various extension methods for Unity Engine classes to make them more elegant.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12
*/

// ReSharper disable once CheckNamespace - These extension methods need to be tied to the engine to be available everywhere
// ReSharper disable MemberCanBePrivate.Global - Not all of them are always in use but they should all be available
namespace UnityEngine
{
    using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	using PushForward.ExtensionMethods;
	using UIElements;
	#if UNITY_EDITOR
	using UnityEditor;
	#endif
	using UnityEngine;

    public static class EngineExtensionMethods
    {
        #region Object and Components
		#region Activation
		public static bool IsActive(this Component component) => component.gameObject.activeSelf;
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
		public static void ToggleActive(this GameObject go) => go.SetActive(!go.activeSelf);
		public static void ToggleActive(this Component component)
		{ component.gameObject.ToggleActive(); }
		#endregion // activation
		#region Components
        /// <summary>Adds the given component type to the underlying GameObject.</summary>
        /// <returns>A reference to the new Component</returns>
        public static T AddComponent<T>(this Component component) where T : Component
        { return component.gameObject.AddComponent<T>(); }
        public static RectTransform GetRectTransform(this GameObject gameObject)
        { return (RectTransform)gameObject.transform; }
        /// <summary>Gets the component's RectTransform if available. Null, otherwise.</summary>
        public static RectTransform GetRectTransform(this Component component)
        { return component.gameObject.GetRectTransform(); }
		/// <summary>Creates an empty object with transform values equal to base transform.</summary>
		/// <remarks>This instantiates an object. It's empty but clean after yourselves.</remarks>
		/// <param name="tr">The transform to clone.</param>
		/// <returns>The transform of a new empty object.</returns>
		public static Transform EmptyClone(this Transform tr)
		{
			GameObject go = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.DestroyAllComponents();
			go.transform.position = tr.position;
			go.transform.rotation = tr.rotation;
			go.transform.localScale = tr.localScale;
			return go.transform;
		}
		#endregion // components
		#region Destruction
		public static void DestroyThisBehaviour(this Component component) => Object.Destroy(component);
		public static void DestroyThisObject(this GameObject go) => Object.Destroy(go);
		public static void DestroyThisObject(this Component component) => component.gameObject.DestroyThisObject();
		public static void DestroyAllChildren(this Component component) => component.transform.DestroyAllChildren();
		public static void DestroyAllComponents(this GameObject go)
		{
			Component[] components = go.GetComponents<UnityEngine.Component>();
			components.DoForEach(comp => { if (!(comp is Transform)) { comp.DestroyThisBehaviour(); } });
		}
		public static void DestroyAllComponents(this Component component) => component.gameObject.DestroyAllComponents();
		#endregion // destruction

		/// <summary>Gets the prefab's path if it is in the resources folder.</summary>
		/// <remarks>ASSERT: You are using a prefab!
		///		ASSERT: That prefab is in the Resources folder.
		///		ASSERT: You are running this from Editor and not at runtime.
		///		If any of these are false, behaviour is unknown.
		///		If the last one is false, it just won't work.</remarks>
		/// <param name="prefab">The prefab in question.</param>
		/// <returns>The path to it INSIDE resources (excluding that folder)</returns>
		public static string GetPrefabPathInResources(this GameObject prefab)
		{
			if (prefab == null)
			{ return null; }
			
			#if UNITY_EDITOR
			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefab, out string guid, out long file))
			{ return AssetDatabase.GUIDToAssetPath(guid).Replace("Assets/Resources/", string.Empty); }
			#endif
			
			return null;
		}
        #endregion // Objects and Components

		#region Monobehaviour
		#region properties
		/// <summary>The path to this object through the hierarchy.</summary>
		/// <remarks>This is a heavy calculating call. Use sparingly and cache your results.</remarks>
		public static string ObjectPath(this MonoBehaviour mb)
		{
			// create a list of objects from this one to the root
			List<GameObject> pathObjects = new List<GameObject>();
			Transform current = mb.transform;
			do
			{
				pathObjects.Add(current.gameObject);
				current = current.transform.parent;
			} while (current != null);

			// create a backlash delimited string of the objects
			pathObjects.Reverse();
			StringBuilder pathBuilder = new StringBuilder();
			foreach (GameObject pathObject in pathObjects)
			{ pathBuilder.Append(pathObject.name).Append('\\'); }

			pathBuilder.RemoveCharactersFromEnd(1);

			return pathBuilder.ToString();
		}
		/// <summary>Access GameObject.activeInHierarchy.</summary>
		/// <remarks>Is the GameObject active in the scene?</remarks>
		public static bool ActiveInHierarchy(this MonoBehaviour mb)
		{ return mb.gameObject.activeInHierarchy; }
		private static Camera mainCamera;
		public static Camera GetMainCamera(bool forceRefresh = false)
		{
			if (mainCamera == null || forceRefresh)
			{ mainCamera = Camera.main; }
			return mainCamera;
		}
		#endregion // properties

		#region coroutines
		private static readonly WaitForEndOfFrame frameWait = new WaitForEndOfFrame();
		public static void Stop(this Coroutine coroutine, MonoBehaviour parent) => parent.StopCoroutine(coroutine);
		private static IEnumerator ActionWhenPredicateCoroutine(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate)
		{
			if (actionToCall == null || predicate == null)
			{ yield break; }

			do { yield return frameWait; }
			// ReSharper disable once LoopVariableIsNeverChangedInsideLoop - changed outside in the caller
			while (!predicate());

			actionToCall();
		}

		/// <summary>Checks predicate every frame until predicate resolves to true and then calls action.</summary>
		/// <param name="mb">The Monobehaviour to run this from.</param>
		/// <param name="actionToCall">The action to call when predicate fulfilled.</param>
		/// <param name="predicate">The predicate to check for every frame.</param>
		public static Coroutine ActionWhenPredicate(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionWhenPredicateCoroutine(actionToCall, predicate)) : null; }
		private static IEnumerator ActionEachFrameForSecondsCoroutine(this MonoBehaviour mb, Action<float> actionToCall, float amountOfSecondsToTake)
		{
			if (actionToCall == null || amountOfSecondsToTake < 0)
			{ yield break; }

			float secondsCounter = 0f;

			while (secondsCounter < amountOfSecondsToTake)
			{
				actionToCall(secondsCounter);
				yield return new WaitForEndOfFrame();
				secondsCounter += Time.deltaTime;
			}
			actionToCall(amountOfSecondsToTake);
		}

		///  <summary>Calls the given action every frame for the given amount of seconds.
		/// 		The parameter will be the amount of seconds already passed since the start frame.</summary>
		///  <param name="mb">The MonoBehaviour to run this from.</param>
		///  <param name="actionToCall">The action to call every frame with the seconds count.</param>
		///  <param name="amountOfSecondsToTake">The time frame in which to call the action every frame.</param>
		public static Coroutine ActionEachFrameForSeconds(this MonoBehaviour mb, Action<float> actionToCall, float amountOfSecondsToTake)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionEachFrameForSecondsCoroutine(actionToCall, amountOfSecondsToTake)) : null; }
		private static IEnumerator ActionInSecondsCoroutine(this MonoBehaviour mb, Action actionToStart, float secondsToWait)
		{
			// if parameters are in error, abort
			if (actionToStart == null || secondsToWait < 0)
			{ yield break; }

			if (!secondsToWait.FloatEqual(0))
			{ yield return new WaitForSeconds(secondsToWait); }

			actionToStart();
		}

		/// <summary>Invoke the given action in the given amount of seconds</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToStart">The given action to invoke</param>
		/// <param name="secondsToWait">The amount to wait before action, in seconds</param>
		public static Coroutine ActionInSeconds(this MonoBehaviour mb, Action actionToStart, float secondsToWait)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionInSecondsCoroutine(actionToStart, secondsToWait)) : null; }
		private static IEnumerator ActionInFramesCoroutine(this MonoBehaviour mb, Action actionToStart, int framesToWait)
		{
			// if parameters are in error, abort
			if (actionToStart == null || framesToWait < 0)
			{ yield break; }

			for (int frameIndex = 0; frameIndex < framesToWait; frameIndex++)
			{ yield return frameWait; }

			actionToStart();
		}

		/// <summary>Invoke the given action after the given amount of render frames</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToStart">The given action to invoke</param>
		/// <param name="framesToWait">The amount to wait before action, in frames</param>
		public static Coroutine ActionInFrames(this MonoBehaviour mb, Action actionToStart, int framesToWait)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionInFramesCoroutine(actionToStart, framesToWait)) : null; }
		private static IEnumerator ActionEachFrameWhilePredicateCoroutine(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate, int skipFrames)
		{
			if (actionToCall == null || predicate == null)
			{ yield break; }

			int countFrames = 1;

			// ReSharper disable once LoopVariableIsNeverChangedInsideLoop - changed outside in the caller
			while (predicate())
			{
				if (countFrames > skipFrames)
				{
					countFrames = 1;
					actionToCall();
				}
				else { countFrames++; }
				yield return frameWait;
			}
		}

		/// <summary>Invoke the given action each frame while predicate resolves to true.</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToCall">The action to invoke.</param>
		/// <param name="predicate">The predicate to check.</param>
		/// <param name="skipFrames">The amount of frames to skip between actions.</param>
		public static Coroutine ActionEachFrameWhilePredicate(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate, int skipFrames = 0)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionEachFrameWhilePredicateCoroutine(actionToCall, predicate, skipFrames)) : null; }

		/// <summary>Does the action now if the scene is loaded or when it finishes loading.</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="action">The action to take.</param>
		public static Coroutine ActionNowOrWhenSceneLoaded(this MonoBehaviour mb, Action action)
		{
			if (action == null)
			{ return null; }

			if (SceneManagement.SceneManager.GetActiveScene().isLoaded)
			{ action(); }
			else { return mb.ActionWhenPredicate(action, () => SceneManagement.SceneManager.GetActiveScene().isLoaded); }
			return null;
		}
		#endregion // coroutines

		#region timing
		public static string CurrentFullTimeIn24Hours(char dateDelimiter = '-', char timeDelimiter = ';', char millisecondsDelimiter = ',')
		{
			string standardOutput = DateTime.Now.ToString("u");
			standardOutput = standardOutput.Replace('-', dateDelimiter)
										   .Replace(':', timeDelimiter)
										   .Replace('Z', millisecondsDelimiter);

			return standardOutput + DateTime.Now.Millisecond;
		}
		/// <summary>Milliseconds since epoch</summary>
		public static double CurrentTimeInMilliseconds
			=> DateTime.Now.Subtract(new DateTime(1970, 01, 01)).TotalMilliseconds;
		#endregion

		#region logging
		private static Dictionary<string, double> timeTagsDictionary;

		/// <summary>Regular debug log.</summary>
		/// <param name="methodName">The name of the method in which this is used.</param>
		/// <param name="mb">The monobehaviour from which this was called.</param>
		/// <param name="logMessage">The message to display.</param>
		public static void Log(this MonoBehaviour mb, string logMessage,
							   [System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("{0}.{1}.{2}: {3}", mb.ObjectPath(), mb.GetType().Name, methodName, logMessage)); }
		public static void Log(this ScriptableObject scriptableObject, string debugMessage,
							   [System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("{0}.{1}: {2}", scriptableObject.GetType().Name, methodName, debugMessage)); }
		
		/// <summary>A debug log with a timed segment letting you know the time passed since the last call of the same tag.</summary>
		/// <param name="mb">The monobehaviour from where this was called.</param>
		/// <param name="timeTag">The tag to use to group calls.</param>
		/// <param name="debugMessage">The message to display.</param>
		/// <param name="resetTimer">Wether to reset the timer when called.</param>
		/// <param name="methodName">The name of the method in which this is used.</param>
		public static void TimedLog(this MonoBehaviour mb, string timeTag, string debugMessage, bool resetTimer = false,
									[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{
			double timeSinceLastCall;
			// if no dictionary, create one
			EngineExtensionMethods.timeTagsDictionary ??= new Dictionary<string, double>();

			// no tag, make one and set last time to 0.
			if (!EngineExtensionMethods.timeTagsDictionary.ContainsKey(timeTag) || resetTimer)
			{
				timeSinceLastCall = 0;
				EngineExtensionMethods.timeTagsDictionary[timeTag] = EngineExtensionMethods.CurrentTimeInMilliseconds;
			}
			else // with tag, get last call and replace it.
			{
				timeSinceLastCall = EngineExtensionMethods.CurrentTimeInMilliseconds - EngineExtensionMethods.timeTagsDictionary[timeTag];
				EngineExtensionMethods.timeTagsDictionary[timeTag] = EngineExtensionMethods.CurrentTimeInMilliseconds;
			}

			mb.Log(string.Format("{0}; {1} (Time Since Epoch: {2}; Time Since Last Call: {3})",
								 timeTag, debugMessage, EngineExtensionMethods.CurrentTimeInMilliseconds, timeSinceLastCall));
		}

		/// <summary>Temp logging is like debug but in green. Used for logs during actual debugging. Dispose of after use.</summary>
		/// <param name="methodName">The name of the method in which this is used.</param>
		/// <param name="mb">The monobehaviour from which this was called.</param>
		/// <param name="debugMessage">The message to display.</param>
		public static void Temp(this MonoBehaviour mb, string debugMessage,
								[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("<color=green>{0}.{1}.{2}: {3}</color>",
								  mb.ObjectPath(), mb.GetType().Name, methodName, debugMessage)); }
		public static void Temp(this ScriptableObject scriptableObject, string debugMessage,
								[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("<color=green>{0}.{1}: {2}</color>",
								  scriptableObject.GetType().Name, methodName, debugMessage)); }

		/// <summary>Warning log is marked with a yellow warning symbol.</summary>
		/// <param name="methodName">The name of the method in which this is used.</param>
		/// <param name="mb">The monobehaviour from which this was called.</param>
		/// <param name="warningMessage">The message to display.</param>
		public static void Warn(this MonoBehaviour mb, string warningMessage,
								[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.LogWarning(string.Format("{0}.{1}.{2}: {3}", mb.ObjectPath(), mb.GetType().Name, methodName, warningMessage)); }
		public static void Warn(this ScriptableObject scriptableObject, string debugMessage,
								[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.LogWarning(string.Format("{0}.{1}: {2}", scriptableObject.GetType().Name, methodName, debugMessage)); }

		/// <summary>Error log is marked with a red error symbol.</summary>
		/// <param name="methodName">The name of the method in which this is used.</param>
		/// <param name="mb">The monobehaviour from which this was called.</param>
		/// <param name="errorMessage">The message to display.</param>
		public static void Error(this MonoBehaviour mb, string errorMessage,
								 [System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.LogError(string.Format("{0}.{1}.{2}: {3}", mb.ObjectPath(), mb.GetType().Name, methodName, errorMessage)); }
		public static void Error(this ScriptableObject scriptableObject, string debugMessage,
								[System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.LogError(string.Format("{0}.{1}: {2}", scriptableObject.GetType().Name, methodName, debugMessage)); }
		#endregion
		#endregion // MonoBehaviour

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
			{ throw new ArgumentException("GameObject can't be null.", nameof(gameObject)); }
			if (string.IsNullOrEmpty(name))
			{ throw new ArgumentException("Name can't be null or empty", nameof(name)); }

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
			List<T> list = new List<T>();

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
				#if UNITY_EDITOR
				List<Transform> removeThese = new List<Transform>();
				// if we have more key code objects
				for (int childIndex = 0; childIndex < transform.transform.childCount; childIndex++)
				{
					// save them for removal
					Transform unnecessaryTransform = transform.transform.GetChild(childIndex);
					removeThese.Add(unnecessaryTransform);
				}
				// destroy unnecessary objects
				removeThese.DoForEach(tform => { UnityEditor.EditorApplication.delayCall += () => Object.DestroyImmediate(tform.gameObject); });
				#endif
			}
		}
		#endregion
    }
}
