using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenSBP
{
    class BuiltIns
    {
        public static void InstallAll(Interpreter interpreter)
        {
            // TODO add OpenSBP math functions here.
            // Any new function that's added here must also have its keyword added to the
            // function list at the top of Lexer.cs!
            interpreter.AddFunction("str", Str);
            interpreter.AddFunction("num", Num);
            interpreter.AddFunction("abs", Abs);
            interpreter.AddFunction("min", Min);
            interpreter.AddFunction("max", Max);
            interpreter.AddFunction("not", Not);
            interpreter.AddFunction("msgbox", MsgBox);

        }

        public static Value Str(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return args[0].Convert(ValueType.String);
        }

        public static Value Num(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return args[0].Convert(ValueType.Real);
        }

        public static Value Abs(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(Math.Abs(args[0].Real));
        }

        public static Value Min(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 2)
                throw new ArgumentException();

            return new Value(Math.Min(args[0].Real, args[1].Real));
        }

        public static Value Max(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(Math.Max(args[0].Real, args[1].Real));
        }

        public static Value Not(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(args[0].Real == 0 ? 1 : 0);
        }

        public static Value MsgBox(Interpreter interpreter, List<Value> args) {
            // format is MSGBOX(body text, button type, title text)
            if (args.Count < 3)
                throw new ArgumentException();
            MessageBoxButtons buttons = new MessageBoxButtons();
            MessageBoxIcon icons = new MessageBoxIcon();
            MessageBoxDefaultButton defButton = new MessageBoxDefaultButton();
            DialogResult result;

            // we have to work out the dialog box options we've been given.
            // args[1] is 

            int features = (int)args[1].Real;
            //if (args[1].Real > 0 && features == 0)
            //    features = (int)args[1].Real;

            if ((features & 0) == 0)
                buttons = MessageBoxButtons.OK;
            if ((features & 1) == 1)
                buttons = MessageBoxButtons.OKCancel;
            if ((features & 2) == 2)
                buttons = MessageBoxButtons.AbortRetryIgnore;
            if ((features & 3) == 3)
                buttons = MessageBoxButtons.YesNoCancel;
            if ((features & 4) == 4)
                buttons = MessageBoxButtons.YesNo;
            if ((features & 5) == 5)
                buttons = MessageBoxButtons.RetryCancel;

            if ((features & 16) == 16)
                icons = MessageBoxIcon.Stop;
            if ((features & 32) == 32)
                icons = MessageBoxIcon.Question;
            if ((features & 48) == 48)
                icons = MessageBoxIcon.Exclamation;
            if ((features & 64) == 64)
                icons = MessageBoxIcon.Information;

            if ((features & 256) == 256)
                defButton = MessageBoxDefaultButton.Button1;
            if ((features & 512) == 512)
                defButton = MessageBoxDefaultButton.Button2;
            if ((features & 768) == 768)
                defButton = MessageBoxDefaultButton.Button3;

            result = MessageBox.Show(null, args[0].String, args[2].String, buttons, icons, defButton);

            switch (result) {
                case DialogResult.OK:
                    return new Value("Ok");
                case DialogResult.Cancel:
                    return new Value("Cancel");
                case DialogResult.Abort:
                    return new Value("Abort");
                case DialogResult.Retry:
                    return new Value("Retry");
                case DialogResult.Ignore:
                    return new Value("Ignore");
                case DialogResult.Yes:
                    return new Value("Yes");
                case DialogResult.No:
                    return new Value("No");
                default:
                    return new Value("Invalid");
            }
        }
    }
}
