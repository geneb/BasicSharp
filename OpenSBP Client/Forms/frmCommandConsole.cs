using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using OpenSBP;
using OpenSBP_Client.Forms;



namespace OpenSBP_Client {
    public partial class frmCommandConsole : Form {

        internal static bool isMono { get; } = Type.GetType("Mono.Runtime") != null;

        private bool ExternallyClosed { get; set; }

        public void PositionClosedUs() {
            // this is called by the Position dialog in order to tell us to close.
            ExternallyClosed = true;
            this.Close();
            Application.Exit();
        }

        private static string FileFilter = "Part Files (*.sbp)|*.sbp|" +
                             "Default Config Files (*.sbd)|*.sbd|" +
                             "Custom Cut Files (*.sbc)|*.sbc|" +
                             "Rotary Files (*.sbr)|*.sbr|" +
                             "All OpenSBP Files (*.sb*)|*.sb*|" +
                             "G Code Files (*.gcode)|*.gcode|" +
                             "All Files (*.*)|*.*";

        private frmPosition PositionWindow;
        private frmPartFileLoad loadForm;
        private Interpreter sbProg;

        public frmOutputWindow OutputWindow = new frmOutputWindow();

        public frmCommandConsole() {
            InitializeComponent();
        }

        private void FrmCommandConsole_Load(object sender, EventArgs e) {
            txtCmdInput.Width = this.Width - (txtCmdInput.Left + btnSaveHistory.Width + 30);
            lstPreviousCmds.Width = this.Width - (txtCmdInput.Left + btnSaveHistory.Width + 30);
            btnSaveHistory.Left = lstPreviousCmds.Left + lstPreviousCmds.Width + 5;
            btnReplayHistory.Left = lstPreviousCmds.Left + lstPreviousCmds.Width + 5;
            lstProgram.Width = this.Width - (grpSpeeds.Left + grpSpeeds.Width + 32);

            ResetForm();
            ConfigOpenFileDialog();
            ConfigPositionWindow();
        }

        private void FrmCommandConsole_Resize(object sender, EventArgs e) {
            // we need to prevent the window width from being narrowed too much.
            if (this.Width < 665)
                this.Width = 700;

            txtCmdInput.Width = this.Width - (txtCmdInput.Left + btnSaveHistory.Width + 30);
            lstPreviousCmds.Width = this.Width - (txtCmdInput.Left + btnSaveHistory.Width + 30);
            btnSaveHistory.Left = lstPreviousCmds.Left + lstPreviousCmds.Width + 5;
            btnReplayHistory.Left = lstPreviousCmds.Left + lstPreviousCmds.Width + 5;
            lstProgram.Width = this.Width - (grpSpeeds.Left + grpSpeeds.Width + 32);

        }

        private void FrmCommandConsole_FormClosing(object sender, FormClosingEventArgs e) {
            ofdMain.Dispose();
            PositionWindow.Dispose();
            if (loadForm != null)
                loadForm.Dispose();
        }

        private void FrmCommandConsole_ResizeEnd(object sender, EventArgs e) {
        }

        private void ConfigOpenFileDialog() {
            ofdMain.Title = "Select an OpenSBP Part File";
            //ofdMain.Filter = "Part Files (*.sbp)|*.sbp|" +
            //                 "Default Config Files (*.sbd)|*.sbd|" +
            //                 "Custom Cut Files (*.sbc)|*.sbc|" +
            //                 "Rotary Files (*.sbr)|*.sbr|" +
            //                 "All OpenSBP Files (*.sb*)|*.sb*|" +
            //                 "G Code Files (*.gcode)|*.gcode|" +
            //                 "All Files (*.*)|*.*";
            ofdMain.FileName = FileFilter;


        }

        private void ConfigPositionWindow() {
            // this controls the location of the Position window that shows the 
            // current axis position information for the tool.
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            PositionWindow = new frmPosition();
            int x = resolution.Width - PositionWindow.Width - 20;
            int y = 30;
            PositionWindow.Location = new Point(x, y);
            PositionWindow.PermitMove = false; // should pin the window in place.
            PositionWindow.MyParent = this;
            PositionWindow.Show();

        }

        private void ResetForm() {
            txtCmdInput.Text = "";
            lstPreviousCmds.Items.Clear();
            lstProgram.Items.Clear();

            lblXYMove.Text = "0.00";
            lblZMove.Text = "0.00";
            lblXYJog.Text = "0.00";
            lblZJog.Text = "0.00";
        }

        public void MenuPartFileLoad_Click(object sender, EventArgs e) {
            ofdMain.FileName = "";  // TODO Should remember location of and name of last file opened.
            ofdMain.Filter = FileFilter;
            if (ofdMain.ShowDialog() == DialogResult.OK) {
                loadForm = new frmPartFileLoad(PositionWindow, isMono, ofdMain.FileName);
                loadForm.Show();
            }
        }

        public void OpenOutputWindow() {
            OutputWindow.Show();
        }
        public bool LoadSBProgram(string fileName) {
            string fileData = "";
            if (!File.Exists(fileName)) {
                return false;
            }
            fileData = File.ReadAllText(fileName);
            // we need to stuff this into the program display...
            StringBuilder strBuf = new StringBuilder();
            foreach( char ch in fileData) {
                if (ch != '\r' && ch != '\n')
                    strBuf.Append(ch);
                if (ch == '\n') {
                    lstProgram.Items.Add(strBuf.ToString());
                    strBuf.Clear();
                }
            }

            sbProg = new Interpreter(fileData);
            sbProg.StandAlone = false; // enables console output redirect to our OutputWindow
            sbProg.OutputWindow = OutputWindow.OutputWindow;
            // configure the redirect!
            Console.SetOut(new TextBoxWriter(OutputWindow.OutputWindow));

            return true;
        }

        public void RunSBProgram() {
            bool exit = false;
                sbProg.GetNextToken();
                while (!exit)
                    sbProg.Line();

        }
    }
}
