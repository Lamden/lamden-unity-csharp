using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LamdenUnity
{
    [Serializable]
    public class KT_TimeDelta : KwargType
    {        
        public int[] deltaTime;

        public KT_TimeDelta(long dateStamp)
        {
            kwargType = KwargTypes.DeltaTime;
            int days = (int)(dateStamp / 1000 / 60 / 60 / 24);
            int seconds = (int)((dateStamp - (days * 24 * 60 * 60 * 1000)) / 1000);
            deltaTime = new int[] { days, seconds };
        }

        public override string ToString()
        {
            return $"[{deltaTime[0]},{deltaTime[1]}]";
        }

    }
}
