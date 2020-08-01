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
            var line = default(String);
            var kvp = default(MatchCollection);
            try
            {
                String AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                using (var sR = new StreamReader(File.OpenRead(AppData + @"\metroid-prime-randomizer\patch-settings.json")))
                {
                    while (!sR.EndOfStream)
                    {
                        line = sR.ReadLine().Trim().Trim(' ');
                        kvp = new Regex("\"(.*?)\"").Matches(line);
                        if (kvp.Count == 2)
                        {
                            if (kvp[0].Groups[1].Value == "baseIso")
                                this.baseIso = kvp[1].Groups[1].Value;
                            if (kvp[0].Groups[1].Value == "outputFolder")
                                this.outputFolder = kvp[1].Groups[1].Value;
                            if (kvp[0].Groups[1].Value == "outputType")
                                this.outputType = kvp[1].Groups[1].Value;
                            if (kvp[0].Groups[1].Value == "trilogyIso")
                                this.trilogyIso = kvp[1].Groups[1].Value;
                        }
                    }
                }
            } catch {
                this.baseIso = "";
                this.outputFolder = "";
                this.outputType = "iso";
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
            using (var sW = new StreamWriter(File.OpenWrite(AppData + @"\metroid-prime-randomizer\patch-settings.json")))
            {
                sW.WriteLine("{");
                sW.WriteLine("\t\"baseIso\": \""+this.baseIso+"\",");
                sW.WriteLine("\t\"outputFolder\": \"" + this.outputFolder + "\",");
                sW.WriteLine("\t\"outputType\": \"" + this.outputType + "\",");
                sW.WriteLine("\t\"trilogyIso\": \"" + this.trilogyIso + "\"");
                sW.WriteLine("}");
            }
        }
    }
}
