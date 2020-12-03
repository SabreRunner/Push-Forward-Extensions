/*
	VideoPlayerExtender

	Description: Extends the VideoPlayer class.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-11-21
*/

namespace PushForward.Extenders
{
	using UnityEngine;
	using UnityEngine.Video;
	using PushForward.ExtensionMethods;

	[RequireComponent(typeof(VideoPlayer))]
	public class VideoPlayerExtender : BaseMonoBehaviour
	{
		[SerializeField] private VideoPlayer videoPlayer;
		[SerializeField] bool resetFrameOnEnable = true;
		[SerializeField] bool stopOnDisable = true;

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

		private void OnValidate()
		{
			this.videoPlayer = this.GetComponent<VideoPlayer>();
			this.videoLengthInSeconds = this.videoPlayer.frameCount / this.videoPlayer.frameRate;
		}

		private void OnEnable()
		{
			if (this.resetFrameOnEnable)
			{
				this.videoPlayer.Play();
				this.videoPlayer.Pause();
			}
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
		}
	}
}