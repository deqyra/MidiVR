using System;
using UnityEngine;

class SliderBody : MonoBehaviour
{
    /// <summary>
    ///     The maximum x position knobs can take.
    /// </summary>
    [Tooltip("The maximum x position knobs can take.")]
    [SerializeField]
    private float maxXKnobPosition;

    /// <summary>
    ///     The minimum y position knobs can take.
    /// </summary>
    [Tooltip("The minimum X position knobs can take.")]
    [SerializeField]
    private float minXKnobPosition;

    /// <summary>
    ///     The maximum y position knobs can take.
    /// </summary>
    [Tooltip("The maximum y position knobs can take.")]
    [SerializeField]
    private float maxYKnobPosition;

    /// <summary>
    ///     The minimum y position knobs can take.
    /// </summary>
    [Tooltip("The minimum y position knobs can take.")]
    [SerializeField]
    private float minYKnobPosition;

    /// <summary>
    ///     Surface knob children object.
    /// </summary>
    [Tooltip("Attached SurfaceKnobs objects.")]
    [SerializeField]
    private SlideInteraction[] knobs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///     Returns xValue from knob i.
    /// </summary>
    public float KnobGetX(int i)
    {
        return knobs[i].GetX();
    }

    /// <summary>
    ///     Returns yValue from knob i.
    /// </summary>
    public float KnobGetY(int i)
    {
        return knobs[i].GetY();
    }

    /// <summary>
    ///     Returns the minimum X position knobs can take.
    /// </summary>
    public float GetMinXKnobPosition()
    {
        return minXKnobPosition;
    }

    /// <summary>
    ///     Returns the minimum X position knobs can take.
    /// </summary>
    public float GetMinYKnobPosition()
    {
        return minYKnobPosition;
    }

    /// <summary>
    ///     Returns the minimum X position knobs can take.
    /// </summary>
    public float GetMaxXKnobPosition()
    {
        return maxXKnobPosition;
    }

    /// <summary>
    ///     Returns the minimum X position knobs can take.
    /// </summary>
    public float GetMaxYKnobPosition()
    {
        return maxYKnobPosition;
    }
}
