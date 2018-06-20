using System.Threading;
using UnityEngine;

namespace MIDI
{
    public class MIDINoteSender
    {
        public Note baseNote
        {
			get; set;
        }

        public static MIDIWrapper w = null;

		private static bool errorDisplayed = false;

        public MIDINoteSender(Note n)
        {
            baseNote = n;
			init();
        }

        public MIDINoteSender()
        {
            baseNote = MIDI.Note.DefaultNote;
			init();
        }

		protected void init()
		{
			if (w == null)
			{
                w = MIDIWrapperAccess.w;
			}
		}

        public void NoteOn(Note note, byte velocity = 0x64)
        {
			if (w != null)
			{
	            w.NoteOn(note, velocity);
			}
			else if (!errorDisplayed)
			{
				Debug.Log("MIDINoteSender: MIDI wrapper is null!");
				errorDisplayed = true;
			}
        }

        public void NoteOff(Note note, byte velocity = 0x40)
        {
			if (w != null)
			{
	            w.NoteOff(note, velocity);
			}
			else if (!errorDisplayed)
			{
				Debug.Log("MIDINoteSender: MIDI wrapper is null!");
				errorDisplayed = true;
			}
        }

        public void NoteOnOff(Note note, byte velocity, int time)
        {
			if (w != null)
			{
	            w.NoteOnOff(note, velocity, time);
			}
			else if (!errorDisplayed)
			{
				Debug.Log("MIDINoteSender: MIDI wrapper is null!");
				errorDisplayed = true;
			}
        }

        public void Note(Note note, byte velocity, int time)
        {
            Thread thread = new Thread(() => NoteOnOff(note, velocity, time));
            thread.Start();
        }

        public void NoteOn(byte velocity = 0x64)
        {
            NoteOn(baseNote, velocity);
        }

        public void NoteOff(byte velocity = 0x40)
        {
            NoteOff(baseNote, velocity);
        }

        public void NoteOnOff(byte velocity, int time)
        {
            NoteOnOff(baseNote, velocity, time);
        }

        public void Note(byte velocity, int time)
        {
            Thread thread = new Thread(() => NoteOnOff(velocity, time));
            thread.Start();
        }
    }
}
