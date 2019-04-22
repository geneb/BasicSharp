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
    public partial class frmOutputWindow : Form {

        public System.Windows.Forms.TextBox OutputWindow;

        public frmOutputWindow() {
            InitializeComponent();
            OutputWindow = txtOutput;
        }

        private void FrmOutputWindow_Load(object sender, EventArgs e) {
            //OutputWindow = txtOutput;
        }

        private void FrmOutputWindow_TextChanged(object sender, EventArgs e) {
            if (!this.Visible)
                this.Show();
        }
    }
}
