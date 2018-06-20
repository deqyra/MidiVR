using System;

namespace MIDI
{
    /// <summary>
    ///     Represents scales and their layout based on a root note.
    ///     The literals should be interpreted as binary. Bits represents
    ///     which tones (in classical 12-tone representation) make up the scale.
    ///     For example, a C major scale is C, D, E, F, G, A and B :
    ///     Notes:  B   A#  A   G#  G   F#  F   E   D#  D   C#  C
    ///     Order:  11  10  9   8   7   6   5   4   3   2   1   0
    ///     Bits:   1   0   1   0   1   0   1   1   0   1   0   1
    ///     Value: (Decimal)2741, (Binary)1010 1011 0101
    /// </summary>
    public enum ScaleColor
    {
        Major           = 1 + (1 << 2) + (1 << 4) + (1 << 5) + (1 << 7) + (1 << 9) + (1 << 11),
        HarmonicMinor   = 1 + (1 << 2) + (1 << 3) + (1 << 5) + (1 << 7) + (1 << 8) + (1 << 11),
        MelodicMinor    = 1 + (1 << 2) + (1 << 3) + (1 << 5) + (1 << 7) + (1 << 9) + (1 << 11),
        NaturalMinor    = 1 + (1 << 2) + (1 << 3) + (1 << 5) + (1 << 7) + (1 << 8) + (1 << 10),
        Ionian          = Major,
        Dorian          = MelodicMinor,
        Phrygian        = 1 + (1 << 1) + (1 << 3) + (1 << 5) + (1 << 7) + (1 << 8) + (1 << 10),
        Lydian          = 1 + (1 << 2) + (1 << 4) + (1 << 6) + (1 << 7) + (1 << 9) + (1 << 11),
        Myxolidian      = 1 + (1 << 2) + (1 << 4) + (1 << 5) + (1 << 7) + (1 << 9) + (1 << 10),
        Aeolian         = NaturalMinor,
        Locrian         = 1 + (1 << 1) + (1 << 3) + (1 << 5) + (1 << 6) + (1 << 8) + (1 << 10)
    }

    public static class ScalColorExtension
    {
        //A scale is comprised of at most 12 notes (the 12 occidental semitones), which loop over themselves upon reaching one end.
        //The resolution is 12 in all cases.
        public static int GetResolution(this ScaleColor s)
        {
            return 12;
        }
    }
}
