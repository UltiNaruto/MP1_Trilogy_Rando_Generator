using MP1_Trilogy_Rando_Generator.Patcher;
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

        String RandomizeDeveloperCode()
        {
            const String AllowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int len = AllowedCharacters.Length;
            String DeveloperCode = "01";
            while (DeveloperCode == "01" || UsedDeveloperCodes.Contains(DeveloperCode))
                DeveloperCode = "" + AllowedCharacters[rand.Next(len)] + AllowedCharacters[rand.Next(len)];
            UsedDeveloperCodes.Add(DeveloperCode);
            File.WriteAllLines("udc.bin", UsedDeveloperCodes.ToArray());
            return DeveloperCode;
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

            File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP2", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP3", true);

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", SearchOption.TopDirectoryOnly))
                if (!file.Contains("rs5mp1_p.dol"))
                    File.Delete(file);

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

            if (!Directory.Exists(@".\tmp"))
                Directory.CreateDirectory(@".\tmp");

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

                    File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP2", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP3", true);

                    foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", SearchOption.TopDirectoryOnly))
                        if(!file.Contains("rs5mp1_p.dol"))
                            File.Delete(file);
                }
            }

            NodManager.ExtractISO(".\\tmp\\"+ gc_iso_filename, true);

            File.Delete(".\\tmp\\" + gc_iso_filename);

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\wii\\DATA\\files\\MP1\\" + Path.GetFileName(file), true);

            File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

            Directory.Delete(".\\tmp\\gc", true);

            /* Applying patches to dol file */

            if(randomizerSettings.skipFrigate)
            {
                if(randomizerSettings.spawnRoom == null)
                {
                    MessageBox.Show("No spawn room defined!");
                    return;
                }
                Patches.SetStartingArea(randomizerSettings.spawnRoom);
            }

            Patches.ApplySkipCutscenePatch(true);
            Patches.ApplyHeatProtectionPatch(randomizerSettings.heatProtection);

            if (randomizerSettings.suitDamageReduction == "Progressive")
            {
                MessageBox.Show("Progressive damage reduction is not yet supported!");
                return;
            }

            Patches.ApplyScanDashPatch(true);

            /*  */

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save output iso to :";
                saveFileDialog.Filter = "Wii Compressed ISO File|*.ciso|Wii WBFS File|*.wbfs";
                saveFileDialog.FileName = Path.ChangeExtension(gc_iso_filename, ".ciso");
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    File.Delete(".\\tmp\\" + saveFileDialog.FileName);
                    return;
                }
                new_wii_iso_path = saveFileDialog.FileName;
            }

            // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
            if (new_wii_iso_path.ToLower().EndsWith(".ciso"))
                WITManager.CreateCompressISO(".\\tmp\\mpt.ciso", false, "R3ME"+RandomizeDeveloperCode());
            else if (new_wii_iso_path.ToLower().EndsWith(".wbfs"))
                WITManager.CreateWBFS(".\\tmp\\mpt.wbfs", "R3ME" + RandomizeDeveloperCode());

            File.Move(".\\tmp\\mpt" + Path.GetExtension(new_wii_iso_path), new_wii_iso_path);

            if (this.checkBox1.Checked)
                File.Copy(".\\tmp\\"+Path.ChangeExtension(gc_iso_filename, ".json").Replace(".json", " - Spoiler.json"), Path.ChangeExtension(new_wii_iso_path, ".json").Replace(".json", " - Spoiler.json"));
            MessageBox.Show("Game has been randomized! Have fun!");
        }
    }
}
