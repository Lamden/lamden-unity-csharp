using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_UID :KwargType
    {
        private string value;
        public KT_UID(string s)
        {
            kwargType = KwargTypes.UID;
            value = s;
        }

        public override string ToString()
        {
            return $"\"{value}\"";
        }
    }
}
