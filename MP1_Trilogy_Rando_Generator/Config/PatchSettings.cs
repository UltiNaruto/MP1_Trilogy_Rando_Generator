using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class PatchSettings
    {
        public readonly String baseIso;
        public readonly String outputFolder;
        public readonly String outputType;
        public readonly String trilogyIso;

        public PatchSettings()
        {
            String AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dynamic json = JObject.Parse(File.ReadAllText(AppData + @"\metroid-prime-randomizer\patch-settings.json"));

            try {
                this.baseIso = json.baseIso;
            } catch {
                this.baseIso = "";
            }

            try {
                this.outputFolder = json.outputFolder;
            } catch {
                this.outputFolder = "";
            }

            try {
                this.outputType = json.outputType;
            } catch {
                this.outputType = "iso";
            }

            try {
                this.trilogyIso = json.trilogyIso;
            } catch {
                this.trilogyIso = "";
            }
        }

        public PatchSettings(String baseIso, String outputFolder)
        {
            this.baseIso = baseIso;
            this.outputFolder = outputFolder;
            this.outputType = "iso";
            this.trilogyIso = "";
        }

        public void SaveToJson()
        {
            String AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (File.Exists(AppData + @"\metroid-prime-randomizer\patch-settings.json"))
                File.Delete(AppData + @"\metroid-prime-randomizer\patch-settings.json");
            File.WriteAllText(AppData + @"\metroid-prime-randomizer\patch-settings.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
