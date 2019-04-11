using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpenSBP_Client.Forms {
    public partial class frmPosition : Form {

        public bool PermitMove { get; set; }
        public bool ParentClosing { get; set; }

        public frmCommandConsole MyParent { get; set; }

        public frmPartFileLoad FileLoader { get; set; }

        public frmPosition() {
            InitializeComponent();
            ConfigurePane();

        }

        protected override void WndProc(ref Message message) {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            if (!PermitMove) {
                switch (message.Msg) {
                    case WM_SYSCOMMAND:
                        int command = message.WParam.ToInt32() & 0xfff0;
                        if (command == SC_MOVE)  // prevents the user from moving the window.
                            return;
                        break;
                }
            }

            base.WndProc(ref message);
        }

        public void SetStartMode() {
            btnLoadOrStart.BackColor = Color.Green;
            btnLoadOrStart.ForeColor = Color.Yellow;
            btnLoadOrStart.Text = "Start";
            btnLoadOrStart.Font = new Font(btnLoadOrStart.Font, FontStyle.Bold);
        }

        public void SetIdleMode() {
            btnLoadOrStart.BackColor = Color.DarkGreen;
            btnLoadOrStart.ForeColor = Color.Gold;
            btnLoadOrStart.Text = "Cut Part";

        }

        private void ConfigurePane() {
            // set up the control defaults...
            ResetLamps();
            btnLoadOrStart.BackColor = Color.DarkGreen;
            btnLoadOrStart.ForeColor = Color.Gold;
            btnLoadOrStart.Text = "Cut Part";
            btnLoadOrStart.Font = new Font(btnLoadOrStart.Font, FontStyle.Regular);
            togMoveOrPreview.Checked = false;
        }

        private void ResetLamps() {
            // turns all the I/O LEDs off.
            ledInput1.On = false;
            ledInput2.On = false;
            ledInput3.On = false;
            ledInput4.On = false;
            ledInput5.On = false;
            ledInput6.On = false;
            ledInput7.On = false;
            ledInput8.On = false;

            ledOutput1.On = false;
            ledOutput2.On = false;
            ledOutput3.On = false;
            ledOutput4.On = false;
            ledOutput5.On = false;
            ledOutput6.On = false;
            ledOutput7.On = false;
            ledOutput8.On = false;
        }

        void SetMoveMode() {
            grpInputs.Visible = true;
            grpOutputs.Visible = true;
        }

        void SetPreviewMode() {
            grpInputs.Visible = false;
            grpOutputs.Visible = false;
        }
        private void FrmPosition_FormClosing(object sender, FormClosingEventArgs e) {
            if (!ParentClosing) {
                MyParent.PositionClosedUs();

            }

        }

        private void TogMoveOrPreview_CheckedChanged(object sender, EventArgs e) {
            if (togMoveOrPreview.Checked)
                SetPreviewMode();
            else
                SetMoveMode();
        }

        private void BtnLoadOrStart_Click(object sender, EventArgs e) {
            if (btnLoadOrStart.Text == "Cut Part") {
                MyParent.MenuPartFileLoad_Click(sender, e);
            }else if (btnLoadOrStart.Text == "Start") {
                if (MyParent.LoadSBProgram(FileLoader.partFilename)) {
                    FileLoader.Close();
                    MyParent.RunSBProgram();
                }
            }
        }
    }
}
