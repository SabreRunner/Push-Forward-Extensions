/*
	GraphicExtender

	Description: Extends the Graphic inherited class.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-08-27
*/

namespace PushForward.Extenders
{
	#region using
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	#endregion

	public class GraphicExtender : MonoBehaviour
	{
		#region inspector fields
		[Tooltip("The graphic that requires fading.")]
		[SerializeField] private Graphic graphicToExtend;
		[Tooltip("Whether to fade the colour of the graphic.")]
		[SerializeField] private bool fadeColour = true;
		[Tooltip("Whether to fade the material of the graphic.")]
		[SerializeField] private bool fadeMaterial;
		[Tooltip("This event will trigger when the fade in is finished.")]
		[SerializeField] private UnityEvent triggerAtFadeInEnd;
		[Tooltip("This event will trigger when the fade out is finished.")]
		[SerializeField] private UnityEvent triggerAtFadeOutEnd;
		#endregion // inspector fields

		#region properties
		public bool Interactable
		{
			get => this.enabled;
			set => this.enabled = value;
		}

		public Color Colour
		{
			get => this.graphicToExtend.color;
			set => this.graphicToExtend.color = value;
		}

		public string ColourString
		{
			get => ColorUtility.ToHtmlStringRGBA(this.graphicToExtend.color);
			set
			{
				ColorUtility.TryParseHtmlString(value, out Color color);
				this.graphicToExtend.color = color;
			}
		}

		// ReSharper disable Unity.InefficientPropertyAccess
		/// <summary>This graphic's colour alpha.</summary>
		public float GraphicColourAlpha
		{
			get => this.graphicToExtend.color.a;
			set => this.graphicToExtend.color
					   = new Color(this.graphicToExtend.color[0], this.graphicToExtend.color[1], this.graphicToExtend.color[2], value);
		}

		/// <summary>This graphic's material alpha.</summary>
		public float GraphicMaterialAlpha
		{
			get => this.graphicToExtend.material.color.a;
			set => this.graphicToExtend.material.color = new Color(this.graphicToExtend.material.color[0],
																   this.graphicToExtend.material.color[1],
																   this.graphicToExtend.material.color[2], value);
		}
		// ReSharper restore Unity.InefficientPropertyAccess
		#endregion // properties

		#region private fields
		#endregion // private fields

		#region methods
		/// <summary>Fade in this graphic.</summary>
		/// <param name="secondsToFade">The amount of seconds to take to fade in.</param>
		public void FadeIn(float secondsToFade)
		{
			if (this.graphicToExtend != null)
			{
				this.ActionEachFrameForSeconds(seconds =>
												{
													float fraction = seconds / secondsToFade;
													if (this.fadeColour)
													{ this.GraphicColourAlpha = fraction; }
													if (this.fadeMaterial)
													{ this.GraphicMaterialAlpha = fraction; }
												}, secondsToFade);
				this.ActionInSeconds(this.triggerAtFadeInEnd.Invoke, secondsToFade);
			}
		}

		/// <summary>Fade out this graphic.</summary>
		/// <param name="secondsToFade">The amount of seconds to take to fade out.</param>
		public void FadeOut(float secondsToFade)
		{
			if (this.graphicToExtend != null)
			{
				this.ActionEachFrameForSeconds(seconds =>
												{
													float fraction = seconds / secondsToFade;
													if (this.fadeColour)
													{ this.GraphicColourAlpha = 1 - fraction; }
													if (this.fadeMaterial)
													{ this.GraphicMaterialAlpha = 1 - fraction; }
												}, secondsToFade);
				this.ActionInSeconds(this.triggerAtFadeOutEnd.Invoke, secondsToFade);
			}
		}
		#endregion // methods

		#region engine
		private void OnValidate()
		{
			if (this.graphicToExtend == null)
			{ this.graphicToExtend = this.GetComponent(typeof(Graphic)) as Graphic; }
		}
		#endregion // engine
	}
}
