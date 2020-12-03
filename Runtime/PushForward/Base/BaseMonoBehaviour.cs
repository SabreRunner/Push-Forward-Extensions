/*
	BaseMonoBehaviour

	Description: A wrapper class for Unity Engine's MonoBehaviour class to add additional functionality at this level.
				 All new behaviours should inherit from this class.
	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2019-05-05 - Memory optimisations
*/

#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using PushForward.ExtensionMethods;
#endregion

/// <summary>Wrapper class for Unity Monobehaviour to inject more useful functionality.</summary>
public class BaseMonoBehaviour : MonoBehaviour
{
	#region properties
	private string objectPath;
	/// <summary>The path to this object through the hierarchy.</summary>
	public string ObjectPath
	{
		set { this.objectPath = value; }
		get
		{
			if (this.objectPath != null)
			{ return this.objectPath; }

			// create a list of objects from this one to the root
			List<GameObject> pathObjects = new List<GameObject>();
			Transform current = this.transform;
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

			// save the string
			this.objectPath = pathBuilder.ToString();

			return this.objectPath;
		}
	}
	public RectTransform RectTransform
	{ get { return (RectTransform)this.transform; } }
	#endregion // properties

	#region private fields
	private static Dictionary<string, double> m_timeTagsDictionary;
	#endregion

	#region object methods
	/// <summary>Access GameObject.activeSelf</summary>
	public bool ActiveSelf
	{ get { return this.gameObject.activeSelf; } }
	/// <summary>Access GameObject.activeInHierarchy.</summary>
	/// <remarks>Is the GameObject active in the scene?</remarks>
	public bool ActiveInHierarchy
	{ get { return this.gameObject.activeInHierarchy; } }
	/// <summary>Activate the monobehaviour's game object in hirearchy.</summary>
	public void Activate()
	{ this.gameObject.Activate(); }
	/// <summary>Deactivate the monobehaviour's game object in hirearchy.</summary>
	public void Deactivate()
	{ this.gameObject.Deactivate(); }
	/// <summary>Set the monobehaviour's game object active state in hirearchy.</summary>
	/// <param name="value">true for active and false for inactive.</param>
	public void SetActive(bool value)
	{ this.gameObject.SetActive(value); }
	/// <summary>Toggle the monobehaviour's game object's active state in the hirearchy.</summary>
	public void ToggleActive()
	{ this.SetActive(!this.ActiveSelf); }
	/// <summary>Add a given component to the game object.</summary>
	/// <param name="type">The type of component to add.</param>
	/// <returns>A reference to the new component.</returns>
	public Component AddComponent(Type type)
	{ return this.gameObject.AddComponent(type); }
	/// <summary>Destroys the monobehaviour instance.</summary>
	public void DestroyThisBehaviour()
	{ Object.Destroy(this); }
	public void DestroyThisObject()
	{ Object.Destroy(this.gameObject); }
	public void DestroyAllChildren()
	{ this.gameObject.DestroyAllChildren(); }

	private static Camera mainCamera;
	public Camera GetMainCamera(bool forceRefresh = false)
	{
		if (BaseMonoBehaviour.mainCamera == null || forceRefresh)
		{ BaseMonoBehaviour.mainCamera = Camera.main; }
		return BaseMonoBehaviour.mainCamera;
	}
	#endregion

