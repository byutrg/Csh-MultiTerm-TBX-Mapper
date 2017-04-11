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
    /// Interaction logic for PickListHandler.xaml
    /// </summary>
    public partial class PickListHandler : UserControl, ISwitchable
    {
        private Dictionary<string, object[]> fullMapping = new Dictionary<string, object[]>();
        private List<string> dcs_with_picklists = new List<string>();

        private Dictionary<string, string> datcat_mapping;
        private Dictionary<string, List<string>> user_dc_values;
        private Dictionary<string, List<string[]>> tbx_picklists = TBXDatabase.getPicklists();

        private int dcIndex = 0;

        public PickListHandler(ref Dictionary<string, string> mapping, ref Dictionary<string, List<string>> user_dc_values)
        {
            InitializeComponent();

            datcat_mapping = mapping;
            this.user_dc_values = user_dc_values;

            fillFullMappingKeys();
            display();
        }

        private void display()
        {
            string dc = dcs_with_picklists[dcIndex];

            clearGrid();
            fillGrid(dc);
            editHead(dc);
        }

        private void editHead(string dc_key)
        {
            string headText = "Picklist values for " + dc_key;
            textblock_head.Text = headText;
        }

        private void clearGrid()
        {
            canvas_pls.Children.RemoveRange(0, canvas_pls.Children.Count);
            
        }

        private void fillGrid(string dc_key)
        {
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            canvas_pls.Children.Add(grid);

            Dictionary<string, string>.KeyCollection pl_mapKeys = (fullMapping[dc_key][1] as Dictionary<string, string>).Keys;

            string[] keys = Methods.getKeyArray(ref pl_mapKeys);

            foreach (string key in keys)
            {
                Dictionary<string, string> entry = (fullMapping[dc_key][1] as Dictionary<string, string>);

                createPicklistMapControl(dc_key, key, ref entry, ref grid);
            }
        }

        private void createPicklistMapControl(string dc_key, string pl_key, ref Dictionary<string, string> entry, ref Grid grid)
        {
            PickListMapControl plmc = new PickListMapControl();
            plmc.label_picklist_item.Content = pl_key;
            plmc.HorizontalAlignment = HorizontalAlignment.Center;
            plmc.Margin = new Thickness(50, 2, 50, 2);
            plmc.combo_tbx_picklist.SelectionChanged += Item_Selected;

            fillTBXComboBox(ref plmc.combo_tbx_picklist, fullMapping[dc_key][0].ToString());

            if (entry[pl_key] != null)
            {
                foreach (ComboBoxItem item in plmc.combo_tbx_picklist.Items)
                {
                    if (entry[pl_key] == item.Content.ToString())
                    {
                        plmc.combo_tbx_picklist.SelectedItem = item;
                        break;
                    }
                }
            }


            grid.Children.Add(plmc);

            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(30);
            grid.RowDefinitions.Add(rd);

            plmc.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
        }

        private void fillTBXComboBox(ref ComboBox box, string tbx_dc)
        {
            for(int i = 0; i < tbx_picklists[tbx_dc].Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = tbx_picklists[tbx_dc][i][0];

                box.Items.Add(item);
            }
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            PickListMapControl plmc = (((sender as ComboBox).Parent as Grid).Parent as PickListMapControl);

            setMapping(plmc.label_picklist_item.Content.ToString(), ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString());
        }

        private void setMapping(string user_pl, string tbx_pl)
        {
            string dc = dcs_with_picklists[dcIndex];

            (fullMapping[dc][1] as Dictionary<string, string>)[user_pl] = tbx_pl;

            checkCompletion();
        }

        private void checkCompletion()
        {
            bool complete = true;
            foreach (string dc in dcs_with_picklists)
            {
                foreach(string key in (fullMapping[dc][1] as Dictionary<string, string>).Keys)
                {
                    if ((fullMapping[dc][1] as Dictionary<string, string>)[key] == null)
                    {
                        complete = false;
                        break;
                    }
                }
            }
            if (complete)
            {
                submit.IsEnabled = true;
            }
        }

        private void fillFullMappingKeys()
        {
            foreach(string key in datcat_mapping.Keys)
            {
                if (hasTBXPicklist(key) || datcat_mapping[key] == "VARIES")
                {
                    if (datcat_mapping[key] != "VARIES")
                    {
                        fullMapping.Add(key, new object[2] { datcat_mapping[key], new Dictionary<string, string>() });

                        for (int i = 0; i < user_dc_values[key].Count; i++)
                        {
                            (fullMapping[key][1] as Dictionary<string, string>).Add(user_dc_values[key][i], null);
                        }


                        dcs_with_picklists.Add(key);
                    }
                    else
                    {
                        fullMapping.Add(key, new object[3] { null, new string[user_dc_values[key].Count], new Dictionary<string,object[]>() });
                        
                        for (int i = 0; i < user_dc_values[key].Count; i++)
                        {
                            (fullMapping[key][1] as string[])[i] = user_dc_values[key][i];
                        }
                    }
                }
                else
                {

                    fullMapping.Add(key, new object[1] { datcat_mapping[key] });
                }
            }
        }

        private void findUserPicklists(string key)
        {
            foreach(string ukey in user_dc_values.Keys)
            {
                if (fullMapping[ukey].Length == 2)
                {
                    for (int i = 0; i < user_dc_values[ukey].Count; i++)
                    {
                        (fullMapping[ukey][1] as Dictionary<string, string>).Add(user_dc_values[ukey][i], null);
                    }
                }
            }
        }

        private bool hasTBXPicklist(string key)
        {
            if (tbx_picklists.Keys.Contains(datcat_mapping[key]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Methods.incrementIndex(ref dcIndex, dcs_with_picklists.Count);
            display();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Methods.decrementIndex(ref dcIndex);
            display();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new VariantHandler(fullMapping));
        }
    }
}
