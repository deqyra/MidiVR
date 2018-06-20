using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDI
{
    [Serializable]
    public class Chord
    {
        public Note baseNote;

        public ChordColor color;

        private List<Note> _collection;

        public List<Note> collection
        {
            get { return _collection; }
            private set { _collection = value; }
        }

        public Chord()
        {
            baseNote = Note.DefaultNote;
            color = ChordColor.Major;
            init();
        }

        public Chord(Note n, ChordColor c = ChordColor.Major)
        {
            baseNote = n;
            color = c;
            init();
        }

        public Chord(Chord c)
        {
            baseNote = c.baseNote;
            color = c.color;
            _collection = c._collection;
        }

        private void init()
        {
            // The ChordColor enum stores nearly every chord's "blueprint" as a collection of Int32 labels.
            // A chord blueprint must be read as a bitwise mask, from LSB to MSB, to know which notes to include in the chord.
            // 32 bits = 32 values to read = 32-sized bool array.
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
    }
}
