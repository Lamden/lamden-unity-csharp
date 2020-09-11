using LamdenUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Values : MonoBehaviour
{
    public LamdenTest lamdenTest;

    public InputField inUid, inStr, inFloat, InInt;
    public Dropdown ddBool;

    public Text txtStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Submit()
    {
        txtStatus.text = "Sending...";
        TxInfo txInfo = new TxInfo()
        {
            sender = lamdenTest.GetWallet(),
            contractName = "con_values_testing",
            methodName = "test_values",
            stampLimit = 100,
            //kwargs = new Dictionary<string, KwargType>
            //    {
            //        {"UID", new KT_UID(inUid.text)},
            //        {"Str", new KT_String(inStr.text)},
            //        {"Float", new KT_Numerical(float.Parse(inFloat.text))},
            //        {"Int", new KT_Int(int.Parse(InInt.text))},
            //        {"Bool", new KT_Bool(ddBool.value == 0 ? true : false)},

            //        {"Dict", new KT_Dict(new Dictionary<string, KwargType>{ {"test", new KT_String("my test")}})},
            //        {"List", new KT_List(new List<KwargType>{ new KT_Numerical(1.2f), new KT_String("test2")})},
            //        {"ANY", new KT_Numerical(1.1f)},
            //        {"DateTime", new KT_DateTime(DateTime.Now)},
            //        {"TimeDelta", new KT_TimeDelta(4898)}
            //    }

            kwargs = new Dictionary<string, KwargType>
              {
                  {"UID", new KT_UID("testing2")},
                  {"Str", new KT_String("this is another string")},
                  {"Float", new KT_Numerical(1.1f)},
                  {"Int", new KT_Int(2)},
                  {"Bool", new KT_Bool(false)},
                  {"Dict", new KT_Dict(new Dictionary<string, KwargType>{ {"test", new KT_String("my test")}})},
                  {"List", new KT_List(new List<KwargType>{ new KT_Numerical(1.2f), new KT_String("test2")})},
                  {"ANY", new KT_Numerical(1.1f) },
                  {"DateTime", new KT_DateTime(DateTime.Now)},
                  {"TimeDelta", new KT_TimeDelta(4898)}
              }
        };

        Transaction tx = new Transaction(lamdenTest.masterNodeApi, txInfo, (Transaction.TransactionStatus txStatus, TxResponse txResponse) => {
            if (txStatus == Transaction.TransactionStatus.SubmittedProcessing)
                txtStatus.text = "Sumbitted and processing...";

            if (txStatus == Transaction.TransactionStatus.Completed)
                txtStatus.text = "Processed successfully!";

            if (txStatus == Transaction.TransactionStatus.Error)
                txtStatus.text = txResponse.error;
        });
    }
}
