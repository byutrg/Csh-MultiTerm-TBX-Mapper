using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Diagnostics;

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

            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(fullMapping, settings);
            json = fixJSON(json);

            string mappingFile = Path.GetTempFileName();
            
            File.WriteAllText(mappingFile, json);

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ".\\Perl\\mt2tbx.exe",
                    Arguments = string.Format("\"{0}\" \"{1}\"", mappingFile.Replace(" ", "%%%%"), Globals.filename.Replace(" ", "%%%%")),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            string tbxOutput = "";
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                tbxOutput += proc.StandardOutput.ReadLine();
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".tbx";
            dlg.Filter = "TBX Files (*.tbx)|*.tbx";

            bool? result = dlg.ShowDialog();

            if (result == true)
            { 
                if (File.Exists(dlg.FileName))
                {
                    File.Delete(dlg.FileName);
                }

                File.Move(tbxOutput.Replace("/", "\\"), dlg.FileName);
            }

            textblock_conversionStatus.Text = "Your termbase has been converted and saved as " + dlg.FileName;
        }

        private void map()
        {
            foreach (string dc in mappingDict.Keys)
            {
                if (isConceptGrp(dc))
                {
                    handleConceptGrp(dc);
                }

                if (isLanguageGrp(dc))
                {
                    handleLanguageGrp(dc);
                }

                if (isTermGrp(dc))
                {
                    handleTermGrp(dc);
                }
            }


        }

        private string fixJSON(string json)
        { 
            json = Methods.rgxReplace("^\\{", "[", json);
            json = Methods.rgxReplace("\\}(?!.+?)", "]", json);
            json = Methods.rgxReplace("\\\"(dialect|xcs|catMap|empty)\\\":", "", json);
            json = Methods.rgxReplace("\\{\\\"teasp\\\":", "", json);
            json = Methods.rgxReplace("\\]\\}\\]\\]", "]]]", json);
            return json;
        }

        

        private void handleConceptGrp(string dc)
        {
            //Handle all simple cases: no multiple tbx datcats, no picklists
            if (mappingDict.getTBXMappingList(dc).Count < 2 || (!mappingDict.hasPicklist(dc) && !mappingDict.hasSplitContents(dc)))
            {
                TemplateSet ts = createTemplateSet(dc);
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

        private void handleLanguageGrp(string dc)
        {

            //Handle all simple cases: no multiple tbx datcats, no picklists
            if (mappingDict.getTBXMappingList(dc).Count < 2 || (!mappingDict.hasPicklist(dc) && !mappingDict.hasSplitContents(dc)))
            {
                TemplateSet ts = createTemplateSet(dc);


                fullMapping.catMap["language"].Add(dc, ts);
            }
        }

        private void handleTermGrp(string dc)
        {
            //Handle all simple cases: no multiple tbx datcats, no picklists
            if (mappingDict.getTBXMappingList(dc).Count < 2 || (!mappingDict.hasPicklist(dc) && !mappingDict.hasSplitContents(dc)))
            {
                TemplateSet ts = createTemplateSet(dc);


                fullMapping.catMap["term"].Add(dc, ts);
            }
        }

        private string getTBXdc(string dc)
        {
            string tbx_dc = null;

            foreach (string val in mappingDict.getTBXContentMap(dc)?.Values)
            {
                tbx_dc = val;
                break;
            }
            if (tbx_dc == null)
            {
                tbx_dc = mappingDict.getTBXMappingList(dc)?[0];
            }

            return tbx_dc;
        }

        private TemplateSet createTemplateSet(string user_dc)
        {
            string tbx_dc = getTBXdc(user_dc);

            string elt = tbxInfo[tbx_dc]?["element"];

            string target = fullMapping.getTarget(elt);
            string eltAtt = fullMapping.getEltAtt(elt, tbx_dc);

            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(target, eltAtt);

            return ts;
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
