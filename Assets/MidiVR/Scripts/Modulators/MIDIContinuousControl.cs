using System.Collections;
using System.Collections.Generic;
using MIDI;
using UnityEngine;

public class MIDIContinuousControl : MonoBehaviour
{
    [SerializeField]
    private SlideInteraction knob;

    [SerializeField]
    private Direction direction;

    private static MIDIWrapper w = null;

    private static byte range = 127;

    private byte controllerID;

    private byte value;

	private static bool wrapperErrorDisplayed = false;

    private bool knobErrorDisplayed = false;

	// Use this for initialization
	void Start ()
    {
		if (w == null)
        {
            w = MIDIWrapperAccess.w;
        }

        controllerID = w.GetNewController();

        if (knob == null)
        {
            knob = GetComponent<SlideInteraction>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        byte tmp = 0;

        if (knob)
        {
            if (direction == Direction.X)
            {
                tmp = (byte)(knob.GetX() * range);
            }
            else if (direction == Direction.Y)
            {
                tmp = (byte)(knob.GetY() * range);
            }

            if (tmp != value)
            {
                value = tmp;
                CCMessage();
            }
        }
        else
        {
            if (!knobErrorDisplayed)
            {
                Debug.Log("No knob was assigned to MIDI CC handler" + name + "!");
            }
        }
    }

    void CCMessage()
    {
		if (w != null)
		{
	        w.ContinuousControl(controllerID, value);
		}
		else if (!wrapperErrorDisplayed)
		{
			Debug.Log("MIDIContinuousControl: MIDI wrapper is null!");
			wrapperErrorDisplayed = true;
		}
    }
}
