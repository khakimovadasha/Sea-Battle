using System.Windows.Forms;

namespace МорскойБой
{
    public partial class Victrory : Form
    {
        public Victrory(string str)
        {
            InitializeComponent();
            label1.Text = str;
        }

        private void VictroryGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
