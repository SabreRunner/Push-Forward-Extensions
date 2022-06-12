/*
 * RaycastHitter
 *
 * Description: Raycasts on command and outputs hit data.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2018-11-08
*/

namespace PushForward.Physics
{
	using System.Text;
	using Base;
	using UnityEngine.Events;
	using UnityEngine;

	public class RaycastHitter : MonoBehaviour
	{
		#region inspector fields
		[Tooltip("How far to draw the ray.")]
		[SerializeField] private float maxDistance = 10f;
		[Header("Return Info on Raycast Hit.")]
		[SerializeField] private UnityEvent hitNothingEvent;
		[SerializeField] private HitInfoEvent hitInfoEvent;
		[SerializeField] private TransformEvent transformHitEvent;
		[SerializeField] private FloatEvent hitDistanceEvent;
		[SerializeField] private Vector3Event hitWorldPositionEvent;
		[SerializeField] private StringEvent hitInfoStringEvent;
		#endregion // inspector fields

		private Ray TargetingRay => new Ray(this.transform.position, this.transform.forward * this.maxDistance);

		public void Raycast()
		{
			// this.Temp("Casting from " + this.transform.position + " towards " + this.transform.forward
					  // + " for " + this.maxDistance + " meters.");
			if (UnityEngine.Physics.Raycast(this.TargetingRay, out RaycastHit hitInfo, this.maxDistance))
			{
				StringBuilder hitInfoSB = new StringBuilder("Hit.")
					.Append("\nTarget: ").Append(hitInfo.transform.gameObject.name)
					.Append("\nPosition: ").Append(hitInfo.point.StringRepresentation())
					.Append("\nDistance: ").Append(hitInfo.distance)
					.Append("\nProjected from: ").Append(this.transform.position.StringRepresentation())
					.Append("\nAt Direction: ").Append(this.transform.forward.StringRepresentation());
				// this.Temp("Hit: " + hitInfoSB);
				this.hitInfoEvent?.Invoke(hitInfo);
				this.transformHitEvent?.Invoke(hitInfo.transform);
				this.hitDistanceEvent?.Invoke(hitInfo.distance);
				this.hitWorldPositionEvent?.Invoke(hitInfo.point);
				this.hitInfoStringEvent?.Invoke(hitInfoSB.ToString());
			}
			else { this.hitNothingEvent?.Invoke(); }
		}

		#if DEBUG
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(this.TargetingRay);
		}
		#endif
	}
}
