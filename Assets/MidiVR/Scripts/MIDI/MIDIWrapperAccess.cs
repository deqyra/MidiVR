using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDI
{
    class MIDIWrapperAccess
    {
        private static MIDIWrapper instance = null;

        public static MIDIWrapper w
        {
            get
            {
                if (instance == null)
                {
                    instance = new MIDIWrapper("MidiVR");
                }
                return instance;
            }
        }
    }
}
