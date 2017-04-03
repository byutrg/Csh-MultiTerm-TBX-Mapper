using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTermTBXMapper
{
    public class Methods
    {
        public static bool inList(ref List<string> a, string e)
        {
            if (a != null && a.Count > 1)
            {
                foreach (string elt in a)
                {
                    if (e == elt)
                        return true;
                }
            }
            return false;
        }

        public static bool inList(ref List<int> a, int e)
        {
            if (a != null && a.Count > 1)
            {
                foreach (int elt in a)
                {
                    if (e == elt)
                        return true;
                }
            }
            return false;
        }
    }
}
