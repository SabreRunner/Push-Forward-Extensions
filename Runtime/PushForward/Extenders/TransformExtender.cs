using UnityEngine;

public class TransformExtender : MonoBehaviour
{
	#region position
	public float LocalPositionX
	{
		get { return this.transform.localPosition.x; }
		set { this.transform.localPosition = new Vector3(value, this.LocalPositionY, this.LocalPositionZ); }
	}

	public float LocalPositionY
	{
		get { return this.transform.localPosition.y; }
		set { this.transform.localPosition = new Vector3(this.LocalPositionX, value, this.LocalPositionZ); }
	}

	public float LocalPositionZ
	{
		get { return this.transform.localPosition.z; }
		set { this.transform.localPosition = new Vector3(this.LocalPositionX, this.LocalPositionY, value); }
	}
	#endregion // position

	#region rotation
	public float LocalRotationX
	{
		get { return this.transform.localRotation.eulerAngles.x; }
		set { this.transform.localRotation = Quaternion.Euler(value, this.LocalRotationY, this.LocalRotationZ); }
	}

	public float LocalRotationY
	{
		get { return this.transform.localRotation.eulerAngles.y; }
		set { this.transform.localRotation = Quaternion.Euler(this.LocalRotationX, value, this.LocalRotationZ); }
	}

	public float LocalRotationZ
	{
		get { return this.transform.localRotation.eulerAngles.z; }
		set { this.transform.localRotation = Quaternion.Euler(this.LocalRotationX, this.LocalRotationY, value); }
	}
	#endregion // rotation
}
