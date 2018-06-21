using UnityEngine;
using MIDI;

[RequireComponent(typeof(PressInteraction))]
public class BasicPianoKey : MonoBehaviour
{
    [Tooltip("Defines the MIDI note that this key will produce.")]
    public Note note;

    /// <summary>
    ///     Will send MIDI signals to the open MIDI port.
    /// </summary>
    private MIDINoteSender midiSender;

    /// <summary>
    ///     Reference to the PressInteraction element of the key.
    /// </summary>
    private PressInteraction pressButton;

    /// <summary>
    ///     Useful to display some warnings only once among all keys.
    /// </summary>
    private static bool errorDisplayed = false;

    void Start()
    {
        pressButton = GetComponent<PressInteraction>();

        midiSender = new MIDINoteSender(note);
    }

    void Update()
    {
        if (pressButton.IsPressed())
        {
            if (!pressButton.IsActioned())
            {
				if (midiSender != null)
				{
	                midiSender.NoteOn();
                    pressButton.ActionPerformed();
				}
				else if (!errorDisplayed)
				{
					Debug.Log("BasicPianoKey: MIDI sender is null!");
					errorDisplayed = true;
				}
            }
        }
        else if (pressButton.WasJustReleased())         // (1)
        {
			if (midiSender != null)
			{
	            midiSender.NoteOff();
                pressButton.AcceptRelease();
			}
			else if (!errorDisplayed)
			{
				Debug.Log("BasicPianoKey: MIDI sender is null!");
				errorDisplayed = true;
			}
        }

        // Note: there are two exceptional cases to consider here:
        // Case 1 - the user presses a key and releases it immediately afterwards,
        // Case 2 - the user is holding a key pressed. He releases it and then presses it back again
        //          immediately afterwards, still holding on it.

        // Removing the else from the line marked (1), so that it looks like if(){} if(){}, would allow
        // to handle case 1, where the expected result would be to fire a note and shut it down
        // immediatly afterwards.
        // However, it would not handle properly case 2, because of the order of the if statements. The
        // expected result would be that a new note is fired and held, but the actual result would be
        // the following: a new note would be fired, adding up to the previous one that was being held
        // because it did not shut down, and then only both notes would immediately be shut down.

        // By adding an else at the beginning of line (1), such cases are "not seen" by the key.
        // The button does see them, but the key does not handle them, leaving the button in a 
        // valid state whatever happens.

        // In case 1, the key will behave like it wasn't pressed at all.
        // In case 2, the key will behave like it hasn't been released at all.

        // But this would really be a problem if a human was capable of releasing a key and pressing
        // it back in less than a frame, that is 1/60 second. Let's just suppose it won't happen. :)
    }
}
