/*
 * UnityEvents
 *
 * Description: A collection of useful unity events.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2018-11-15
*/

namespace PushForward.Base
{
	#region using
	using System;
	using UnityEngine;
	using UnityEngine.Events;
	#endregion //using

	[Serializable] public class BoolEvent : UnityEvent<bool> { }
	[Serializable] public class StringEvent : UnityEvent<string> { }
	[Serializable] public class IntEvent : UnityEvent<int> { }
	[Serializable] public class FloatEvent : UnityEvent<float> { }
	[Serializable] public class Vector2Event : UnityEvent<Vector2> { }
	[Serializable] public class Vector3Event : UnityEvent<Vector3> { }
	[Serializable] public class HitInfoEvent : UnityEvent<RaycastHit> { }
	[Serializable] public class TransformEvent : UnityEvent<Transform> { }
	[Serializable] public class TimeSpanEvent : UnityEvent<TimeSpan> { }
	[Serializable] public class PrefabEvent : UnityEvent<GameObject> { }
}
