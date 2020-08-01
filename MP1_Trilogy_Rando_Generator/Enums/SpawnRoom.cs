using System;
using System.Collections.Generic;

namespace MP1_Trilogy_Rando_Generator.Enums
{
    class SpawnRoom
    {
        public readonly uint MLVL;
        public readonly uint PakIndex;
        public readonly uint MREA_ID;
        SpawnRoom(uint MLVL, uint PakIndex, uint MREA_ID)
        {
            this.MLVL = MLVL;
            this.PakIndex = PakIndex;
            this.MREA_ID = MREA_ID;
        }

        public static readonly Dictionary<String, SpawnRoom> Values = new Dictionary<String, SpawnRoom>()
        {
            { "Chozo Ruins West (Main Plaza)", new SpawnRoom(0x83f6ff6f, 2, 0) }, // Chozo Ruins - Transport to Tallon Overworld North
            { "Chozo Ruins North (Sun Tower)", new SpawnRoom(0x83f6ff6f, 2, 24) }, // Chozo Ruins - Transport to Magmoor Caverns North
            { "Chozo Ruins South (Reflecting Pool, Save Station)", new SpawnRoom(0x83f6ff6f, 2, 62) }, // Chozo Ruins - Transport to Tallon Overworld East
            { "Chozo Ruins South (Reflecting Pool, Far End)", new SpawnRoom(0x83f6ff6f, 2, 63) }, // Chozo Ruins - Transport to Tallon Overworld South
            { "Phendrana Drifts North (Phendrana Shorelines)", new SpawnRoom(0xa8be6291, 3, 0) }, // Phendrana Drifts - Transport to Magmoor Caverns West
            { "Phendrana Drifts South (Quarantine Cave)", new SpawnRoom(0xa8be6291, 3, 29) }, // Phendrana Drifts - Transport to Magmoor Caverns South
            { "Tallon Overworld North (Tallon Canyon)", new SpawnRoom(0x39f2de28, 4, 14) }, // Tallon Overworld - Transport to Chozo Ruins West
            { "Tallon Overworld East (Frigate Crash Site)", new SpawnRoom(0x39f2de28, 4, 22) }, // Tallon Overworld - Transport to Chozo Ruins East
            { "Tallon Overworld West (Root Cave)", new SpawnRoom(0x39f2de28, 4, 23) }, // Tallon Overworld - Transport to Magmoor Caverns East
            { "Tallon Overworld South (Great Tree Hall, Upper)", new SpawnRoom(0x39f2de28, 4, 41) }, // Transport to Chozo Ruins South
            { "Tallon Overworld South (Great Tree Hall, Lower)", new SpawnRoom(0x39f2de28, 4, 43) }, // Transport to Phazon Mines East
            { "Artifact Temple", new SpawnRoom(0x39f2de28, 4, 16) }, // Tallon Overworld - Artifact Temple
            { "Landing Site", new SpawnRoom(0x39f2de28, 4, 0) }, // Tallon Overworld - Landing Site
            { "Phazon Mines East (Main Quarry)", new SpawnRoom(0xb1ac4d65, 5, 0) }, // Phazon Mines - Transport to Tallon Overworld South
            { "Phazon Mines West (Phazon Processing Center)", new SpawnRoom(0xb1ac4d65, 5, 25) }, // Phazon Mines - Transport to Magmoor Caverns South
            { "Magmoor Caverns North (Lava Lake)", new SpawnRoom(0x3ef8237c, 6, 0) }, // Magmoor Caverns - Transport to Chozo Ruins North
            { "Magmoor Caverns West (Monitor Station)", new SpawnRoom(0x3ef8237c, 6, 13) }, // Magmoor Caverns - Transport to Phendrana Drifts North
            { "Magmoor Caverns East (Twin Fires)", new SpawnRoom(0x3ef8237c, 6, 16) }, // Magmoor Caverns - Transport to Tallon Overworld West
            { "Magmoor Caverns East (Magmoor Workstation, Debris)", new SpawnRoom(0x3ef8237c, 6, 26) }, // Magmoor Caverns - Transport to Phazon Mines West
            { "Magmoor Caverns East (Magmoor Workstation, Save Station)", new SpawnRoom(0x3ef8237c, 6, 27) } // Magmoor Caverns - Transport to Phendrana Drifts South
        };
    }
}
