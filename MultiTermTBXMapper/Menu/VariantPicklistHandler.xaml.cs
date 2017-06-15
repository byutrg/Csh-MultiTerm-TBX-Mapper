using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MultiTermTBXMapper.Menu
{
    /// <summary>
    /// Interaction logic for VariantPicklistHandler.xaml
    /// </summary>
    public partial class VariantPicklistHandler : UserControl
    {
        private List<string> datcats = new List<string>();

        private int index = 0;

        public VariantPicklistHandler(List<string> datcats)
        {
            InitializeComponent();

            Globals.stage = 2;
            this.datcats = datcats;
            Globals.locals = new List<object> { datcats };

            vpmc.map += value =>
            {
                string user_dc = datcats[index];
                string user_pl = value[0];
                string tbx_dc = value[1];

                Globals.mappingDict.setTBXContentMap(user_dc, user_pl, tbx_dc);

                checkCompletion();    
            };

            vpmc.next += value =>
            {
                if (value)
                {
                    nextPage();
                }
            };

            display();
        }

        private void checkCompletion()
        {
            if(Globals.mappingDict.isGroupMappedToTBX(ref datcats))
            {
                vpmc.btn_submit.IsEnabled = true;
            }
        }


        private void display()
        {
            if (datcats.Count > 0)
            { 
                textblock_user_dc.Text = datcats[index];
                vpmc.clear();
                vpmc.fillListBoxes(Globals.mappingDict.getTBXMappingList(datcats[index]), Globals.mappingDict.getContentList(datcats[index]) as List<string>);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Methods.decrementIndex(ref index);
            display();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Methods.incrementIndex(ref index, datcats.Count);
            display();
        }

        private void nextPage()
        {
            Switcher.Switch(new PickListHandler());
        }
    }
}
