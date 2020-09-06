using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LamdenUnity
{
    [System.Serializable]
    public class NetworkInfo
    {
        public string[] hosts;
        public string networkType;
        public string currencySymbol;
        public bool lamden;
        public string blockExplorer;
    }
}
