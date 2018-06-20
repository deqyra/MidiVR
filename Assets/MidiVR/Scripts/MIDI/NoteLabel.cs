using System;

namespace MIDI
{
    [Serializable]
    public enum NoteLabel
    {
        C   = 0,        // C
        CS  = 1,        // C#
        D   = 2,        // D
        DS  = 3,        // D#
        E   = 4,        // E
        F   = 5,        // F
        FS  = 6,        // ...
        G   = 7,
        GS  = 8,
        A   = 9,
        AS  = 10,
        B   = 11
    }

    [Serializable()]
    public class MIDINoteException : System.Exception
    {
        public const int ERROR_OUT_OF_BOUNDS = 1;

        public MIDINoteException() : base()
        {
        }

        public MIDINoteException(string message) : base(message)
        {
        }

        public MIDINoteException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected MIDINoteException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
        }

        public MIDINoteException(int reasonCode) : base(reasonCodeToString(reasonCode))
        {
        }

        public static string reasonCodeToString(int reasonCode)
        {
            switch (reasonCode)
            {
                case ERROR_OUT_OF_BOUNDS:
                    return "Note is too high or too low for MIDI to handle!";
                default:
                    return "Unspecified MIDI note error: " + reasonCode;
            }
        }
    }

    public static class NoteLabelExtension
    {
        public static string GetString(this NoteLabel n)
        {
            switch (n)
            {
                case NoteLabel.A:
                    return "A";
                case NoteLabel.AS:
                    return "A#";
                case NoteLabel.B:
                    return "B";
                case NoteLabel.C:
                    return "C";
                case NoteLabel.CS:
                    return "C#";
                case NoteLabel.D:
                    return "D";
                case NoteLabel.DS:
                    return "D#";
                case NoteLabel.E:
                    return "E";
                case NoteLabel.F:
                    return "F";
                case NoteLabel.FS:
                    return "F#";
                case NoteLabel.G:
                    return "G";
                case NoteLabel.GS:
                    return "G#";
                default:
                    return "";
            }
        }
    }
}