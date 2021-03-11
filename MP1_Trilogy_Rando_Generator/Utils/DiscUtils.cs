using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class DiscUtils
    {
        static Random rand = null;
        static readonly List<String> UsedDeveloperCodes = new List<String>();
        internal static bool IsMetroidPrimeTrilogyNTSC_UOrPAL(string fileName)
        {
            String GameID = GetGameID(fileName);
            if (GameID.Substring(0, 3) != "R3M")
                return false;
            return GameID[3] == 'E' || GameID[3] == 'P';
        }

        internal static bool IsMetroidPrimeWiiNTSC_J(string fileName)
        {
            String GameID = GetGameID(fileName);
            return GameID.Substring(0, 3) == "R3I" && GameID[3] == 'J';
        }

        internal static String GetGameID(string fileName)
        {
            if (!File.Exists(fileName))
                return "";
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                return Encoding.ASCII.GetString(bR.ReadBytes(6));
            }
        }

        internal static String RandomizeDeveloperCode()
        {
            const String AllowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int len = AllowedCharacters.Length;
            String DeveloperCode = "01";
            if (rand == null)
            {
                rand = new Random((int)((DateTime.Now.Ticks / TimeSpan.TicksPerSecond)));
                UsedDeveloperCodes.Clear();
                if (File.Exists("udc.bin"))
                    UsedDeveloperCodes.AddRange(File.ReadAllLines("udc.bin"));
            }
            while (DeveloperCode == "01" || UsedDeveloperCodes.Contains(DeveloperCode))
                DeveloperCode = "" + AllowedCharacters[rand.Next(len)] + AllowedCharacters[rand.Next(len)];
            UsedDeveloperCodes.Add(DeveloperCode);
            File.WriteAllLines("udc.bin", UsedDeveloperCodes.ToArray());
            return DeveloperCode;
        }
    }
}
