/*
	VideoPlayerExtender

	Description: Extends the VideoPlayer class.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-11-21
*/

using UnityEngine.Events;

namespace PushForward.Extenders
{
	using UnityEngine;
	using UnityEngine.Video;
	using ExtensionMethods;

	[RequireComponent(typeof(VideoPlayer))]
	public class VideoPlayerExtender : MonoBehaviour
	{
		[SerializeField] private VideoPlayer videoPlayer;
		[SerializeField] private bool playOnEnable = true;
		[SerializeField] private bool resetFrameOnEnable;
		[SerializeField] private bool stopOnDisable = true;
		[SerializeField] private UnityEvent eventOnEnd;

		private float videoLengthInSeconds;

		/// <summary>Seek to a specific second in the video.</summary>
		/// <param name="second">The second to seek to.</param>
		public void SeekToSecond(double second)
		{
			second = second.Clamp(0, this.videoLengthInSeconds);

			bool wasPlaying = this.videoPlayer.isPlaying;

			if (!wasPlaying)
			{ this.videoPlayer.Play(); }

			this.videoPlayer.time = second;

			if (!wasPlaying)
			{ this.videoPlayer.Pause();  }
		}

		/// <summary>Seek to a specific frame in the video.</summary>
		/// <param name="frame">The frame to seek to.</param>
		public void SeekToFrame(long frame)
		{
			frame = frame.Clamp(0, (long)this.videoPlayer.frameCount);

			bool wasPlaying = this.videoPlayer.isPlaying;

			if (!wasPlaying)
			{ this.videoPlayer.Play(); }

			this.videoPlayer.frame = frame;

			if (!wasPlaying)
			{ this.videoPlayer.Pause(); }
		}

		/// <summary>Seek to relative position.</summary>
		/// <param name="position">The relative position to seek to. Must be between 0 and 1 (inclusive)</param>
		public void SeekToPosition(float position)
		{
			position = position.Clamp01();

			this.SeekToFrame((long)(this.videoPlayer.frameCount * position));
		}

		[ContextMenu("PlayPause")]
		public void PlayPause()
		{
			if (this.videoPlayer.isPlaying)
			{ this.videoPlayer.Pause(); }
			else { this.videoPlayer.Play(); }
		}

		private void InvokeEndEvent(VideoPlayer vp)
		{
			this.eventOnEnd?.Invoke();
		}

		#region engine
		#if UNITY_EDITOR
		private void OnValidate()
		{
			this.videoPlayer = this.GetComponent<VideoPlayer>();
			this.videoLengthInSeconds = this.videoPlayer.frameCount / this.videoPlayer.frameRate;
		}
		#endif

		private void OnEnable()
		{
			if (this.playOnEnable || this.resetFrameOnEnable)
			{
				this.videoPlayer.Play();
				if (this.resetFrameOnEnable)
				{ this.videoPlayer.Pause(); }
			}

			this.videoPlayer.loopPointReached += this.InvokeEndEvent;
		}

		private void OnDisable()
		{
			if (this.stopOnDisable)
			{
				this.videoPlayer.Play();
				this.videoPlayer.frame = 0;
				this.videoPlayer.Pause();
				this.videoPlayer.Stop();
			}

			this.videoPlayer.loopPointReached -= this.InvokeEndEvent;
		}
		#endregion // engine
	}
}