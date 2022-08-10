namespace LostArkLogger
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.loggedPacketCountLabel = new System.Windows.Forms.Label();
            this.weblink = new System.Windows.Forms.LinkLabel();
            this.overlayEnabled = new System.Windows.Forms.CheckBox();
            this.debugLog = new System.Windows.Forms.CheckBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.regionSelector = new System.Windows.Forms.ComboBox();
            this.autoUpload = new System.Windows.Forms.CheckBox();
            this.displayName = new System.Windows.Forms.CheckBox();
            this.nicListBox = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loggedPacketCountLabel
            // 
            this.loggedPacketCountLabel.Location = new System.Drawing.Point(13, 9);
            this.loggedPacketCountLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.loggedPacketCountLabel.Name = "loggedPacketCountLabel";
            this.loggedPacketCountLabel.Size = new System.Drawing.Size(121, 12);
            this.loggedPacketCountLabel.TabIndex = 2;
            this.loggedPacketCountLabel.Text = "Packets: 0";
            // 
            // weblink
            // 
            this.weblink.Location = new System.Drawing.Point(144, 9);
            this.weblink.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.weblink.Name = "weblink";
            this.weblink.Size = new System.Drawing.Size(127, 12);
            this.weblink.TabIndex = 4;
            this.weblink.TabStop = true;
            this.weblink.Text = "by shalzuth";
            this.weblink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.weblink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.weblink_LinkClicked);
            // 
            // overlayEnabled
            // 
            this.overlayEnabled.AutoSize = true;
            this.overlayEnabled.Checked = true;
            this.overlayEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overlayEnabled.Enabled = false;
            this.overlayEnabled.Location = new System.Drawing.Point(13, 24);
            this.overlayEnabled.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.overlayEnabled.Name = "overlayEnabled";
            this.overlayEnabled.Size = new System.Drawing.Size(67, 16);
            this.overlayEnabled.TabIndex = 5;
            this.overlayEnabled.Text = "Overlay";
            this.overlayEnabled.UseVisualStyleBackColor = true;
            this.overlayEnabled.CheckedChanged += new System.EventHandler(this.overlayEnabled_CheckedChanged);
            // 
            // debugLog
            // 
            this.debugLog.AutoSize = true;
            this.debugLog.Location = new System.Drawing.Point(85, 24);
            this.debugLog.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.debugLog.Name = "debugLog";
            this.debugLog.Size = new System.Drawing.Size(60, 16);
            this.debugLog.TabIndex = 9;
            this.debugLog.Text = "Debug";
            this.debugLog.UseVisualStyleBackColor = true;
            this.debugLog.CheckedChanged += new System.EventHandler(this.debugLog_CheckedChanged);
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(11, 69);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(260, 22);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.Text = "EN 1.34.58.1816579 KR 1.985.1060.1822798";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // regionSelector
            // 
            this.regionSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionSelector.FormattingEnabled = true;
            this.regionSelector.Location = new System.Drawing.Point(190, 46);
            this.regionSelector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.regionSelector.Name = "regionSelector";
            this.regionSelector.Size = new System.Drawing.Size(81, 20);
            this.regionSelector.TabIndex = 12;
            this.regionSelector.SelectedIndexChanged += new System.EventHandler(this.regionSelector_SelectedIndexChanged);
            // 
            // autoUpload
            // 
            this.autoUpload.AutoSize = true;
            this.autoUpload.Enabled = false;
            this.autoUpload.Location = new System.Drawing.Point(144, 24);
            this.autoUpload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.autoUpload.Name = "autoUpload";
            this.autoUpload.Size = new System.Drawing.Size(63, 16);
            this.autoUpload.TabIndex = 13;
            this.autoUpload.Text = "Upload";
            this.autoUpload.UseVisualStyleBackColor = true;
            // 
            // displayName
            // 
            this.displayName.AutoSize = true;
            this.displayName.Checked = true;
            this.displayName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displayName.Location = new System.Drawing.Point(206, 24);
            this.displayName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.displayName.Name = "displayName";
            this.displayName.Size = new System.Drawing.Size(65, 16);
            this.displayName.TabIndex = 14;
            this.displayName.Text = "Names";
            this.displayName.UseVisualStyleBackColor = true;
            // 
            // nicListBox
            // 
            this.nicListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nicListBox.FormattingEnabled = true;
            this.nicListBox.Location = new System.Drawing.Point(13, 46);
            this.nicListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nicListBox.Name = "nicListBox";
            this.nicListBox.Size = new System.Drawing.Size(175, 20);
            this.nicListBox.TabIndex = 15;
            this.nicListBox.SelectedIndexChanged += new System.EventHandler(this.nicListBox_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 98);
            this.Controls.Add(this.nicListBox);
            this.Controls.Add(this.displayName);
            this.Controls.Add(this.autoUpload);
            this.Controls.Add(this.regionSelector);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.debugLog);
            this.Controls.Add(this.overlayEnabled);
            this.Controls.Add(this.weblink);
            this.Controls.Add(this.loggedPacketCountLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "2PC";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label loggedPacketCountLabel;
        private System.Windows.Forms.LinkLabel weblink;
        private System.Windows.Forms.CheckBox overlayEnabled;
        private System.Windows.Forms.CheckBox debugLog;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.ComboBox regionSelector;
        private System.Windows.Forms.CheckBox autoUpload;
        private System.Windows.Forms.CheckBox displayName;
        private System.Windows.Forms.ComboBox nicListBox;
        private System.Windows.Forms.Timer timer1;
    }
}
