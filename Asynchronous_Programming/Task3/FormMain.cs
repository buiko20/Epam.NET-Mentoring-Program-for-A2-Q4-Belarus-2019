using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task3
{
    public partial class FormMain : Form
    {
        private readonly IStorage _storage = new Storage();
        private readonly ObservableCollection<Product> _basket = new ObservableCollection<Product>();

        public FormMain()
        {
            InitializeComponent();

            _basket.CollectionChanged += BasketOnCollectionChanged;
        }

        private async void BasketOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await Task.Delay(200);
            decimal totalPrice = _basket.Sum(product => product.Price);
            lblPrice.Text = $@"Стоимость: {totalPrice:C0}";
        }

        private async void FormMain_Load(object sender, System.EventArgs e)
        {
            var products = await _storage.GetProductsAsync();
            foreach (Product product in products)
            {
                AddProduct(product, lvProducts);
            }
        }

        private void BtnAdd_Click(object sender, System.EventArgs e)
        {
            (Product product, ListViewItem _) = GetSelectedProduct(lvProducts);
            if (product != null)
            {
                _basket.Add(product);
                AddProduct(product, lvBasket);
            }
        }

        private void BtnRemove_Click(object sender, System.EventArgs e)
        {
            (Product product, ListViewItem lvi) = GetSelectedProduct(lvBasket);
            if (product != null)
            {
                lvBasket.Items.Remove(lvi);
                _basket.Remove(product);
            }
        }

        private static void AddProduct(Product product, ListView lv)
        {
            var lvi = new ListViewItem { Text = product.Name };
            var subItem = new ListViewItem.ListViewSubItem(lvi, $"{product.Price:C0}");
            lvi.SubItems.Add(subItem);
            lvi.Tag = product;
            lv.Items.Add(lvi);
        }

        private static (Product product, ListViewItem lvi) GetSelectedProduct(ListView lv)
        {
            if (lv.SelectedItems.Count != 1)
            {
                return (null, null);
            }

            var lvi = lv.SelectedItems[0];
            return (lvi.Tag as Product, lvi);
        }
    }
}
