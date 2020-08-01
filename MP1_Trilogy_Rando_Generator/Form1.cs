using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MP1_Trilogy_Rando_Generator
{
    public partial class Form1 : Form
    {
        Random rand = null;
        static Config.AppSettings appSettings = new Config.AppSettings();
        static List<String> UsedDeveloperCodes = new List<String>();
        bool IsTemplateISOGenerated()
        {
            return File.Exists("gc_template.iso");
        }

        bool IsMetroidPrimeTrilogyNTSC(string fileName)
        {
            var GameID = default(String);
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                GameID = Encoding.ASCII.GetString(bR.ReadBytes(6));
                if (GameID.Substring(0, 3) != "R3M")
                    return false;
                return GameID[3] == 'E';
            }
        }

        bool IsMetroidPrimeNTSC_0_00(string fileName)
        {
            var GameID = default(String);
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                GameID = Encoding.ASCII.GetString(bR.ReadBytes(6));
                if (GameID.Substring(0, 3) != "GM8")
                    return false;
                if (GameID[3] != 'E')
                    return false;
                bR.ReadByte();
                return bR.ReadByte() == 0;
            }
        }

        void SetStartingArea(Enums.SpawnRoom spawnRoom)
        {
            UInt16 MLVL_H = (UInt16)((spawnRoom.MLVL & 0xFFFF0000) >> 16);
            UInt16 MLVL_L = (UInt16)(spawnRoom.MLVL & 0xFFFF);
            if (MLVL_L > 0x7FFF)
                MLVL_H++;

            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801098EC + 2, MLVL_H).Apply();
            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801098EC + 14, MLVL_L).Apply();
            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x8010D278 + 2, MLVL_H).Apply();
            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x8010D278 + 14, MLVL_L).Apply();
            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x8010D27C + 2, (UInt16)spawnRoom.PakIndex).Apply();
            new Patcher.DOL_Patch<UInt16>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x800CD1F8 + 18, (UInt16)spawnRoom.MREA_ID).Apply();
        }

        void ApplySkipCutscenePatch(bool enabled)
        {
            if (enabled)
            {
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FC70C, (UInt32)0x38600001).Apply(); // li r3, 1
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FC70C + 4, (UInt32)0x4E800020).Apply(); // blr
            }
            else
            {
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FC70C, (UInt32)0x9421FFE0).Apply(); // stwu r1, local_20(r1)
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FC70C + 4, (UInt32)0x7C0802A6).Apply(); // mfspr r0, LR
            }
        }

        void ApplyHeatProtectionPatch(String type)
        {
            if (type == "Any Suit")
            {
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48, (UInt32)0x7C0800D0).Apply(); // neg r0, r8
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 4, (UInt32)0x7C004078).Apply(); // andc r0, r0, r8
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 8, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 12, (UInt32)0x7CC60034).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 16, (UInt32)0x54070FFE).Apply(); // rlwinm r7, r0, 0x1, 0x1f, 0x1f
            }
            if (type == "Varia Only")
            {
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48, (UInt32)0x7CC63850).Apply(); // subf r6, r6, r7
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 4, (UInt32)0x80F000E0).Apply(); // cntlzw r6, r6
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 8, (UInt32)0x80E600E0).Apply(); // lwz r7, 0xe0(r6)
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 12, (UInt32)0x60000000).Apply(); // nop
                new Patcher.DOL_Patch<UInt32>(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", 0x801FEBCC + 0x48 + 16, (UInt32)0x60000000).Apply(); // nop
            }
        }

        void RandomizeDeveloperCode(String outFile)
        {
            const String AllowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int len = AllowedCharacters.Length;
            String BaseGameCode = "R3ME";
            String DeveloperCode = "01";
            while (DeveloperCode == "01" || UsedDeveloperCodes.Contains(DeveloperCode))
                DeveloperCode = "" + AllowedCharacters[rand.Next(len)] + AllowedCharacters[rand.Next(len)];
            using (var file = File.Open(outFile, FileMode.Open))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(Encoding.ASCII.GetBytes(BaseGameCode + DeveloperCode), 0, 6);
            }
            UsedDeveloperCodes.Add(DeveloperCode);
            File.WriteAllLines("udc.bin", UsedDeveloperCodes.ToArray());
        }

        public Form1()
        {
            rand = new Random((int)((DateTime.Now.Ticks / TimeSpan.TicksPerSecond)));
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(File.Exists("udc.bin"))
                UsedDeveloperCodes.AddRange(File.ReadAllLines("udc.bin"));
            if (IsTemplateISOGenerated())
            {
                this.label1.Text = "Trilogy ISO template for Prime 1 Randomizer detected!";
                this.button1.Enabled = false;
                this.button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialogResult = default(DialogResult);
            var wii_iso_path = default(String);
            var gc_iso_path = default(String);

            MessageBox.Show(@"/!\ This operation can take 10 mins or more on a 5400 RPM HDD. So please be patient!");

            if (Directory.Exists("tmp"))
                Directory.Delete("tmp", true);

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a NTSC-U iso of Metroid Prime Trilogy";
                openFileDialog.Filter = "Wii ISO File|*.iso";
                openFileDialog.FileName = "";
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                while (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                {
                    dialogResult = openFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        if (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a Wii iso");
                            continue;
                        }
                        if (!IsMetroidPrimeTrilogyNTSC(openFileDialog.FileName))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a Metroid Prime Trilogy NTSC-U iso");
                            continue;
                        }
                        wii_iso_path = openFileDialog.FileName;
                    }
                    if (dialogResult == DialogResult.Cancel)
                        return;
                }

                openFileDialog.Title = "Select a NTSC 0-00 iso of Metroid Prime";
                openFileDialog.Filter = "GC ISO File|*.iso";
                openFileDialog.FileName = "";
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                while (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                {
                    dialogResult = openFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        if (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a GC iso");
                            continue;
                        }
                        if (!IsMetroidPrimeNTSC_0_00(openFileDialog.FileName))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a Metroid Prime NTSC 0-00 iso");
                            continue;
                        }
                        gc_iso_path = openFileDialog.FileName;
                    }
                    if (dialogResult == DialogResult.Cancel)
                        return;
                }
            }

            if(!NodManager.ExtractISO(wii_iso_path, false))
            {
                MessageBox.Show("Failed extracting wii iso!");
                return;
            }

            if(!NodManager.ExtractISO(gc_iso_path, true))
            {
                Directory.Delete(".\\tmp\\wii", true);
                MessageBox.Show("Failed extracting gc iso!");
                return;
            }

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\MP1", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\gc\\files\\"+Path.GetFileName(file), true);
            NodManager.CreateISO("gc_template.iso", true);
            Directory.Delete(".\\tmp\\gc", true);
            this.label1.Text = "Trilogy ISO template for Prime 1 Randomizer detected!";
            this.button1.Enabled = false;
            this.button2.Enabled = true;
            MessageBox.Show("Done generating the GC template iso!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete("gc_template.iso");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dialogResult = default(DialogResult);
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select the executable of Prime 1 Randomizer";
                openFileDialog.Filter = "EXE File|*.exe";
                openFileDialog.FileName = "";
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                while (!openFileDialog.FileName.ToLower().EndsWith(".exe"))
                {
                    dialogResult = openFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        if (!openFileDialog.FileName.ToLower().EndsWith(".exe"))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Invalid selection!");
                            continue;
                        }
                        appSettings.prime1RandomizerPath = openFileDialog.FileName;
                        appSettings.SaveToJson();
                    }
                    if (dialogResult == DialogResult.Cancel)
                        return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dialogResult = default(DialogResult);
            var wii_iso_path = default(String);
            var new_wii_iso_path = default(String);
            var gc_iso_filename = default(String);
            var curDir = default(String);
            var randomizerSettings = default(Config.RandomizerSettings);

            curDir = Directory.GetCurrentDirectory();

            if(appSettings.prime1RandomizerPath == "")
            {
                MessageBox.Show("Please click on \"Locate BashPrime's Randomizer\" before attempting to randomize!");
                return;
            }

            Config.PatchSettings patchSettingsBak = new Config.PatchSettings();
            Config.PatchSettings patchSettingsWii = new Config.PatchSettings((curDir + @"\gc_template.iso").Replace("\\", "\\\\"), (curDir + @"\tmp").Replace("\\", "\\\\"));
            patchSettingsWii.SaveToJson();

            if(!RandomizerManager.Run(appSettings.prime1RandomizerPath))
            {
                MessageBox.Show("Prime 1 Randomizer haven't properly exited! Cancelling...");
                patchSettingsBak.SaveToJson();
                return;
            }

            patchSettingsBak.SaveToJson();

            if(Directory.EnumerateFiles(".\\tmp", "*.iso").Count() == 0)
            {
                MessageBox.Show("No Randomized ISO generated! Cancelling...");
                return;
            }

            gc_iso_filename = Path.GetFileName(Directory.EnumerateFiles(".\\tmp", "*.iso").First());

            if(!File.Exists(".\\tmp\\"+gc_iso_filename.Replace(".iso", " - Spoiler.json")))
            {
                MessageBox.Show("Spoiler is required to get the randomizer settings used!");
                return;
            }

            randomizerSettings = new Config.RandomizerSettings(".\\tmp\\" + gc_iso_filename.Replace(".iso", " - Spoiler.json"));

            if (!this.checkBox1.Checked)
                File.Delete(".\\tmp\\"+gc_iso_filename.Replace(".iso", " - Spoiler.json"));

            if (!Directory.Exists(".\\tmp\\wii"))
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Select a NTSC-U iso of Metroid Prime Trilogy";
                    openFileDialog.Filter = "Wii ISO File|*.iso";
                    openFileDialog.FileName = "";
                    openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    while (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                    {
                        dialogResult = openFileDialog.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            if (!openFileDialog.FileName.ToLower().EndsWith(".iso"))
                            {
                                openFileDialog.FileName = "";
                                MessageBox.Show("Select a Wii iso");
                                continue;
                            }
                            if (!IsMetroidPrimeTrilogyNTSC(openFileDialog.FileName))
                            {
                                openFileDialog.FileName = "";
                                MessageBox.Show("Select a Metroid Prime Trilogy NTSC-U iso");
                                continue;
                            }
                            wii_iso_path = openFileDialog.FileName;
                        }
                        if (dialogResult == DialogResult.Cancel)
                            return;
                    }

                    if (!NodManager.ExtractISO(wii_iso_path, false))
                    {
                        MessageBox.Show("Failed extracting wii iso!");
                        return;
                    }
                }
            }

            NodManager.ExtractISO(".\\tmp\\"+ gc_iso_filename, true);

            File.Delete(".\\tmp\\" + gc_iso_filename);

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\wii\\DATA\\files\\MP1\\" + Path.GetFileName(file), true);

            Directory.Delete(".\\tmp\\gc", true);

            /* Applying patches to dol file */

            if(randomizerSettings.skipFrigate)
            {
                if(randomizerSettings.spawnRoom == null)
                {
                    MessageBox.Show("No spawn room defined!");
                    return;
                }
                SetStartingArea(randomizerSettings.spawnRoom);
            }

            ApplySkipCutscenePatch(true);
            ApplyHeatProtectionPatch(randomizerSettings.heatProtection);

            if (randomizerSettings.suitDamageReduction == "Progressive")
            {
                MessageBox.Show("Progressive damage reduction is not yet supported!");
                return;
            }

            /*  */

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save output iso to :";
                saveFileDialog.Filter = "Wii ISO File|*.iso";
                saveFileDialog.FileName = gc_iso_filename;
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    File.Delete(".\\tmp\\" + saveFileDialog.FileName);
                    return;
                }
                new_wii_iso_path = saveFileDialog.FileName;
            }

            NodManager.CreateISO(new_wii_iso_path, false);
            RandomizeDeveloperCode(new_wii_iso_path);
            if (this.checkBox1.Checked)
                File.Copy(".\\tmp\\"+gc_iso_filename.Replace(".iso", " - Spoiler.json"), new_wii_iso_path.Replace(".iso", " - Spoiler.json"));
            MessageBox.Show("Game has been randomized! Have fun!");
        }
    }
}
