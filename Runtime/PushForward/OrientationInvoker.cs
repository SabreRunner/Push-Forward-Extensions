/*
	OrientationInvoker

	Description: Will register device orientation changes and report them through events.

	Created by: Eran "Sabre Runner" Arbel.
	Email: eran.arbel.ea@gmail.com
	Last Updated: 2018-08-23
*/

namespace PushForward
{
	using UnityEngine;
	using UnityEngine.Events;

	public class OrientationInvoker : BaseMonoBehaviour
	{
		#region fields
		[SerializeField] private UnityEvent onLandscape;
		[SerializeField] private UnityEvent onPortrait;

		private DeviceOrientation lastOrientation = DeviceOrientation.Unknown;
		#endregion

		[ContextMenu("OnLandscape")]
		private void OnLandscape()
		{
			this.onLandscape.Invoke();
		}

		[ContextMenu("OnPortrait")]
		private void OnPortrait()
		{
			this.onPortrait.Invoke();
		}

		/// <summary>Handle orientation changes.</summary>
		private void OrientationChanges()
		{
			if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
				//|| Input.deviceOrientation == DeviceOrientation.LandscapeRight)
			{ this.OnLandscape(); }
			else if (Input.deviceOrientation == DeviceOrientation.Portrait)
					//|| Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
			{ this.OnPortrait(); }
		}

		private void Update()
		{
			if (this.lastOrientation != Input.deviceOrientation)
			{
				this.OrientationChanges();
				this.lastOrientation = Input.deviceOrientation;
			}
		}

		private void OnEnable()
		{
			this.OrientationChanges();
		}
	}
}