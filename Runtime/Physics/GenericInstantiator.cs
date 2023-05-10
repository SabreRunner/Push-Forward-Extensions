/*
	GenericInstantiator

	Description: Simplifies (especially repetitive) instantiation of prefabs.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

using UnityEngine;

namespace PushForward.Physics
{
	using System;

	public class GenericInstantiator : MonoBehaviour
	{
		// ReSharper disable once MemberCanBePrivate.Global -- used in inspector
		public enum LineageEnum { Child, Sibling, Parent, Root }

		#region inspector fields
		#pragma warning disable IDE0044 // Add readonly modifier
		[Tooltip("The Prefab to instantiate.")]
		[SerializeField] protected GameObject prefab;
		[Tooltip("Where in the hierarchy to put the new object?")]
		[SerializeField] protected LineageEnum lineage;
		[Tooltip("What is the position offset to add?")]
		[SerializeField] protected Vector3 positionOffset = Vector3.zero;
		[Tooltip("What is the rotation offset to add?")]
		[SerializeField] protected Vector3 rotationOffset = Vector3.zero;
		#pragma warning restore IDE0044 // Add readonly modifier
		#endregion // inspector fields

		public GameObject Prefab
		{
			get => this.prefab;
			set => this.prefab = value;
		}

		protected Transform GetParentObject(Transform thisTransform)
		{
			return this.lineage switch
				{
					LineageEnum.Child => thisTransform,
					LineageEnum.Sibling => thisTransform.parent,
					LineageEnum.Parent => thisTransform.parent,
					LineageEnum.Root => null,
					_ => throw new ArgumentOutOfRangeException()
				};
		}

		/// <summary>Instantiates set prefab by parameters and returns it</summary>
		/// <returns>Reference to instantiated prefab.</returns>
		public GameObject Instantiate()
		{
			if (this.Prefab == null)
			{ return null; }

			// instantiate according to lineage
			Transform thisTransform = this.transform;
			GameObject newObject = Instantiate(this.prefab, this.GetParentObject(thisTransform));

			bool isRootObject = newObject.transform.parent == null;

			// add required offsets
			newObject.transform.localPosition += this.positionOffset + (isRootObject ? this.transform.position : Vector3.zero);
			newObject.transform.localRotation *= Quaternion.Euler(this.rotationOffset) * (isRootObject ? this.transform.rotation : Quaternion.identity);

			if (this.lineage == LineageEnum.Parent)
			{ thisTransform.SetParent(newObject.transform, true); }

			return newObject;
		}

		public GameObject InstantiateOtherDontReplace(GameObject otherPrefab)
		{
			GameObject tempSave = this.Prefab;
			this.Prefab = otherPrefab;
			GameObject instantiated = this.Instantiate();
			this.Prefab = tempSave;
			return instantiated;
		}

		public T Instantiate<T>() where T : Component
			=> this.Instantiate().GetComponent<T>();

		public T InstantiateOtherDontReplace<T>(GameObject otherPrefab) where T : Component
			=> this.InstantiateOtherDontReplace(otherPrefab).GetComponent<T>();
	}
}
