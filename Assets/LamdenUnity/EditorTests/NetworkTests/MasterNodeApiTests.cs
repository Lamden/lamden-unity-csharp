using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LamdenUnity;
using System.Text.RegularExpressions;
using static LamdenUnity.ContractMethodsData;
using SimpleJSON;

namespace Tests
{
    public class MasterNodeApiTests
    {
        string vk = "4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9";
        bool calledBack;

        MasterNodeApi masterNodeApiGood;
        MasterNodeApi masterNodeApiBad;
        const string goodHost = "https://testnet-master-1.lamden.io/";
        const string badHost = "http://127.1.1.1:18080/";

        NetworkInfo goodNetwork = new NetworkInfo()
        {
            hosts = new string[] { goodHost },
            networkType = "testnet",
            currencySymbol = "TAU",
            lamden = true,
            blockExplorer = "https://explorer.lamden.io"
        };

        NetworkInfo badNetwork = new NetworkInfo()
        {
            hosts = new string[] { badHost },
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

        void SetupBad()
        {
            calledBack = false;
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MasterNodeApi>();
            masterNodeApiBad = gameObject.GetComponent<MasterNodeApi>();
            masterNodeApiBad.SetNetworkInfo(badNetwork);
        }

        [Test]
        public void TestSetup()
        {
            SetupGood();
            Assert.AreEqual(masterNodeApiGood.host, goodHost);
            var ex = Assert.Throws<Exception>(() => masterNodeApiGood.SetNetworkInfo(null));
            Assert.That(ex.Message, Is.EqualTo("networkInfo cannot be null"));
        }

        [UnityTest]
        public IEnumerator PingTest()
        {
            SetupGood();
            masterNodeApiGood.PingServer((bool success, string json) => {
                // Test that ping can reach testnet
                calledBack = true;               
                Assert.True(success);                                       
            });            
            while (!calledBack) { yield return null; }

            SetupBad();
            LogAssert.Expect(LogType.Warning, new Regex($".*{badHost}.*"));
            masterNodeApiBad.PingServer((bool success, string json) =>
            {
                // Test that ping failed
                calledBack = true;
                Assert.True(!success);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetCurrencyBalanceTest()
        {
            SetupGood();
            masterNodeApiGood.GetCurrencyBalance(vk, (bool callCompleted, float balance) =>
            {
                // TODO: Need to handle integeter balances
                Debug.Log($"GetCurrencyBalance results: {balance}");
                calledBack = true;                     
                Assert.True(callCompleted);
                Assert.True(balance >= 0);                
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetVariableTest()
        {
            SetupGood();
            Dictionary<string, string> keys = new Dictionary<string, string> { {"key", "testing:Str"} };
            masterNodeApiGood.GetVariable("con_values_testing", "S", keys, (bool callCompleted, string json) =>
            {
                Debug.Log($"GetVariableTest results: {json}");
                calledBack = true;                
                Assert.True(callCompleted);              
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetNonceTest()
        {
            SetupGood();
            masterNodeApiGood.GetNonce(vk, (bool callCompleted, string json, string uri) =>
            {
                Debug.Log($"GetNonceTest results: {json}");
                calledBack = true;
                Assert.True(callCompleted);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetContractMethodsTest()
        {
            SetupGood();
            masterNodeApiGood.GetContractMethods("currency", (bool callCompleted, Dictionary<string, Methods> methods) =>
            {
                Debug.Log($"GetContractMethodsTest results with number of keys: {methods.Keys.Count}");
                calledBack = true;
                Assert.True(callCompleted);
                if (callCompleted)
                {
                    Assert.IsNotNull(methods);
                    Assert.IsTrue(methods.ContainsKey("transfer"));
                }
                
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetContractInfoTest()
        {
            SetupGood();
            masterNodeApiGood.GetContractInfo("currency", (bool callCompleted, string json) =>
            {
                Debug.Log($"GetContractMethodsTest results: {json}");
                calledBack = true;
                Assert.True(callCompleted);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator CheckTransactionTest()
        {
            SetupGood();
            string hash = "440d856d3b21b76a40fc8cbc4a79e168225ec1688a470cdc43eb8329bf36ebb0";
            masterNodeApiGood.CheckTransaction(null, hash, (bool callCompleted, string json) =>
            {
                Debug.Log($"CheckTransactionTest results: {json}");
                CheckTransactionData transactionData = JsonUtility.FromJson<CheckTransactionData>(json);
                calledBack = true;
                var n = JSON.Parse(json);
                Assert.AreEqual(transactionData.status, 0);
                Assert.True(callCompleted);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetStampRatioTest()
        {
            SetupGood();
            masterNodeApiGood.GetStampRatio((bool callCompleted, int stampRatio) =>
            {
                calledBack = true;
                Assert.True(callCompleted);
                Assert.Greater(stampRatio, -1);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetMaxStamps()
        {
            SetupGood();
            masterNodeApiGood.GetMaxStamps(vk, (bool callCompleted, int maxStamps) =>
            {
                calledBack = true;
                Assert.True(callCompleted);
                Assert.Greater(maxStamps, -1);
            });
            while (!calledBack) { yield return null; }
        }
    }
}
