/*
	TextExtender

	Description: Extends the standard UI.Text element.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-08-27
*/

namespace PushForward.Extenders
{
	using UnityEngine;
	using UnityEngine.UI;

	[RequireComponent(typeof(Text))]
	public class TextExtender : BaseMonoBehaviour
	{
		[SerializeField] private Text text;
		[SerializeField] private int decimalAccuracy = 4;

		public void SetText(string str)
		{ this.text.text = str; }

		public void SetInteger(int integer)
		{ this.text.text = integer.ToString(); }

		public void SetFloat(float fl)
		{ this.text.text = fl.ToString($"N{decimalAccuracy}"); }

		public void SetVector3(Vector3 vec3)
		{ this.text.text = vec3.StringRepresentation(decimalAccuracy); }

		public void SetVector2(Vector2 vec2)
		{ this.text.text = vec2.StringRepresentation(decimalAccuracy); }

		private void OnValidate()
		{
			if (this.text == null)
			{ this.text = this.GetComponent<Text>(); }
		}
	}
}