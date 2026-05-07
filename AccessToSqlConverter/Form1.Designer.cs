namespace AccessToSqlConverter
{
    partial class Form1
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnSelectAccessDb = new System.Windows.Forms.Button();
            this.btnDbImport = new System.Windows.Forms.Button();
            this.lblSelectedDatabase = new System.Windows.Forms.Label();
            this.tsLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1568, 28);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 425);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1568, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnSelectAccessDb
            // 
            this.btnSelectAccessDb.Location = new System.Drawing.Point(43, 84);
            this.btnSelectAccessDb.Name = "btnSelectAccessDb";
            this.btnSelectAccessDb.Size = new System.Drawing.Size(181, 57);
            this.btnSelectAccessDb.TabIndex = 2;
            this.btnSelectAccessDb.Text = "Select Access Database";
            this.btnSelectAccessDb.UseVisualStyleBackColor = true;
            this.btnSelectAccessDb.Click += new System.EventHandler(this.btnSelectAccessDb_Click);
            // 
            // btnDbImport
            // 
            this.btnDbImport.Enabled = false;
            this.btnDbImport.Location = new System.Drawing.Point(43, 151);
            this.btnDbImport.Name = "btnDbImport";
            this.btnDbImport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDbImport.Size = new System.Drawing.Size(181, 57);
            this.btnDbImport.TabIndex = 3;
            this.btnDbImport.Text = "Import Data";
            this.btnDbImport.UseVisualStyleBackColor = true;
            this.btnDbImport.Click += new System.EventHandler(this.btnDbImport_Click);
            // 
            // lblSelectedDatabase
            // 
            this.lblSelectedDatabase.AutoSize = true;
            this.lblSelectedDatabase.Location = new System.Drawing.Point(276, 104);
            this.lblSelectedDatabase.Name = "lblSelectedDatabase";
            this.lblSelectedDatabase.Size = new System.Drawing.Size(0, 17);
            this.lblSelectedDatabase.TabIndex = 4;
            // 
            // tsLabel1
            // 
            this.tsLabel1.Name = "tsLabel1";
            this.tsLabel1.Size = new System.Drawing.Size(151, 20);
            this.tsLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 20);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1568, 450);
            this.Controls.Add(this.lblSelectedDatabase);
            this.Controls.Add(this.btnDbImport);
            this.Controls.Add(this.btnSelectAccessDb);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "Form1";
            this.Text = "Form1";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnSelectAccessDb;
        private System.Windows.Forms.Button btnDbImport;
        private System.Windows.Forms.Label lblSelectedDatabase;
        private System.Windows.Forms.ToolStripStatusLabel tsLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

