using System;
using System.Threading;

namespace MIDI
{
    public class MIDIWrapper
    {
        public VirtualMIDI port;

        private static byte controllerCount = 0;

        public MIDIWrapper(string portName)
        {
            Guid manufacturer = new Guid("aa4e075f-3504-4aab-9b06-9a4104a91cf0");
            Guid product = new Guid("bb4e075f-3504-4aab-9b06-9a4104a91cf0");

            port = new VirtualMIDI(portName, 65535, VirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref manufacturer, ref product);
        }

        public void NoteOn(Note note, byte velocity = 0x64)
        {
            byte[] command = new byte[3];
            // Byte 1 : 0x90 = Note ON
            command[0] = 0x90;
            // Byte 2 : note height
            command[1] = (byte)note.ToInt();
            // Byte 3 : note velocity
            command[2] = velocity;

            port.sendCommand(command);
        }

        public void NoteOff(Note note, byte velocity = 0x40)
        {
            byte[] command = new byte[3];
            // Byte 1 : 0x80 = Note OFF
            command[0] = 0x80;
            // Byte 2 : note height
            command[1] = (byte)note.ToInt();
            // Byte 3 : note velocity
            command[2] = velocity;

            port.sendCommand(command);
        }

        public void ContinuousControl(byte controller, byte value)
        {
            byte[] command = new byte[3];
            // Byte 1 : 0xBz = Continuous control on MIDI channel z. We use channel 0 here.
            command[0] = 0xB0;
            // Byte 2 : controller id
            command[1] = controller;
            // Byte 3 : control value
            command[2] = value;

            port.sendCommand(command);
        }

        public void NoteOnOff(Note note, byte velocity, int time)
        {
            NoteOn(note, velocity);
            Thread.Sleep(time);
            NoteOff(note, velocity);
        }

        public void Note(Note note, byte velocity, int time)
        {
            Thread thread = new Thread(()=>NoteOnOff(note, velocity, time));
            thread.Start();
        }

        public byte GetNewController()
        {
            return controllerCount++;
        }
    }
}
