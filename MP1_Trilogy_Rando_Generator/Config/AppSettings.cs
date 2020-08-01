using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class AppSettings
    {
        public String prime1RandomizerPath;

        public AppSettings()
        {
            var line = default(String);
            var kvp = default(MatchCollection);
            try
            {
                String CurDir = Directory.GetCurrentDirectory();
                using (var sR = new StreamReader(File.OpenRead(CurDir + @"\settings.json")))
                {
                    while (!sR.EndOfStream)
                    {
                        line = sR.ReadLine().Trim().Trim(' ');
                        kvp = new Regex("\"(.*?)\"").Matches(line);
                        if (kvp.Count == 2)
                        {
                            if (kvp[0].Groups[1].Value == "prime1RandomizerPath")
                                this.prime1RandomizerPath = kvp[1].Groups[1].Value;
                        }
                    }
                }
            }
            catch
            {
                this.prime1RandomizerPath = "";
                this.SaveToJson();
            }
        }

        public void SaveToJson()
        {
            String CurDir = Directory.GetCurrentDirectory();
            if (File.Exists(CurDir + @"\settings.json"))
                File.Delete(CurDir + @"\settings.json");
            using (var sW = new StreamWriter(File.OpenWrite(CurDir + @"\settings.json")))
            {
                sW.WriteLine("{");
                sW.WriteLine("\t\"prime1RandomizerPath\": \"" + this.prime1RandomizerPath + "\"");
                sW.WriteLine("}");
            }
        }
    }
}
