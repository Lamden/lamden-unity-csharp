using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace LamdenUnity
{ 
    public class KwargType
    {
        public enum KwargTypes { UID, Str, Float, Int, Bool, Dict, List, Any, DateTime, DeltaTime };
        public static KwargTypes kwargType;
        public string kwargName;

        static Dictionary<KwargTypes, Type> kwargToType = new Dictionary<KwargTypes, Type>
        {
            { KwargTypes.UID, typeof(string) },
            { KwargTypes.Str, typeof(string) },
            { KwargTypes.Float, typeof(float) },
            { KwargTypes.Int, typeof(int) },
            { KwargTypes.Bool, typeof(bool) },
            { KwargTypes.Dict, typeof(object) },
            { KwargTypes.List, typeof(object) },
            { KwargTypes.DateTime, typeof(KT_DateTime) },
            { KwargTypes.DeltaTime, typeof(string) },

        };

        private void LogError(KwargTypes type, object value)
        {
            Debug.LogError($"Error encoding {value} to {type}");
        }
    }
}
