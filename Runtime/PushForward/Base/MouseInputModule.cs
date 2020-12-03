
namespace PushForward.Base
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class MouseInputModule : BaseMonoBehaviour//, IBeginDragHandler, IEndDragHandler
	{
		#region fields
		[Range(10f, 1000f)] [SerializeField] private float swipeThreshold;
		[SerializeField] private Vector2Event swipeEvent;

		private Vector3 mouseDragStartPosition = -Vector2.one;
		#endregion // fields

		/*
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.Temp("OnBeginDrag", "Start position: " + eventData.position);

			this.mouseDragStartPosition = eventData.position;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			this.Temp("OnEndDrag", "End position: " + eventData.position);

			Vector2 drag = eventData.position - (Vector2)this.mouseDragStartPosition;
			this.swipeEvent.Invoke(drag);
		}

		private void OnMouseDown()
		{
			this.Temp("OnMouseDown", "Starting position: " + Input.mousePosition.StringRepresentation(3));

			this.mouseDragStartPosition = Input.mousePosition;
		}

		private void OnMouseUp()
		{
			this.Temp("OnMouseUp", "Resetting mouse drag start.");

			Vector2 drag = Input.mousePosition - this.mouseDragStartPosition;
			this.swipeEvent.Invoke(drag);
		}
		*/

		private void CheckForTouchInput()
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{ this.mouseDragStartPosition = Input.GetTouch(0).position; }

			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				Vector2 drag = Input.GetTouch(0).position - (Vector2)this.mouseDragStartPosition;
				this.Temp("CheckForTouchInput", "Drag: " + drag.magnitude + "Touch Delta: " + Input.GetTouch(0).deltaPosition.magnitude);

				if (drag.magnitude > this.swipeThreshold)
				{ this.swipeEvent.Invoke(drag); }
			}
		}

		private void CheckForMouseInput()
		{
			if (Input.GetMouseButtonDown(0))
			{ this.mouseDragStartPosition = Input.mousePosition; }

			if (Input.GetMouseButtonUp(0))
			{
				Vector2 drag = Input.mousePosition - this.mouseDragStartPosition;

				this.Temp("CheckForMouseInput", "Drag: " + drag.magnitude);

				if (drag.magnitude > this.swipeThreshold)
				{ this.swipeEvent.Invoke(drag); }
			}
		}

		private void Update()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			this.CheckForMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
			this.CheckForTouchInput();
#endif
		}
	}
}