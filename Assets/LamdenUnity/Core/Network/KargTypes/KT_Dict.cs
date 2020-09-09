using LamdenUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    public class KT_Dict :KwargType
    {
        private Dictionary<string, KwargType> values;
        public KT_Dict(Dictionary<string, KwargType> v)
        {
            kwargType = KwargTypes.Dict;
            values = v;
        }

        public override string ToString()
        {
            var sortedKeys = values.OrderBy(key => key.Key);
            var sortedValues = sortedKeys.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool isFirst = true;
            foreach (var item in sortedValues)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(",");

                sb.Append("\"").Append(item.Key).Append("\"").Append(":").Append(item.Value.ToString());
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
