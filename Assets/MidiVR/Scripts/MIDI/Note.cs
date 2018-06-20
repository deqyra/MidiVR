using System;
using UnityEngine;

namespace MIDI
{
    [Serializable]
    public class Note
    {
        [Tooltip("The label of the note.")]
        public NoteLabel Label;

        [Tooltip("The octave of the note.")]
        [Range(0f, 10f)]
        public byte Octave;

        public static readonly Note DefaultNote = new Note(NoteLabel.C, 5);

        public static readonly byte OctaveLength = (byte) Enum.GetNames(typeof(NoteLabel)).Length;

        public Note(NoteLabel note, byte octave)
        {
            Label = note;
            Octave = octave;
        }

        public Note PitchUp(int semitones)
        {
            return FromInt((byte)(ToInt() + semitones));
        }

        public Note PitchDown(int semitones)
        {
            return FromInt((byte)(ToInt() - semitones));
        }

        public Note Pitch(int semitones)
        {
            return FromInt((byte)(ToInt() + semitones));
        }

        public static int ToInt(Note n)
        {
            return (int)n.Label + (n.Octave * OctaveLength);
        }

        public int ToInt()
        {
            return ToInt(this);
        }

        public static Note FromInt(int note)
        {
            NoteLabel n = (NoteLabel)(note % OctaveLength);
            byte o = (byte)(note / OctaveLength);
            return new Note(n, o);
        }
    }
}
