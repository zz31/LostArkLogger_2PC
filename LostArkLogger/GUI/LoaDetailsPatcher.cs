﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LostArkLogger.GUI
{
    public partial class LoaDetailsPatcher : Form
    {
        public LoaDetailsPatcher()
        {
            InitializeComponent();
        }

        bool patchToForkedVersion = true;
        public void showNotice()
        {
            MessageBox.Show("if you're running loa-details, exit loa-details first.\n\n"+
                "running process's filename can't be changed so if you're wanna roll back to original binary, "+
                "copy this exe and paste same folder and run it.", "Notice");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\loa-details\";
            string binaryPath = path + @"binary\";
            string binaryName = "ae92984b-6f1b-4b0d-ad31-504e1905d5e6.exe";
            string oo2Name = "oo2net_9_win64.dll";
            DirectoryInfo di = new DirectoryInfo(binaryPath);
            FileInfo[] files = di.GetFiles();
            bool fileFound = false;
            bool originalFileFound = false;
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name == binaryName) fileFound = true;
                if (files[i].Name == binaryName + ".original") originalFileFound = true;
            }

            if (patchToForkedVersion == true &&
                MessageBox.Show("Patch Loa-details binary to 2pc forked version?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bool hasException = false;
                try
                {
                    string curPath = Directory.GetCurrentDirectory()+@"\";
                    FileInfo[] fi = new DirectoryInfo(curPath).GetFiles();
                    bool oo2Found = false;
                    bool meterFound = false;
                    string fname = AppDomain.CurrentDomain.FriendlyName;
                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (fi[i].Name == AppDomain.CurrentDomain.FriendlyName) meterFound = true;
                        if (fi[i].Name == oo2Name) oo2Found = true;
                    }
                    if (!oo2Found)
                    {
                        MessageBox.Show("don't delete oo2net_9_win64.dll. re-unzip debug.zip and retry.");
                        return;
                    }
                    if (!fileFound)
                    {
                        MessageBox.Show("original file not exist, needs reinstall loa-details");
                        return;
                    }
                    if (!meterFound)
                    {
                        MessageBox.Show("?? don't delete dps meter's .exe file. why you're deleted it?");
                        Environment.Exit(Environment.ExitCode);
                        return;
                    }
                    if (originalFileFound == false)
                    {
                        File.Move(binaryPath + binaryName, binaryPath + binaryName + ".original");
                        File.Copy(curPath + AppDomain.CurrentDomain.FriendlyName, binaryPath + binaryName);
                        if (!File.Exists(path + oo2Name)) File.Copy(curPath + oo2Name, path + oo2Name);
                        if (!File.Exists(binaryPath + oo2Name)) File.Copy(curPath + oo2Name, binaryPath + oo2Name);
                    }
                    else//patch new meter only
                    {
                        File.Delete(binaryPath + binaryName);
                        File.Copy(curPath + AppDomain.CurrentDomain.FriendlyName, binaryPath + binaryName);
                        if (!File.Exists(path + oo2Name)) File.Copy(curPath + oo2Name, path + oo2Name);
                        if (!File.Exists(binaryPath + oo2Name)) File.Copy(curPath + oo2Name, binaryPath + oo2Name);
                        MessageBox.Show("overwrited loa-details binary to current binary.");
                    }
                } catch(Exception) { hasException = true; }
                if (hasException)
                {
                    MessageBox.Show("unknown error");
                } else
                {
                    MessageBox.Show("successfully patched loa-details binary. close this meter and start loa-details to use it.");
                }
            }
            else if (patchToForkedVersion == false &&
                MessageBox.Show("Delete 2pc forked version binary and roll back to default Loa-details binary?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bool hasException = false;
                try
                {
                    if (originalFileFound == false)
                    {
                        MessageBox.Show("patcher can't find original binary. reinstall loa-details to roll back to original binary. sorry");
                        return;
                    }

                    File.Delete(binaryPath + binaryName);
                    File.Delete(binaryPath + oo2Name);
                    File.Move(binaryPath + binaryName + ".original", binaryPath + binaryName);
                } catch(Exception) { hasException = true; }
                if (hasException)
                {
                    MessageBox.Show("unknown error");
                }
                else
                {
                    MessageBox.Show("successfully removed 2pc fork version binary. at next start, loa-details will use default binary.");
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            patchToForkedVersion = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            patchToForkedVersion = false;
        }
    }
}
