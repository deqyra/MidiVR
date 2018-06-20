using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDI
{
    public class ChordScheme
    {
        private Note baseNote;

        private ChordColor color;

        public ChordScheme()
        {
            baseNote = Note.DefaultNote;
            color = ChordColor.Major;
        }

        public ChordScheme(Note n, ChordColor c = ChordColor.Major)
        {
            baseNote = n;
            color = c;
        }

        public ChordScheme(Chord c)
        {
            baseNote = c.baseNote;
            color = c.color;
        }

        // Considering that the base chord is a coordinates (0,0), this method adds to the base note one fifth per +1 along X,
        // and flips the chord color for each value along Y, like so:
        // -1,1     0,1     1,1             Cm      Gm      Dm
        // -1,0     0,0     1,0             FM      CM      GM
        // -1,-1    0,-1    1,-1            Dm      Am      Em
        public Chord GetChordAtXY(int x, int y)
        {
            Note n = baseNote;
            ChordColor c = color;

            byte tmp = baseNote.Octave;

            //Moving along the X axis: one fifth per column (one fifth = 7 semitones).
            n = n.Pitch(7 * x);

            //Moving along the Y axis: switch for the opposite color every row.
            //Considering any given row: the tone two rows above should be one second (2 semitones) below.
            //Considering any row of the original color: the tone one row below should be one minor third (3 semitones) below.
            if (y > 0)
            {
                int rowPairs = y / 2;
                n = n.PitchDown(rowPairs * 2);
                if (y % 2 == 1)
                {
                    n = n.PitchUp(7);
                    c = c.Opposite();
                }
            }
            else if (y < 0)
            {
                int rowPairs = (-y) / 2;
                n = n.PitchUp(rowPairs * 2);
                if (-y % 2 == 1)
                {
                    n = n.PitchDown(3);
                    c = c.Opposite();
                }
            }

            n.Octave = tmp;
            return new Chord(n, c);
        }
    }
}
