namespace PushForward.Extenders
{
	using UnityEngine;

	public class TransformExtender : MonoBehaviour
	{
		#region position
		public float LocalPositionX
		{
			get => this.transform.localPosition.x;
			set => this.transform.localPosition = new Vector3(value, this.LocalPositionY, this.LocalPositionZ);
		}

		public float LocalPositionY
		{
			get => this.transform.localPosition.y;
			set => this.transform.localPosition = new Vector3(this.LocalPositionX, value, this.LocalPositionZ);
		}

		public float LocalPositionZ
		{
			get => this.transform.localPosition.z;
			set => this.transform.localPosition = new Vector3(this.LocalPositionX, this.LocalPositionY, value);
		}
		#endregion // position

		#region rotation
		public float LocalRotationX
		{
			get => this.transform.localRotation.eulerAngles.x;
			set => this.transform.localRotation = Quaternion.Euler(value, this.LocalRotationY, this.LocalRotationZ);
		}

		public float LocalRotationY
		{
			get => this.transform.localRotation.eulerAngles.y;
			set => this.transform.localRotation = Quaternion.Euler(this.LocalRotationX, value, this.LocalRotationZ);
		}

		public float LocalRotationZ
		{
			get => this.transform.localRotation.eulerAngles.z;
			set => this.transform.localRotation = Quaternion.Euler(this.LocalRotationX, this.LocalRotationY, value);
		}
		#endregion // rotation

		#region scale
		public float LocalScaleX
		{
			get => this.transform.localScale.x;
			set => this.transform.localScale = new Vector3(value, this.LocalScaleY, this.LocalScaleZ);
		}

		public float LocalScaleY
		{
			get => this.transform.localScale.y;
			set => this.transform.localScale = new Vector3(this.LocalScaleX, value, this.LocalScaleZ);
		}

		public float LocalScaleZ
		{
			get => this.transform.localScale.z;
			set => this.transform.localScale = new Vector3(this.LocalScaleX, this.LocalScaleY, value);
		}
		#endregion // scale
	}
}
