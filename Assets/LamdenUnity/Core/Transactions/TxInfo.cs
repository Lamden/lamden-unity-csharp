using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LamdenUnity
{
    [System.Serializable]
    public class TxInfo
    {
        public string uid;             // [Optional] <string> unique ID for tracking purposes
        public string senderVk;          // <hex string> public key of the transaction sender,   
        public string contractName;    // name of lamden smart contract
        public string methodName;      // name of method to call in contractName
        public Dictionary<string, object> kwargs;          // key/values of args to pass to methodName
                                                           //        example: kwargs.to = "270add00fc708791c97aeb5255107c770434bd2ab71c2e103fbee75e202aa15e"
                                                           //        kwargs.amount = 1000
        public int stampLimit;         // the max amount of stamps the tx should use.  tx could use less. if tx needs more the tx will fail.
        public int nonce;              // send() will attempt to retrieve this info automatically
        public string processor;       // send() will attempt to retrieve this info automatically
    }
}

