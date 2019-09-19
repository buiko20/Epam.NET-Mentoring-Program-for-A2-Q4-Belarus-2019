using System.Windows.Forms;

namespace Task2.Forms
{
    public partial class FormViewer : Form
    {
        public FormViewer()
        {
            InitializeComponent();
        }

        public void SetHtml(string html)
        {
            rtbText.Text = html;
        }
    }
}
