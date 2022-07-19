/*
	ApplicationPublicAccess

	Description: Open Unity Engine calls for inspector use.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
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
	public class ApplicationPublicAccess : MonoBehaviour
	{
		[SerializeField] private bool quitOnAndroidBackButton;
		[Tooltip("If set to (-1,-1) will not force.")]
		[SerializeField] private Vector2 forceResolution = -Vector2.one;

		//<summary>Find all the local ip addresses on the network.</summary>
		public static List<byte[]> LocalIPs
		{
			get
			{
				List<byte[]> bytes = new List<byte[]>();
				IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

				foreach (IPAddress ipAddress in host.AddressList)
				{
					if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
					{ bytes.Add(ipAddress.GetAddressBytes()); }
				}

				return bytes.Any() ? bytes : null;
			}
		}

		///<summary>Open an external browser with the given url.</summary>
		public void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		///<summary>Close the applicating within the given amount of seconds.</summary>
		public void Quit(float delayInSeconds = 0)
		{
			this.ActionInSeconds(() => Application.Quit(), delayInSeconds);
		}

		///<summary>Wait for escape key.</summary>
		// TODO: change to new InputSystem
		private void Update()
		{
			if (this.quitOnAndroidBackButton && Input.GetKey(KeyCode.Escape))
			{ this.Quit(); }
		}

		///<summary>Force resolution.<//summary>
		private void Awake()
		{
			if (!this.forceResolution.Equals(-Vector2.one))
			{ Screen.SetResolution((int)this.forceResolution.x, (int)this.forceResolution.y, Screen.fullScreenMode); }
		}
	}
}
