using System;
using System.Collections.Generic;

namespace MP1_Trilogy_Rando_Generator.Patcher
{
    // Add suit damage reduction patch
    class Patcher
    {
        static Patches.TrilogyPrime prime = null;

        /*static void WriteFunction(UInt32 address, UInt32[] opcodes)
        {
            UInt32 max_opcodes = (0x800 - (address - 0x80001800)) / 4;
            if (opcodes.Length > max_opcodes)
                throw new Exception("Cannot create function there's not enough memory allocated in that section");

            for (int i = 0; i < opcodes.Length; i++)
                new Patcher.DOL_Patch<UInt32>(MP1_Dol_Path, (UInt32)(address + i * 4), opcodes[i]).Apply();
        }*/

        public static void Init(String game_id, int mprime)
        {
            if (mprime == 1)
            {
                if (game_id == "R3MP")
                    prime = new Patches.Prime.PAL();
                if (game_id == "R3ME")
                    prime = new Patches.Prime.NTSC_U();
                if (game_id == "R3IJ")
                    prime = new Patches.Prime.NTSC_J();
            }
            else
            {
                prime = null;
            }
        }

        internal static void Apply(Dictionary<String, Object> config)
        {
            if (prime != null)
                prime.Apply(config);
        }
    }
}
