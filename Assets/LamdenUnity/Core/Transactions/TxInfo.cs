using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LamdenUnity
{
    [System.Serializable]
    public class TxInfo
    {
        public string uid;             // [Optional] <string> unique ID for tracking purposes
        public Wallet sender;          // <hex string> public key of the transaction sender,   
        public string contractName;    // name of lamden smart contract
        public string methodName;      // name of method to call in contractName
        public Dictionary<string, KwargType> kwargs = new Dictionary<string, KwargType>();
                                                           // key/values of args to pass to methodName
                                                           //        example: kwargs.to = "270add00fc708791c97aeb5255107c770434bd2ab71c2e103fbee75e202aa15e"
                                                           //        kwargs.amount = 1000
        public int stampLimit;         // the max amount of stamps the tx should use.  tx could use less. if tx needs more the tx will fail.
        public int nonce;              // send() will attempt to retrieve this info automatically
        public string processor;       // send() will attempt to retrieve this info automatically
        
        public string getKwargString()
        {            
            var sortedKeys = kwargs.OrderBy(key => key.Key);
            var sortedKwargs = sortedKeys.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (var item in sortedKwargs)
            {
                if(!isFirst)
                    sb.Append(",");
                else
                    isFirst = false;

                sb.Append("\"").Append(item.Key).Append("\":").Append(item.Value.ToString()).Append("");
            }
            return sb.ToString();
        }
    }
}

