using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_List :KwargType
    {
        private List<KwargType> values;
        public KT_List(List<KwargType> v)
        {
            kwargType = KwargTypes.List;
            values = v;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirst = true;
            foreach (var item in values)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(",");

                sb.Append(item.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
