using System;
using System.Collections.Generic;

namespace MIDI
{
    [Serializable]
    public class Scale
    {
        public Note baseNote;

        public ScaleColor color;

        private List<Note> _collection;

        public List<Note> collection
        {
            get { return _collection; }
            private set { _collection = value; }
        }

        public Scale()
        {
            baseNote = Note.DefaultNote;
            color = ScaleColor.Major;
            init();
        }

        public Scale(Note n, ScaleColor c = ScaleColor.Major)
        {
            baseNote = n;
            color = c;
            init();
        }

        public Scale(Scale c)
        {
            baseNote = c.baseNote;
            color = c.color;
            _collection = c._collection;
        }

        private void init()
        {
            // The ScaleColor enum stores nearly every scale's "blueprint" as a collection of Int32 labels.
            // A scale blueprint must be read as a bitwise mask, from LSB to MSB, to know which notes to include in the scale.
            int res = color.GetResolution();
            _collection = new List<Note>();

            for (int i = 0; i < res; i++)
            {
                // If i-th order bit is set in the ChordColor, add the according note to the collection
                if ((((int)color >> i) % 2) == 1)
                {
                    _collection.Add(baseNote.PitchUp((byte)i));
                }
            }
        }

        public Note NoteAtIndex(int i)
        {
            int size = _collection.Count;
            byte oShift = (byte)(i / size);
            int baseIndex = i % size;
            Note n = _collection[baseIndex];
            n.Octave += oShift;
            return n;
        }
    }
}
