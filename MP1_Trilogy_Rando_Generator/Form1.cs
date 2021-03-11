using DarkUI.Forms;
using MP1_Trilogy_Rando_Generator.Enums;
using MP1_Trilogy_Rando_Generator.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace MP1_Trilogy_Rando_Generator
{
    public partial class main_form : DarkForm
    {
        internal bool isLoading = false;
        static readonly Config.AppSettings appSettings = new Config.AppSettings();
        bool IsTemplateISOGenerated()
        {
            try
            {
                return File.Exists("gc_template.iso") &&
                      Directory.Exists(".\\tmp\\wii") &&
                      (DiscUtils.IsMetroidPrimeTrilogyNTSC_UOrPAL(".\\tmp\\wii\\DATA\\sys\\boot.bin") || DiscUtils.IsMetroidPrimeWiiNTSC_J(".\\tmp\\wii\\DATA\\sys\\boot.bin"));
            } catch {
                return false;
            }
        }

        public main_form()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
            FormUtils.Init(this);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@".\tmp"))
                Directory.CreateDirectory(@".\tmp");

            if (IsTemplateISOGenerated())
            {
                this.template_iso_lbl.Text = "Metroid Prime Wii ISO template for Prime 1 patcher detected!";
                this.generate_template_iso_btn.Enabled = false;
                this.delete_cache_btn.Enabled = true;
            }
            if (appSettings.prime1RandomizerPath.EndsWith(".exe") && File.Exists(appSettings.prime1RandomizerPath))
                this.randomizer_lbl.Text = "BashPrime's Randomizer found!";
            if (appSettings.prime1PlandomizerPath.EndsWith(".exe") && File.Exists(appSettings.prime1PlandomizerPath))
                this.plandomizer_lbl.Text = "toasterparty's Plandomizer found!";
            this.input_json_txt_box.Text = appSettings.prime1PlandomizerLastJsonPath;
            this.output_path_txt_box.Text = appSettings.outputPath;
            this.output_type_combo_box.SelectedIndex = this.output_type_combo_box.Items.IndexOf(appSettings.outputType);

            if (!ISOUtils.NOD.Installed())
            {
                FormUtils.SetProgressStatus(1, 2);
                FormUtils.SetStatus("Downloading and installing nod...");
                if (!ISOUtils.NOD.Init())
                {
                    MessageBox.Show("Couldn't download nod");
                    this.Close();
                }
                FormUtils.SetProgressStatus(2, 2);
                FormUtils.SetStatus("Idle");
            }

            if (!ISOUtils.WIT.Installed())
            {
                FormUtils.SetProgressStatus(1, 2);
                FormUtils.SetStatus("Downloading and installing WIT...");
                if (!ISOUtils.WIT.Init())
                {
                    MessageBox.Show("Couldn't download WIT");
                    this.Close();
                }
                FormUtils.SetProgressStatus(2, 2);
                FormUtils.SetStatus("Idle");
            }
        }

        private void generate_template_iso_btn_click(object sender, EventArgs e)
        {
            var wii_iso_path = default(String);
            var GameID = default(String);

            if(isLoading && ((Button)sender).Text == "Cancel")
            {
                ProcessUtils.KillChildrenProcesses(Process.GetCurrentProcess().Id);
                if (Directory.Exists(@".\tmp"))
                    Directory.Delete(@".\tmp", true);
                if(File.Exists(@".\gc_template.iso"))
                    File.Delete(@".\gc_template.iso");
                FormUtils.ShowMessageBox("Cancelled generation!");
                FormUtils.SetControlText(this.generate_template_iso_btn, "Generate a template ISO for the patcher");
                isLoading = false;
                return;
            }

            isLoading = true;
            MessageBox.Show(@"/!\ This operation can take 10 mins or more on a 5400 RPM HDD. So please be patient!");

            using (var openFileDialog = new OpenFileDialog())
            {
                var dialogResult = default(DialogResult);

                openFileDialog.Title = "Select an iso of Metroid Prime Trilogy or Metroid Prime New Play Controls";
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
                        if (!DiscUtils.IsMetroidPrimeTrilogyNTSC_UOrPAL(openFileDialog.FileName) && !DiscUtils.IsMetroidPrimeWiiNTSC_J(openFileDialog.FileName))
                        {
                            openFileDialog.FileName = "";
                            MessageBox.Show("Select a Metroid Prime Wii iso (NTSC-U/NTSC-J/PAL)");
                            continue;
                        }
                        GameID = DiscUtils.GetGameID(openFileDialog.FileName);
                        wii_iso_path = openFileDialog.FileName;
                    }
                    if (dialogResult == DialogResult.Cancel)
                    {
                        isLoading = false;
                        return;
                    }
                }
            }

            FormUtils.RunAsynchrousTask(new Action(() =>
            {
                FormUtils.SetControlText(this.generate_template_iso_btn, "Cancel");
                FormUtils.SetProgressStatus(0, 5);
                FormUtils.SetStatus("Extracting Metroid Prime Wii ISO...");
                if (wii_iso_path.ToLower().EndsWith(".nkit.iso") || wii_iso_path.ToLower().EndsWith(".nkit.gcz"))
                {
                    if (!ISOUtils.NKIT.ExtractISO(wii_iso_path))
                    {
                        FormUtils.ShowMessageBox("Failed extracting wii nkit iso!");
                        FormUtils.SetProgressStatus(5, 5);
                        FormUtils.SetStatus("Idle");
                        FormUtils.SetControlText(this.generate_template_iso_btn, "Generate a template ISO for the patcher");
                        isLoading = false;
                        return;
                    }

                    try
                    {
                        if (GameID == "R3ME01")
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
                    }
                    catch
                    {
                        Directory.Delete(@".\tmp\nkit", true);
                    }
                }
                else if (wii_iso_path.ToLower().EndsWith(".iso"))
                {
                    if (!ISOUtils.NOD.ExtractISO(wii_iso_path, false))
                    {
                        FormUtils.ShowMessageBox("Failed extracting wii iso!");
                        FormUtils.SetProgressStatus(5, 5);
                        FormUtils.SetStatus("Idle");
                        FormUtils.SetControlText(this.generate_template_iso_btn, "Generate a template ISO for the patcher");
                        isLoading = false;
                        return;
                    }
                }

                if (!Directory.Exists(@".\tmp\wii"))
                {
                    FormUtils.ShowMessageBox("Failed extracting wii iso!");
                    FormUtils.SetProgressStatus(5, 5);
                    FormUtils.SetStatus("Idle");
                    FormUtils.SetControlText(this.generate_template_iso_btn, "Generate a template ISO for the patcher");
                    isLoading = false;
                    return;
                }

                FormUtils.SetProgressStatus(1, 5);
                FormUtils.SetStatus("Backing up original DOLs...");

                if (!Directory.Exists(".\\tmp\\dol_backup\\"))
                    Directory.CreateDirectory(".\\tmp\\dol_backup\\");

                if (GameID == "R3IJ01")
                    File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol", ".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", true);
                else
                    File.Copy(".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", ".\\tmp\\dol_backup\\rs5mp1_p.dol", true);
                File.Copy(".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol", ".\\tmp\\dol_backup\\rs5fe_p.dol", true);

                FormUtils.SetProgressStatus(2, 5);
                FormUtils.SetStatus("Stripping unnecessary files from Metroid Prime ISO...");
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

                FormUtils.SetProgressStatus(3, 5);
                FormUtils.SetStatus("Copying resources from Metroid Prime Wii to Template ISO...");

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

                FormUtils.SetProgressStatus(4, 5);
                FormUtils.SetStatus("Creating Metroid Prime Wii ISO template to be used with Prime 1 patcher");

                ISOUtils.NOD.CreateISO("gc_template.iso", true, GameID);

                FormUtils.SetProgressStatus(5, 5);
                FormUtils.SetStatus("Idle");

                Directory.Delete(".\\tmp\\gc", true);
                FormUtils.SetControlText(this.template_iso_lbl, "Metroid Prime Wii ISO template for Prime 1 patcher detected!");
                FormUtils.SetControlEnabled(this.generate_template_iso_btn, false);
                FormUtils.SetControlEnabled(this.delete_cache_btn, true);
                FormUtils.SetControlText(this.generate_template_iso_btn, "Generate a template ISO for the patcher");
                isLoading = false;
            }));
        }

        private void delete_cache_btn_click(object sender, EventArgs e)
        {
            File.Delete("gc_template.iso");
            Directory.Delete("tmp", true);
            FormUtils.SetControlText(this.template_iso_lbl, "No Metroid Prime Wii ISO template for Prime 1 patcher detected!");
            FormUtils.SetControlEnabled(this.generate_template_iso_btn, true);
            FormUtils.SetControlEnabled(this.delete_cache_btn, false);
        }

        private void locate_randomizer_btn_click(object sender, EventArgs e)
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
                        this.randomizer_lbl.Text = "BashPrime's Randomizer found!";
                    }
                    if (dialogResult == DialogResult.Cancel)
                    {
                        if (!appSettings.prime1RandomizerPath.EndsWith(".exe"))
                            this.randomizer_lbl.Text = "BashPrime's Randomizer not found! (v2.6.0 or later)";
                        return;
                    }
                }
            }
        }

        private void randomize_btn_click(object sender, EventArgs e)
        {
            var new_wii_iso_path = default(String);
            var gc_iso_filename = default(String);
            var spoiler_filename = default(String);
            var GameID = default(String);
            var PatchedGameID = default(String);
            var Pak_Path = default(String);

            String GameSettingsDolphinEmuPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar+ "Dolphin Emulator" + Path.DirectorySeparatorChar + "GameSettings" + Path.DirectorySeparatorChar;
            String curDir = Directory.GetCurrentDirectory();

            if (isLoading && ((Button)sender).Text == "Cancel")
            {
                ProcessUtils.KillChildrenProcesses(Process.GetCurrentProcess().Id);
                if (Directory.Exists(@".\tmp\gc"))
                    Directory.Delete(@".\tmp\gc", true);
                FormUtils.ShowMessageBox("Cancelled randomization!");
                FormUtils.SetControlText(this.randomize_btn, "Randomize");
                isLoading = false;
                return;
            }

            if (!IsTemplateISOGenerated())
            {
                MessageBox.Show("Please click on \"Generate a template ISO for BashPrime's Randomizer\" before attempting to randomize!");
                return;
            }

            if (!DiscUtils.IsMetroidPrimeTrilogyNTSC_UOrPAL("gc_template.iso") && !DiscUtils.IsMetroidPrimeWiiNTSC_J("gc_template.iso"))
                return;

            GameID = DiscUtils.GetGameID("gc_template.iso");

            if (appSettings.prime1RandomizerPath == "" || !File.Exists(appSettings.prime1RandomizerPath))
            {
                MessageBox.Show("Please click on \"Locate BashPrime's Randomizer\" before attempting to randomize!");
                return;
            }

            if (output_type_combo_box.SelectedIndex == -1)
            {
                MessageBox.Show("Select an output type to save the iso in that format!");
                return;
            }

            if (output_path_txt_box.Text == "")
            {
                MessageBox.Show("Select a folder to save the "+ output_type_combo_box.SelectedItem + " file !");
                return;
            }

            isLoading = true;

            FormUtils.RunAsynchrousTask(new Action(() =>
            {
                FormUtils.SetControlText(this.randomize_btn, "Cancel");
                Config.PatchSettings patchSettingsBak = new Config.PatchSettings();
                new Config.PatchSettings((curDir + @"\gc_template.iso").Replace("\\", "\\\\"), (curDir + @"\tmp").Replace("\\", "\\\\")).SaveToJson();

                FormUtils.SetProgressStatus(0, 5);
                FormUtils.SetStatus("Running BashPrime's Randomizer...");
                if (!ProcessUtils.Run(appSettings.prime1RandomizerPath))
                {
                    MessageBox.Show("BashPrime's Randomizer hasn't properly exited! Cancelling...");
                    patchSettingsBak.SaveToJson();
                    FormUtils.SetProgressStatus(5, 5);
                    FormUtils.SetStatus("Idle");
                    FormUtils.SetControlText(this.randomize_btn, "Randomize");
                    isLoading = false;
                    return;
                }

                patchSettingsBak.SaveToJson();

                if (Directory.EnumerateFiles(".\\tmp", "*.iso").Count() == 0)
                {
                    MessageBox.Show("No Randomized ISO generated! Cancelling...");
                    FormUtils.SetProgressStatus(5, 5);
                    FormUtils.SetStatus("Idle");
                    FormUtils.SetControlText(this.randomize_btn, "Randomize");
                    isLoading = false;
                    return;
                }
                int i = 0;
                String[] gc_iso_files = Directory.EnumerateFiles(".\\tmp", "*.iso").ToArray();
                foreach (var gc_iso_file in gc_iso_files)
                {
                    gc_iso_filename = Path.GetFileName(gc_iso_file);
                    spoiler_filename = Path.ChangeExtension(gc_iso_filename, ".json").Replace(".json", " - Spoiler.json");

                    FormUtils.SetProgressStatus(1, 5);
                    FormUtils.SetStatus("Extracting randomized ISO...", i, gc_iso_files.Length);

                    if (!ISOUtils.NOD.ExtractISO(".\\tmp\\" + gc_iso_filename, true))
                    {
                        FormUtils.ShowMessageBox("Failed extracting randomized iso!");
                        FormUtils.SetProgressStatus(5, 5);
                        FormUtils.SetStatus("Idle");
                        FormUtils.SetControlText(this.randomize_btn, "Randomize");
                        isLoading = false;
                        return;
                    }

                    File.Delete(".\\tmp\\" + gc_iso_filename);

                    var randomizerSettings = new Config.RandomizerSettings(".\\tmp\\gc\\files\\randomprime.txt");

                    FormUtils.SetProgressStatus(2, 5);
                    FormUtils.SetStatus("Replacing original PAKs with randomized PAKs...", i, gc_iso_files.Length);

                    Pak_Path = ".\\tmp\\wii\\DATA\\files\\MP1";
                    if (GameID.Substring(2, 2) == "IJ")
                        Pak_Path += "JPN";

                    File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                    File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol", true);
                    if (GameID[3] == 'J')
                        File.Copy(".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol", true);
                    else
                        File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", true);

                    foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                        File.Copy(file, Pak_Path + "\\" + Path.GetFileName(file), true);

                    File.Copy(".\\tmp\\gc\\files\\Tweaks.pak", Pak_Path + "\\Tweaks.pak", true);
                    File.Copy(".\\tmp\\gc\\files\\NoARAM.pak", Pak_Path + "\\NoARAM.pak", true);
                    File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

                    Directory.Delete(".\\tmp\\gc", true);

                    FormUtils.SetProgressStatus(3, 5);
                    FormUtils.SetStatus("Applying patches to MP1 executable...", i, gc_iso_files.Length);

                    PatchedGameID = GameID.Substring(0, 4) + DiscUtils.RandomizeDeveloperCode();

                    /* Applying patches to dol file */

                    Patcher.Patcher.Init(PatchedGameID[3]);

                    if (randomizerSettings.skipFrigate)
                    {
                        if (randomizerSettings.spawnRoom == null)
                        {
                            FormUtils.ShowMessageBox("No spawn room defined!");
                            FormUtils.SetProgressStatus(5, 5);
                            FormUtils.SetStatus("Idle");
                            FormUtils.SetControlText(this.randomize_btn, "Randomize");
                            isLoading = false;
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

                    FormUtils.SetProgressStatus(4, 5);
                    FormUtils.SetStatus("Packing Metroid Prime Wii to " + ((String)output_type_combo_box.SelectedItem).Substring(1).ToUpper() + " format...", i, gc_iso_files.Length);
                    // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
                    if (((String)output_type_combo_box.SelectedItem).ToLower().EndsWith(".ciso"))
                    {
                        ISOUtils.WIT.CreateCompressISO(".\\tmp\\mpt.ciso", false, PatchedGameID);
                        if (File.Exists(GameSettingsDolphinEmuPath + GameID + ".ini") && !File.Exists(GameSettingsDolphinEmuPath + PatchedGameID + ".ini"))
                            File.Copy(GameSettingsDolphinEmuPath + GameID + ".ini", GameSettingsDolphinEmuPath + PatchedGameID + ".ini");
                    }
                    else if (((String)output_type_combo_box.SelectedItem).ToLower().EndsWith(".wbfs"))
                        ISOUtils.WIT.CreateWBFS(".\\tmp\\mpt.wbfs", PatchedGameID);

                    if (File.Exists(this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem))
                        File.Delete(this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem);
                    if ((String)output_type_combo_box.SelectedItem == ".wbfs")
                        new_wii_iso_path = this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + "[" + PatchedGameID + "]\\" + PatchedGameID + ".wbfs";
                    else
                        new_wii_iso_path = this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem;

                    if (!Directory.Exists(Path.GetDirectoryName(new_wii_iso_path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(new_wii_iso_path));

                    File.Move(".\\tmp\\mpt" + (String)output_type_combo_box.SelectedItem, new_wii_iso_path);

                    if (File.Exists(".\\tmp" + spoiler_filename))
                    {
                        if (File.Exists(Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename))
                            File.Delete(Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename);
                        File.Move(".\\tmp\\" + spoiler_filename, Path.GetDirectoryName(new_wii_iso_path) + "\\" + spoiler_filename);
                    }
                    i++;
                }
                FormUtils.SetProgressStatus(5, 5);
                FormUtils.SetStatus("Idle");
                FormUtils.ShowMessageBox(gc_iso_files.Length + " ISO" + (gc_iso_files.Length > 1 ? "s have" : " has") + " been randomized! Have fun!");
                FormUtils.SetControlText(this.randomize_btn, "Randomize");
                isLoading = false;
            }));
        }

        private void input_json_btn_click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON File|*.json";
                openFileDialog.FileName = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.input_json_txt_box.Text = openFileDialog.FileName;
                    appSettings.prime1PlandomizerLastJsonPath = openFileDialog.FileName;
                    appSettings.SaveToJson();
                }
            }
        }

        private void locate_plandomizer_btn_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                var dialogResult = default(DialogResult);

                openFileDialog.Title = "Select the executable of toasterparty's Plandomizer";
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
                        appSettings.prime1PlandomizerPath = openFileDialog.FileName;
                        appSettings.SaveToJson();
                        this.plandomizer_lbl.Text = "toasterparty's Plandomizer found!";
                    }
                    if (dialogResult == DialogResult.Cancel)
                    {
                        if (!appSettings.prime1RandomizerPath.EndsWith(".exe"))
                            this.plandomizer_lbl.Text = "toasterparty's Plandomizer not found! (Recommended version : 1.6)";
                        return;
                    }
                }
            }
        }

        private void plandomize_btn_Click(object sender, EventArgs e)
        {
            var new_wii_iso_path = default(String);
            var gc_iso_filename = default(String);
            var GameID = default(String);
            var PatchedGameID = default(String);
            var Pak_Path = default(String);

            String GameSettingsDolphinEmuPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Dolphin Emulator" + Path.DirectorySeparatorChar + "GameSettings" + Path.DirectorySeparatorChar;
            String curDir = Directory.GetCurrentDirectory();
            String json_file_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\world_layout.json";

            if (isLoading && ((Button)sender).Text == "Cancel")
            {
                ProcessUtils.KillChildrenProcesses(Process.GetCurrentProcess().Id);
                if (Directory.Exists(@".\tmp\gc"))
                    Directory.Delete(@".\tmp\gc", true);
                FormUtils.ShowMessageBox("Cancelled plandomization!");
                FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                isLoading = false;
                return;
            }

            if (!IsTemplateISOGenerated())
            {
                MessageBox.Show("Please click on \"Generate a template ISO for toasterparty's Plandomizer\" before attempting to plandomize!");
                return;
            }

            if (!DiscUtils.IsMetroidPrimeTrilogyNTSC_UOrPAL("gc_template.iso") && !DiscUtils.IsMetroidPrimeWiiNTSC_J("gc_template.iso"))
                return;

            GameID = DiscUtils.GetGameID("gc_template.iso");

            if (appSettings.prime1PlandomizerPath == "" || !File.Exists(appSettings.prime1PlandomizerPath))
            {
                MessageBox.Show("Please click on \"Locate toasterparty's Plandomizer\" before attempting to plandomize!");
                return;
            }

            if (appSettings.prime1PlandomizerLastJsonPath == "" || !File.Exists(appSettings.prime1PlandomizerLastJsonPath))
            {
                MessageBox.Show("Please select a valid plando json file!");
                return;
            }

            if (output_type_combo_box.SelectedIndex == -1)
            {
                MessageBox.Show("Select an output type to save the iso in that format!");
                return;
            }

            if (output_path_txt_box.Text == "")
            {
                MessageBox.Show("Select a folder to save the " + output_type_combo_box.SelectedItem + " file !");
                return;
            }

            isLoading = true;

            FormUtils.RunAsynchrousTask(new Action(() =>
            {
                FormUtils.SetControlText(this.plandomize_btn, "Cancel");
                dynamic json = JObject.Parse(File.ReadAllText(appSettings.prime1PlandomizerLastJsonPath));
                json.input_iso = Path.GetDirectoryName(Application.ExecutablePath) + @"\gc_template.iso";
                json.output_iso = Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\" + json.output_iso;
                File.WriteAllText(json_file_path, JsonConvert.SerializeObject(json, Formatting.Indented));
                FormUtils.SetProgressStatus(0, 5);
                FormUtils.SetStatus("Running toasterparty's Plandomizer...");
                if (!ProcessUtils.Run(appSettings.prime1PlandomizerPath, "--profile", "\"" + json_file_path + "\""))
                {
                    FormUtils.ShowMessageBox("toasterparty's Plandomizer hasn't properly exited! Cancelling...");
                    File.Delete(json_file_path);
                    FormUtils.SetProgressStatus(5, 5);
                    FormUtils.SetStatus("Idle");
                    isLoading = false;
                    FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                    return;
                }

                File.Delete(json_file_path);

                if (Directory.EnumerateFiles(".\\tmp", "*.iso").Count() == 0)
                {
                    FormUtils.ShowMessageBox("No Plandomized ISO generated! Cancelling...");
                    FormUtils.SetProgressStatus(5, 5);
                    FormUtils.SetStatus("Idle");
                    isLoading = false;
                    FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                    return;
                }

                int i = 0;
                String[] gc_iso_files = Directory.EnumerateFiles(".\\tmp", "*.iso").ToArray();
                foreach (var gc_iso_file in gc_iso_files)
                {
                    gc_iso_filename = Path.GetFileName(gc_iso_file);

                    FormUtils.SetProgressStatus(1, 5);
                    FormUtils.SetStatus("Extracting plandomized ISO...", i, gc_iso_files.Length);

                    if (!ISOUtils.NOD.ExtractISO(".\\tmp\\" + gc_iso_filename, true))
                    {
                        FormUtils.ShowMessageBox("Failed extracting plandomized iso!");
                        FormUtils.SetProgressStatus(5, 5);
                        FormUtils.SetStatus("Idle");
                        isLoading = false;
                        FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                        return;
                    }

                    File.Delete(".\\tmp\\" + gc_iso_filename);

                    var randomizerSettings = new Config.RandomizerSettings(".\\tmp\\gc\\files\\randomprime.txt");

                    FormUtils.SetProgressStatus(2, 5);
                    FormUtils.SetStatus("Replacing original PAKs with plandomized PAKs...", i, gc_iso_files.Length);

                    Pak_Path = ".\\tmp\\wii\\DATA\\files\\MP1";
                    if (GameID.Substring(2, 2) == "IJ")
                        Pak_Path += "JPN";

                    File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\sys\\main.dol", true);
                    File.Copy(".\\tmp\\dol_backup\\rs5fe_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5fe_p.dol", true);
                    if (GameID[3] == 'J')
                        File.Copy(".\\tmp\\dol_backup\\rs5mp1jpn_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1jpn_p.dol", true);
                    else
                        File.Copy(".\\tmp\\dol_backup\\rs5mp1_p.dol", ".\\tmp\\wii\\DATA\\files\\rs5mp1_p.dol", true);

                    foreach (var file in Directory.EnumerateFiles(".\\tmp\\gc\\files", "Metroid*.pak", SearchOption.TopDirectoryOnly))
                        File.Copy(file, Pak_Path + "\\" + Path.GetFileName(file), true);

                    File.Copy(".\\tmp\\gc\\files\\NoARAM.pak", Pak_Path + "\\NoARAM.pak", true);
                    File.Copy(".\\tmp\\gc\\files\\randomprime.txt", ".\\tmp\\wii\\DATA\\files\\randomprime.txt", true);

                    Directory.Delete(".\\tmp\\gc", true);

                    FormUtils.SetProgressStatus(3, 5);
                    FormUtils.SetStatus("Applying patches to MP1 executable...", i, gc_iso_files.Length);

                    PatchedGameID = GameID.Substring(0, 4) + DiscUtils.RandomizeDeveloperCode();

                    /* Applying patches to dol file */

                    Patcher.Patcher.Init(PatchedGameID[3]);
                    try
                    {
                        SpawnRoom spawnRoom = SpawnRoom.plandomizerValues[json.new_save_spawn_room];
                        if (spawnRoom == null)
                        {
                            FormUtils.ShowMessageBox("Spawn room defined!");
                            FormUtils.SetProgressStatus(5, 5);
                            FormUtils.SetStatus("Idle");
                            isLoading = false;
                            FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                            return;
                        }
                        Patcher.Patcher.SetStartingArea(spawnRoom);
                    }
                    catch
                    {
                        if (json.patchSettings.skip_frigate)
                            Patcher.Patcher.SetStartingArea(SpawnRoom.plandomizerValues["Tallon:Landing Site"]);
                    }

                    Patcher.Patcher.ApplySkipCutscenePatch();
                    Patcher.Patcher.ApplyHeatProtectionPatch(json.patchSettings.varia_heat_protection);
                    Patcher.Patcher.ApplySuitDamageReductionPatch(json.patchSettings.stagger_suit_damage);
                    Patcher.Patcher.ApplyScanDashPatch();
                    Patcher.Patcher.ApplyUnderwaterSlopeJumpFixPatch(true);
                    Patcher.Patcher.SetSaveFilename(PatchedGameID.Substring(4, 2) + ".bin");
                    if (disable_spring_ball_check_box.Checked)
                        Patcher.Patcher.ApplyDisableSpringBallPatch();

                    /*  */

                    FormUtils.SetProgressStatus(4, 5);
                    FormUtils.SetStatus("Packing Metroid Prime Wii to " + ((String)output_type_combo_box.SelectedItem).Substring(1).ToUpper() + " format...", i, gc_iso_files.Length);
                    // WIT doesn't like complex paths so make the image in tmp folder then move back to the output folder
                    if (((String)output_type_combo_box.SelectedItem).ToLower().EndsWith(".ciso"))
                    {
                        ISOUtils.WIT.CreateCompressISO(".\\tmp\\mpt.ciso", false, PatchedGameID);
                        if (File.Exists(GameSettingsDolphinEmuPath + GameID + ".ini") && !File.Exists(GameSettingsDolphinEmuPath + PatchedGameID + ".ini"))
                            File.Copy(GameSettingsDolphinEmuPath + GameID + ".ini", GameSettingsDolphinEmuPath + PatchedGameID + ".ini");
                    }
                    else if (((String)output_type_combo_box.SelectedItem).ToLower().EndsWith(".wbfs"))
                        ISOUtils.WIT.CreateWBFS(".\\tmp\\mpt.wbfs", PatchedGameID);

                    if (File.Exists(this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem))
                        File.Delete(this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem);
                    if ((String)output_type_combo_box.SelectedItem == ".wbfs")
                        new_wii_iso_path = this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + "[" + PatchedGameID + "]\\" + PatchedGameID + ".wbfs";
                    else
                        new_wii_iso_path = this.output_path_txt_box.Text + "\\" + Path.GetFileNameWithoutExtension(gc_iso_filename) + (String)output_type_combo_box.SelectedItem;

                    if (!Directory.Exists(Path.GetDirectoryName(new_wii_iso_path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(new_wii_iso_path));

                    File.Move(".\\tmp\\mpt" + (String)output_type_combo_box.SelectedItem, new_wii_iso_path);
                    i++;
                }
                FormUtils.SetProgressStatus(5, 5);
                FormUtils.SetStatus("Idle");
                FormUtils.ShowMessageBox(gc_iso_files.Length + " ISO" + (gc_iso_files.Length > 1 ? "s have" : " has") + " been plandomized! Have fun!");
                FormUtils.SetControlText(this.plandomize_btn, "Plandomize");
                isLoading = false;
            }));
        }

        private void output_path_btn_click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.output_path_txt_box.Text = folderBrowserDialog.SelectedPath;
                    appSettings.outputPath = folderBrowserDialog.SelectedPath;
                    appSettings.SaveToJson();
                }
            }
        }

        private void output_type_combo_box_selected_index_changed(object sender, EventArgs e)
        {
            if (output_type_combo_box.SelectedIndex != -1)
            {
                appSettings.outputType = (String)output_type_combo_box.SelectedItem;
                appSettings.SaveToJson();
            }
        }

        private void form_closing(object sender, FormClosingEventArgs e)
        {
            // Closing any nod or wit application running
            ProcessUtils.KillChildrenProcesses(Process.GetCurrentProcess().Id);
        }

        private void form1_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.SendClickToCaption();
        }

        private void help_btn_click(object sender, EventArgs e)
        {
            try
            {
                new HelpForm().ShowDialog();
            }
            catch
            {
                MessageBox.Show("No wiki files found! Connect to internet to download the wiki files!");
            }
        }

        private void close_btn_click(object sender, EventArgs e)
        {
            FormUtils.SafeClose();
        }

        private void tab_bar_btn_click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Template ISO")
                FormUtils.SwitchTab(0);
            if (((Button)sender).Text == "Randomizer")
                FormUtils.SwitchTab(1);
            if (((Button)sender).Text == "Plandomizer")
                FormUtils.SwitchTab(2);
            if (((Button)sender).Text == "Settings")
                FormUtils.SwitchTab(3);
        }
    }
}
