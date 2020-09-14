using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_Numeric :KwargType
    {
        private float value;
        public KT_Numeric(float f)
        {
            kwargType = KwargTypes.Float;
            value = f;
        }

        public override string ToString()
        {
            return $"{{\"__fixed__\":\"{value.ToString("0.0#########")}\"}}";
        }
    }
}
