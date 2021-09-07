using System;
using System.Text;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class StringUtils
    {
        internal static String ReplaceAt(String orig, int index, char c)
        {
            StringBuilder sb = new StringBuilder(orig);
            sb[index] = c;
            return sb.ToString();
        }
    }
}
