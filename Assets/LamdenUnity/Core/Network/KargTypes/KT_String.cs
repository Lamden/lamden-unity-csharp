using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_String :KwargType
    {
        private string value;
        public KT_String(string s)
        {
            kwargType = KwargTypes.Str;
            value = s;
        }

        public override string ToString()
        {
            return $"\"{value}\"";
        }
    }
}
