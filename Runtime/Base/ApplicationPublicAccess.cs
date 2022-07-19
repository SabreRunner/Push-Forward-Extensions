/*
	ApplicationPublicAccess

	Description: Open Unity Engine calls for inspector use.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace PushForward.Base
{
	public class ApplicationPublicAccess : MonoBehaviour
	{
		[SerializeField] private bool quitOnAndroidBackButton;
		[Tooltip("If set to (-1,-1) will not force.")]
		[SerializeField] private Vector2 forceResolution = -Vector2.one;

		public static List<byte[]> LocalIPs
		{
			get
			{
				List<byte[]> bytes = new List<byte[]>();
				IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (IPAddress ipAddress in host.AddressList)
				{
					if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
					{ bytes.Add(ipAddress.GetAddressBytes()); }
				}

				return bytes.Any() ? bytes : null;
			}
		}

		public void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		public void Quit(float delayInSeconds = 0)
		{
			this.ActionInSeconds(() => Application.Quit(), delayInSeconds);
		}

		private void Update()
		{
			if (this.quitOnAndroidBackButton && Input.GetKey(KeyCode.Escape))
			{ this.Quit(); }
		}

		private void Awake()
		{
			if (!this.forceResolution.Equals(-Vector2.one))
			{ Screen.SetResolution((int)this.forceResolution.x, (int)this.forceResolution.y, Screen.fullScreenMode); }
		}
	}
}
