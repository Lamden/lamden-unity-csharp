using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionBuilder
{
    public TransactionBuilder(
        NetworkInfo networkInfo,
        txInfo txInfo)
    {
       // NOTE: The same host must be used throughout the transaction, specifically the tx needs to be sent to the same
       // host that the nonce was received from
    }

}

public class txInfo
{
    string uid;             // [Optional] <string> unique ID for tracking purposes
    string sendVk;          // <hex string> public key of the transaction sender,   
    string contractName;    // name of lamden smart contract
    string methodName;      // name of method to call in contractName
    object kwargs;          // key/values of args to pass to methodName
                            //        example: kwargs.to = "270add00fc708791c97aeb5255107c770434bd2ab71c2e103fbee75e202aa15e"
                            //        kwargs.amount = 1000
    int stampLimit;         // the max amount of stamps the tx should use.  tx could use less. if tx needs more the tx will fail.
    int nonce;              // send() will attempt to retrieve this info automatically
    string processor;       // send() will attempt to retrieve this info automatically
}


