using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTermTBXMapper
{
    public class MappingDict : Dictionary<string, object[]>
    {
        public Dictionary<string, List<string>> levelMap = new Dictionary<string, List<string>>() { { "conceptGrp", new List<string>() },{ "languageGrp", new List<string>() },{ "termGrp", new List<string>() } };

        public void Add(string key)
        {
            Add(key, new object[4] { new TBXMappingList(), new UserDatCatContentList(), new TBXContentMap(), new PicklistMap() });
        }

        public bool hasPicklist(string key)
        {
            return (getPicklistMap(key)?.Keys.Count > 0) ? true : false;
        }

        public bool hasSplitContents(string key)
        {
            return (getTBXContentMap(key)?.Keys.Count > 1) ? true : false;
        }

        public PicklistMap setPicklistMap(string key, string content_key, string value)
        {
            return getPicklistMap(key)?.Set(content_key, value);
        }

        public PicklistMap getPicklistMap(string key)
        {
            return ContainsKey(key) ? this[key][3] as PicklistMap : null;
        }

        public string getPicklistMapValue(string key, string user_pl)
        {
            return ContainsKey(key) ? (this[key][3] as PicklistMap).Get(user_pl) : null;
        }

        public bool isTBXMapped()
        {
            foreach (string key in Keys)
            {
                if (!isKeyMappedToTBX(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool isTBXPicklistMapped()
        {
            foreach (string key in Keys)
            {
                if (!isKeyMappedToTBXPicklist(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool isKeyMappedToTBX(string key)
        {
            foreach (string k in getTBXContentMap(key).Keys)
            {
                if (!getTBXContentMap(key).isMappedToTBX(k))
                {
                    return false;
                }
            }

            return true;
        }

        public bool isKeyMappedToTBXPicklist(string key)
        {
            foreach (string k in getPicklistMap(key).Keys)
            {
                if (!getPicklistMap(key).isMappedToTBXPicklist(k))
                {
                    return false;
                }
            }

            return true;
        }

        public bool isGroupMappedToTBX(ref List<string> user_dcs)
        {
            foreach (string dc in user_dcs)
            {
                if(!isKeyMappedToTBX(dc))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isGroupMappedToTBXPicklist(ref List<string> user_dcs)
        {
            foreach (string dc in user_dcs)
            {
                if (!isKeyMappedToTBXPicklist(dc))
                {
                    return false;
                }
            }

            return true;
        }

        public List<string> getDCsWithPicklists()
        {
            List<string> dcs = new List<string>();
            TBXDatabase.getDCsWithPicklists().ForEach(delegate (string tbx_dc) {
                
                foreach(string user_dc in Keys)
                {
                    List<string> tml = getTBXMappingList(user_dc);
                    if (Methods.inList(ref tml, tbx_dc))
                    {
                        dcs.Add(user_dc);
                    }
                }
            });

            return dcs;
        }


        public List<string> getDCsWithContent()
        {
            List<string> dcs = new List<string>();

            foreach (string key in Keys)
            {
                if(getContentList(key)?.Count > 0)
                {
                    dcs.Add(key);
                }
            }

            return dcs;
        }

        public TBXMappingList getTBXMappingList(string key)
        {
            return (Count > 0) ? this[key][0] as TBXMappingList : null;
        }

        public MappingDict setTBXMappingList(string key, TBXMappingList value = null)
        {
            if (ContainsKey(key))
            {
                this[key][0] = (value != null) ? value : new TBXMappingList();
            }
            else
            {
                Add(key);
            }
            return this;
        }
        
        public MappingDict setContentList(string key, UserDatCatContentList value)
        {
            if (ContainsKey(key))
            {
                this[key][1] = value;
            }
            else
            {
                Add(key);
            }

            return this;
        }

        public UserDatCatContentList getContentList(string key)
        {
            return ContainsKey(key) ? this[key][1] as UserDatCatContentList : null;
        }

        public MappingDict addContentMap(string dc_key, string content_key, string tbx_dc = null)
        {
            if (ContainsKey(dc_key))
            {
                if (getTBXContentMap(dc_key).ContainsKey(content_key))
                {
                    getTBXContentMap(dc_key)?.Add(content_key, tbx_dc);
                }
            }

            return this;
        }

        public MappingDict setTBXContentMap(string dc_key, string content_key, string tbx_dc = null)
        {
            if (ContainsKey(dc_key))
            {
                if (getTBXContentMap(dc_key).ContainsKey(content_key))
                {
                    getTBXContentMap(dc_key)?.Set(content_key, tbx_dc);
                }
                else
                {
                    getTBXContentMap(dc_key)?.Add(content_key, tbx_dc);
                }
            }

            return this;
        }

        public TBXContentMap getTBXContentMap(string key)
        {
            return ContainsKey(key) ? this[key][2] as TBXContentMap: null;
        }
    }

    public class TBXMappingList : List<string> {}

    public class UserDatCatContentList : List<string> {}

    public class TBXContentMap : Dictionary<string, string>
    {
        public bool isMappedToTBX(string key)
        {
            if (ContainsKey(key) && this[key] != null)
            {
                return true;
            }

            return false;
        }

        public TBXContentMap Set(string key, string tbx_dc)
        {
            if (ContainsKey(key))
            {
                this[key] = tbx_dc;
            }
            else
            {
                Add(key, tbx_dc);
            }

            return this;
        }

        public string Get(string key)
        {
            return ContainsKey(key) ? this[key] : null;
        }
    }

    public class PicklistMap : Dictionary<string, string>
    {
        public bool isMappedToTBXPicklist(string key)
        {
            if (ContainsKey(key) && this[key] != null)
            {
                return true;
            }

            return false;
        }

        public PicklistMap Set(string key, string tbx_dc)
        {
            if (ContainsKey(key))
            {
                this[key] = tbx_dc;
            }
            else
            {
                Add(key, tbx_dc);
            }

            return this;
        }

        public string Get(string key)
        {
            return ContainsKey(key) ? this[key] : null;
        }
    }
}
