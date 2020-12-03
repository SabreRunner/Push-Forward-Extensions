using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>A remake of the regular button that, alternatively, allows different events for pressing down and letting go.</summary>
public class UpDownButton : Selectable, IPointerClickHandler, ISubmitHandler
{
    #region fields
    [SerializeField] private UnityEvent onClickEvent;
    [SerializeField] private UnityEvent downEvent;
    [SerializeField] private UnityEvent upEvent;

    /// <summary>
    ///     UnityEvent that is triggered when the button is pressed.
    ///     Note: Triggered on MouseUp after MouseDown on the same object.
    /// </summary>
    /// <example>
    ///     <code>
    ///  using UnityEngine;
    ///  using UnityEngine.UI;
    ///  using System.Collections;
    /// 
    ///  public class ClickExample : MonoBehaviour
    ///  {
    ///      public Button yourButton;
    /// 
    ///      void Start()
    ///      {
    ///          Button btn = yourButton.GetComponent<Button>
    ///             ();
    ///             btn.onClick.AddListener(TaskOnClick);
    ///             }
    ///             void TaskOnClick()
    ///             {
    ///             Debug.Log("You have clicked the button!");
    ///             }
    ///             }
    /// </code>
    /// </example>
    public UnityEvent OnClickEvent
    {
        get => this.onClickEvent;
        set => this.onClickEvent = value;
    }
    #endregion // fields

    private void Press()
    {
        if (!this.IsActive() || !this.IsInteractable())
        { return; }

        UISystemProfilerApi.AddMarker("UpDownButton.onClick", this);
        this.onClickEvent.Invoke();
    }

    /// <summary>
    ///     Call all registered IPointerClickHandlers.
    ///     Register button presses using the IPointerClickHandler. You can also use it to tell what type of click happened
    ///     (left, right etc.).
    ///     Make sure your Scene has an EventSystem.
    /// </summary>
    /// <param name="eventData">Pointer Data associated with the event. Typically by the event system.</param>
    /// <example>
    ///     <code>
    /// //Attatch this script to a Button GameObject
    /// using UnityEngine;
    /// using UnityEngine.EventSystems;
    /// 
    /// public class Example : MonoBehaviour, IPointerClickHandler
    /// {
    ///     //Detect if a click occurs
    ///     public void OnPointerClick(PointerEventData pointerEventData)
    ///     {
    ///             //Use this to tell when the user right-clicks on the Button
    ///         if (pointerEventData.button == PointerEventData.InputButton.Right)
    ///         {
    ///             //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
    ///             Debug.Log(name + " Game Object Right Clicked!");
    ///         }
    /// 
    ///         //Use this to tell when the user left-clicks on the Button
    ///         if (pointerEventData.button == PointerEventData.InputButton.Left)
    ///         {
    ///             Debug.Log(name + " Game Object Left Clicked!");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        { return; }

        this.Press();
    }

    /// <summary>
    ///     Call all registered ISubmitHandler.
    /// </summary>
    /// <param name="eventData">Associated data with the event. Typically by the event system.</param>
    /// <remarks>
    ///     This detects when a Button has been selected via a "submit" key you specify (default is the return key).
    ///     To change the submit key, either:
    ///     1. Go to Edit->Project Settings->Input.
    ///     2. Next, expand the Axes section and go to the Submit section if it exists.
    ///     3. If Submit doesn’t exist, add 1 number to the Size field. This creates a new section at the bottom. Expand the
    ///     new section and change the Name field to “Submit”.
    ///     4. Change the Positive Button field to the key you want (e.g. space).
    ///     Or:
    ///     1. Go to your EventSystem in your Project
    ///     2. Go to the Inspector window and change the Submit Button field to one of the sections in the Input Manager (e.g.
    ///     "Submit"), or create your own by naming it what you like, then following the next few steps.
    ///     3. Go to Edit->Project Settings->Input to get to the Input Manager.
    ///     4. Expand the Axes section in the Inspector window. Add 1 to the number in the Size field. This creates a new
    ///     section at the bottom.
    ///     5. Expand the new section and name it the same as the name you inserted in the Submit Button field in the
    ///     EventSystem. Set the Positive Button field to the key you want (e.g. space)
    /// </remarks>
    public virtual void OnSubmit(BaseEventData eventData)
    {
        this.Press();

        // if we get set disabled during the press
        // don't run the coroutine.
        if (!this.IsActive() || !this.IsInteractable())
        { return; }

        this.DoStateTransition(SelectionState.Pressed, false);
        this.StartCoroutine(this.OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        float fadeTime = this.colors.fadeDuration;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        this.DoStateTransition(this.currentSelectionState, false);
    }

    /// <summary>Reacts to the down press on the button.</summary>
    /// <param name="eventData">The pointer event data generated by the press.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!this.IsActive() || !this.IsInteractable())
        { return; }

        base.OnPointerDown(eventData);
        
        this.downEvent.Invoke();
    }

    /// <summary>Reacts to the up release off the button.</summary>
    /// <param name="eventData">The pointer event data generated by the release.</param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!this.IsActive() || !this.IsInteractable())
        { return; }

        base.OnPointerUp(eventData);
        
        this.upEvent.Invoke();
    }
}