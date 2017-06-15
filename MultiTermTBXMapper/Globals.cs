using MultiTermTBXMapper.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTermTBXMapper
{
    static class Globals
    {
        public static string filename;
        public static MappingDict mappingDict = new MappingDict();
        public static int? stage = 0;
        public static List<object> locals = null;

        public static void clearGlobals()
        {
            filename = "";
            mappingDict = new MappingDict();
            stage = 0;
        }

        public static void clearLocals()
        {
            locals = null;
        }

        public static void saveGlobals()
        {
            object[] storage = new object[0];

            switch(stage)
            {
                case 0:
                case 3:
                case 4:
                    storage = new object[3] { filename, mappingDict, stage };
                    break;
                case 1:
                case 2:
                    storage = new object[4] { filename, mappingDict, stage, locals };
                    break;

            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".mt2tbx";
            dlg.Filter = "MultiTerm2TBX Files (*.mt2tbx)|*.mt2tbx";

            bool? result = dlg.ShowDialog();
            
            if (result == true)
            {
                using (StreamWriter file = File.CreateText(dlg.FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, storage);
                }
            }
            else
            {
                MessageBox.Show("Could not save.  Try again.");
            }
        }

        public static void loadGlobals()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mt2tbx";
            dlg.Filter = "MultiTerm2TBX Files (*.mt2tbx)|*.mt2tbx";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                using (StreamReader file = File.OpenText(dlg.FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    object[] storage = (object[])serializer.Deserialize(file, typeof(object[]));

                    clearGlobals();

                    filename = storage[0] as string;
                    mappingDict = JsonConvert.DeserializeObject<MappingDict>(storage[1].ToString());
                    stage = JsonConvert.DeserializeObject<int?>(storage[2].ToString());
                    
                    switch(stage)
                    {
                        case 1:
                        case 2:
                            locals = JsonConvert.DeserializeObject<List<object>>(storage[3].ToString());
                            break;
                    }
                }

                if (stage != null)
                {
                    switch (stage)
                    {
                        case 0:
                            Switcher.Switch(new Welcome());
                            break;
                        case 1:
                            Switcher.Switch(new DatCatHandler(true));
                            break;
                        case 2:
                            Switcher.Switch(new VariantPicklistHandler(locals[0] as List<string>));
                            break;
                        case 3:
                            Switcher.Switch(new PickListHandler());
                            break;
                        case 4:
                            Switcher.Switch(new ConversionHandler());
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Could not open file. Please choose another.");
            }
        }
    }
}
