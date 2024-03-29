/*
 * Engine Extension Methods
 *
 * Description: Various extension methods for Unity Engine classes to make them more elegant and useful.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-11-13
*/

// ReSharper disable once CheckNamespace - These extension methods need to be tied to the engine to be available everywhere
// ReSharper disable MemberCanBePrivate.Global - Not all of them are always in use but they should all be available
namespace UnityEngine
{
	#region using
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
	#endregion // using

    public static class EngineExtensionMethods
    {
		// Methods for handling objects and components
		#region Object and Components
		// Methods for handling objects' active states
		#region Activation
		/// <summary>Whether the component's object is active or not.</summary>
		/// <param name="component">The component checked.</param>
		public static bool IsActive(this Component component)
			=> component != null && component.gameObject != null ? component.gameObject.activeSelf : false;
		/// <summary>Activates the game object</summary>
        /// <remarks>Checks for null</remarks>
        public static void Activate(this GameObject obj) => obj?.SetActive(true);
		/// <summary>Deactivates the game object</summary>
        /// <remarks>Checks for null</remarks>
        public static void Deactivate(this GameObject obj) => obj?.SetActive(false);

		/// <summary>Sets the active state of the underlying game object</summary>
        /// <remarks>Checks for null</remarks>
        public static void SetActive(this Component component, bool state) => component?.gameObject?.SetActive(state);

		/// <summary>Activates the component's underlying game object</summary>
        /// <remarks>Checks for null</remarks>
        public static void Activate(this Component component) => component?.SetActive(true);
		/// <summary>Deactivates the component's underlying game object</summary>
        /// <remarks>Checks for null</remarks>
        public static void Deactivate(this Component component) => component?.SetActive(false);
		public static void ToggleActive(this GameObject go) => go?.SetActive(!go.activeSelf);
		public static void ToggleActive(this Component component) => component?.gameObject?.ToggleActive();
		#endregion // activation
		// Methods for adding and getting components
		#region Components
        /// <summary>Adds the given component type to the underlying GameObject.</summary>
        /// <returns>A reference to the new Component</returns>
        public static T AddComponent<T>(this Component component) where T : Component
			=> component.gameObject.AddComponent<T>();
		public static RectTransform GetRectTransform(this GameObject gameObject)
			=> (RectTransform)gameObject.transform;
		/// <summary>Gets the component's RectTransform if available. Null, otherwise.</summary>
        public static RectTransform GetRectTransform(this Component component)
			=> component.gameObject.GetRectTransform();
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
		// Methods for destroying objects
		#region Destruction
		/// <summary>Just destroy the behaviour.</summary>
		/// <param name="component">The component to destroy.</param>
		public static void DestroyThisBehaviour(this Component component) => Object.Destroy(component);
		/// <summary>Just destroy the game object.</summary>
		/// <param name="go">The game object to destroy.</param>
		public static void DestroyThisObject(this GameObject go) => Object.Destroy(go);
		/// <summary>Destroy the object of this component.</summary>
		/// <param name="component">The component whos object to destroy</param>
		public static void DestroyThisObject(this Component component) => component.gameObject.DestroyThisObject();
		/// <summary>Destroy the children of this component's object's transform.</summary>
		/// <param name="component">The component whos children to destroy.</param>
		public static void DestroyAllChildren(this Component component) => component.transform.DestroyAllChildren();
		/// <summary>Destroys all components on the object except the transform.</summary>
		/// <param name="go">The game object to clear.</param>
		/// <remarks>Makes a given object into a clean transform.</remarks>
		public static void DestroyAllComponents(this GameObject go)
		{
			Component[] components = go.GetComponents<UnityEngine.Component>();
			components.DoForEach(component => { if (!(component is Transform)) { component.DestroyThisBehaviour(); } });
		}
		/// <summary>Destroys all components on the given component except the transform.</summary>
		/// <param name="component">The component on the object to clear.</param>
		public static void DestroyAllComponents(this Component component) => component.gameObject.DestroyAllComponents();
		#endregion // destruction

		public static bool ReferenceEqual(this object obj1, object obj2) => ReferenceEquals(obj1, obj2);
        #endregion // Objects and Components

