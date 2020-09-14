using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LamdenUnity
{
    [Serializable]
    class NonceData
    {
        public int nonce;
        public string processor;
        public string sender;

        public static NonceData FromJson(string json)
        {
            return JsonUtility.FromJson<NonceData>(json);
        }

    }
}
