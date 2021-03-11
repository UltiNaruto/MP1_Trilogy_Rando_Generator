namespace MP1_Trilogy_Rando_Generator
{
    partial class main_form
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.main_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.output_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.output_type_lbl = new DarkUI.Controls.DarkLabel();
            this.output_path_lbl = new DarkUI.Controls.DarkLabel();
            this.output_path_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.output_path_txt_box = new DarkUI.Controls.DarkTextBox();
            this.output_path_btn = new DarkUI.Controls.DarkButton();
            this.output_type_combo_box = new DarkUI.Controls.DarkComboBox();
            this.status_bar_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.status_progress_bar = new System.Windows.Forms.ProgressBar();
            this.status_lbl = new DarkUI.Controls.DarkLabel();
            this.title_bar_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.close_btn = new System.Windows.Forms.Button();
            this.help_btn = new System.Windows.Forms.Button();
            this.title_lbl = new DarkUI.Controls.DarkLabel();
            this.tab_btn_bar_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.settings_btn = new System.Windows.Forms.Button();
            this.plandomizer_btn = new System.Windows.Forms.Button();
            this.randomizer_btn = new System.Windows.Forms.Button();
            this.template_iso_btn = new System.Windows.Forms.Button();
            this.tab_manager = new System.Windows.Forms.TabControlNoHeader();
            this.template_iso_tab = new System.Windows.Forms.TabPage();
            this.template_iso_tab_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.template_iso_btn_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.generate_template_iso_btn = new DarkUI.Controls.DarkButton();
            this.delete_cache_btn = new DarkUI.Controls.DarkButton();
            this.template_iso_lbl = new DarkUI.Controls.DarkLabel();
            this.randomizer_tab = new System.Windows.Forms.TabPage();
            this.randomizer_tab_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.randomizer_btn_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.locate_randomizer_btn = new DarkUI.Controls.DarkButton();
            this.randomize_btn = new DarkUI.Controls.DarkButton();
            this.randomizer_lbl = new DarkUI.Controls.DarkLabel();
            this.plandomizer_tab = new System.Windows.Forms.TabPage();
            this.plandomizer_tab_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.plandomizer_lbl = new DarkUI.Controls.DarkLabel();
            this.input_json_lbl = new DarkUI.Controls.DarkLabel();
            this.input_json_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.input_json_txt_box = new DarkUI.Controls.DarkTextBox();
            this.input_json_btn = new DarkUI.Controls.DarkButton();
            this.plandomizer_btn_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.locate_plandomizer_btn = new DarkUI.Controls.DarkButton();
            this.plandomize_btn = new DarkUI.Controls.DarkButton();
            this.settings_tab = new System.Windows.Forms.TabPage();
            this.settings_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.disable_spring_ball_check_box = new DarkUI.Controls.DarkCheckBox();
            this.main_table_layout.SuspendLayout();
            this.output_table_layout.SuspendLayout();
            this.output_path_table_layout.SuspendLayout();
            this.status_bar_table_layout.SuspendLayout();
            this.title_bar_table_layout.SuspendLayout();
            this.tab_btn_bar_table_layout.SuspendLayout();
            this.tab_manager.SuspendLayout();
            this.template_iso_tab.SuspendLayout();
            this.template_iso_tab_table_layout.SuspendLayout();
            this.template_iso_btn_table_layout.SuspendLayout();
            this.randomizer_tab.SuspendLayout();
            this.randomizer_tab_table_layout.SuspendLayout();
            this.randomizer_btn_table_layout.SuspendLayout();
            this.plandomizer_tab.SuspendLayout();
            this.plandomizer_tab_table_layout.SuspendLayout();
            this.input_json_table_layout.SuspendLayout();
            this.plandomizer_btn_table_layout.SuspendLayout();
            this.settings_tab.SuspendLayout();
            this.settings_table_layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_table_layout
            // 
            this.main_table_layout.ColumnCount = 1;
            this.main_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_table_layout.Controls.Add(this.output_table_layout, 0, 3);
            this.main_table_layout.Controls.Add(this.status_bar_table_layout, 0, 4);
            this.main_table_layout.Controls.Add(this.title_bar_table_layout, 0, 0);
            this.main_table_layout.Controls.Add(this.tab_btn_bar_table_layout, 0, 1);
            this.main_table_layout.Controls.Add(this.tab_manager, 0, 2);
            this.main_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_table_layout.Location = new System.Drawing.Point(0, 0);
            this.main_table_layout.Name = "main_table_layout";
            this.main_table_layout.RowCount = 5;
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_table_layout.Size = new System.Drawing.Size(800, 360);
            this.main_table_layout.TabIndex = 0;
            // 
            // output_table_layout
            // 
            this.output_table_layout.ColumnCount = 1;
            this.output_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.output_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.output_table_layout.Controls.Add(this.output_type_lbl, 0, 2);
            this.output_table_layout.Controls.Add(this.output_path_lbl, 0, 0);
            this.output_table_layout.Controls.Add(this.output_path_table_layout, 0, 1);
            this.output_table_layout.Controls.Add(this.output_type_combo_box, 0, 3);
            this.output_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_table_layout.Location = new System.Drawing.Point(3, 232);
            this.output_table_layout.Name = "output_table_layout";
            this.output_table_layout.RowCount = 4;
            this.output_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.output_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.output_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.output_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.output_table_layout.Size = new System.Drawing.Size(794, 104);
            this.output_table_layout.TabIndex = 9;
            // 
            // output_type_lbl
            // 
            this.output_type_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_type_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.output_type_lbl.Location = new System.Drawing.Point(3, 52);
            this.output_type_lbl.Name = "output_type_lbl";
            this.output_type_lbl.Size = new System.Drawing.Size(788, 26);
            this.output_type_lbl.TabIndex = 6;
            this.output_type_lbl.Text = "Output Type";
            this.output_type_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // output_path_lbl
            // 
            this.output_path_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_path_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.output_path_lbl.Location = new System.Drawing.Point(3, 0);
            this.output_path_lbl.Name = "output_path_lbl";
            this.output_path_lbl.Size = new System.Drawing.Size(788, 26);
            this.output_path_lbl.TabIndex = 0;
            this.output_path_lbl.Text = "Output Path";
            this.output_path_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // output_path_table_layout
            // 
            this.output_path_table_layout.ColumnCount = 2;
            this.output_path_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.output_path_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.output_path_table_layout.Controls.Add(this.output_path_txt_box, 0, 0);
            this.output_path_table_layout.Controls.Add(this.output_path_btn, 1, 0);
            this.output_path_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_path_table_layout.Location = new System.Drawing.Point(0, 26);
            this.output_path_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.output_path_table_layout.Name = "output_path_table_layout";
            this.output_path_table_layout.RowCount = 1;
            this.output_path_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.output_path_table_layout.Size = new System.Drawing.Size(794, 26);
            this.output_path_table_layout.TabIndex = 5;
            // 
            // output_path_txt_box
            // 
            this.output_path_txt_box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.output_path_txt_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output_path_txt_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_path_txt_box.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.output_path_txt_box.Location = new System.Drawing.Point(3, 3);
            this.output_path_txt_box.MaxLength = 260;
            this.output_path_txt_box.Name = "output_path_txt_box";
            this.output_path_txt_box.Size = new System.Drawing.Size(688, 20);
            this.output_path_txt_box.TabIndex = 4;
            // 
            // output_path_btn
            // 
            this.output_path_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_path_btn.Location = new System.Drawing.Point(697, 3);
            this.output_path_btn.Name = "output_path_btn";
            this.output_path_btn.Padding = new System.Windows.Forms.Padding(5);
            this.output_path_btn.Size = new System.Drawing.Size(94, 20);
            this.output_path_btn.TabIndex = 5;
            this.output_path_btn.Text = "Browse";
            this.output_path_btn.Click += new System.EventHandler(this.output_path_btn_click);
            // 
            // output_type_combo_box
            // 
            this.output_type_combo_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output_type_combo_box.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.output_type_combo_box.FormattingEnabled = true;
            this.output_type_combo_box.Items.AddRange(new object[] {
            ".ciso",
            ".wbfs"});
            this.output_type_combo_box.Location = new System.Drawing.Point(3, 81);
            this.output_type_combo_box.Name = "output_type_combo_box";
            this.output_type_combo_box.Size = new System.Drawing.Size(788, 21);
            this.output_type_combo_box.TabIndex = 7;
            this.output_type_combo_box.SelectedIndexChanged += new System.EventHandler(this.output_type_combo_box_selected_index_changed);
            // 
            // status_bar_table_layout
            // 
            this.status_bar_table_layout.ColumnCount = 2;
            this.status_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.status_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.status_bar_table_layout.Controls.Add(this.status_progress_bar, 1, 0);
            this.status_bar_table_layout.Controls.Add(this.status_lbl, 0, 0);
            this.status_bar_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_bar_table_layout.Location = new System.Drawing.Point(0, 339);
            this.status_bar_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.status_bar_table_layout.Name = "status_bar_table_layout";
            this.status_bar_table_layout.RowCount = 1;
            this.status_bar_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.status_bar_table_layout.Size = new System.Drawing.Size(800, 21);
            this.status_bar_table_layout.TabIndex = 5;
            // 
            // status_progress_bar
            // 
            this.status_progress_bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_progress_bar.Location = new System.Drawing.Point(483, 3);
            this.status_progress_bar.Name = "status_progress_bar";
            this.status_progress_bar.Size = new System.Drawing.Size(314, 15);
            this.status_progress_bar.TabIndex = 6;
            this.status_progress_bar.Visible = false;
            // 
            // status_lbl
            // 
            this.status_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.status_lbl.Location = new System.Drawing.Point(3, 0);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(474, 21);
            this.status_lbl.TabIndex = 7;
            this.status_lbl.Text = "Status : Idle";
            this.status_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // title_bar_table_layout
            // 
            this.title_bar_table_layout.ColumnCount = 3;
            this.title_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.title_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.title_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.title_bar_table_layout.Controls.Add(this.close_btn, 2, 0);
            this.title_bar_table_layout.Controls.Add(this.help_btn, 1, 0);
            this.title_bar_table_layout.Controls.Add(this.title_lbl, 0, 0);
            this.title_bar_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_bar_table_layout.Location = new System.Drawing.Point(0, 0);
            this.title_bar_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.title_bar_table_layout.Name = "title_bar_table_layout";
            this.title_bar_table_layout.RowCount = 1;
            this.title_bar_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.title_bar_table_layout.Size = new System.Drawing.Size(800, 32);
            this.title_bar_table_layout.TabIndex = 6;
            // 
            // close_btn
            // 
            this.close_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.close_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.close_btn.Location = new System.Drawing.Point(771, 3);
            this.close_btn.Name = "close_btn";
            this.close_btn.Size = new System.Drawing.Size(26, 26);
            this.close_btn.TabIndex = 0;
            this.close_btn.Text = "X";
            this.close_btn.UseVisualStyleBackColor = false;
            this.close_btn.Click += new System.EventHandler(this.close_btn_click);
            // 
            // help_btn
            // 
            this.help_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.help_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.help_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.help_btn.Location = new System.Drawing.Point(739, 3);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(26, 26);
            this.help_btn.TabIndex = 1;
            this.help_btn.Text = "?";
            this.help_btn.UseVisualStyleBackColor = false;
            this.help_btn.Click += new System.EventHandler(this.help_btn_click);
            // 
            // title_lbl
            // 
            this.title_lbl.AutoSize = true;
            this.title_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_lbl.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.title_lbl.Location = new System.Drawing.Point(3, 0);
            this.title_lbl.Name = "title_lbl";
            this.title_lbl.Size = new System.Drawing.Size(730, 32);
            this.title_lbl.TabIndex = 2;
            this.title_lbl.Text = "MP1 Trilogy Rando Generator v2.0";
            this.title_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.title_lbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.form1_MouseDown);
            // 
            // tab_btn_bar_table_layout
            // 
            this.tab_btn_bar_table_layout.ColumnCount = 5;
            this.tab_btn_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tab_btn_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tab_btn_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tab_btn_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tab_btn_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tab_btn_bar_table_layout.Controls.Add(this.settings_btn, 3, 0);
            this.tab_btn_bar_table_layout.Controls.Add(this.plandomizer_btn, 2, 0);
            this.tab_btn_bar_table_layout.Controls.Add(this.randomizer_btn, 1, 0);
            this.tab_btn_bar_table_layout.Controls.Add(this.template_iso_btn, 0, 0);
            this.tab_btn_bar_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_btn_bar_table_layout.Location = new System.Drawing.Point(0, 32);
            this.tab_btn_bar_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.tab_btn_bar_table_layout.Name = "tab_btn_bar_table_layout";
            this.tab_btn_bar_table_layout.RowCount = 1;
            this.tab_btn_bar_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tab_btn_bar_table_layout.Size = new System.Drawing.Size(800, 32);
            this.tab_btn_bar_table_layout.TabIndex = 7;
            // 
            // settings_btn
            // 
            this.settings_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.settings_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.settings_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.settings_btn.Location = new System.Drawing.Point(303, 3);
            this.settings_btn.Name = "settings_btn";
            this.settings_btn.Size = new System.Drawing.Size(94, 26);
            this.settings_btn.TabIndex = 3;
            this.settings_btn.Text = "Settings";
            this.settings_btn.UseVisualStyleBackColor = false;
            this.settings_btn.Click += new System.EventHandler(this.tab_bar_btn_click);
            // 
            // plandomizer_btn
            // 
            this.plandomizer_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.plandomizer_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plandomizer_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plandomizer_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plandomizer_btn.Location = new System.Drawing.Point(203, 3);
            this.plandomizer_btn.Name = "plandomizer_btn";
            this.plandomizer_btn.Size = new System.Drawing.Size(94, 26);
            this.plandomizer_btn.TabIndex = 2;
            this.plandomizer_btn.Text = "Plandomizer";
            this.plandomizer_btn.UseVisualStyleBackColor = false;
            this.plandomizer_btn.Click += new System.EventHandler(this.tab_bar_btn_click);
            // 
            // randomizer_btn
            // 
            this.randomizer_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.randomizer_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.randomizer_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.randomizer_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.randomizer_btn.Location = new System.Drawing.Point(103, 3);
            this.randomizer_btn.Name = "randomizer_btn";
            this.randomizer_btn.Size = new System.Drawing.Size(94, 26);
            this.randomizer_btn.TabIndex = 1;
            this.randomizer_btn.Text = "Randomizer";
            this.randomizer_btn.UseVisualStyleBackColor = false;
            this.randomizer_btn.Click += new System.EventHandler(this.tab_bar_btn_click);
            // 
            // template_iso_btn
            // 
            this.template_iso_btn.BackColor = System.Drawing.Color.Gray;
            this.template_iso_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.template_iso_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.template_iso_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.template_iso_btn.Location = new System.Drawing.Point(3, 3);
            this.template_iso_btn.Name = "template_iso_btn";
            this.template_iso_btn.Size = new System.Drawing.Size(94, 26);
            this.template_iso_btn.TabIndex = 0;
            this.template_iso_btn.Text = "Template ISO";
            this.template_iso_btn.UseVisualStyleBackColor = false;
            this.template_iso_btn.Click += new System.EventHandler(this.tab_bar_btn_click);
            // 
            // tab_manager
            // 
            this.tab_manager.Controls.Add(this.template_iso_tab);
            this.tab_manager.Controls.Add(this.randomizer_tab);
            this.tab_manager.Controls.Add(this.plandomizer_tab);
            this.tab_manager.Controls.Add(this.settings_tab);
            this.tab_manager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_manager.Location = new System.Drawing.Point(0, 64);
            this.tab_manager.Margin = new System.Windows.Forms.Padding(0);
            this.tab_manager.Multiline = true;
            this.tab_manager.Name = "tab_manager";
            this.tab_manager.SelectedIndex = 0;
            this.tab_manager.Size = new System.Drawing.Size(800, 165);
            this.tab_manager.TabIndex = 8;
            // 
            // template_iso_tab
            // 
            this.template_iso_tab.Controls.Add(this.template_iso_tab_table_layout);
            this.template_iso_tab.Location = new System.Drawing.Point(4, 22);
            this.template_iso_tab.Margin = new System.Windows.Forms.Padding(0);
            this.template_iso_tab.Name = "template_iso_tab";
            this.template_iso_tab.Size = new System.Drawing.Size(792, 139);
            this.template_iso_tab.TabIndex = 0;
            this.template_iso_tab.Text = "Template ISO";
            this.template_iso_tab.UseVisualStyleBackColor = true;
            // 
            // template_iso_tab_table_layout
            // 
            this.template_iso_tab_table_layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.template_iso_tab_table_layout.ColumnCount = 1;
            this.template_iso_tab_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.template_iso_tab_table_layout.Controls.Add(this.template_iso_btn_table_layout, 0, 1);
            this.template_iso_tab_table_layout.Controls.Add(this.template_iso_lbl, 0, 0);
            this.template_iso_tab_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.template_iso_tab_table_layout.Location = new System.Drawing.Point(0, 0);
            this.template_iso_tab_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.template_iso_tab_table_layout.Name = "template_iso_tab_table_layout";
            this.template_iso_tab_table_layout.RowCount = 3;
            this.template_iso_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.template_iso_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.template_iso_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.template_iso_tab_table_layout.Size = new System.Drawing.Size(792, 139);
            this.template_iso_tab_table_layout.TabIndex = 2;
            // 
            // template_iso_btn_table_layout
            // 
            this.template_iso_btn_table_layout.ColumnCount = 2;
            this.template_iso_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.template_iso_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.template_iso_btn_table_layout.Controls.Add(this.generate_template_iso_btn, 0, 0);
            this.template_iso_btn_table_layout.Controls.Add(this.delete_cache_btn, 1, 0);
            this.template_iso_btn_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.template_iso_btn_table_layout.Location = new System.Drawing.Point(0, 42);
            this.template_iso_btn_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.template_iso_btn_table_layout.Name = "template_iso_btn_table_layout";
            this.template_iso_btn_table_layout.RowCount = 1;
            this.template_iso_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.template_iso_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.template_iso_btn_table_layout.Size = new System.Drawing.Size(792, 42);
            this.template_iso_btn_table_layout.TabIndex = 3;
            // 
            // generate_template_iso_btn
            // 
            this.generate_template_iso_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generate_template_iso_btn.Location = new System.Drawing.Point(3, 3);
            this.generate_template_iso_btn.Name = "generate_template_iso_btn";
            this.generate_template_iso_btn.Padding = new System.Windows.Forms.Padding(5);
            this.generate_template_iso_btn.Size = new System.Drawing.Size(390, 36);
            this.generate_template_iso_btn.TabIndex = 4;
            this.generate_template_iso_btn.Text = "Generate a template ISO for the patcher";
            this.generate_template_iso_btn.Click += new System.EventHandler(this.generate_template_iso_btn_click);
            // 
            // delete_cache_btn
            // 
            this.delete_cache_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delete_cache_btn.Enabled = false;
            this.delete_cache_btn.Location = new System.Drawing.Point(399, 3);
            this.delete_cache_btn.Name = "delete_cache_btn";
            this.delete_cache_btn.Padding = new System.Windows.Forms.Padding(5);
            this.delete_cache_btn.Size = new System.Drawing.Size(390, 36);
            this.delete_cache_btn.TabIndex = 5;
            this.delete_cache_btn.Text = "Delete cache (Template ISO + tmp folder)";
            this.delete_cache_btn.Click += new System.EventHandler(this.delete_cache_btn_click);
            // 
            // template_iso_lbl
            // 
            this.template_iso_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.template_iso_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.template_iso_lbl.Location = new System.Drawing.Point(3, 0);
            this.template_iso_lbl.Name = "template_iso_lbl";
            this.template_iso_lbl.Size = new System.Drawing.Size(786, 42);
            this.template_iso_lbl.TabIndex = 4;
            this.template_iso_lbl.Text = "No Metroid Prime Wii ISO template for Prime 1 Randomizer detected!";
            this.template_iso_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // randomizer_tab
            // 
            this.randomizer_tab.Controls.Add(this.randomizer_tab_table_layout);
            this.randomizer_tab.Location = new System.Drawing.Point(4, 22);
            this.randomizer_tab.Margin = new System.Windows.Forms.Padding(0);
            this.randomizer_tab.Name = "randomizer_tab";
            this.randomizer_tab.Size = new System.Drawing.Size(792, 139);
            this.randomizer_tab.TabIndex = 1;
            this.randomizer_tab.Text = "Randomizer";
            this.randomizer_tab.UseVisualStyleBackColor = true;
            // 
            // randomizer_tab_table_layout
            // 
            this.randomizer_tab_table_layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.randomizer_tab_table_layout.ColumnCount = 1;
            this.randomizer_tab_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.randomizer_tab_table_layout.Controls.Add(this.randomizer_btn_table_layout, 0, 1);
            this.randomizer_tab_table_layout.Controls.Add(this.randomizer_lbl, 0, 0);
            this.randomizer_tab_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.randomizer_tab_table_layout.Location = new System.Drawing.Point(0, 0);
            this.randomizer_tab_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.randomizer_tab_table_layout.Name = "randomizer_tab_table_layout";
            this.randomizer_tab_table_layout.RowCount = 3;
            this.randomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.randomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.randomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.randomizer_tab_table_layout.Size = new System.Drawing.Size(792, 139);
            this.randomizer_tab_table_layout.TabIndex = 0;
            // 
            // randomizer_btn_table_layout
            // 
            this.randomizer_btn_table_layout.ColumnCount = 2;
            this.randomizer_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.randomizer_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.randomizer_btn_table_layout.Controls.Add(this.locate_randomizer_btn, 0, 0);
            this.randomizer_btn_table_layout.Controls.Add(this.randomize_btn, 1, 0);
            this.randomizer_btn_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.randomizer_btn_table_layout.Location = new System.Drawing.Point(0, 40);
            this.randomizer_btn_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.randomizer_btn_table_layout.Name = "randomizer_btn_table_layout";
            this.randomizer_btn_table_layout.RowCount = 1;
            this.randomizer_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.randomizer_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.randomizer_btn_table_layout.Size = new System.Drawing.Size(792, 40);
            this.randomizer_btn_table_layout.TabIndex = 8;
            // 
            // locate_randomizer_btn
            // 
            this.locate_randomizer_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locate_randomizer_btn.Location = new System.Drawing.Point(3, 3);
            this.locate_randomizer_btn.Name = "locate_randomizer_btn";
            this.locate_randomizer_btn.Padding = new System.Windows.Forms.Padding(5);
            this.locate_randomizer_btn.Size = new System.Drawing.Size(390, 34);
            this.locate_randomizer_btn.TabIndex = 4;
            this.locate_randomizer_btn.Text = "Locate BashPrime\'s Randomizer";
            this.locate_randomizer_btn.Click += new System.EventHandler(this.locate_randomizer_btn_click);
            // 
            // randomize_btn
            // 
            this.randomize_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.randomize_btn.Location = new System.Drawing.Point(399, 3);
            this.randomize_btn.Name = "randomize_btn";
            this.randomize_btn.Padding = new System.Windows.Forms.Padding(5);
            this.randomize_btn.Size = new System.Drawing.Size(390, 34);
            this.randomize_btn.TabIndex = 5;
            this.randomize_btn.Text = "Randomize";
            this.randomize_btn.Click += new System.EventHandler(this.randomize_btn_click);
            // 
            // randomizer_lbl
            // 
            this.randomizer_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.randomizer_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.randomizer_lbl.Location = new System.Drawing.Point(3, 0);
            this.randomizer_lbl.Name = "randomizer_lbl";
            this.randomizer_lbl.Size = new System.Drawing.Size(786, 40);
            this.randomizer_lbl.TabIndex = 9;
            this.randomizer_lbl.Text = "BashPrime\'s Randomizer not found! (v2.6.0 or later required)";
            this.randomizer_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // plandomizer_tab
            // 
            this.plandomizer_tab.Controls.Add(this.plandomizer_tab_table_layout);
            this.plandomizer_tab.Location = new System.Drawing.Point(4, 22);
            this.plandomizer_tab.Name = "plandomizer_tab";
            this.plandomizer_tab.Size = new System.Drawing.Size(792, 139);
            this.plandomizer_tab.TabIndex = 2;
            this.plandomizer_tab.Text = "Plandomizer";
            this.plandomizer_tab.UseVisualStyleBackColor = true;
            // 
            // plandomizer_tab_table_layout
            // 
            this.plandomizer_tab_table_layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.plandomizer_tab_table_layout.ColumnCount = 1;
            this.plandomizer_tab_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.plandomizer_tab_table_layout.Controls.Add(this.plandomizer_lbl, 0, 2);
            this.plandomizer_tab_table_layout.Controls.Add(this.input_json_lbl, 0, 0);
            this.plandomizer_tab_table_layout.Controls.Add(this.input_json_table_layout, 0, 1);
            this.plandomizer_tab_table_layout.Controls.Add(this.plandomizer_btn_table_layout, 0, 3);
            this.plandomizer_tab_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plandomizer_tab_table_layout.Location = new System.Drawing.Point(0, 0);
            this.plandomizer_tab_table_layout.Name = "plandomizer_tab_table_layout";
            this.plandomizer_tab_table_layout.RowCount = 5;
            this.plandomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.plandomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.plandomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.plandomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.plandomizer_tab_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.plandomizer_tab_table_layout.Size = new System.Drawing.Size(792, 139);
            this.plandomizer_tab_table_layout.TabIndex = 1;
            // 
            // plandomizer_lbl
            // 
            this.plandomizer_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plandomizer_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.plandomizer_lbl.Location = new System.Drawing.Point(3, 52);
            this.plandomizer_lbl.Name = "plandomizer_lbl";
            this.plandomizer_lbl.Size = new System.Drawing.Size(786, 40);
            this.plandomizer_lbl.TabIndex = 9;
            this.plandomizer_lbl.Text = "toasterparty\'s Plandomizer not found! (Recommended version : 1.6)";
            this.plandomizer_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // input_json_lbl
            // 
            this.input_json_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.input_json_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.input_json_lbl.Location = new System.Drawing.Point(3, 0);
            this.input_json_lbl.Name = "input_json_lbl";
            this.input_json_lbl.Size = new System.Drawing.Size(786, 26);
            this.input_json_lbl.TabIndex = 12;
            this.input_json_lbl.Text = "Input JSON :";
            this.input_json_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // input_json_table_layout
            // 
            this.input_json_table_layout.ColumnCount = 2;
            this.input_json_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.input_json_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.input_json_table_layout.Controls.Add(this.input_json_txt_box, 0, 0);
            this.input_json_table_layout.Controls.Add(this.input_json_btn, 1, 0);
            this.input_json_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.input_json_table_layout.Location = new System.Drawing.Point(0, 26);
            this.input_json_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.input_json_table_layout.Name = "input_json_table_layout";
            this.input_json_table_layout.RowCount = 1;
            this.input_json_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.input_json_table_layout.Size = new System.Drawing.Size(792, 26);
            this.input_json_table_layout.TabIndex = 11;
            // 
            // input_json_txt_box
            // 
            this.input_json_txt_box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.input_json_txt_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input_json_txt_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.input_json_txt_box.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.input_json_txt_box.Location = new System.Drawing.Point(3, 3);
            this.input_json_txt_box.MaxLength = 260;
            this.input_json_txt_box.Name = "input_json_txt_box";
            this.input_json_txt_box.Size = new System.Drawing.Size(686, 20);
            this.input_json_txt_box.TabIndex = 4;
            // 
            // input_json_btn
            // 
            this.input_json_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.input_json_btn.Location = new System.Drawing.Point(695, 3);
            this.input_json_btn.Name = "input_json_btn";
            this.input_json_btn.Padding = new System.Windows.Forms.Padding(5);
            this.input_json_btn.Size = new System.Drawing.Size(94, 20);
            this.input_json_btn.TabIndex = 5;
            this.input_json_btn.Text = "Browse";
            this.input_json_btn.Click += new System.EventHandler(this.input_json_btn_click);
            // 
            // plandomizer_btn_table_layout
            // 
            this.plandomizer_btn_table_layout.ColumnCount = 2;
            this.plandomizer_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.plandomizer_btn_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.plandomizer_btn_table_layout.Controls.Add(this.locate_plandomizer_btn, 0, 0);
            this.plandomizer_btn_table_layout.Controls.Add(this.plandomize_btn, 1, 0);
            this.plandomizer_btn_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plandomizer_btn_table_layout.Location = new System.Drawing.Point(0, 92);
            this.plandomizer_btn_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.plandomizer_btn_table_layout.Name = "plandomizer_btn_table_layout";
            this.plandomizer_btn_table_layout.RowCount = 1;
            this.plandomizer_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.plandomizer_btn_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.plandomizer_btn_table_layout.Size = new System.Drawing.Size(792, 40);
            this.plandomizer_btn_table_layout.TabIndex = 8;
            // 
            // locate_plandomizer_btn
            // 
            this.locate_plandomizer_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.locate_plandomizer_btn.Location = new System.Drawing.Point(3, 3);
            this.locate_plandomizer_btn.Name = "locate_plandomizer_btn";
            this.locate_plandomizer_btn.Padding = new System.Windows.Forms.Padding(5);
            this.locate_plandomizer_btn.Size = new System.Drawing.Size(390, 34);
            this.locate_plandomizer_btn.TabIndex = 4;
            this.locate_plandomizer_btn.Text = "Locate toasterparty\'s Plandomizer";
            this.locate_plandomizer_btn.Click += new System.EventHandler(this.locate_plandomizer_btn_Click);
            // 
            // plandomize_btn
            // 
            this.plandomize_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plandomize_btn.Location = new System.Drawing.Point(399, 3);
            this.plandomize_btn.Name = "plandomize_btn";
            this.plandomize_btn.Padding = new System.Windows.Forms.Padding(5);
            this.plandomize_btn.Size = new System.Drawing.Size(390, 34);
            this.plandomize_btn.TabIndex = 5;
            this.plandomize_btn.Text = "Plandomize";
            this.plandomize_btn.Click += new System.EventHandler(this.plandomize_btn_Click);
            // 
            // settings_tab
            // 
            this.settings_tab.Controls.Add(this.settings_table_layout);
            this.settings_tab.Location = new System.Drawing.Point(4, 22);
            this.settings_tab.Name = "settings_tab";
            this.settings_tab.Size = new System.Drawing.Size(792, 139);
            this.settings_tab.TabIndex = 3;
            this.settings_tab.Text = "Settings";
            this.settings_tab.UseVisualStyleBackColor = true;
            // 
            // settings_table_layout
            // 
            this.settings_table_layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.settings_table_layout.ColumnCount = 2;
            this.settings_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.settings_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.settings_table_layout.Controls.Add(this.disable_spring_ball_check_box, 0, 0);
            this.settings_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings_table_layout.Location = new System.Drawing.Point(0, 0);
            this.settings_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.settings_table_layout.Name = "settings_table_layout";
            this.settings_table_layout.RowCount = 6;
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.settings_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.settings_table_layout.Size = new System.Drawing.Size(792, 139);
            this.settings_table_layout.TabIndex = 3;
            // 
            // disable_spring_ball_check_box
            // 
            this.disable_spring_ball_check_box.AutoSize = true;
            this.disable_spring_ball_check_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.disable_spring_ball_check_box.Location = new System.Drawing.Point(3, 3);
            this.disable_spring_ball_check_box.Name = "disable_spring_ball_check_box";
            this.disable_spring_ball_check_box.Size = new System.Drawing.Size(390, 20);
            this.disable_spring_ball_check_box.TabIndex = 0;
            this.disable_spring_ball_check_box.Text = "Disable Spring Ball";
            // 
            // main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 360);
            this.Controls.Add(this.main_table_layout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 360);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 360);
            this.Name = "main_form";
            this.ShowIcon = false;
            this.Text = "Metroid Prime 1 - Trilogy Rando Generator v1.8";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.form1_MouseDown);
            this.main_table_layout.ResumeLayout(false);
            this.output_table_layout.ResumeLayout(false);
            this.output_path_table_layout.ResumeLayout(false);
            this.output_path_table_layout.PerformLayout();
            this.status_bar_table_layout.ResumeLayout(false);
            this.title_bar_table_layout.ResumeLayout(false);
            this.title_bar_table_layout.PerformLayout();
            this.tab_btn_bar_table_layout.ResumeLayout(false);
            this.tab_manager.ResumeLayout(false);
            this.template_iso_tab.ResumeLayout(false);
            this.template_iso_tab_table_layout.ResumeLayout(false);
            this.template_iso_btn_table_layout.ResumeLayout(false);
            this.randomizer_tab.ResumeLayout(false);
            this.randomizer_tab_table_layout.ResumeLayout(false);
            this.randomizer_btn_table_layout.ResumeLayout(false);
            this.plandomizer_tab.ResumeLayout(false);
            this.plandomizer_tab_table_layout.ResumeLayout(false);
            this.input_json_table_layout.ResumeLayout(false);
            this.input_json_table_layout.PerformLayout();
            this.plandomizer_btn_table_layout.ResumeLayout(false);
            this.settings_tab.ResumeLayout(false);
            this.settings_table_layout.ResumeLayout(false);
            this.settings_table_layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel main_table_layout;
        private System.Windows.Forms.TableLayoutPanel randomizer_tab_table_layout;
        private System.Windows.Forms.TableLayoutPanel output_path_table_layout;
        private System.Windows.Forms.TableLayoutPanel status_bar_table_layout;
        private System.Windows.Forms.TableLayoutPanel randomizer_btn_table_layout;
        private System.Windows.Forms.TableLayoutPanel title_bar_table_layout;
        private System.Windows.Forms.Button close_btn;
        private System.Windows.Forms.Button help_btn;
        private DarkUI.Controls.DarkLabel title_lbl;
        private System.Windows.Forms.TableLayoutPanel tab_btn_bar_table_layout;
        private System.Windows.Forms.TabPage template_iso_tab;
        private System.Windows.Forms.TabPage randomizer_tab;
        private System.Windows.Forms.TabPage plandomizer_tab;
        private System.Windows.Forms.TableLayoutPanel template_iso_tab_table_layout;
        private System.Windows.Forms.TableLayoutPanel template_iso_btn_table_layout;
        private DarkUI.Controls.DarkButton generate_template_iso_btn;
        private DarkUI.Controls.DarkButton delete_cache_btn;
        private DarkUI.Controls.DarkLabel template_iso_lbl;
        private System.Windows.Forms.TableLayoutPanel output_table_layout;
        private DarkUI.Controls.DarkLabel output_path_lbl;
        private DarkUI.Controls.DarkLabel output_type_lbl;
        private DarkUI.Controls.DarkTextBox output_path_txt_box;
        private DarkUI.Controls.DarkButton output_path_btn;
        private DarkUI.Controls.DarkComboBox output_type_combo_box;
        private DarkUI.Controls.DarkButton locate_randomizer_btn;
        private DarkUI.Controls.DarkButton randomize_btn;
        private DarkUI.Controls.DarkLabel randomizer_lbl;
        private System.Windows.Forms.TableLayoutPanel plandomizer_tab_table_layout;
        private System.Windows.Forms.TableLayoutPanel plandomizer_btn_table_layout;
        private DarkUI.Controls.DarkButton locate_plandomizer_btn;
        private DarkUI.Controls.DarkButton plandomize_btn;
        private DarkUI.Controls.DarkLabel plandomizer_lbl;
        internal System.Windows.Forms.Button template_iso_btn;
        internal System.Windows.Forms.Button randomizer_btn;
        internal System.Windows.Forms.Button plandomizer_btn;
        internal System.Windows.Forms.TabControlNoHeader tab_manager;
        internal DarkUI.Controls.DarkLabel status_lbl;
        internal System.Windows.Forms.ProgressBar status_progress_bar;
        private System.Windows.Forms.TableLayoutPanel input_json_table_layout;
        private DarkUI.Controls.DarkTextBox input_json_txt_box;
        private DarkUI.Controls.DarkButton input_json_btn;
        private DarkUI.Controls.DarkLabel input_json_lbl;
        private System.Windows.Forms.TabPage settings_tab;
        private System.Windows.Forms.TableLayoutPanel settings_table_layout;
        internal System.Windows.Forms.Button settings_btn;
        private DarkUI.Controls.DarkCheckBox disable_spring_ball_check_box;
    }
}

