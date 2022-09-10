namespace LostArkLogger.GUI
{
    partial class LoaDetailsPatcher
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
            this.btn_patch = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_select = new System.Windows.Forms.Button();
            this.lbl_path = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_patch
            // 
            this.btn_patch.AllowDrop = true;
            this.btn_patch.Location = new System.Drawing.Point(12, 9);
            this.btn_patch.Name = "btn_patch";
            this.btn_patch.Size = new System.Drawing.Size(282, 24);
            this.btn_patch.TabIndex = 0;
            this.btn_patch.Text = "Patch";
            this.btn_patch.UseVisualStyleBackColor = true;
            this.btn_patch.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AllowDrop = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(282, 28);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "original binary -> fork binary\r\n(or update fork binary)";
            this.radioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AllowDrop = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 70);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(282, 28);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.Text = "fork binary -> original binary";
            this.radioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 99);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(282, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "ae92984b-6f1b-4b0d-ad31-504e1905d5e6.exe";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(12, 126);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(282, 23);
            this.btn_select.TabIndex = 5;
            this.btn_select.Text = "Manual selection(installation path)";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // lbl_path
            // 
            this.lbl_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_path.Location = new System.Drawing.Point(12, 152);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(282, 46);
            this.lbl_path.TabIndex = 6;
            this.lbl_path.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoaDetailsPatcher
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 205);
            this.Controls.Add(this.lbl_path);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.btn_patch);
            this.Name = "LoaDetailsPatcher";
            this.Text = "LoaDetailsPatcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_patch;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.Label lbl_path;
    }
}