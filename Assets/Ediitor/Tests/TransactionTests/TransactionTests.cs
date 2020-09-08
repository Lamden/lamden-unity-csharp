using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LamdenUnity;

namespace Tests
{
    public class TransactionTests
    {

        string vk = "d41b8ed0d747ca6dfacdc58b78e1dba86cd9616359014eebd5f3443509111120";
        bool calledBack;

        MasterNodeApi masterNodeApiGood;
        const string goodHost = "http://167.172.126.5:18080/";




        NetworkInfo goodNetwork = new NetworkInfo()
        {
            hosts = new string[] { goodHost },
            networkType = "testnet",
            currencySymbol = "TAU",
            lamden = true,
            blockExplorer = "https://explorer.lamden.io"
        };

        void SetupGood()
        {
            calledBack = false;
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MasterNodeApi>();
            masterNodeApiGood = gameObject.GetComponent<MasterNodeApi>();
            masterNodeApiGood.SetNetworkInfo(goodNetwork);
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestTransaction()
        {
            SetupGood();
            Wallet wallet = new Wallet();
            wallet.Load("c8a3c5333aa3b058c4fa16d48db52355ab62ddc8daa9a183706a912e522440b6");
          
        
            TxInfo txInfo = new TxInfo()
            {
                sender = wallet,
                contractName = "currency",
                methodName = "transfer",
                stampLimit = 100,
                kwargs = new Dictionary<string, KwargType>
                {
                    {"to", new KT_String("4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9")},
                    {"amount", new KT_Int(1)}
                }
            };

            Transaction tx = new Transaction(masterNodeApiGood, txInfo, (bool result, TxResponse txResponse) => {
                Assert.IsTrue(result);
                Assert.IsTrue(txResponse.success.Equals("Transaction successfully submitted to the network."));
                Assert.IsTrue(!txResponse.hash.Equals(""));
                calledBack = true;
            });

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
            while (!calledBack) { yield return null; }
        }
    }
}
