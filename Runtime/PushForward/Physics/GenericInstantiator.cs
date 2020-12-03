
namespace PushForward
{
	using UnityEngine;

	public class GenericInstantiator : BaseMonoBehaviour
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public enum Lineage { Child, Sibling, Parent, Root }

		#region inspector fields
		#pragma warning disable IDE0044 // Add readonly modifier
		[Tooltip("The Prefab to instantiate.")]
		[SerializeField] private GameObject prefab;
		[Tooltip("Where in the hierarchy to put the new object?")]
		[SerializeField] private Lineage lineage;
		[Tooltip("What is the position offset to add?")]
		[SerializeField] private Vector3 positionOffset = Vector3.zero;
		[Tooltip("What is the rotation offset to add?")]
		[SerializeField] private Vector3 rotationOffset = Vector3.zero;
		#pragma warning restore IDE0044 // Add readonly modifier
		#endregion // inspector fields

		public GameObject Prefab
		{
			get => this.prefab;
			set => this.prefab = value;
		}

		/// <summary>Instantiates set prefab by parameters and returns it</summary>
		/// <returns>Reference to instantiated prefab.</returns>
		public GameObject Instantiate()
		{
			if (this.Prefab == null)
			{ return null; }
			
			// instantiate according to lineage
			GameObject newObject = Instantiate(this.prefab,
											   this.lineage == Lineage.Child ? this.transform
												: this.lineage == Lineage.Sibling
													|| this.lineage == Lineage.Parent ? this.transform.parent : null);

			// add required offsets
			newObject.transform.localPosition += this.positionOffset
													+ (newObject.transform.parent == null ? this.transform.position : Vector3.zero);
			newObject.transform.localRotation *= Quaternion.Euler(this.rotationOffset)
													* (newObject.transform.parent == null ? this.transform.rotation : Quaternion.identity);
			return newObject;
		}

		public void InstantiateStatic()
		{
			this.Instantiate();
		}
		
		public GameObject InstantiateOtherDontReplace(GameObject otherPrefab)
		{
			GameObject tempSave = this.Prefab;
			this.Prefab = otherPrefab;
			GameObject instantiated = this.Instantiate();
			this.Prefab = tempSave;
			return instantiated;
		}

		public void InstantiateOtherDontReplaceStatic(GameObject otherPrefab)
		{
			this.InstantiateOtherDontReplace(otherPrefab);
		}
	}
}
