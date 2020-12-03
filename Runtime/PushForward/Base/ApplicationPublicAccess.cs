
namespace PushForward
{
	using UnityEngine;

	public class ApplicationPublicAccess : BaseMonoBehaviour
	{
		[SerializeField] private bool quitOnAndroidBackButton;
		[Tooltip("If set to (-1,-1) will not force.")]
		[SerializeField] private Vector2 forceResolution = -Vector2.one;

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
