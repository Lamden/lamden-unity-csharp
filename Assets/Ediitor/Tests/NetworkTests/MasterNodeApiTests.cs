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
            masterNodeApiGood.PingServer((int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted) => {
                // Test that ping can reach testnet
                calledBack = true;
                Debug.Log($"Request:{requestID} of API {apiCall} {(callCompleted ? "successful" : "failed")}: {json}");
                Assert.True(callCompleted);
                Assert.True(apiCall == MasterNodeApi.ApiCall.Ping);
                Assert.True(json.Contains("{\"status\":\"online\"}"));                
            });            
            while (!calledBack) { yield return null; }

            SetupBad();            
            masterNodeApiBad.PingServer((int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted) => {
                // Test that ping failed
                calledBack = true;
                Debug.Log($"Request:{requestID} of API {apiCall} {(callCompleted ? "successful" : "failed")}: {json}");
                Assert.True(!callCompleted);
                Assert.True(apiCall == MasterNodeApi.ApiCall.Ping);           
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetCurrencyBalanceTest()
        {
            SetupGood();
            masterNodeApiGood.GetCurrencyBalance(vk, (int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted) =>
            {
                calledBack = true;
                Debug.Log($"Request:{requestID} of API {apiCall} {(callCompleted ? "successful" : "failed")}: {json}");            
                Assert.True(callCompleted);
                Assert.True(apiCall == MasterNodeApi.ApiCall.GetCurrencyBalance);
            });
            while (!calledBack) { yield return null; }
        }

        [UnityTest]
        public IEnumerator GetVariableTest()
        {
            SetupGood();
            masterNodeApiGood.GetVariable("currency", "balances", vk, (int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted) =>
            {
                calledBack = true;
                Debug.Log($"Request:{requestID} of API {apiCall} {(callCompleted ? "successful" : "failed")}: {json}");
                Assert.True(callCompleted);
                Assert.True(apiCall == MasterNodeApi.ApiCall.GetVariable);
            });
            while (!calledBack) { yield return null; }
        }       
    }
}