	#region logging
	/// <summary>A debug log with a timed segment letting you know the time passed since the last call of the same tag.</summary>
	/// <param name="methodName">The name of the method in which this is used.</param>
	/// <param name="timeTag">The tag to use to group calls.</param>
	/// <param name="debugMessage">The message to display.</param>
	/// <param name="resetTimer">Wether to reset the timer when called.</param>
	protected void TimedLog(string methodName, string timeTag, string debugMessage, bool resetTimer = false)
	{
		double timeSinceLastCall;
        // if no dictionary, create one
		if (BaseMonoBehaviour.m_timeTagsDictionary == null)
		{ BaseMonoBehaviour.m_timeTagsDictionary = new Dictionary<string, double>(); }

        // no tag, make one and set last time to 0.
		if (!BaseMonoBehaviour.m_timeTagsDictionary.ContainsKey(timeTag) || resetTimer)
		{
		    timeSinceLastCall = 0;
		    BaseMonoBehaviour.m_timeTagsDictionary[timeTag] = this.CurrentTimeInMilliseconds;
		}
		else // with tag, get last call and replace it.
		{
		    timeSinceLastCall = this.CurrentTimeInMilliseconds - BaseMonoBehaviour.m_timeTagsDictionary[timeTag];
			BaseMonoBehaviour.m_timeTagsDictionary[timeTag] = this.CurrentTimeInMilliseconds;
		}

		this.Log(methodName, string.Format("{0}; {1} (Time Since Epoch: {2}; Time Since Last Call: {3})",
											timeTag, debugMessage, this.CurrentTimeInMilliseconds, timeSinceLastCall));
	}
	/// <summary>Temp logging is like debug but in green. Used for logs during actual debugging. Dispose of after use.</summary>
	/// <param name="methodName">The name of the method in which this is used.</param>
	/// <param name="debugMessage">The message to display.</param>
	protected void Temp(string methodName, string debugMessage)
	{ Debug.Log(string.Format("<color=green>{0}.{1}.{2}: {3}</color>", this.ObjectPath, this.GetType().Name, methodName, debugMessage)); }
	/// <summary>Regular debug log.</summary>
	/// <param name="methodName">The name of the method in which this is used.</param>
	/// <param name="logMessage">The message to display.</param>
	protected void Log(string methodName, string logMessage)
	{ Debug.Log(string.Format("{0}.{1}.{2}: {3}", this.ObjectPath, this.GetType().Name, methodName, logMessage)); }
	/// <summary>Warning log is marked with a yellow warning symbol.</summary>
	/// <param name="methodName">The name of the method in which this is used.</param>
	/// <param name="warningMessage">The message to display.</param>
	protected void Warn(string methodName, string warningMessage)
	{ Debug.LogWarning(string.Format("{0}.{1}.{2}: {3}", this.ObjectPath, this.GetType().Name, methodName, warningMessage)); }
	/// <summary>Error log is marked with a red error symbol.</summary>
	/// <param name="methodName">The name of the method in which this is used.</param>
	/// <param name="errorMessage">The message to display.</param>
	protected void Error(string methodName, string errorMessage)
	{ Debug.LogError(string.Format("{0}.{1}.{2}: {3}", this.ObjectPath, this.GetType().Name, methodName, errorMessage)); }
	#endregion

