using MP1_Trilogy_Rando_Generator.Enums;
using System;
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
                using (var sR = new StreamReader(File.OpenRead(path)))
                {
                    while (!sR.EndOfStream)
                    {
                        line = sR.ReadLine();
                        if (!line.Contains(":"))
                            continue;
                        if (line.EndsWith(":"))
                            continue;

                        kvp = line.Split(':');
                        if (kvp.Length == 2)
                        {
                            if (kvp[0].Trim() == "skip frigate")
                                this.skipFrigate = kvp[1].Trim() == "true";
                            if (kvp[0].Trim() == "nonvaria heat damage")
                                this.heatProtection = kvp[1].Trim() == "true" ? "Varia Only" : "Any Suit";
                            if (kvp[0].Trim() == "staggered suit damage")
                                this.suitDamageReduction = kvp[1].Trim() == "true" ? "Progressive" : "Default";
                            if (kvp[0].Trim() == "starting area")
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