		// Methods that enhances MonoBehaviour
		#region Monobehaviour
		// Added properties for easy access
		#region properties
		/// <summary>The path to this object through the hierarchy.</summary>
		/// <remarks>This is a heavy calculating call. Use sparingly and cache your results.</remarks>
		public static string ObjectPath(this Component component)
		{
			// create a list of objects from this one to the root
			List<GameObject> pathObjects = new List<GameObject>();
			Transform current = component.transform;
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
		public static bool ActiveInHierarchy(this MonoBehaviour mb) => mb.gameObject.activeInHierarchy;

		/// <summary>Lazy loads the main camera for saving runtime.</summary>
		private static Camera mainCamera;
		public static Camera GetMainCamera(this Component comp, bool forceRefresh = false)
		{
			if (mainCamera == null || forceRefresh)
			{ mainCamera = Camera.main; }
			return mainCamera;
		}
		#endregion // properties

		// Methods for handling coroutines and making delayed actions easier to use.
		#region coroutines
		private static readonly WaitForEndOfFrame frameWait = new WaitForEndOfFrame();
		/// <summary>Stops the given coroutine on the given monobehaviour</summary>
		/// <remarks>For use with checking if the coroutine is null or not.</remarks>
		public static void Stop(this Coroutine coroutine, MonoBehaviour parent) => parent.StopCoroutine(coroutine);
		private static IEnumerator ActionWhenPredicateCoroutine(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate)
		{
			if (actionToCall == null || predicate == null)
			{ yield break; }

			yield return new WaitUntil(predicate);

			actionToCall();
		}
		[ObsoleteAttribute("Use the other version of this method.", false)]
		public static Coroutine ActionWhenPredicate(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionWhenPredicateCoroutine(actionToCall, predicate)) : null; }
		/// <summary>Checks predicate every frame until predicate resolves to true and then calls action.</summary>
		/// <param name="mb">The Monobehaviour to run this from.</param>
		/// <param name="actionToCall">The action to call when predicate fulfilled.</param>
		/// <param name="predicate">The predicate to check for every frame.</param>
		public static Coroutine ActionWhenPredicate(this MonoBehaviour mb, Func<bool> predicate, Action actionToCall)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionWhenPredicateCoroutine(actionToCall, predicate)) : null; }
		private static IEnumerator ActionEachFrameForSecondsCoroutine(this MonoBehaviour mb, Action<float> actionToCall, float amountOfSecondsToTake)
		{
			if (actionToCall == null || amountOfSecondsToTake < 0)
			{ yield break; }

			float secondsCounter = 0f;

			while (secondsCounter < amountOfSecondsToTake)
			{
				actionToCall(secondsCounter);
				yield return null;
				secondsCounter += Time.deltaTime;
			}
			actionToCall(amountOfSecondsToTake);
		}

		[ObsoleteAttribute("Use the other version of this method.", false)]
		public static Coroutine ActionEachFrameForSeconds(this MonoBehaviour mb, Action<float> actionToCall, float amountOfSecondsToTake)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionEachFrameForSecondsCoroutine(actionToCall, amountOfSecondsToTake)) : null; }
		///  <summary>Calls the given action every frame for the given amount of seconds.
		/// 		The parameter will be the amount of seconds already passed since the start frame.</summary>
		///  <param name="mb">The MonoBehaviour to run this from.</param>
		///  <param name="actionToCall">The action to call every frame with the seconds count.</param>
		///  <param name="amountOfSecondsToTake">The time frame in which to call the action every frame.</param>
		public static Coroutine ActionEachFrameForSeconds(this MonoBehaviour mb, float amountOfSecondsToTake, Action<float> actionToCall)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionEachFrameForSecondsCoroutine(actionToCall, amountOfSecondsToTake)) : null; }
		private static IEnumerator ActionInSecondsCoroutine(this MonoBehaviour mb, Action actionToStart, float secondsToWait)
		{
			// if parameters are in error, abort
			if (actionToStart == null || secondsToWait < 0)
			{ yield break; }

			if (!secondsToWait.IsApproximately(0))
			{ yield return new WaitForSeconds(secondsToWait); }

			actionToStart();
		}

		[ObsoleteAttribute("Use the other version of this method.", false)]
		public static Coroutine ActionInSeconds(this MonoBehaviour mb, Action actionToStart, float secondsToWait)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionInSecondsCoroutine(actionToStart, secondsToWait)) : null; }
		/// <summary>Invoke the given action in the given amount of seconds</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToStart">The given action to invoke</param>
		/// <param name="secondsToWait">The amount to wait before action, in seconds</param>
		public static Coroutine ActionInSeconds(this MonoBehaviour mb, float secondsToWait, Action actionToStart)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionInSecondsCoroutine(actionToStart, secondsToWait)) : null; }
		private static IEnumerator ActionInFramesCoroutine(this MonoBehaviour mb, Action actionToStart, int framesToWait)
		{
			// if parameters are in error, abort
			if (actionToStart == null || framesToWait < 0)
			{ yield break; }

			for (int frameIndex = 0; frameIndex < framesToWait; frameIndex++)
			{ yield return null; }

			actionToStart();
		}

		[ObsoleteAttribute("Use the other version of this method.", false)]
		public static Coroutine ActionInFrames(this MonoBehaviour mb, Action actionToStart, int framesToWait)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionInFramesCoroutine(actionToStart, framesToWait)) : null; }
		/// <summary>Invoke the given action after the given amount of render frames</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToStart">The given action to invoke</param>
		/// <param name="framesToWait">The amount to wait before action, in frames</param>
		public static Coroutine ActionInFrames(this MonoBehaviour mb, int framesToWait, Action actionToStart)
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
				yield return null;
			}
		}

		[ObsoleteAttribute("Use the other version of this method.", false)]
		public static Coroutine ActionEachFrameWhilePredicate(this MonoBehaviour mb, Action actionToCall, Func<bool> predicate, int skipFrames = 0)
		{ return mb.ActiveInHierarchy() ? mb.StartCoroutine(mb.ActionEachFrameWhilePredicateCoroutine(actionToCall, predicate, skipFrames)) : null; }
		/// <summary>Invoke the given action each frame while predicate resolves to true.</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="actionToCall">The action to invoke.</param>
		/// <param name="predicate">The predicate to check.</param>
		/// <param name="skipFrames">The amount of frames to skip between actions.</param>
		public static Coroutine ActionEachFrameWhilePredicate(this MonoBehaviour mb, Func<bool> predicate, Action actionToCall, int skipFrames = 0)
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
			else { return mb.ActionWhenPredicate(()=> SceneManagement.SceneManager.GetActiveScene().isLoaded, action); }
			return null;
		}
		/// <summary>The time until a stalling sequence is aborted. (To save memory).</summary>
		public static float ActionSequenceTimeoutInSeconds = 60f;
		private static IEnumerator ActionSequenceCoroutine(this MonoBehaviour mb, params (Func<bool>, Action)[] sequence)
		{
			Func<bool> sequencePredicate;
			Action sequenceAction;
			float timeoutCounter = 0f;
			
			for (int sequenceIndex = 0; sequenceIndex < sequence.Length
									    && (sequencePredicate = sequence[sequenceIndex].Item1) is not null
										&& (sequenceAction = sequence[sequenceIndex].Item2) is not null; sequenceIndex++)
			{
				// yield return new WaitUntil(sequencePredicate);
				// if not time yet, advance the timeout counter and wait
				while (!sequencePredicate())
				{
					timeoutCounter += Time.deltaTime;
					// if it's time to timeout, break out.
					if (timeoutCounter > EngineExtensionMethods.ActionSequenceTimeoutInSeconds)
					{
						mb.Warn("Operation timed out after " + EngineExtensionMethods.ActionSequenceTimeoutInSeconds);
						yield break;
					}
					yield return null;
				}
				sequenceAction();
			}
		}
		/// <summary>Runs a sequence of actions in sequence after the action's predicate returns true.</summary>
		/// <param name="mb">The MonoBehaviour to run this from.</param>
		/// <param name="sequence">The sequence of tuples defining a predicate to run an action and the action to run.</param>
		/// <returns>The running coroutine (if you want to stop it manually)</returns>
		public static Coroutine ActionSequence(this MonoBehaviour mb, params (Func<bool>, Action)[] sequence)
		{
			if (sequence is not { Length: >0} || sequence[0].Item1 is null || sequence[0].Item2 is null)
			{
				mb.Warn("Sequence is null or empty.");
				return null;
			}

			return mb.StartCoroutine(mb.ActionSequenceCoroutine(sequence));
		}
		#endregion // coroutines

		// Methods for handling time.
		#region timing
		/// <summary>Get a string representing DateTime.Now replacing the delimiters with the ones specified.</summary>
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
		#endregion // timing

		// Various Log Methods to make logging easier and more functional.
		// They always include the path to the object and component and method from which they were called so they will be easy to find.
		#region logging
		private static Dictionary<string, double> timeTagsDictionary;

		/// <summary>Regular debug log.</summary>
		/// <param name="mb">The monobehaviour from which this was called.</param>
		/// <param name="logMessage">The message to display.</param>
		/// <param name="methodName">The name of the method in which this is used.</param>
		public static void Log(this MonoBehaviour mb, string logMessage,
							   [System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("{0}.{1}.{2}: {3}", mb.ObjectPath(), mb.GetType().Name, methodName, logMessage)); }
		/// <summary>Regular debug log.</summary>
		/// <param name="scriptableObject">The monobehaviour from which this was called.</param>
		/// <param name="logMessage">The message to display.</param>
		/// <param name="methodName">The name of the method in which this is used.</param>
		public static void Log(this ScriptableObject scriptableObject, string logMessage,
							   [System.Runtime.CompilerServices.CallerMemberName] string methodName = default)
		{ Debug.Log(string.Format("{0}.{1}: {2}", scriptableObject.GetType().Name, methodName, logMessage)); }
		
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

			mb.Log(string.Format("{0}; {1} (Time Since Epoch: {2}; Time Since Last Call (In milliseconds): {3})",
								 timeTag, debugMessage, EngineExtensionMethods.CurrentTimeInMilliseconds, timeSinceLastCall));
		}

		#if UNITY_EDITOR
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
		{ Debug.Log(string.Format("<color=cyan>{0}.{1}: {2}</color>",
								  scriptableObject.GetType().Name, methodName, debugMessage)); }
		#endif

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
		#endregion // logging
		#endregion // MonoBehaviour

		// Methods for handling childrens of objects
        #region children methods
		/// <summary>Get all the children objects of the given object.</summary>
		public static GameObject[] GetAllChildObjects(this GameObject gameObject)
		{
			GameObject[] objects = new GameObject[gameObject.transform.childCount];
			for (int objectIndex = 0; objectIndex < gameObject.transform.childCount; objectIndex++)
			{ objects[objectIndex] = gameObject.transform.GetChild(objectIndex).gameObject; }
			return objects;
		}

		/// <summary>Recursive method that searches all children of given object (included) to return the one with the given name.</summary>
		/// <exception cref="ArgumentException">If the given parameters are null.</exception>
		public static GameObject FindChildByName(this GameObject gameObject, string name)
		{
			// if bad parameters, abort
			if (gameObject == null) { throw new ArgumentException("GameObject can't be null.", nameof(gameObject)); }
			if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name can't be null or empty", nameof(name)); }

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
				// get the children in a list
				List<GameObject> childList = new List<GameObject>();
				for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
				{ childList.Add(transform.GetChild(childIndex).gameObject); }
				// unparent everything
				transform.DetachChildren();
				// destroy objects
				childList.ForEach(go => Object.Destroy(go));
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
					unnecessaryTransform.SetParent(null);
					removeThese.Add(unnecessaryTransform);
				}
				// destroy unnecessary objects
				removeThese.DoForEach(tform => { UnityEditor.EditorApplication.delayCall += () => Object.Destroy(tform.gameObject); });
				#endif
			}
		}
		#endregion // children methods
	}
}
