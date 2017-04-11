using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MultiTermTBXMapper.Menu
{
    /// <summary>
    /// Interaction logic for VariantPicklistMapControl.xaml
    /// </summary>
    public partial class VariantPicklistMapControl : UserControl
    {
        public Action<string[]> map;
        private Dictionary<string, int> mapping = new Dictionary<string,int>();

        public VariantPicklistMapControl()
        {
            InitializeComponent();
        }

        public void clear()
        {
            Methods.removeListBoxItems(ref lb_tbx_dcs);
            Methods.removeListBoxItems(ref lb_user_picklists);
        }

        public void fillListBoxes(string[] tbx_dcs, string[] values)
        {
            for (int i = 0; i < tbx_dcs.Length; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = tbx_dcs[i];

                item.Selected += Item_Selected;

                lb_tbx_dcs.Items.Add(item);
            }

            for (int i = 0; i < values.Length; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = values[i];

                item.Selected += Item_Selected1;

                lb_user_picklists.Items.Add(item);
            }

            lb_user_picklists.SelectedIndex = 0;
        }

        private void Item_Selected1(object sender, RoutedEventArgs e)
        {
            lb_tbx_dcs.SelectedItem = null;

            if (lb_user_picklists.SelectedItem != null)
            {
                string picklist_value = (lb_user_picklists.SelectedItem as ListBoxItem).Content.ToString();
                if (mapping.ContainsKey(picklist_value))
                {
                    lb_tbx_dcs.SelectedIndex = mapping[picklist_value];
                }
            }

        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            string picklist_value = (lb_user_picklists.SelectedItem as ListBoxItem).Content.ToString();
            string tbx_dc = (sender as ListBoxItem).Content.ToString();
            int tbx_dc_index = lb_tbx_dcs.SelectedIndex;


            string[] map_string = new string[2] { picklist_value, tbx_dc };
            map(map_string);

            if (mapping.ContainsKey(picklist_value))
            {
                mapping[picklist_value] = tbx_dc_index;
            }
            else
            {
                mapping.Add(picklist_value, tbx_dc_index);
            }
    }
    }
}
