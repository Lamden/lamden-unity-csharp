using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    // TODO: Remove as KT_Numerical has replaced all number types
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
