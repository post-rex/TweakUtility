﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TweakUtility.Forms
{
    internal partial class BackupsForm : Form
    {
        internal BackupsForm()
        {
            this.InitializeComponent();

            this.applyButton.Text = Properties.Strings.Button_Apply;
            this.cancelButton.Text = Properties.Strings.Button_Cancel;
            this.createButton.Text = Properties.Strings.Button_Create;
            this.deleteButton.Text = Properties.Strings.Button_Delete;
            this.openFolderButton.Text = Properties.Strings.Backups_OpenFolder;
        }

        private void BackupsForm_Load(object sender, EventArgs e)
        {
            listView.Items.Clear();

            foreach (Backup backup in Program.Backups)
            {
                var item = new ListViewItem(backup.Name)
                {
                    Tag = backup
                };

                item.SubItems.Add(backup.Date.ToShortDateString());
                item.SubItems.Add($"{backup.Size / 1000} KB");

                listView.Items.Add(item);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            var backup = listView.SelectedItems[0].Tag as Backup;

            this.Enabled = false;

            backup.Apply();

            this.Close();
        }

        private void OpenFolderButton_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("explorer.exe", Path.GetFullPath("backups")) { UseShellExecute = true });

        private void CreateButton_Click(object sender, EventArgs e)
        {
            using (var form = new BackupCreateForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Program.LoadBackups();
                    this.BackupsForm_Load(this, EventArgs.Empty);
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var backup = listView.SelectedItems[0].Tag as Backup;

            this.Enabled = false;

            backup.Delete();
            this.BackupsForm_Load(this, EventArgs.Empty);

            this.Enabled = true;
        }
    }
}