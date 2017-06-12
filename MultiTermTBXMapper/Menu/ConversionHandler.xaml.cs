using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MultiTermTBXMapper.Menu
{
    /// <summary>
    /// Interaction logic for VariantHandler.xaml
    /// </summary>
    public partial class ConversionHandler : UserControl, ISwitchable
    {
        private Mapping fullMapping = new Mapping();
        private MappingDict mappingDict;
        private Dictionary<string, Dictionary<string, string>> tbxInfo = TBXDatabase.getDCInfo();

        public ConversionHandler(MappingDict mapping)
        {
            InitializeComponent();

            mappingDict = mapping;

            map();
        }

        private void map()
        {
            


        }

        private bool isConceptGrp(string dc)
        {
            List<string> grp = mappingDict.levelMap["conceptGrp"];
            return (Methods.inList(ref grp, dc)) ? true : false;
        }

        private bool isLanguageGrp(string dc)
        {
            List<string> grp = mappingDict.levelMap["languageGrp"];
            return (Methods.inList(ref grp, dc)) ? true : false;
        }

        private bool isTermGrp(string dc)
        {
            List<string> grp = mappingDict.levelMap["termGrp"];
            return (Methods.inList(ref grp, dc)) ? true : false;   
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
