using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MasterNodeApiTests
    {
        string vk = "d41b8ed0d747ca6dfacdc58b78e1dba86cd9616359014eebd5f3443509111120";
        bool calledBack;

        MasterNodeApi masterNodeApiGood;
        MasterNodeApi masterNodeApiBad;
        const string goodHost = "http://167.172.126.5:18080/";
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
            Assert.True(masterNodeApiGood.host.Equals(goodHost));
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
            masterNodeApiBad.PingServer((bool success, string json) => {
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
                Debug.Log($"GetCurrencyBalance results: {balance}");
                calledBack = true;                     
                Assert.True(callCompleted);
                
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetVariableTest()
        {
            SetupGood();
            masterNodeApiGood.GetVariable("currency", "balances", vk, (bool callCompleted, string json) =>
            {
                Debug.Log($"GetCurrencyBalance results: {json}");
                calledBack = true;                
                Assert.True(callCompleted);              
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetNonceTest()
        {
            SetupGood();
            masterNodeApiGood.GetNonce(vk, (bool callCompleted, string json) =>
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
    }
}
