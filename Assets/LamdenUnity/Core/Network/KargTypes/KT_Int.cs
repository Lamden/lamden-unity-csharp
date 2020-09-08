using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_Int :KwargType
    {
        private int value;
        public KT_Int(int i)
        {
            kwargType = KwargTypes.Int;
            value = i;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
