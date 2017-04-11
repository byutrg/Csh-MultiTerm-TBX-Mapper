using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MultiTermTBXMapper
{
    public class Methods
    {
        public static void removeListBoxItems(ref ListBox lb)
        {
            if (lb.Items.Count > 0)
            {
                lb.Items.RemoveAt(lb.Items.Count - 1);
                removeListBoxItems(ref lb);
            }
        }

        public static void decrementIndex(ref int index)
        {
            if (index > 0)
            {
                index--;
            }
        }

        public static void incrementIndex(ref int index, int max)
        {
            if (index + 1 < max)
            {
                index++;
            }
        }

        public static string[] getKeyArray(ref Dictionary<string,string>.KeyCollection keyCol)
        {
            string[] keys = new string[keyCol.Count];
            int i = 0;
            foreach(string key in keyCol)
            {
                keys[i] = key;
                i++;
            }

            return keys;
        }

        public static bool inList(ref List<string> a, string e)
        {
            if (a != null && a.Count > 0)
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
            if (a != null && a.Count > 0)
            {
                foreach (int elt in a)
                {
                    if (e == elt)
                        return true;
                }
            }
            return false;
        }

        public static bool inArray(ref string[] a, string e)
        {
            if (a != null && a.Length > 0)
            {
                foreach (string elt in a)
                {
                    if (e == elt)
                        return true;
                }
            }
            return false;
        }

        public static bool inArray(ref int[] a, int e)
        {
            if (a != null && a.Length > 0)
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
