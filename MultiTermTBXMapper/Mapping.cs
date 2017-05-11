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
        public object empty = new object();

        public Mapping()
        {
           
        }

        public string getTarget(string elt)
        {
            switch (elt)
            {
                case "<termNote>":
                    return "termNote";
                case "<termComp>":
                    return "termComp";
                case "<termCompList>":
                    return "termCompList";
                case "<adminNote>":
                case "<transac>":
                case "<transacNote>":
                case "<xref>":
                case "<ref>":
                case "<hi>":
                case "<bpt>":
                case "<date>":
                case "<ept>":
                case "<foreign>":
                case "<ph>":
                case "<term>":
                case "<descrip>":
                case "<descripNote>":
                case "<admin>":
                case "<note>":
                    return "auxInfo";
                default:
                    return "unhandled";
            }
        }

        public string getEltAtt(string elt, string type)
        {
            switch(elt)
            {
                case "<adminNote>":
                    return getXMLnoContent("adminNote", type);
                case "<transac>":
                    return getXMLnoContent("transac", type);
                case "<transacNote>":
                    return getXMLnoContent("transacNote", type);
                case "<xref>":
                    return "<xref type='" + type + "' >see target</xref>";
                case "<ref>":
                    return getXMLnoContent("ref", type);
                case "<hi>":
                    return getXMLnoContent("hi", type);
                case "<bpt>":
                    return "<bpt />";
                case "<date>":
                    return "<date />";
                case "<ept>":
                    return "<ept />";
                case "<foreign>":
                    return "<foregin />";
                case "<ph>":
                    return "<ph />";
                case "<term>":
                    return "<term />";
                case "<descrip>":
                    return getXMLnoContent("descrip", type);
                case "<descripNote>":
                    return getXMLnoContent("descripNote", type);
                case "<admin>":
                    return getXMLnoContent("admin", type);
                case "<termNote>":
                    return getXMLnoContent("termNote", type);
                case "<termCompList>":
                    return getXMLnoContent("termCompList", type);
                case "<note>":
                    return "<note />";
                default:
                    return getXMLnoContent("termNote", "unknownTermType");
            }
        }

        private string getXMLnoContent(string elt, string type)
        {
            return "<" + elt + " type='" + type + "' />";
        }
    }

    class CategoricalMapping : Dictionary<string, OneLevel>
    {
        public CategoricalMapping()
        {
            Add("concept", new OneLevel("concept"));
            Add("language", new OneLevel("language"));
            Add("term", new OneLevel("term"));
        }
    }

    class OneLevel : Dictionary<string, TemplateSet>
    {
        public OneLevel(string level) { }

        public void addConcept(string datcat, TemplateSet ts)
        {

        }

        private void addDefault(string level)
        {
            switch(level)
            {
                case "concept":
                    Add("Subject", addSubject());
                    Add("Status", addStatus());
                    Add("Source", addSource());
                    Add("Note", addNote());
                    break;
                case "language":
                    Add("Definition", addDefinition());
                    Add("Note", addNote());
                    break;
                case "term":
                    Add("Status", addTermStatus());
                    Add("Note", addNote());
                    Add("Context", addContext());
                    Add("Grammatical Gender", addGrammaticalGender());
                    Add("Grammatical Number", addGrammaticalNumber());
                    Add("Usage Regiser", addUsageRegister());
                    Add("Part of Speech", addPartOfSpeech());
                    Add("Source", addSource());
                    Add("Category", addCategory());
                    break;
            }
        }

        private string getAuxInfo()
        {
            return "auxInfo";
        }

        private string getTermNote()
        {
            return "termNote";
        }

        private string getAdmin()
        {
            return "admin";
        }

        private string getUnhandled()
        {
            return "unhandled";
        }

        private TemplateSet addSubject()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new DescripXML("subjectField"));
            return ts;
        }

        private TemplateSet addStatus()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new NoteXML(), "category tag");
            return ts;
        }

        private TemplateSet addSource()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new AdminXML("source"));
            return ts;
        }

        private TemplateSet addNote()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new NoteXML());
            return ts;
        }

        private TemplateSet addDefinition()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new DescripXML("definition"));
            return ts;
        }

        private TemplateSet addTermStatus()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getUnhandled(), new AdminXML("Status"));

            ValueGroup vg1 = new ValueGroup() { "new", "nonstandardized", "proposed", "recommended" };
            ValueGroup vg2 = new ValueGroup() { "admitted", "deprecated", "legal", "preferred", "regulated", "standardized", "superseded" };
            (ts[0] as KeyList).addValueGroup(vg1);
            (ts[0] as KeyList).addValueGroup(vg2);

            Teasp specialTeasp1 = new Teasp();
            specialTeasp1.setAll("termNote", new TermNoteXML("language-planningQualifier"), 
                new Dictionary<string, string>()
                {
                    { "nonstandardized", "nonstandardizedTerm" },
                    { "proposed", "proposedTerm" },
                    { "new", "newTerm" },
                    { "recommended", "recommendedTerm" }
                }
            );
            Teasp specialTeasp2 = new Teasp();
            specialTeasp2.setAll("termNote", new TermNoteXML("administrativeStatus"),
                new Dictionary<string, string>()
                {
                    { "deprecated", "deprecatedTerm-admn-sts" },
                    { "regulated", "regulatedTerm-admn-sts" },
                    { "admitted", "admittedTerm-admn-sts" },
                    { "standardized", "standardizedTerm-admin-sts" },
                    { "legal", "legalTerm-admn-sts" },
                    { "superseded", "superseded-admn-sts" },
                    { "preferred", "preferredTerm-admn-sts" }
                }
            );

            ts.addSpecialTeasp(specialTeasp1);
            ts.addSpecialTeasp(specialTeasp2);

            return ts;
        }

        private TemplateSet addContext()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getAuxInfo(), new DescripXML("context"));
            return ts;
        }

        private TemplateSet addGrammaticalGender()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getTermNote(), new TermNoteXML("grammaticalGender"), 
                new Dictionary<string, string>()
                {
                    { "other", "otherGender" }
                }
            );
            return ts;
        }

        private TemplateSet addGrammaticalNumber()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getTermNote(), new TermNoteXML("grammaticalNumber"), 
                new Dictionary<string, string>()
                {
                    { "other", "otherNumber" }
                }
            );
            return ts;
        }

        private TemplateSet addUsageRegister()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getTermNote(), new TermNoteXML("register"),
                new Dictionary<string, string>()
                {
                    { "slang", "slangRegister" },
                    { "in-house", "in-houseRegister" },
                    { "bench-level", "bench-levelRegister" },
                    { "vulgar", "vulgarRegister" },
                    { "technical", "technicalRegister" },
                    { "neutral", "neutralRegister" }
                }
                
            );
            return ts;
        }

        private TemplateSet addPartOfSpeech()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getTermNote(), new TermNoteXML("partOfSpeech"));
            return ts;
        }

        private TemplateSet addCategory()
        {
            TemplateSet ts = new TemplateSet();
            ((ts[0] as KeyList)[0] as Teasp).setAll(getUnhandled(), new TermNoteXML("unknownTermType"));

            ValueGroup vg1 = new ValueGroup() { "abbreviation","acronym","equation","formula","internationalism","symbol"};
            ValueGroup vg2 = new ValueGroup() { "common name", "full form", "international scientific term", "part number", "short form", "transcribed form", "transliterated form" };
            ValueGroup vg3 = new ValueGroup() { "phraseologism", "stock keeping unit", "orthographical variant" };
            ValueGroup vg4 = new ValueGroup() { "antonym" };
            (ts[0] as KeyList).addValueGroup(vg1);
            (ts[0] as KeyList).addValueGroup(vg2);
            (ts[0] as KeyList).addValueGroup(vg3);
            (ts[0] as KeyList).addValueGroup(vg4);

            TermNoteXML xml = new TermNoteXML("termType");

            Teasp specialTeasp1 = new Teasp();
            specialTeasp1.setAll(getTermNote(), xml);
            Teasp specialTeasp2 = new Teasp();
            specialTeasp2.setAll(getTermNote(), xml, "camel case");
            Teasp specialTeasp3 = new Teasp();
            specialTeasp3.setAll(getTermNote(), xml,
                new Dictionary<string, string>()
                {
                    { "stock keeping unit", "sku" },
                    { "phraseologism", "phraseologicalUnit" },
                    { "orthographical variant", "variant" }
                }
            );
            Teasp specialTeasp4 = new Teasp();
            specialTeasp4.setAll(getTermNote(), new TermNoteXML("antonymTerm"), placement: "null");

            ts.addSpecialTeasp(specialTeasp1);
            ts.addSpecialTeasp(specialTeasp2);
            ts.addSpecialTeasp(specialTeasp3);
            ts.addSpecialTeasp(specialTeasp4);

            return ts;
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

        public void setAll(string target, string xml, Dictionary<string,string> sub, string placement = "content")
        {
            setTarget(target);
            setEltAtt(xml);
            setSub(sub);
            setPlacement(placement);
        }

        public void setAll(string target, string xml, string sub = "null", string placement = "content")
        {
            setTarget(target);
            setEltAtt(xml);
            setSub(sub);
            setPlacement(placement);
        }

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

        public void setSub(string sub = "null")
        {
            teasp[2] = sub;
        }

        public void setPlacement(string placement)
        {
            teasp[3] = placement;
        }

        public void setUnhandled()
        {
            setAll("unhandled", new TermNoteXML("unknownTermType"));
        }
    }

    class ValueGroup : List<string>{}

    class QueueDrainOrders : List<object>{}

    class AdminXML
    {
        readonly string _value;

        public AdminXML(string value)
        {
            _value = "<admin type='"+value+"' />";
        }

        public static implicit operator string(AdminXML admin)
        {
            return admin._value;
        }
        public static implicit operator AdminXML(string admin)
        {
            return new AdminXML(admin);
        }
    }

    class DescripXML
    {
        readonly string _value;

        public DescripXML(string value)
        {
            _value = "<descrip type='" + value + "' />";
        }

        public static implicit operator string(DescripXML descrip)
        {
            return descrip._value;
        }
        public static implicit operator DescripXML(string descrip)
        {
            return new DescripXML(descrip);
        }
    }

    class TermNoteXML
    {
        readonly string _value;

        public TermNoteXML(string value)
        {
            _value = "<termNote type='" + value + "' />";
        }

        public static implicit operator string(TermNoteXML termNote)
        {
            return termNote._value;
        }
        public static implicit operator TermNoteXML(string termNote)
        {
            return new TermNoteXML(termNote);
        }
    }

    class NoteXML
    {
        readonly string _value;

        public NoteXML()
        {
            _value = "<note />";
        }

        public static implicit operator string(NoteXML note)
        {
            return note._value;
        }

        public static implicit operator NoteXML(string n = null)
        {
            return new NoteXML();
        }
    }

    class XrefXML
    {
        readonly string _value;

        public XrefXML(string value)
        {
            _value = "<xref type='"+value+"' >see target</xref>";
        }

        public static implicit operator string(XrefXML xref)
        {
            return xref._value;
        }

        public static implicit operator XrefXML(string xref)
        {
            return new XrefXML(xref);
        }
    }

    class RefXML
    {
        readonly string _value;

        public RefXML(string value)
        {
            _value = "<ref type='" + value + "' />";
        }

        public static implicit operator string(RefXML @ref)
        {
            return @ref._value;
        }

        public static implicit operator RefXML(string @ref)
        {
            return new RefXML(@ref);
        }
    }

    class TermCompListXML
    {
        readonly string _value;

        public TermCompListXML(string value)
        {
            _value = "<termCompList type='" + value + "' />";
        }

        public static implicit operator string(TermCompListXML xml)
        {
            return xml._value;
        }

        public static implicit operator TermCompListXML(string xml)
        {
            return new TermCompListXML(xml);
        }
    }
}
