using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LamdenUnity
{
    public class MyJson
    {
        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }     
    }
}
