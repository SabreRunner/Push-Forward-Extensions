/*
	NoteTaker

	Description: This is just a tiny script to allow you to put notes on objects. It's just a string.
	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-09-06
*/

using UnityEngine;

public class NoteTaker : BaseMonoBehaviour
{
#pragma warning disable IDE0044 // Add readonly modifier
	[Multiline(10), Tooltip("Put notes on the object here."), SerializeField] private string notes = null;
#pragma warning restore IDE0044 // Add readonly modifier

	private void OnValidate()
	{
		this.notes = this.notes.Trim();
	}
}