	#region coroutines
	private IEnumerator ActionWhenPredicateCoroutine(Action actionToCall, Func<bool> predicate)
	{
		if (actionToCall == null || predicate == null)
		{ yield break; }

		do { yield return new WaitForEndOfFrame(); }
		// ReSharper disable once LoopVariableIsNeverChangedInsideLoop - changed outside in the caller
		while (!predicate());

		actionToCall();
	}
	/// <summary>Checks predicate every frame until predicate resolves to true and then calls action.</summary>
	/// <param name="actionToCall">The action to call when predicate fulfilled.</param>
	/// <param name="predicate">The predicate to check for every frame.</param>
	protected Coroutine ActionWhenPredicate(Action actionToCall, Func<bool> predicate)
	{ return this.ActiveInHierarchy ? this.StartCoroutine(this.ActionWhenPredicateCoroutine(actionToCall, predicate)) : null; }
	private IEnumerator ActionEveryFrameForSecondsCoroutine(Action<float> actionToCall, float amountOfSecondsToTake)
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
	}
	/// <summary>Calls the given action every frame for the given amount of seconds.
	///		The parameter will be the amount of seconds already passed since the start frame.</summary>
	/// <param name="actionToCall">The action to call every frame with the seconds count.</param>
	/// <param name="amountOfSecondsToTake">The time frame in which to call the action every frame.</param>
	protected Coroutine ActionEachFrameForSeconds(Action<float> actionToCall, float amountOfSecondsToTake)
	{ return this.ActiveInHierarchy ? this.StartCoroutine(this.ActionEveryFrameForSecondsCoroutine(actionToCall, amountOfSecondsToTake)) : null; }
	private IEnumerator ActionInSecondsCoroutine(Action actionToStart, float secondsToWait)
	{
		// if parameters are in error, abort
		if (actionToStart == null || secondsToWait < 0)
		{ yield break; }

		if (!secondsToWait.FloatEqual(0))
		{ yield return new WaitForSeconds(secondsToWait); }

		actionToStart();
	}
	/// <summary>Invoke the given action in the given amount of seconds</summary>
	/// <param name="actionToStart">The given action to invoke</param>
	/// <param name="secondsToWait">The amount to wait before action, in seconds</param>
	protected Coroutine ActionInSeconds(Action actionToStart, float secondsToWait)
	{ return this.ActiveInHierarchy ? this.StartCoroutine(this.ActionInSecondsCoroutine(actionToStart, secondsToWait)) : null; }
	private IEnumerator ActionInFramesCoroutine(Action actionToStart, int framesToWait)
	{
		// if parameters are in error, abort
		if (actionToStart == null || framesToWait < 0)
		{ yield break; }

		for (int frameIndex = 0; frameIndex < framesToWait; frameIndex++)
		{ yield return new WaitForEndOfFrame(); }

		actionToStart();
	}
	/// <summary>Invoke the given action after the given amount of render frames</summary>
	/// <param name="actionToStart">The given action to invoke</param>
	/// <param name="framesToWait">The amount to wait before action, in frames</param>
	protected Coroutine ActionInFrames(Action actionToStart, int framesToWait)
	{ return this.ActiveInHierarchy ? this.StartCoroutine(this.ActionInFramesCoroutine(actionToStart, framesToWait)) : null; }
	private IEnumerator ActionEachFrameWhilePredicateCoroutine(Action actionToCall, Func<bool> predicate, int skipFrames)
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
			yield return new WaitForEndOfFrame();
		}
	}
	/// <summary>Invoke the given action each frame while predicate resolves to true.</summary>
	/// <param name="actionToCall">The action to invoke.</param>
	/// <param name="predicate">The predicate to check.</param>
	/// <param name="skipFrames">The amount of frames to skip between actions.</param>
	protected Coroutine ActionEachFrameWhilePredicate(Action actionToCall, Func<bool> predicate, int skipFrames = 0)
	{ return this.ActiveInHierarchy ? this.StartCoroutine(this.ActionEachFrameWhilePredicateCoroutine(actionToCall, predicate, skipFrames)) : null; }
	/// <summary>Does the action now if the scene is loaded or when it finishes loading.</summary>
	/// <param name="action">The action to take.</param>
	protected Coroutine ActionNowOrWhenSceneLoaded(Action action)
	{
		if (action == null)
		{ return null; }

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isLoaded)
		{ action(); }
		else { return this.ActionWhenPredicate(action, () => UnityEngine.SceneManagement.SceneManager.GetActiveScene().isLoaded); }
		return null;
	}
	#endregion

	#region engine
	// public bool canMove;
	/// <summary>Use this instead of validate to make sure it runs only in the Editor.</summary>
	public virtual bool OnValidateProperty(string propertyName)
	{
		return false;

		// if (propertyName == "canMove")
		// {
		//  	if (!m_CanMove)
		// 		{
		// 			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		// 			return true;
		// 		}
		// }
		//
		// return base.OnValidateProperty(propertyName);
	}

	/// <summary>Make sure the GC knows to collect all the referenced objects.</summary>
	private void ClearReferenceFields()
	{
		foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
		{
			Type fieldType = field.FieldType;

			if (typeof(IList).IsAssignableFrom(fieldType))
			{
				IList list = field.GetValue(this) as IList;
				list?.Clear();
			}

			if (typeof(IDictionary).IsAssignableFrom(fieldType))
			{
				IDictionary dictionary = field.GetValue(this) as IDictionary;
				dictionary?.Clear();
			}

			if (!fieldType.IsPrimitive)
			{ field.SetValue(this, null); }
		}
	}

	private void OnDestroy()
	{
		this.ClearReferenceFields();
	}
	#endregion // engine

	public string CurrentFullTimeIn24Hours(char dateDelimiter = '-', char timeDelimiter = ';',
											char milisecondsDelimiter = ',')
	{
		string standardOutput = DateTime.Now.ToString("u");
		standardOutput = standardOutput.Replace('-', dateDelimiter)
							.Replace(':', timeDelimiter).Replace('Z', milisecondsDelimiter);

		return standardOutput + DateTime.Now.Millisecond;
	}
	/// <summary>Milliseconds since epoch</summary>
	public double CurrentTimeInMilliseconds
	{ get { return DateTime.Now.Subtract(new DateTime(1970, 01, 01)).TotalMilliseconds; } }
}
