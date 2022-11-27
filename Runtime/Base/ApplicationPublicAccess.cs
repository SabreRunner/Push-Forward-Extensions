/*
	ApplicationPublicAccess

	Description: Open Unity Engine calls for inspector use.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2022-11-22
*/

#region using
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
#endregion //using

namespace PushForward.Base
{
	using System;

	public class ApplicationPublicAccess : SingletonBehaviour<ApplicationPublicAccess>
	{
		#if UNITY_ANDROID
		[SerializeField] private bool quitOnAndroidBackButton;
		#endif
		[Tooltip("If set to (-1,-1) will not force.")]
		[SerializeField] private Vector2 forceResolution = -Vector2.one;

		public event Action<List<byte[]>> LocalIPsEvent;
		//<summary>Find all the local ip addresses on the network.</summary>
		public List<byte[]> GetLocalIPs()
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			List<byte[]> bytes
				= new(host.AddressList.Where(address => address.AddressFamily is AddressFamily.InterNetwork or AddressFamily.InterNetworkV6)
						  .Select(address => address.GetAddressBytes()));

			ApplicationPublicAccess.Instance.LocalIPsEvent?.Invoke(bytes.Any() ? bytes : null);
			return bytes.Any() ? bytes : null;
		}

		///<summary>Open an external browser with the given url.</summary>
		public void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		///<summary>Close the application within the given amount of seconds.</summary>
		public void Quit(float delayInSeconds = 0) => this.ActionInSeconds(delayInSeconds, Application.Quit);

		///<summary>Wait for escape key.</summary>
		// TODO: change to new InputSystem
		private void Update()
		{
			#if UNITY_ANDROID
			if (this.quitOnAndroidBackButton && Input.GetKey(KeyCode.Escape))
			{ this.Quit(); }
			#endif
		}

		///<summary>Force resolution.</summary>
		private void Awake()
		{
			if (!this.forceResolution.Equals(-Vector2.one))
			{ Screen.SetResolution((int)this.forceResolution.x, (int)this.forceResolution.y, Screen.fullScreenMode); }
		}
	}
}
