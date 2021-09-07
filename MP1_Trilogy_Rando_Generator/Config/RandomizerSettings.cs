using MP1_Trilogy_Rando_Generator.Enums;
using MP1_Trilogy_Rando_Generator.Utils;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class RandomizerSettings
    {
        [DefaultValue(false)]
        public bool skipFrigate { get; set; }

        [DefaultValue("Any Suit")]
        public String heatProtection { get; set; }

        [DefaultValue("Default")]
        public String suitDamageReduction { get; set; }

        [DefaultValue("Default")]
        public String mapDefaultState { get; set; }

        [DefaultValue(100.0f)]
        public float etankCapacity { get; set; }

        [DefaultValue(250)]
        public int maxObtainableMissiles { get; set; }

        [DefaultValue(8)]
        public int maxObtainablePowerBombs { get; set; }

        public readonly SpawnRoom spawnRoom;

        RandomizerSettings()
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute attr = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (attr == null) continue;
                prop.SetValue(this, attr.Value);
            }
        }

        public RandomizerSettings(String path) : this()
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
                            if (kvp[0].Trim() == "map default state")
                            {
                                this.mapDefaultState = kvp[1].Trim();
                                this.mapDefaultState = StringUtils.ReplaceAt(this.mapDefaultState, 0, Char.ToUpperInvariant(mapDefaultState[0]));
                            }
                            if (kvp[0].Trim() == "starting area")
                                this.spawnRoom = SpawnRoom.randomizerValues[kvp[1].Trim()];

                            if (kvp[0].Trim() == "etank capacity")
                                this.etankCapacity = Single.Parse(kvp[1].Trim(), NumberStyles.Float);

                            if (kvp[0].Trim() == "max obtainable missiles")
                                this.maxObtainableMissiles = Int32.Parse(kvp[1].Trim());

                            if (kvp[0].Trim() == "max obtainable power bombs")
                                this.maxObtainablePowerBombs = Int32.Parse(kvp[1].Trim());
                        }
                    }
                }
            } catch { }
        }
    }
}
