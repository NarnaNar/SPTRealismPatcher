namespace SPTRealismPatcher
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtTemplatesPath = new TextBox();
            txtNewItemsPath = new TextBox();
            txtExistingPatchPath = new TextBox();
            lblTemplatePath = new Label();
            lblDBPath = new Label();
            lblPatchPath = new Label();
            cmdLoadFiles = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // txtTemplatesPath
            // 
            txtTemplatesPath.Location = new Point(246, 28);
            txtTemplatesPath.Name = "txtTemplatesPath";
            txtTemplatesPath.Size = new Size(616, 23);
            txtTemplatesPath.TabIndex = 2;
            // 
            // txtNewItemsPath
            // 
            txtNewItemsPath.Location = new Point(246, 57);
            txtNewItemsPath.Name = "txtNewItemsPath";
            txtNewItemsPath.Size = new Size(616, 23);
            txtNewItemsPath.TabIndex = 3;
            // 
            // txtExistingPatchPath
            // 
            txtExistingPatchPath.Location = new Point(246, 86);
            txtExistingPatchPath.Name = "txtExistingPatchPath";
            txtExistingPatchPath.Size = new Size(616, 23);
            txtExistingPatchPath.TabIndex = 4;
            // 
            // lblTemplatePath
            // 
            lblTemplatePath.AutoSize = true;
            lblTemplatePath.Location = new Point(158, 31);
            lblTemplatePath.Name = "lblTemplatePath";
            lblTemplatePath.Size = new Size(82, 15);
            lblTemplatePath.TabIndex = 5;
            lblTemplatePath.Text = "Template Path";
            // 
            // lblDBPath
            // 
            lblDBPath.AutoSize = true;
            lblDBPath.Location = new Point(158, 65);
            lblDBPath.Name = "lblDBPath";
            lblDBPath.Size = new Size(82, 15);
            lblDBPath.TabIndex = 6;
            lblDBPath.Text = "Jsons to Patch";
            // 
            // lblPatchPath
            // 
            lblPatchPath.AutoSize = true;
            lblPatchPath.Location = new Point(148, 94);
            lblPatchPath.Name = "lblPatchPath";
            lblPatchPath.Size = new Size(92, 15);
            lblPatchPath.TabIndex = 7;
            lblPatchPath.Text = "Existing Patches";
            // 
            // cmdLoadFiles
            // 
            cmdLoadFiles.Location = new Point(148, 127);
            cmdLoadFiles.Name = "cmdLoadFiles";
            cmdLoadFiles.Size = new Size(146, 29);
            cmdLoadFiles.TabIndex = 10;
            cmdLoadFiles.Text = "Export Patches To";
            cmdLoadFiles.UseVisualStyleBackColor = true;
            cmdLoadFiles.Click += cmdLoadFiles_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(300, 131);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(616, 23);
            textBox1.TabIndex = 11;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 185);
            Controls.Add(textBox1);
            Controls.Add(cmdLoadFiles);
            Controls.Add(lblPatchPath);
            Controls.Add(lblDBPath);
            Controls.Add(lblTemplatePath);
            Controls.Add(txtExistingPatchPath);
            Controls.Add(txtNewItemsPath);
            Controls.Add(txtTemplatesPath);
            Name = "frmMain";
            Text = "Form1";
            FormClosing += frmMain_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtTemplatesPath;
        private TextBox txtNewItemsPath;
        private TextBox txtExistingPatchPath;
        private Label lblTemplatePath;
        private Label lblDBPath;
        private Label lblPatchPath;
        private Button cmdLoadFiles;
        private TextBox textBox1;
    }
}