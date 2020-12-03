/*
 * RaycastHitter
 *
 * Description: Raycasts on command and outputs hit data.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2018-11-08
*/

namespace PushForward
{
	using UnityEngine;
	using PushForward.Base;

	public class RaycastHitter : BaseMonoBehaviour
	{
		[Tooltip("How far to draw the ray.")]
		[SerializeField] private float maxDistance = 10f;
		[Header("Return Info on Raycast Hit.")]
		[SerializeField] private HitInfoEvent hitInfoEvent;
		[SerializeField] private TransformEvent transformHitEvent;
		[SerializeField] private FloatEvent hitDistanceEvent;
		[SerializeField] private Vector3Event hitWorldPositionEvent;
		[SerializeField] private StringEvent hitInfoStringEvent;

		public void Raycast()
		{
			RaycastHit hitInfo;

			if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, this.maxDistance))
			{
				this.hitInfoEvent?.Invoke(hitInfo);
				this.transformHitEvent?.Invoke(hitInfo.transform);
				this.hitDistanceEvent?.Invoke(hitInfo.distance);
				this.hitWorldPositionEvent?.Invoke(hitInfo.point);
				this.hitInfoStringEvent?.Invoke("Hit." +
												"\nTarget: " + hitInfo.transform.gameObject.name +
												"\nPosition: " + hitInfo.point.StringRepresentation() +
												"\nDistance: " + hitInfo.distance +
												"\nProjected from: " + this.transform.position.StringRepresentation() +
												"\nAt Direction: " + this.transform.forward.StringRepresentation());
			}
		}
	}
}
