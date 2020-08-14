using MP1_Trilogy_Rando_Generator.Patcher;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        void SetProgressStatus(int cur, int max)
        {
            if (cur != max)
            {
                this.progressBar1.Visible = true;
                this.progressBar1.Value = (cur * 100) / (max - 1);
            }
            else
            {
                this.progressBar1.Visible = false;
                this.progressBar1.Value = 0;
            }
            this.progressBar1.Update();
        }

        void SetStatus(String status)
        {
            this.label3.Text = "Status : " + status;
            this.label3.Update();
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
            if (appSettings.prime1RandomizerPath.EndsWith(".exe"))
                this.label2.Text = "BashPrime's Randomizer found!";
            this.textBox1.Text = appSettings.outputPath;
            this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(appSettings.outputType);
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
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

            SetProgressStatus(0, 4);
            SetStatus("Extracting Metroid Prime Trilogy ISO...");
            if (!NodManager.ExtractISO(wii_iso_path, false))
            {
                MessageBox.Show("Failed extracting wii iso!");
                return;
            }

            SetProgressStatus(1, 4);
            SetStatus("Extracting Metroid Prime ISO...");
            if (!NodManager.ExtractISO(gc_iso_path, true))
            {
                Directory.Delete(".\\tmp\\wii", true);
                MessageBox.Show("Failed extracting gc iso!");
                return;
            }

            SetProgressStatus(2, 4);
            SetStatus("Stripping MP2 and MP3 from Metroid Prime Trilogy...");
            File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP2", true);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP3", true);

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", SearchOption.TopDirectoryOnly))
                if (!file.Contains("rs5mp1_p.dol"))
                    File.Delete(file);

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\MP1", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\gc\\files\\"+Path.GetFileName(file), true);
            SetProgressStatus(3, 4);
            SetStatus("Creating Trilogy ISO template to be used with BashPrime's Randomizer");
            NodManager.CreateISO("gc_template.iso", true);
            Directory.Delete(".\\tmp\\gc", true);
            this.label1.Text = "Trilogy ISO template for BashPrime's Randomizer detected!";
            this.button1.Enabled = false;
            this.button2.Enabled = true;
            SetProgressStatus(4, 4);
            SetStatus("Idle");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete("gc_template.iso");
            this.label1.Text = "No Trilogy ISO template for BashPrime's Randomizer detected!(Only NTSC - U supported for now)";
            this.button1.Enabled = true;
            this.button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dialogResult = default(DialogResult);
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select the executable of BashPrime's Randomizer";
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
                        this.label2.Text = "BashPrime's Randomizer found!";
                    }
                    if (dialogResult == DialogResult.Cancel)
                    {
                        if (!appSettings.prime1RandomizerPath.EndsWith(".exe"))
                            this.label2.Text = "BashPrime's Randomizer not found! (v2.2.2 required at least)";
                        return;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dialogResult = default(DialogResult);
            var wii_iso_path = default(String);
            var new_wii_iso_path = default(String);
            var gc_iso_filename = default(String);
            var spoiler_filename = default(String);
            var curDir = default(String);
            var randomizerSettings = default(Config.RandomizerSettings);

            curDir = Directory.GetCurrentDirectory();

            if(appSettings.prime1RandomizerPath == "")
            {
                MessageBox.Show("Please click on \"Locate BashPrime's Randomizer\" before attempting to randomize!");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select an output type to save the iso in that format!");
                return;
            }

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("Select a folder to save the "+ comboBox1.SelectedItem + " file !");
                return;
            }

            if (!Directory.Exists(@".\tmp"))
                Directory.CreateDirectory(@".\tmp");

            Config.PatchSettings patchSettingsBak = new Config.PatchSettings();
            Config.PatchSettings patchSettingsWii = new Config.PatchSettings((curDir + @"\gc_template.iso").Replace("\\", "\\\\"), (curDir + @"\tmp").Replace("\\", "\\\\"));
            patchSettingsWii.SaveToJson();

            SetProgressStatus(0, 5);
            SetStatus("Running BashPrime's Randomizer...");
            if (!RandomizerManager.Run(appSettings.prime1RandomizerPath))
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
            spoiler_filename = Path.ChangeExtension(gc_iso_filename, ".json").Replace(".json", " - Spoiler.json");

            if (!Directory.Exists(".\\tmp\\wii"))
            {
                SetStatus("Extracting Metroid Prime Trilogy ISO...");
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

                    SetStatus("Stripping MP2 and MP3 from Metroid Prime Trilogy...");

                    File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP2", true);
                    Directory.Delete(".\\tmp\\wii\\DATA\\files\\MP3", true);

                    foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", SearchOption.TopDirectoryOnly))
                        if(!file.Contains("rs5mp1_p.dol"))
                            File.Delete(file);
                }
            }

            File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);

            SetProgressStatus(1, 5);
            SetStatus("Extracting randomized ISO...");

            NodManager.ExtractISO(".\\tmp\\"+ gc_iso_filename, true);

            File.Delete(".\\tmp\\" + gc_iso_filename);

            randomizerSettings = new Config.RandomizerSettings(".\\tmp\\gc\\files\\randomprime.txt");

            SetProgressStatus(2, 5);
            SetStatus("Replacing original PAKs with randomized PAKs...");

            foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\wii\\DATA\\files\\MP1\\" + Path.GetFileName(file), true);

            File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

            Directory.Delete(".\\tmp\\gc", true);

            SetProgressStatus(3, 5);
            SetStatus("Applying patches to MP1 executable...");

            /* Applying patches to dol file */

            if (randomizerSettings.skipFrigate)
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

            /*if (randomizerSettings.suitDamageReduction == "Progressive")
            {
                MessageBox.Show("Progressive damage reduction is not yet supported!");
                return;
            }*/

            Patches.ApplyScanDashPatch(true);
            Patches.ApplyUnderwaterSlopeJumpFixPatch(true);

            /*  */


            SetProgressStatus(4, 5);
            SetStatus("Packing Metroid Prime Trilogy to "+ ((String)comboBox1.SelectedItem).Substring(1).ToUpper() + " format...");
            // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
            if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".ciso"))
                WITManager.CreateCompressISO(".\\tmp\\mpt.ciso", false, "R3ME"+RandomizeDeveloperCode());
            else if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".wbfs"))
                WITManager.CreateWBFS(".\\tmp\\mpt.wbfs", "R3ME" + RandomizeDeveloperCode());

            File.Move(".\\tmp\\mpt" + (String)comboBox1.SelectedItem, this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem);

            if (File.Exists(".\\tmp"+ spoiler_filename))
                File.Move(".\\tmp\\"+ spoiler_filename, Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename);
            SetProgressStatus(5, 5);
            SetStatus("Idle");
            MessageBox.Show("Game has been randomized! Have fun!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBox1.Text = folderBrowserDialog.SelectedPath;
                    appSettings.outputPath = folderBrowserDialog.SelectedPath;
                    appSettings.SaveToJson();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                appSettings.outputType = (String)comboBox1.SelectedItem;
                appSettings.SaveToJson();
            }
        }

        private void helpBtn_Click(object sender, CancelEventArgs e)
        {
            try {
                new HelpForm().ShowDialog();
            } catch {
                MessageBox.Show("No wiki files found! Connect to internet to download the wiki files!");
            }
        }
    }
}
