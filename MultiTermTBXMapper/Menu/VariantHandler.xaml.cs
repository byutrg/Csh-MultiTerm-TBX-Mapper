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
    /// Interaction logic for VariantHandler.xaml
    /// </summary>
    public partial class VariantHandler : UserControl, ISwitchable
    {
        private Dictionary<string, object[]> fullMapping;
        private List<string> variants =  new List<string>();

        private int index = 0;
        

        public VariantHandler(Dictionary<string,object[]> mapping)
        {
            InitializeComponent();

            fullMapping = mapping;

            fillVariants();
            display();

            dclbtc.ListBoxItems += items =>
            {
                string key = textblock_user_dc.Text;

                fullMapping[key][0] = items;

                if (checkCompletion())
                {
                    button_submit.IsEnabled = true;
                }
            };
        }

        private void fillVariants()
        {
            foreach(string key in fullMapping.Keys)
            {
                if (fullMapping[key][0] == null)
                {
                    variants.Add(key);
                }
            }
        }

        private void display()
        {
            textblock_user_dc.Text = variants[index];
            dclbtc.clear();
            dclbtc.fillItems((string[])fullMapping[variants[index]][0]);
        }

        private bool checkCompletion()
        {
            foreach (string key in fullMapping.Keys)
            {
                if (fullMapping[key][0] == null)
                {
                    return false;
                }
            }

            return true;
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Methods.decrementIndex(ref index);
            display();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Methods.incrementIndex(ref index, variants.Count);
            display();
        }

        private void button_submit_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new VariantPicklistHandler(fullMapping, variants));
        }
    }
}
