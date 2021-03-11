namespace MP1_Trilogy_Rando_Generator
{
    partial class HelpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.main_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.title_bar_table_layout = new System.Windows.Forms.TableLayoutPanel();
            this.title_lbl = new DarkUI.Controls.DarkLabel();
            this.close_btn = new System.Windows.Forms.Button();
            this.main_table_layout.SuspendLayout();
            this.title_bar_table_layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_table_layout
            // 
            this.main_table_layout.ColumnCount = 1;
            this.main_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_table_layout.Controls.Add(this.webBrowser1, 0, 1);
            this.main_table_layout.Controls.Add(this.title_bar_table_layout, 0, 0);
            this.main_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_table_layout.Location = new System.Drawing.Point(0, 0);
            this.main_table_layout.Name = "main_table_layout";
            this.main_table_layout.RowCount = 2;
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.main_table_layout.Size = new System.Drawing.Size(1008, 729);
            this.main_table_layout.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 35);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1002, 691);
            this.webBrowser1.TabIndex = 5;
            // 
            // title_bar_table_layout
            // 
            this.title_bar_table_layout.ColumnCount = 2;
            this.title_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.title_bar_table_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.title_bar_table_layout.Controls.Add(this.close_btn, 0, 0);
            this.title_bar_table_layout.Controls.Add(this.title_lbl, 0, 0);
            this.title_bar_table_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_bar_table_layout.Location = new System.Drawing.Point(0, 0);
            this.title_bar_table_layout.Margin = new System.Windows.Forms.Padding(0);
            this.title_bar_table_layout.Name = "title_bar_table_layout";
            this.title_bar_table_layout.RowCount = 1;
            this.title_bar_table_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.title_bar_table_layout.Size = new System.Drawing.Size(1008, 32);
            this.title_bar_table_layout.TabIndex = 6;
            // 
            // title_lbl
            // 
            this.title_lbl.AutoSize = true;
            this.title_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_lbl.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.title_lbl.Location = new System.Drawing.Point(3, 0);
            this.title_lbl.Name = "title_lbl";
            this.title_lbl.Size = new System.Drawing.Size(970, 32);
            this.title_lbl.TabIndex = 5;
            this.title_lbl.Text = "MP1 Trilogy Rando Generator v2.0 - Help";
            this.title_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // close_btn
            // 
            this.close_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.close_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.close_btn.Location = new System.Drawing.Point(979, 3);
            this.close_btn.Name = "close_btn";
            this.close_btn.Size = new System.Drawing.Size(26, 26);
            this.close_btn.TabIndex = 6;
            this.close_btn.Text = "X";
            this.close_btn.UseVisualStyleBackColor = false;
            this.close_btn.Click += new System.EventHandler(this.close_btn_Click);
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.main_table_layout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HelpForm";
            this.Text = "Help";
            this.main_table_layout.ResumeLayout(false);
            this.title_bar_table_layout.ResumeLayout(false);
            this.title_bar_table_layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel main_table_layout;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TableLayoutPanel title_bar_table_layout;
        private DarkUI.Controls.DarkLabel title_lbl;
        private System.Windows.Forms.Button close_btn;
    }
}