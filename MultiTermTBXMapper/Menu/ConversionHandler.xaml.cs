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
            foreach (string dc in mappingDict.Keys)
            {
                if(isConceptGrp(dc))
                {
                    //Handle all simple cases: no multiple tbx datcats, no picklists
                    if (mappingDict.getTBXMappingList(dc).Count < 2 || (!mappingDict.hasPicklist(dc) && !mappingDict.hasSplitContents(dc)))
                    {
                        string tbx_dc = getTBXdc(dc);

                        string elt = tbxInfo[tbx_dc]?["element"];

                        string target = fullMapping.getTarget(elt);
                        string eltAtt = fullMapping.getEltAtt(elt, dc);

                        TemplateSet ts = new TemplateSet();
                        ((ts[0] as KeyList)[0] as Teasp).setAll(target, eltAtt);


                        fullMapping.catMap["concept"].Add(dc, ts);
                    }
                    //Handle single tbx datcat with picklists
                    else if (mappingDict.hasPicklist(dc) && (mappingDict.getTBXMappingList(dc).Count < 2 || !mappingDict.hasSplitContents(dc)))
                    {
                        string tbx_dc = getTBXdc(dc);

                        string elt = tbxInfo[tbx_dc]?["element"];

                        string target = fullMapping.getTarget(elt);
                        string eltAtt = fullMapping.getEltAtt(elt, dc);


                    }
                }
            }


        }

        //private Dictionary<string,string> getSubstitution(string dc)
        //{
        //    Dictionary<string, string> sub = new Dictionary<string, string>();

        //    if(mappingDict.isTBXPicklistMapped)
        //    {
        //        sub = mappingDict.getPicklistMap(dc);
        //    }
        //    else
        //    {

        //    }
        //}

        private string getTBXdc(string dc)
        {
            string tbx_dc = null;

            foreach (string key in mappingDict.getTBXContentMap(dc)?.Keys)
            {
                tbx_dc = key;
                break;
            }

            if (tbx_dc == null)
            {
                tbx_dc = mappingDict.getTBXMappingList(dc)?[0];
            }

            return tbx_dc;
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
