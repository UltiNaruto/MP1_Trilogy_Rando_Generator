using MP1_Trilogy_Rando_Generator.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class RandomizerSettings
    {
        public readonly bool skipFrigate;
        public readonly String heatProtection;
        public readonly String suitDamageReduction;
        public readonly SpawnRoom spawnRoom;

        public RandomizerSettings(String path)
        {
            var line = default(String);
            var kvp = default(String[]);
            try
            {
                String AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                using (var sR = new StreamReader(File.OpenRead(path)))
                {
                    while (!sR.EndOfStream)
                    {
                        line = sR.ReadLine().Trim().Trim(',').Replace("\"", "");
                        kvp = line.Split(':');
                        if (kvp.Length == 2)
                        {
                            if (kvp[0].Trim() == "Skip the Space Pirate Frigate")
                                this.skipFrigate = kvp[1].Trim() == "On";
                            if (kvp[0].Trim() == "Heat Protection")
                                this.heatProtection = kvp[1].Trim();
                            if (kvp[0].Trim() == "Suit Damage Reduction")
                                this.suitDamageReduction = kvp[1].Trim();
                            if (kvp[0].Trim() == "Starting Area")
                                if(kvp[1].Trim() != "Random")
                                    if (this.spawnRoom == null)
                                        this.spawnRoom = SpawnRoom.Values[kvp[1].Trim()];
                        }
                    }
                }
            } catch {
                this.skipFrigate = false;
                this.heatProtection = "Any Suit";
                this.suitDamageReduction = "Default";
                this.spawnRoom = SpawnRoom.Values["Landing Site"];
            }
        }
    }
}
