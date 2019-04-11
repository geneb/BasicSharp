using System.Drawing;
using System.Windows.Forms;

namespace OpenSBP_Client.Controls {
    public partial class Transparent_Text_Box : TextBox {
        public Transparent_Text_Box() {
            InitializeComponent();
            // thank $deity for StackOverflow!
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }
    }
}
