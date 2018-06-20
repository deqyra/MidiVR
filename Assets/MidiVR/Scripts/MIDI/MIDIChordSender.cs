using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MIDI
{
    class MIDIChordSender : MIDINoteSender
    {
        public Chord baseChord;

        public MIDIChordSender(Note n, ChordColor c = ChordColor.Major)
        {
            baseChord = new Chord(n, c);
            init();
        }

        public MIDIChordSender(Chord c)
        {
            baseChord = new Chord(c);
            init();
        }

        public MIDIChordSender()
        {
            baseChord = new Chord(MIDI.Note.DefaultNote);
            init();
        }

        public void ChordOn(Chord chord, byte velocity = 0x64)
        {
            List<Note> tones = chord.collection;
            for (int i = 0; i < tones.Count; i++)
            {
                w.NoteOn(tones[i], velocity);
            }
        }

        public void ChordOff(Chord chord, byte velocity = 0x40)
        {
            List<Note> tones = chord.collection;
            for (int i = 0; i < tones.Count; i++)
            {
                w.NoteOff(tones[i], velocity);
            }
        }

        public void ChordOnOff(Chord chord, byte velocity, int time)
        {
            List<Note> tones = chord.collection;
            for (int i = 0; i < tones.Count; i++)
            {
                w.NoteOnOff(tones[i], velocity, time);
            }
        }

        public void Chord(Chord chord, byte velocity, int time)
        {
            Thread thread = new Thread(() => ChordOnOff(chord, velocity, time));
            thread.Start();
        }

        public void ChordOn(byte velocity = 0x64)
        {
            ChordOn(baseChord, velocity);
        }

        public void ChordOff(byte velocity = 0x40)
        {
            ChordOff(baseChord, velocity);
        }

        public void ChordOnOff(byte velocity, int time)
        {
            ChordOnOff(baseChord, velocity, time);
        }

        public void Chord(byte velocity, int time)
        {
            Thread thread = new Thread(() => ChordOnOff(velocity, time));
            thread.Start();
        }
    }
}
