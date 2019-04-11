using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenSBP_Client.Forms {
    public partial class frmPartFileLoad : Form {
        private static string FileFilter = "Part Files (*.sbp)|*.sbp|" +
                     "Default Config Files (*.sbd)|*.sbd|" +
                     "Custom Cut Files (*.sbc)|*.sbc|" +
                     "Rotary Files (*.sbr)|*.sbr|" +
                     "All OpenSBP Files (*.sb*)|*.sb*|" +
                     "G Code Files (*.gcode)|*.gcode|" +
                     "All Files (*.*)|*.*";

        public bool isMono { get; set; }
        public string partFilename { get; set; }

        public frmPosition PositionWindow { get; set; }

        public frmPartFileLoad(frmPosition posWindow, bool runningOnMono, string fileName) {
            InitializeComponent();
            PositionWindow = posWindow;
            isMono = runningOnMono;
            partFilename = fileName;
        }

        private void FrmPartFileLload_Load(object sender, EventArgs e) {
            foreach (Control C in this.Controls) {
                if (C.GetType() == typeof(System.Windows.Forms.Label)) {
                    C.BackColor = Color.Transparent;
                }
                // adjust top position of controls if we're on Mono...(no idea why they're mis-aligned)
                if (isMono)
                    C.Top -= 7;
            }
            //txtPartFilename.BackColor = Color.Transparent;
            txtPartFilename.Text = partFilename;

            cboOffset.Items.Add("0 - No Offset");
            cboOffset.Items.Add("1 - 3D Offset");
            cboOffset.Items.Add("2 - 2D Offset");
            cboOffset.BackColor = Color.FromArgb(245, 238, 150);
            cboOffset.SelectedIndex = 0;

            txtPropX.Text = "1.00";
            txtPropY.Text = "1.00";
            txtPropZ.Text = "1.00";

            cboTabbing.Items.Add("0 - Off");
            cboTabbing.Items.Add("1 - On");
            cboTabbing.BackColor = Color.FromArgb(245, 238, 150);
            cboTabbing.SelectedIndex = 0;

            txtPlunge.Text = "0.00";
            txtReps.Text = "1";

            cboPlunge.Items.Add("0 - Off");
            cboPlunge.Items.Add("1 - On");
            cboPlunge.BackColor = Color.FromArgb(245, 238, 150);
            cboPlunge.SelectedIndex = 0;
            PositionWindow.SetStartMode();
            PositionWindow.FileLoader = this;
        }

        private void BtnCancel_Click(object sender, EventArgs e) {
            PositionWindow.SetIdleMode();
            this.Close();
        }

        private void BtnRecallLast_Click(object sender, EventArgs e) {

        }

        private void FrmPartFileLoad_FormClosing(object sender, FormClosingEventArgs e) {
            PositionWindow.SetIdleMode();
        }

        private void BtnPickFile_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an OpenSBP Part File";
            ofd.Filter = FileFilter;
            ofd.FileName = txtPartFilename.Text;
            if (ofd.ShowDialog() == DialogResult.OK) {
                txtPartFilename.Text = ofd.FileName;
                ofd.Dispose();
            }
        }

        private void TxtPartFilename_TextChanged(object sender, EventArgs e) {
            partFilename = txtPartFilename.Text;
        }
    }
}
