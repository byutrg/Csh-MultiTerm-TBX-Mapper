using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MultiTermTBXMapper.Menu
{
    /// <summary>
    /// Interaction logic for DatCatHandler.xaml
    /// </summary>
    public partial class DatCatHandler : UserControl, ISwitchable
    {
        public List<string> datcats = new List<string>();
        public Dictionary<string, string> mapping = new Dictionary<string, string>();
        public Dictionary<string, List<string>> user_dc_values = new Dictionary<string, List<string>>();

        protected string filename;

        private List<DatCatMapControl> mapControls = new List<DatCatMapControl>();
        private int index = 0;


        public DatCatHandler(string filename)
        {
            InitializeComponent();

            this.filename = filename;

            loadDatCats();
            display();

            mapControl.convert += value => {
                Switcher.Switch(new PickListHandler(ref mapping, ref user_dc_values));
            };
        }

        private void loadDatCats()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            XmlReader reader = XmlReader.Create(filename, settings);


            bool start = false;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && (reader.Name == "body" || reader.Name == "mtf"))
                    start = true;
                if (start == true)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes && null != reader.GetAttribute("type"))
                    {
                        string dc = reader.GetAttribute("type");
                        if (!Methods.inList(ref datcats, dc))
                        {
                            datcats.Add(dc);
                            mapping[dc] = null;
                        }

                        //Pull out text for use later with picklists
                        XmlReader textReader = reader.ReadSubtree();

                        while (textReader.Read())
                        {
                            if (textReader.NodeType == XmlNodeType.Text)
                            {
                                if (!user_dc_values.ContainsKey(dc))
                                {
                                    user_dc_values[dc] = new List<string>();
                                }

                                List<string> values = user_dc_values[dc];
                                if (!Methods.inList(ref values, reader.Value))
                                {
                                    user_dc_values[dc].Add(textReader.Value);
                                }
                            }
                        }
                    }

                }
            }

            datcats.Sort();
            textTotal.Text = datcats.Count().ToString();
        }

        private void display()
        {
            if (mapping[datcats[index]] != null)
            {
                mapControl.setContent(datcats[index], mapping[datcats[index]]);
            }
            else
            {
                mapControl.setContent(datcats[index]);
            }

            mapControl.mapping += value => setMapping(value);

            updateCounter();
        }

        private void checkCompletion()
        {
            bool mapped = true;
            foreach (string key in mapping.Keys)
            {
                if (mapping[key] == null)
                {
                    mapped = false;
                }
            }

            if (mapped)
            {
                mapControl.button_convert.IsEnabled = true;
            }

        }

        private void updatePercentage()
        {
            double mapped = 0;

            foreach (string key in mapping.Keys)
            {
                if (mapping[key] != null)
                {
                    mapped++;
                }
            }

            textPercent.Text = Math.Round(mapped * 100 / (double)mapping.Keys.Count(),2, MidpointRounding.AwayFromZero).ToString() + "%";
        }

        private void updateCounter()
        {
            textIndex.Text = (index+1).ToString();
        }

        private void setMapping(string datcat_tbx)
        {
            mapping[datcats[index]] = datcat_tbx;
            updatePercentage();
            checkCompletion();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Methods.incrementIndex(ref index, datcats.Count);
            display();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Methods.decrementIndex(ref index);
            display();
        }

    }
}
