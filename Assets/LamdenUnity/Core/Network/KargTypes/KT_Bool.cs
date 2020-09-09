using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_Bool :KwargType
    {
        private bool value;
        public KT_Bool(bool s)
        {
            kwargType = KwargTypes.Bool;
            value = s;
        }

        public override string ToString()
        {
            return value.ToString().ToLower();
        }
    }
}
