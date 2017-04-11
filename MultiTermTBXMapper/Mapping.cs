using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTermTBXMapper
{
    class Mapping
    {
        public string dialect = "TBX-Default";
        public string xcs = "TBXXCSV02.xcs";
        public CategoricalMapping catMap = new CategoricalMapping();

        public Mapping()
        {
           
        }

    }

    class CategoricalMapping : Dictionary<string, OneLevel>
    {
        public CategoricalMapping()
        {
            Add("concept", new OneLevel());
            Add("language", new OneLevel());
            Add("term", new OneLevel());
        }
    }

    class OneLevel : Dictionary<string, TemplateSet>
    {
        public OneLevel()
        {
        }
    }

    class TemplateSet : List<object>
    {
        public TemplateSet()
        {
            Add(new KeyList());
        }

        public void addSpecialTeasp(Teasp teasp)
        {
            Add(teasp);
        }
    }

    class KeyList : List<object>
    {
        public KeyList()
        {
            Add(new Teasp());
        }

        public void addValueGroup(ValueGroup group)
        {
            Add(group);
        }
    }

    class Teasp
    {
        public object[] teasp = new object[4];

        public void setTarget(string target)
        {
            teasp[0] = "@" + target;
        }

        public void setEltAtt(string xml)
        {
            teasp[1] = xml;
        }

        public void setSub(Dictionary<string,string> sub)
        {
            if (sub != null)
            {
                teasp[2] = sub;
            }
            else
            {
                teasp[2] = "null";
            }
        }

        public void setPlacement(string placement)
        {
            teasp[3] = placement;
        }

        public void setUnhandled()
        {
            setTarget("unhandled");
            setEltAtt("<termNote type='unkownTermType' />");
            setSub(null);
            setPlacement("content");
        }
    }

    class ValueGroup : List<string>
    {
        public ValueGroup()
        {

        }
    }
}
