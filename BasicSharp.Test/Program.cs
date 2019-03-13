using System;
using System.IO;
using System.Windows.Forms;
namespace BasicSharp.Test {
    class Program {
        [STAThread]  // needed for the file open dialog.
        static void Main(string[] args) {
            OpenFileDialog fDialog = new OpenFileDialog();
            string fileName = "";
            if (fDialog.ShowDialog() == DialogResult.OK) {
                fileName = fDialog.FileName;
                Interpreter basic = new Interpreter(File.ReadAllText(fileName));
                try {
                    Console.WriteLine("BasicSharp Intepreter Start.");
                    Console.WriteLine("----------------------------\n");
                    basic.Exec();
                    Console.WriteLine("No errors during run.");
                } catch (Exception e) {
                    Console.WriteLine("Runtime Error detected - aborting.");
                    Console.WriteLine("Exception was " + e.Message);
                }
            }
          
            Console.WriteLine("Run complete, press [ENTER] to exit.");
            Console.Read();
        }
    }
}
