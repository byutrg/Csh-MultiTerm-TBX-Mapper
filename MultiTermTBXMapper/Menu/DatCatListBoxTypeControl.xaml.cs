using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiTermTBXMapper.Menu
{
    /// <summary>
    /// Interaction logic for DatCatListBoxTypeControl.xaml
    /// </summary>
    public partial class DatCatListBoxTypeControl : UserControl
    {
        public Action<string[]> ListBoxItems;

        public DatCatListBoxTypeControl()
        {
            InitializeComponent();
        }

        public void clear()
        {
            Methods.removeListBoxItems(ref lb_tbx_dcs);
        }

        public void fillItems(string[] tbx_dcs)
        {
            if (tbx_dcs != null)
            {
                for (int i = 0; i < tbx_dcs.Length; i++)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = tbx_dcs[i];

                    lb_tbx_dcs.Items.Add(item);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TBXDatCatWindow tbxWindow = new TBXDatCatWindow();

            tbxWindow.Show();
            tbxWindow.dcs_tbx.Items.RemoveAt(0);
            tbxWindow.dcs_tbx.Items.RemoveAt(0);

            tbxWindow.selected += value =>
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = value;

                lb_tbx_dcs.Items.Add(item);

                updateItems();
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lb_tbx_dcs.SelectedItem != null)
            {
                lb_tbx_dcs.Items.Remove(lb_tbx_dcs.SelectedItem);

                updateItems();
            }
        }

        private void updateItems()
        {
            string[] items = new string[lb_tbx_dcs.Items.Count];
            for (int i = 0; i < lb_tbx_dcs.Items.Count; i++)
            {
                items[i] = (lb_tbx_dcs.Items[i] as ListBoxItem).Content.ToString();
            }

            ListBoxItems(items);
        }

    }
}
