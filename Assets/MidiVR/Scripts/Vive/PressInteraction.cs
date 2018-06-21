using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class PressInteraction : MidiVRInteraction
{
    [SerializeField]
    [Tooltip("Defines how fast the button moves up and down.")]
    private float movementSpeed = .01f;

    [SerializeField]
    [Tooltip("Defines how far the button moves down from its original poisiton when pressed to maximum.")]
    private float maxPressDistance = .1f;

    [SerializeField]
    [Tooltip("In which direction the button should animate when pressed.")]
    private Direction direction = Direction.X;

    [SerializeField]
    [Tooltip("Whether the button should move towards positive or positive values.")]
    private bool reverse = false;

    /// <summary>
    ///     Defines whether the object is pressed by the controller or not.
    /// </summary>
    private bool pressed = false;

    /// <summary>
    ///     A button is being pressed to perform an action. We don't want to
    ///     perform that action twice, so here's how to keep track of it.
    /// </summary>
    private bool action = false;

    /// <summary>
    ///     Defines whether the action is toggled back to
    ///     false when the button is being released.
    /// </summary>
    private bool actionResets = true;

    /// <summary>
    ///     Indicates whether the button was just released (so that an additional action may be performed).
    /// </summary>
    private bool justReleased = false;

    /// <summary>
    ///     The target position to reach when moving.
    /// </summary>
    private float targetPos;

    /// <summary>
    ///     The minimum target position.
    /// </summary>
    private float minPos;

    /// <summary>
    ///     The maximum target position.
    /// </summary>
    private float maxPos;

    /// <summary>
    ///     Indicates whether the key should be moving (useful for updating position).
    /// </summary>
    private bool moving = false;

    void Start()
    {
        interactionType = InteractionType.Press;

        switch (direction)
        {
            case Direction.X:
                maxPos = transform.localPosition.x;
                minPos = transform.localPosition.x;
                break;
            case Direction.Y:
                maxPos = transform.localPosition.y;
                minPos = transform.localPosition.y;
                break;
            case Direction.Z:
                maxPos = transform.localPosition.z;
                minPos = transform.localPosition.z;
                break;
        }

        if (!reverse)
        {
            minPos += maxPressDistance;
        }
        else
        {
            minPos -= maxPressDistance;
        }
    }

    public override void StartInteract(ViveHand hand)
    {
        interacted = true;
        Press();
    }

    public override void StartInteractDelayed(ViveHand hand)
    {
        StartInteract(hand);
    }

    public override void StopInteract(ViveHand hand)
    {
        interacted = false;
        Unpress();
    }

    /// <summary>
    ///     Sets the button state to true and starts moving it towards its min position.
    /// </summary>
    public void Press()
    {
        pressed = true;
        targetPos = minPos;
        moving = true;
    }

    /// <summary>
    ///     Sets the button state to false and starts moving it towards its max position.
    /// </summary>
    public void Unpress()
    {
        targetPos = maxPos;
        moving = true;
        pressed = false;
        if (actionResets)
        {
            action = false;
        }
        justReleased = true;
    }

    /// <summary>
    ///     Actually moves the button associated transform.
    /// </summary>
    void Update()
    {
        if (moving)
        {
            float currentPos = 0;
            float movementVal = 0;

            switch (direction)
            {
                case Direction.X:
                    currentPos = transform.localPosition.x;
                    break;
                case Direction.Y:
                    currentPos = transform.localPosition.y;
                    break;
                case Direction.Z:
                    currentPos = transform.localPosition.z;
                    break;
            }

            if (Mathf.Abs(targetPos - currentPos) < movementSpeed)
            {
                movementVal = targetPos - currentPos;
                moving = false;
            }
            else if (currentPos > targetPos)
            {
                movementVal = -movementSpeed;
            }
            else
            {
                movementVal = movementSpeed;
            }

            switch (direction)
            {
                case Direction.X:
                    transform.Translate(movementVal, 0f, 0f);
                    break;
                case Direction.Y:
                    transform.Translate(0f, movementVal, 0f);
                    break;
                case Direction.Z:
                    transform.Translate(0f, 0f, movementVal);
                    break;
            }
        }
    }

    /// <summary>
    ///     Tells whether the button is currently pressed.
    /// </summary>
    public bool IsPressed()
    {
        return pressed;
    }

    /// <summary>
    ///     Tells whether the action associated to the button was already fired.
    ///     It is the job of the managing object to call ActionPerformed() in order
    ///     to validate the "action" state of the button.
    ///     ActionCancelled() may be called any time in order to invalidate the action
    ///     state of the button, thus allowing to perfom a second action.
    /// </summary>
    public bool IsActioned()
    {
        return action;
    }

    /// <summary>
    ///     When called, it means the action associated to the button has been performed once
    ///     by the managing object. When the managing object checks again on IsActioned(),
    ///     it will see 'false' and therefore know that it shouldn't perform a second action.
    /// </summary>
    public void ActionPerformed()
    {
        action = true;
    }

    /// <summary>
    ///     Invalidates the action of the button, which would allow to
    ///     perform a second action in special circumstances.
    /// </summary>
    public void ActionCancelled()
    {
        action = false;
    }

    /// <summary>
    ///     Tells whether the button was just released (with no release-associated action performed yet).
    ///     It is the job of the managing object to call AcceptRelease() in order to validate the
    ///     "release" state of the button (thus avoiding several same release action to be perfomed).
    /// </summary>
    public bool WasJustReleased()
    {
        return justReleased;
    }

    /// <summary>
    ///     When called, it means the action associated to the button release has been performed once
    ///     by the managing object. When the managing object checks again on WasJustReleased(),
    ///     it will see 'false' and therefore know that it shouldn't perform a second action.
    /// </summary>
    public void AcceptRelease()
    {
        justReleased = false;
    }
}
