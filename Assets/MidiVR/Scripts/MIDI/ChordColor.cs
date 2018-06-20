using System;

namespace MIDI
{
    /// <summary>
    ///     Represents chords and their layout based on a root note.
    ///     The literals should be interpreted as binary. Bits represents
    ///     which tones (in classical 12-tone representation) make up the chord.
    ///     For example, a C major chord is C, E and G :
    ///     Notes:  B   A#  A   G#  G   F#  F   E   D#  D   C#  C
    ///     Order:  11  10  9   8   7   6   5   4   3   2   1   0
    ///     Bits:   0   0   0   0   1   0   0   1   0   0   0   1
    ///     Value: (Decimal)145, (Binary)10010001
    /// </summary>
    public enum ChordColor
    {
        Major                   = (1 << 7)  + (1 << 4)  + 1,
        Minor                   = (1 << 7)  + (1 << 3)  + 1,

        Augmented               = (1 << 8)  + (1 << 4)  + 1,
        Diminished              = (1 << 6)  + (1 << 3)  + 1,

        FullAugmented           = (1 << 12) + (1 << 8)  + (1 << 4)  + 1,
        DiminishedSeventh       = (1 << 9)  + (1 << 6)  + (1 << 3)  + 1,


        HalfDiminishedSeventh   = (1 << 10) + (1 << 6)  + (1 << 3)  + 1,
        MinorSeventhFlatFive    = HalfDiminishedSeventh,
        MajorSeventhFlatFive    = (1 << 10) + (1 << 6)  + (1 << 4)  + 1,

        AugmentedNinth          = (1 << 15) + (1 << 10) + (1 << 7)  + (1 << 4) + 1,
        Hendrix                 = AugmentedNinth,
        MinorSeventhNinth       = (1 << 14) + (1 << 10) + (1 << 7) + (1 << 3) + 1,

        MajorSeventh            = (1 << 11) + (1 << 7)  + (1 << 4)  + 1,
        MinorSeventh            = (1 << 10) + (1 << 7)  + (1 << 3)  + 1,

        MajorSixth              = (1 << 9)  + (1 << 7)  + (1 << 4)  + 1,
        MinorSixth              = (1 << 9)  + (1 << 7)  + (1 << 3)  + 1,

        MajorSixthNinth         = (1 << 14) + (1 << 9)  + (1 << 7)  + (1 << 4) + 1,
        MinorSixthNinth         = (1 << 14) + (1 << 9)  + (1 << 7)  + (1 << 3) + 1,

        MajorNinth              = (1 << 14) + (1 << 11) + (1 << 7)  + (1 << 4) + 1,
        MinorNinth              = (1 << 14) + (1 << 11) + (1 << 7)  + (1 << 3) + 1,

        MajorEleventh           = (1 << 17) + (1 << 14) + (1 << 11) + (1 << 7) + (1 << 4) + 1,
        MinorEleventh           = (1 << 17) + (1 << 14) + (1 << 10) + (1 << 7) + (1 << 3) + 1,

        DominantSeventh         = (1 << 10) + (1 << 7)  + (1 << 4)  + 1,
        MajorMinorSeventh       = DominantSeventh,
        MinorMajorSeventh       = (1 << 11) + (1 << 7)  + (1 << 3)  + 1,

        Mu1                     = (1 << 7)  + (1 << 4)  + (1 << 2)  + 1,
        Mu2                     = (1 << 16) + (1 << 14) + (1 << 7)  + 1,
        Mu3                     = (1 << 18) + (1 << 16) + (1 << 14) + 1,

        Powerchord              = (1 << 12) + (1 << 7)  + 1,

        SeventhSuspensionFour   = (1 << 10) + (1 << 7)  + (1 << 5)  + 1,

        Suspended               = (1 << 7)  + (1 << 5)  + 1
    }

    public static class ChordColorExtension
    {
        public static int GetResolution(this ChordColor c)
        {
            // The resolution is the number of bits in the literals of the enum.
            // They are stored on 32-bit integers, so 32 is returned.
            return 32;
        }

        public static ChordColor Opposite(this ChordColor c)
        {
            switch (c)
            {
                case ChordColor.Major:
                    return ChordColor.Minor;

                case ChordColor.Minor:
                    return ChordColor.Major;

                case ChordColor.Augmented:
                    return ChordColor.Diminished;

                case ChordColor.Diminished:
                    return ChordColor.Augmented;

                case ChordColor.FullAugmented:
                    return ChordColor.DiminishedSeventh;

                case ChordColor.MinorSeventhFlatFive:
                    return ChordColor.MajorSeventhFlatFive;

                case ChordColor.AugmentedNinth:
                    return ChordColor.MinorSeventhNinth;

                case ChordColor.MinorSeventhNinth:
                    return ChordColor.AugmentedNinth;

                case ChordColor.MajorSeventh:
                    return ChordColor.MinorSeventh;

                case ChordColor.MinorSeventh:
                    return ChordColor.MajorSeventh;

                case ChordColor.MajorSixth:
                    return ChordColor.MinorSixth;

                case ChordColor.MinorSixth:
                    return ChordColor.MajorSixth;

                case ChordColor.MajorSixthNinth:
                    return ChordColor.MinorSixthNinth;

                case ChordColor.MinorSixthNinth:
                    return ChordColor.MajorSixthNinth;

                case ChordColor.MajorNinth:
                    return ChordColor.MinorNinth;

                case ChordColor.MinorNinth:
                    return ChordColor.MajorNinth;

                case ChordColor.MajorEleventh:
                    return ChordColor.MinorEleventh;

                case ChordColor.MinorEleventh:
                    return ChordColor.MajorEleventh;

                case ChordColor.DominantSeventh:
                    return ChordColor.MinorMajorSeventh;

                case ChordColor.MinorMajorSeventh:
                    return ChordColor.DominantSeventh;

                default:
                    return c;
            }
        }

        public static string ToString(this ChordColor c)
        {
            switch(c)
            {
                case ChordColor.Major:
                    return "";

                case ChordColor.Minor:
                    return "-";

                case ChordColor.Augmented:
                    return "aug";

                case ChordColor.Diminished:
                    return "dim";

                case ChordColor.FullAugmented:
                    return "aug";

                case ChordColor.MinorSeventhFlatFive:
                    return "-7b5";

                case ChordColor.AugmentedNinth:
                    return "add9";

                case ChordColor.MinorSeventhNinth:
                    return "-7,9";

                case ChordColor.MajorSeventh:
                    return "7";

                case ChordColor.MinorSeventh:
                    return "-7";

                case ChordColor.MajorSixth:
                    return "6";

                case ChordColor.MinorSixth:
                    return "-6";

                case ChordColor.MajorSixthNinth:
                    return "6,9";

                case ChordColor.MinorSixthNinth:
                    return "-6,9";

                case ChordColor.MajorNinth:
                    return "9";

                case ChordColor.MinorNinth:
                    return "-9";

                case ChordColor.MajorEleventh:
                    return "11";

                case ChordColor.MinorEleventh:
                    return "-11";

                case ChordColor.DominantSeventh:
                    return "Maj7";

                case ChordColor.MinorMajorSeventh:
                    return "-7Maj";

                case ChordColor.Mu1:
                    return "Mu1";

                case ChordColor.Mu2:
                    return "Mu2";

                case ChordColor.Mu3:
                    return "Mu3";

                case ChordColor.Powerchord:
                    return "P";

                case ChordColor.SeventhSuspensionFour:
                    return "7add4";

                case ChordColor.Suspended:
                    return "sus4";

                default:
                    return "";
            }
        }
    }
}
