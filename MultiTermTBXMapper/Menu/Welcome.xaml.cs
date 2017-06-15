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
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : UserControl, ISwitchable
    {
        public Welcome()
        {
            InitializeComponent();
        }

        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".tbx";
            dlg.Filter = "XML Files (*.xml)|*.xml";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Globals.filename = dlg.FileName;
                Switcher.Switch(new DatCatHandler());
            }
        }
    }
}
