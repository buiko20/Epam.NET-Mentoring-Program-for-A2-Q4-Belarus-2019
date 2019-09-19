using System;
using System.Windows.Forms;

namespace Task2.Forms
{
    public partial class FormMain : Form
    {
        private readonly Manager _manager = new Manager();
        private FormViewer _viewer = new FormViewer();

        public FormMain()
        {
            InitializeComponent();

            _viewer.Show();
            BringToFront();
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            var lvi = new ListViewItem { Text = tbUrl.Text };
            var subItem = new ListViewItem.ListViewSubItem(lvi, @"Downloading");
            lvi.SubItems.Add(subItem);
            lvUrls.Items.Add(lvi);

            var data = await _manager.LoadAsync(tbUrl.Text);
            lvi.Tag = data;
            subItem.Text = @"Ready";
        }

        private void LvUrls_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvUrls.SelectedItems.Count != 1)
                return;

            if (_viewer.IsDisposed)
                _viewer = new FormViewer();

            var lvi = lvUrls.SelectedItems[0];
            var data = (SiteData)lvi.Tag;

            _viewer.SetHtml(data?.Html);
            _viewer.Show();
            _viewer.BringToFront();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lvUrls.SelectedItems.Count != 1)
                return;

            var lvi = lvUrls.SelectedItems[0];
            var data = (SiteData)lvi.Tag;
            if (data != null)
                _manager.Cancel(data.Guid);

            lvUrls.Items.Remove(lvi);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _viewer.Dispose();
            _manager.Dispose();
        }
    }
}
