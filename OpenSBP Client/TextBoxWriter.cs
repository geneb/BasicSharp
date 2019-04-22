using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using System.Threading.Tasks;

namespace OpenSBP_Client {

    // This code was shamelessly snatched from
    // http://csharphelper.com/blog/2018/08/redirect-console-window-output-to-a-textbox-in-c/
    public class TextBoxWriter : TextWriter {
        // The control where we will write text.
        private TextBox MyControl;
        public TextBoxWriter(TextBox txtOutput) {
            MyControl = txtOutput;
        }

        public override void Write(char value) {
            //MyControl.Text += value;
            base.Write(value);
            MyControl.AppendText(value.ToString());
        }

        public override void Write(string value) {
            MyControl.Text += value;
        }

        public override Encoding Encoding {
            get { return Encoding.Unicode; }
        }
    }
}
