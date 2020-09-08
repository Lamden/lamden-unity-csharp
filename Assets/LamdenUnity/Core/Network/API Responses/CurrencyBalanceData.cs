using System;
using UnityEditor.Graphs;
using UnityEngine;

namespace LamdenUnity
{
    [Serializable]
    public class CurrencyBalanceData : MyJson
    {
        public Value value;

        public CurrencyBalanceData()
        {
            value = new Value();
        }

        [Serializable]
        public class Value
        {
            public string __fixed__;
        }

        public static float GetValue(string json)
        {
            CurrencyBalanceData currencyBalance = JsonUtility.FromJson<CurrencyBalanceData>(json);
            return float.Parse(currencyBalance.value.__fixed__);
        }
    }
}

