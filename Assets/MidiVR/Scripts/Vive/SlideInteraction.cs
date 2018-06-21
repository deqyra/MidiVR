using System;
using UnityEngine;

public class SlideInteraction : MidiVRInteraction
{
    /// <summary>
    ///     Parent fader body object.
    /// </summary>
    [Tooltip("SurfaceBody object this slider should be attached to.")]
    [SerializeField]
    private SliderBody body;

    /// <summary>
    ///     External surface knob used to control the X dimension of this knob.
    /// </summary>
    public SlideInteraction xControl = null;

    /// <summary>
    ///     External surface knob used to control the Y dimension of this knob.
    /// </summary>
    public SlideInteraction yControl = null;

    /// <summary>
    ///     One-way output linear mapping for X axis [0;1].
    /// </summary>
    private float xValue;

    /// <summary>
    ///     One-way output linear mapping for Y axis [0;1].
    /// </summary>
    private float yValue;

    [SerializeField]
    /// <summary>
    ///     The initial position of the knob [0;1]².
    /// </summary>
    private Vector2 initialPosition;

    /// <summary>
    ///     Defines wether the object is being grabbed by the controller or not.
    /// </summary>
    private bool pressed = false;

    /// <summary>
    ///     The minimum x position the knob can take, relative to the modulator body.
    /// </summary>
    private float minX;

    /// <summary>
    ///     The maximum x position the knob can take, relative to the modulator body.
    /// </summary>
    private float maxX;

    /// <summary>
    ///     The minimum y position the knob can take, relative to the modulator body.
    /// </summary>
    private float minY;

    /// <summary>
    ///     The maximum y position the knob can take, relative to the modulator body.
    /// </summary>
    private float maxY;

    /// <summary>
    ///     The x position of a grabbing hand, relative to the modulator body.
    /// </summary>
    private float handX;

    /// <summary>
    ///     The y position of a grabbing hand, relative to the modulator body.
    /// </summary>
    private float handY;

    /// <summary>
    ///     The hand currently grabbing the slider.
    /// </summary>
    private ViveHand grabbingHand = null;

    /// <summary>
    ///     Useful not to spam the debug log with warnings.
    /// </summary>
    private bool debugDisplayed = false;

    public override void StartInteract(ViveHand hand)
    {
        interacted = true;
        grabbingHand = hand;
        pressed = true;
    }

    public override void StartInteractDelayed(ViveHand hand)
    {

    }

    public override void StopInteract(ViveHand hand)
    {
        interacted = false;
        grabbingHand = null;
        pressed = false;
    }

    // Use this for initialization
    void Start()
    {
        interactionType = InteractionType.Slide;

        minX = body.GetMinXKnobPosition();
        maxX = body.GetMaxXKnobPosition();
        minY = body.GetMinYKnobPosition();
        maxY = body.GetMaxYKnobPosition();

        xValue = initialPosition.x;
        yValue = initialPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbingHand)
        {
            UpdateHandPosition(grabbingHand);
        }

        bool updated = false;

        // Retrieve controls coordinates first, and set the knob to their position.
        if (xControl != null)
        {
            float tmp = xValue;
            xValue = xControl.GetX();

            if (tmp != xValue)
            {
                updated = true;
            }
        }
        if (yControl != null)
        {
            float tmp = yValue;
            yValue = yControl.GetY();

            if (tmp != yValue)
            {
                updated = true;
            }
        }

        // Only then set the knob to the hand position (priority is hand > controls).
        if (pressed)
        {
            if (handX > maxX)
            {
                xValue = 1;
            }
            else if (handX < minX)
            {
                xValue = 0;
            }
            else
            {
                xValue = (handX - minX) / (maxX - minX);
            }

            if (handY > maxY)
            {
                yValue = 1;
            }
            else if (handY < minY)
            {
                yValue = 0;
            }
            else
            {
                yValue = (handY - minY) / (maxY - minY);
            }

            updated = true;
        }

        if (updated)
        {
            SlideTo(xValue, yValue);
        }
    }

    /// <summary>
    ///     Returns xValue.
    /// </summary>
    public float GetX()
    {
        return xValue;
    }

    /// <summary>
    ///     Returns yValue.
    /// </summary>
    public float GetY()
    {
        return yValue;
    }

    /// <summary>
    ///     Set the knob to position (valueX, valueY) on the surface.
    /// </summary>
    private void SlideTo(float valueX, float valueY)
    {
        Vector3 pos = transform.localPosition;
        pos.x = minX + ((maxX - minX) * valueX);
        pos.y = minY + ((maxY - minY) * valueY);
        transform.localPosition = pos;
    }

    /// <summary>
    ///     Set the knob to position (pos.x, pos.y) on the surface.
    /// </summary>
    private void SlideTo(Vector2 pos)
    {
        SlideTo(pos.x, pos.y);
    }


    private void UpdateHandPosition(ViveHand hand)
    {
        if (body)
        {
            handX = body.transform.InverseTransformPoint(hand.transform.position).x;
            handY = body.transform.InverseTransformPoint(hand.transform.position).y;
        }
        else
        {
            if (!debugDisplayed)
            {
                Debug.Log("No SliderBody was registered on slider " + name + "!");
                debugDisplayed = true;
            }
        }
    }
}
