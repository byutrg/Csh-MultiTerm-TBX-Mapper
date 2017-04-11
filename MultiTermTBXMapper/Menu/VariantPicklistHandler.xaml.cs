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
    /// Interaction logic for VariantPicklistHandler.xaml
    /// </summary>
    public partial class VariantPicklistHandler : UserControl
    {
        private Dictionary<string, object[]> fullMapping;
        private List<string> variants;
        private int index = 0;

        public VariantPicklistHandler(Dictionary<string, object[]> mapping, List<string> variants)
        {
            InitializeComponent();

            fullMapping = mapping;
            this.variants = variants;

            display();

            vpmc.map += value =>
            {

            };
        }

        private void display()
        {
            textblock_user_dc.Text = variants[index];

            vpmc.clear();
            vpmc.fillListBoxes(fullMapping[variants[index]][0] as string[], fullMapping[variants[index]][1] as string[]);
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
    }
}
