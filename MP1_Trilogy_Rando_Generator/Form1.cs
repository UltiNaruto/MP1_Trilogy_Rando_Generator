using MP1_Trilogy_Rando_Generator.Patcher;
using MP1_Trilogy_Rando_Generator.Utils;
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
        readonly Random rand;
        static readonly Config.AppSettings appSettings = new Config.AppSettings();
        static readonly List<String> UsedDeveloperCodes = new List<String>();
        bool IsTemplateISOGenerated()
        {
            return File.Exists("gc_template.iso") && Directory.Exists(".\\tmp\\wii");
        }

        bool IsMetroidPrimeTrilogyNTSC(string fileName)
        {
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                String GameID = Encoding.ASCII.GetString(bR.ReadBytes(6));
                if (GameID.Substring(0, 3) != "R3M")
                    return false;
                return GameID[3] == 'E';
            }
        }

        bool IsMetroidPrimeNTSC(string fileName)
        {
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                String GameID = Encoding.ASCII.GetString(bR.ReadBytes(6));
                if (GameID.Substring(0, 3) != "GM8")
                    return false;
                return GameID[3] == 'E';
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

        void SetStatus(String status, int cur=0, int len=0)
        {
            this.label3.Text = "Status : " + status;
            if(len > 1)
                this.label3.Text += " (" + (cur+1) + " / " + len + ")";
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
            if (appSettings.prime1RandomizerPath.EndsWith(".exe") && File.Exists(appSettings.prime1RandomizerPath))
                this.label2.Text = "BashPrime's Randomizer found!";
            this.textBox1.Text = appSettings.outputPath;
            this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(appSettings.outputType);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var wii_iso_path = default(String);
            var gc_iso_path = default(String);

            MessageBox.Show(@"/!\ This operation can take 10 mins or more on a 5400 RPM HDD. So please be patient!");

            if (Directory.Exists("tmp"))
                Directory.Delete("tmp", true);

            using (var openFileDialog = new OpenFileDialog())
            {
                var dialogResult = default(DialogResult);

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
                        if (!IsMetroidPrimeNTSC(openFileDialog.FileName))
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

            FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\fe", true);
            FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\MP2", true);
            FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\MP3", true);
            FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", "rs5mp1_p.dol");
            foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\MP1", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                File.Copy(file, ".\\tmp\\gc\\files\\"+Path.GetFileName(file), true);
            File.Copy(".\\tmp\\wii\\DATA\\files\\MP1\\NoARAM.pak", ".\\tmp\\gc\\files\\NoARAM.pak", true);
            if (!Directory.Exists(".\\tmp\\dol_backup\\"))
                Directory.CreateDirectory(".\\tmp\\dol_backup\\");
            File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\dol_backup\\rs5mp1_p.dol", true);

            SetProgressStatus(3, 4);
            SetStatus("Creating Trilogy ISO template to be used with BashPrime's Randomizer");

            NodManager.CreateISO("gc_template.iso", true, "R3ME01");
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
            Directory.Delete("tmp", true);
            this.label1.Text = "No Trilogy ISO template for BashPrime's Randomizer detected!(Only NTSC - U supported for now)";
            this.button1.Enabled = true;
            this.button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                var dialogResult = default(DialogResult);

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
            var new_wii_iso_path = default(String);
            var gc_iso_filename = default(String);
            var spoiler_filename = default(String);

            String curDir = Directory.GetCurrentDirectory();

            if (!IsTemplateISOGenerated())
            {
                MessageBox.Show("Please click on \"Generate a template ISO for Prime 1 Randomizer\" before attempting to randomize!");
                return;
            }

            if (appSettings.prime1RandomizerPath == "" || !File.Exists(appSettings.prime1RandomizerPath))
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

            Config.PatchSettings patchSettingsBak = new Config.PatchSettings();
            new Config.PatchSettings((curDir + @"\gc_template.iso").Replace("\\", "\\\\"), (curDir + @"\tmp").Replace("\\", "\\\\")).SaveToJson();

            SetProgressStatus(0, 5);
            SetStatus("Running BashPrime's Randomizer...");
            if (!RandomizerManager.Run(appSettings.prime1RandomizerPath))
            {
                MessageBox.Show("Prime 1 Randomizer haven't properly exited! Cancelling...");
                patchSettingsBak.SaveToJson();
                SetProgressStatus(5, 5);
                SetStatus("Idle");
                return;
            }

            patchSettingsBak.SaveToJson();

            if(Directory.EnumerateFiles(".\\tmp", "*.iso").Count() == 0)
            {
                MessageBox.Show("No Randomized ISO generated! Cancelling...");
                SetProgressStatus(5, 5);
                SetStatus("Idle");
                return;
            }
            int i = 0;
            String[] gc_iso_files = Directory.EnumerateFiles(".\\tmp", "*.iso").ToArray();
            foreach (var gc_iso_file in gc_iso_files)
            {
                gc_iso_filename = Path.GetFileName(gc_iso_file);
                spoiler_filename = Path.ChangeExtension(gc_iso_filename, ".json").Replace(".json", " - Spoiler.json");

                SetProgressStatus(1, 5);
                SetStatus("Extracting randomized ISO...", i, gc_iso_files.Length);

                if (!NodManager.ExtractISO(".\\tmp\\" + gc_iso_filename, true))
                {
                    MessageBox.Show("Failed extracting randomized iso!");
                    SetProgressStatus(5, 5);
                    SetStatus("Idle");
                    return;
                }

                File.Delete(".\\tmp\\" + gc_iso_filename);

                var randomizerSettings = new Config.RandomizerSettings(".\\tmp\\gc\\files\\randomprime.txt");

                SetProgressStatus(2, 5);
                SetStatus("Replacing original PAKs with randomized PAKs...", i, gc_iso_files.Length);

                File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", true);

                foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                    File.Copy(file, ".\\tmp\\wii\\DATA\\files\\MP1\\" + Path.GetFileName(file), true);

                File.Copy(".\\tmp\\gc\\files\\NoARAM.pak", ".\\tmp\\wii\\DATA\\files\\MP1\\NoARAM.pak", true);
                File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

                Directory.Delete(".\\tmp\\gc", true);

                SetProgressStatus(3, 5);
                SetStatus("Applying patches to MP1 executable...", i, gc_iso_files.Length);

                /* Applying patches to dol file */

                if (randomizerSettings.skipFrigate)
                {
                    if (randomizerSettings.spawnRoom == null)
                    {
                        MessageBox.Show("No spawn room defined!");
                        SetProgressStatus(5, 5);
                        SetStatus("Idle");
                        return;
                    }
                    Patches.SetStartingArea(randomizerSettings.spawnRoom);
                }

                //Patches.DisableHintSystem(true);
                Patches.ApplySkipCutscenePatch(true);
                Patches.ApplyHeatProtectionPatch(randomizerSettings.heatProtection);
                Patches.ApplySuitDamageReductionPatch(randomizerSettings.suitDamageReduction);
                Patches.ApplyScanDashPatch(true);
                Patches.ApplyUnderwaterSlopeJumpFixPatch(true);
                //Patches.ApplyLJumpFixPatch(true);

                /*  */

                SetProgressStatus(4, 5);
                SetStatus("Packing Metroid Prime Trilogy to " + ((String)comboBox1.SelectedItem).Substring(1).ToUpper() + " format...", i, gc_iso_files.Length);
                // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
                if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".ciso"))
                    WITManager.CreateCompressISO(".\\tmp\\mpt.ciso", false, "R3ME" + RandomizeDeveloperCode());
                else if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".wbfs"))
                    WITManager.CreateWBFS(".\\tmp\\mpt.wbfs", "R3ME" + RandomizeDeveloperCode());

                if (File.Exists(this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem))
                    File.Delete(this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem);
                File.Move(".\\tmp\\mpt" + (String)comboBox1.SelectedItem, this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem);

                if (File.Exists(".\\tmp" + spoiler_filename))
                {
                    if (File.Exists(Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename))
                        File.Delete(Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename);
                    File.Move(".\\tmp\\" + spoiler_filename, Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename);
                }
                i++;
            }
            SetProgressStatus(5, 5);
            SetStatus("Idle");
            MessageBox.Show(gc_iso_files.Length + " ISO"+(gc_iso_files.Length > 1 ? "s have" :" has") +" been randomized! Have fun!");
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
            e.Cancel = true;
        }
    }
}
