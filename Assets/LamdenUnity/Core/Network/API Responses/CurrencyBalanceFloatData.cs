using System;
using UnityEngine;

namespace LamdenUnity
{
    [Serializable]
    public class CurrencyBalanceFloatData
    {
        public Value value;

        public CurrencyBalanceFloatData()
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
            CurrencyBalanceFloatData currencyBalance = JsonUtility.FromJson<CurrencyBalanceFloatData>(json);
            return float.Parse(currencyBalance.value.__fixed__);
        }
    }

    public class CurrencyBalanceIntData
    {
        public int value;

        public static float GetValue(string json)
        {
            CurrencyBalanceIntData currencyBalance = JsonUtility.FromJson<CurrencyBalanceIntData>(json);
            return currencyBalance.value;
        }
    }
}

