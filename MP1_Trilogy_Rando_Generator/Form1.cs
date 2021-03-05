using MP1_Trilogy_Rando_Generator.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
            try
            {
                return File.Exists("gc_template.iso") &&
                      Directory.Exists(".\\tmp\\wii") &&
                      (IsMetroidPrimeTrilogyNTSC_UOrPAL(".\\tmp\\wii\\DATA\\sys\\boot.bin") || IsMetroidPrimeWiiNTSC_J(".\\tmp\\wii\\DATA\\sys\\boot.bin"));
            } catch {
                return false;
            }
        }

        bool IsMetroidPrimeTrilogyNTSC_UOrPAL(string fileName)
        {
            String GameID = GetGameID(fileName);
            if (GameID.Substring(0, 3) != "R3M")
                return false;
            return GameID[3] == 'E' || GameID[3] == 'P';
        }

        bool IsMetroidPrimeWiiNTSC_J(string fileName)
        {
            String GameID = GetGameID(fileName);
            return GameID.Substring(0, 3) == "R3I" && GameID[3] == 'J';
        }

        String GetGameID(string fileName)
        {
            if (!File.Exists(fileName))
                return "";
            using (var bR = new BinaryReader(File.OpenRead(fileName)))
            {
                return Encoding.ASCII.GetString(bR.ReadBytes(6));
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
            if (!Directory.Exists(@".\tmp"))
                Directory.CreateDirectory(@".\tmp");

            if (File.Exists("udc.bin"))
                UsedDeveloperCodes.AddRange(File.ReadAllLines("udc.bin"));
            if (IsTemplateISOGenerated())
            {
                this.label1.Text = "Metroid Prime Wii ISO template for Prime 1 Randomizer detected!";
                this.button1.Enabled = false;
                this.button2.Enabled = true;
            }
            if (appSettings.prime1RandomizerPath.EndsWith(".exe") && File.Exists(appSettings.prime1RandomizerPath))
                this.label2.Text = "BashPrime's Randomizer found!";
            this.textBox1.Text = appSettings.outputPath;
            this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(appSettings.outputType);

            if (!NodManager.Installed())
            {
                SetProgressStatus(1, 2);
                SetStatus("Downloading and installing nod...");
                if (!NodManager.Init())
                {
                    MessageBox.Show("Couldn't download nod");
                    this.Close();
                }
                SetProgressStatus(2, 2);
                SetStatus("Idle");
            }

            if (!WITManager.Installed())
            {
                SetProgressStatus(1, 2);
                SetStatus("Downloading and installing WIT...");
                if (!WITManager.Init())
                {
                    MessageBox.Show("Couldn't download WIT");
                    this.Close();
                }
                SetProgressStatus(2, 2);
                SetStatus("Idle");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var wii_iso_path = default(String);
            var GameID = default(String);

            MessageBox.Show(@"/!\ This operation can take 10 mins or more on a 5400 RPM HDD. So please be patient!");

            using (var openFileDialog = new OpenFileDialog())
            {
                var dialogResult = default(DialogResult);

                openFileDialog.Title = "Select a NTSC-U iso of Metroid Prime Trilogy";
                openFileDialog.Filter = "Wii ISO File|*.iso|Wii NKit ISO File|*.nkit.iso;*.nkit.iso";
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
                        if (!IsMetroidPrimeTrilogyNTSC_UOrPAL(openFileDialog.FileName) && !IsMetroidPrimeWiiNTSC_J(openFileDialog.FileName))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a Metroid Prime Wii iso (NTSC-U/NTSC-J/PAL)");
                            continue;
                        }
                        GameID = GetGameID(openFileDialog.FileName);
                        wii_iso_path = openFileDialog.FileName;
                    }
                    if (dialogResult == DialogResult.Cancel)
                        return;
                }
            }

            SetProgressStatus(0, 5);
            SetStatus("Extracting Metroid Prime Wii ISO...");
            if (wii_iso_path.ToLower().EndsWith(".nkit.iso") || wii_iso_path.ToLower().EndsWith(".nkit.gcz"))
            {
                if (!NKitManager.ExtractISO(wii_iso_path))
                {
                    MessageBox.Show("Failed extracting wii nkit iso!");
                    SetProgressStatus(5, 5);
                    SetStatus("Idle");
                    return;
                }

                try {
                    if(GameID == "R3ME01")
                        File.WriteAllBytes(@".\tmp\nkit\nkit_files.zip", Properties.Resources.R3ME01_nkit);
                    if (GameID == "R3MP01")
                        File.WriteAllBytes(@".\tmp\nkit\nkit_files.zip", Properties.Resources.R3MP01_nkit);
                    if (GameID == "R3IJ01")
                        File.WriteAllBytes(@".\tmp\nkit\nkit_files.zip", Properties.Resources.R3IJ01_nkit);
                    ZipFile.ExtractToDirectory(@".\tmp\nkit\nkit_files.zip", @".\tmp\nkit\DATA");
                    File.Move(@".\tmp\nkit\DATA\files\main.dol", @".\tmp\nkit\DATA\sys\main.dol");
                    File.Delete(@".\tmp\nkit\nkit_files.zip");
                    File.Delete(@".\tmp\nkit\DATA\files\boot.bin");
                    File.Delete(@".\tmp\nkit\DATA\files\appldr.bin");
                    File.Delete(@".\tmp\nkit\DATA\files\bi2.bin");
                    File.Delete(@".\tmp\nkit\DATA\files\fst.bin");
                    File.Delete(@".\tmp\nkit\DATA\sys\R3MEhdr.bin");
                    File.Delete(@".\tmp\nkit\DATA\sys\hdr.bin");
                    Directory.Move(@".\tmp\nkit", @".\tmp\wii");
                } catch {
                    Directory.Delete(@".\tmp\nkit", true);
                }
            }
            else if (wii_iso_path.ToLower().EndsWith(".iso"))
            {
                if (!NodManager.ExtractISO(wii_iso_path, false))
                {
                    MessageBox.Show("Failed extracting wii iso!");
                    SetProgressStatus(5, 5);
                    SetStatus("Idle");
                    return;
                }
            }

            if (!Directory.Exists(@".\tmp\wii"))
            {
                MessageBox.Show("Failed extracting wii iso!");
                SetProgressStatus(5, 5);
                SetStatus("Idle");
                return;
            }

            SetProgressStatus(1, 5);
            SetStatus("Backing up original DOLs...");
            
            if (!Directory.Exists(".\\tmp\\dol_backup\\"))
                Directory.CreateDirectory(".\\tmp\\dol_backup\\");

            if(GameID == "R3IJ01")
                File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol", ".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", true);
            else
                File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\dol_backup\\rs5mp1_p.dol", true);
            File.Copy(".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol", ".\\tmp\\dol_backup\\rs5fe_p.dol", true);
            
            SetProgressStatus(2, 5);
            SetStatus("Stripping unnecessary files from Metroid Prime ISO...");
            File.WriteAllBytes(".\\tmp\\wii\\DATA\\files\\fe\\Video\\mp1\\attract01.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\wii\\DATA\\files\\fe\\Video\\mp1\\Attract02.thp", Properties.Resources.dummy);
            Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe\\Audio\\MP1_Bonus", true);
            if (GameID != "R3IJ01")
            {
                File.WriteAllBytes(".\\tmp\\wii\\DATA\\files\\fe\\Video\\FrontEnd\\attract01.thp", Properties.Resources.dummy);
                File.WriteAllBytes(".\\tmp\\wii\\DATA\\files\\fe\\Video\\FrontEnd\\Attract02.thp", Properties.Resources.dummy);
                FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\MP2", true);
                FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\MP3", true);
                Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe\\Video\\mp2", true);
                foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\fe\\Video\\FrontEnd", "Menu_To_Game_MP2_*.thp"))
                    File.Delete(file);
                foreach (var file in Directory.EnumerateFiles(".\\tmp\\wii\\DATA\\files\\fe\\Video\\FrontEnd", "Menu_To_Game_MP3_*.thp"))
                    File.Delete(file);
                Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe\\Audio\\MP2_Bonus", true);
                Directory.Delete(".\\tmp\\wii\\DATA\\files\\fe\\Audio\\MP3_Bonus", true);
                FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", "rs5mp1_p.dol", "rs5fe_p.dol");
            }
            else
                FileUtils.NullifyFiles(".\\tmp\\wii\\DATA\\files\\", "*.dol", "rs5mp1jpn_p.dol", "rs5fe_p.dol");

            SetProgressStatus(3, 5);
            SetStatus("Copying resources from Metroid Prime Wii to Template ISO...");

            Directory.CreateDirectory(".\\tmp\\gc\\files");
            Directory.CreateDirectory(".\\tmp\\gc\\sys");
            if (GameID == "R3IJ01")
            {
                DirectoryUtils.Copy(".\\tmp\\wii\\DATA\\files\\MP1JPN", ".\\tmp\\gc\\files\\", true);
                File.Copy(".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", ".\\tmp\\gc\\files\\default.dol", true);
            }
            else
            {
                DirectoryUtils.Copy(".\\tmp\\wii\\DATA\\files\\MP1", ".\\tmp\\gc\\files\\", true);
                File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\gc\\files\\default.dol", true);
            }
            Directory.Delete(".\\tmp\\gc\\files\\RSO", true);
            Directory.Delete(".\\tmp\\gc\\files\\rhbm", true);
            DirectoryUtils.Copy(".\\tmp\\wii\\DATA\\sys", ".\\tmp\\gc\\sys\\", true);
            if (GameID == "R3IJ01")
                File.Copy(".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", ".\\tmp\\gc\\sys\\main.dol", true);
            else
                File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\gc\\sys\\main.dol", true);
            FileUtils.Write<UInt32>(".\\tmp\\gc\\sys\\boot.bin", 0x18, (UInt32)0, true);
            FileUtils.Write<UInt32>(".\\tmp\\gc\\sys\\boot.bin", 0x1C, (UInt32)0xC2339F3D, true);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\02_start_fileselect_A.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\02_start_fileselect_B.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\02_start_fileselect_C.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\04_fileselect_playgame_A.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\04_fileselect_playgame_B.thp", Properties.Resources.dummy);
            File.WriteAllBytes(".\\tmp\\gc\\files\\Video\\04_fileselect_playgame_C.thp", Properties.Resources.dummy);

            SetProgressStatus(4, 5);
            SetStatus("Creating Metroid Prime Wii ISO template to be used with BashPrime's Randomizer");

            NodManager.CreateISO("gc_template.iso", true, GameID);

            SetProgressStatus(5, 5);
            SetStatus("Idle");
            
            Directory.Delete(".\\tmp\\gc", true);
            this.label1.Text = "Metroid Prime Wii ISO template for BashPrime's Randomizer detected!";
            this.button1.Enabled = false;
            this.button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete("gc_template.iso");
            Directory.Delete("tmp", true);
            this.label1.Text = "No Metroid Prime Wii ISO template for BashPrime's Randomizer detected!";
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
                            this.label2.Text = "BashPrime's Randomizer not found! (v2.5.0 or later)";
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
            var GameID = default(String);
            var PatchedGameID = default(String);
            var Pak_Path = default(String);

            String GameSettingsDolphinEmuPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar+ "Dolphin Emulator" + Path.DirectorySeparatorChar + "GameSettings" + Path.DirectorySeparatorChar;
            String curDir = Directory.GetCurrentDirectory();

            if (!IsTemplateISOGenerated())
            {
                MessageBox.Show("Please click on \"Generate a template ISO for BashPrime's Randomizer\" before attempting to randomize!");
                return;
            }

            if (!IsMetroidPrimeTrilogyNTSC_UOrPAL("gc_template.iso") && !IsMetroidPrimeWiiNTSC_J("gc_template.iso"))
                return;

            GameID = GetGameID("gc_template.iso");

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
                MessageBox.Show("BashPrime's Randomizer hasn't properly exited! Cancelling...");
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

                Pak_Path = ".\\tmp\\wii\\DATA\\files\\MP1";
                if (GameID.Substring(2, 2) == "IJ")
                    Pak_Path += "JPN";

                File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol", true);
                if(GameID[3] == 'J')
                    File.Copy(".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol", true);
                else
                    File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", true);

                foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                    File.Copy(file, Pak_Path + "\\" + Path.GetFileName(file), true);

                File.Copy(".\\tmp\\gc\\files\\NoARAM.pak", Pak_Path + "\\NoARAM.pak", true);
                File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

                Directory.Delete(".\\tmp\\gc", true);

                SetProgressStatus(3, 5);
                SetStatus("Applying patches to MP1 executable...", i, gc_iso_files.Length);

                PatchedGameID = GameID.Substring(0, 4) + RandomizeDeveloperCode();

                /* Applying patches to dol file */

                Patcher.Patcher.Init(PatchedGameID[3]);

                if (randomizerSettings.skipFrigate)
                {
                    if (randomizerSettings.spawnRoom == null)
                    {
                        MessageBox.Show("No spawn room defined!");
                        SetProgressStatus(5, 5);
                        SetStatus("Idle");
                        return;
                    }
                    Patcher.Patcher.SetStartingArea(randomizerSettings.spawnRoom);
                }

                //new DOL_AddSection_Patch(Patches.MP1_Dol_Path, DOL_AddSection_Patch.SectionType.TEXT, 0x80001800, 0x800).Apply();

                //Patches.DisableHintSystem(true);
                Patcher.Patcher.ApplySkipCutscenePatch();
                Patcher.Patcher.ApplyHeatProtectionPatch(randomizerSettings.heatProtection);
                Patcher.Patcher.ApplySuitDamageReductionPatch(randomizerSettings.suitDamageReduction);
                Patcher.Patcher.ApplyScanDashPatch();
                Patcher.Patcher.ApplyUnderwaterSlopeJumpFixPatch(true);
                Patcher.Patcher.SetSaveFilename(PatchedGameID.Substring(4, 2) + ".bin");
                //Patches.ApplyInputPatch();

                /*  */

                SetProgressStatus(4, 5);
                SetStatus("Packing Metroid Prime Wii to " + ((String)comboBox1.SelectedItem).Substring(1).ToUpper() + " format...", i, gc_iso_files.Length);
                // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
                if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".ciso"))
                {
                    WITManager.CreateCompressISO(".\\tmp\\mpt.ciso", false, PatchedGameID);
                    if (File.Exists(GameSettingsDolphinEmuPath + GameID + ".ini") && !File.Exists(GameSettingsDolphinEmuPath + PatchedGameID + ".ini"))
                        File.Copy(GameSettingsDolphinEmuPath + GameID + ".ini", GameSettingsDolphinEmuPath + PatchedGameID + ".ini");
                }
                else if (((String)comboBox1.SelectedItem).ToLower().EndsWith(".wbfs"))
                    WITManager.CreateWBFS(".\\tmp\\mpt.wbfs", PatchedGameID);

                if (File.Exists(this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem))
                    File.Delete(this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem);
                if ((String)comboBox1.SelectedItem == ".wbfs")
                    new_wii_iso_path = this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + "["+ PatchedGameID + "]\\" + PatchedGameID + ".wbfs";
                else
                    new_wii_iso_path = this.textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)comboBox1.SelectedItem;

                if (!Directory.Exists(Path.GetDirectoryName(new_wii_iso_path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(new_wii_iso_path));

                File.Move(".\\tmp\\mpt" + (String)comboBox1.SelectedItem, new_wii_iso_path);

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

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // Closing any nod or wit application running
            ProcessUtils.KillChildrenProcesses(Process.GetCurrentProcess().Id);
        }
    }
}
