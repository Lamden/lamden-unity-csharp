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

        MasterNodeApi masterNodeApi;        

        void SetupTest()
        {
            calledBack = false;
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MasterNodeApi>();
            masterNodeApi = gameObject.GetComponent<MasterNodeApi>();
            masterNodeApi.SetNetworkInfo(new NetworkInfo()
            {
                hosts = new string[] { "http://167.172.126.5:18080/" },
                networkType = "testnet",
                currencySymbol = "TAU",
                lamden = true,
                blockExplorer = "https://explorer.lamden.io"
            });
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PingTest()
        {
            SetupTest();
            masterNodeApi.PingServer(PingCallBack);            
            while (!calledBack) { yield return null; }
        }

        void PingCallBack(int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted)
        {
            calledBack = true;
            Debug.Log($"Request:{requestID} of API {apiCall.ToString()} {(callCompleted ? "successful" : "failed")}: {json}");
            Assert.True(callCompleted);
            Assert.True(apiCall == MasterNodeApi.ApiCall.Ping);
            Assert.True(json.Contains("{\"status\":\"online\"}"));
        }

        [UnityTest]
        public IEnumerator GetVariableTest()
        {
            SetupTest();
            masterNodeApi.GetVariable("currency", "balances", vk, GetVariableCallBack);
            while (!calledBack) { yield return null; }
        }

        void GetVariableCallBack(int requestID, MasterNodeApi.ApiCall apiCall, string json, bool callCompleted)
        {
            calledBack = true;
            Debug.Log($"Request:{requestID} of API {apiCall.ToString()} {(callCompleted ? "successful" : "failed")}: {json}");
            Assert.True(callCompleted);
            Assert.True(apiCall == MasterNodeApi.ApiCall.GetVariable);           
        }

    }
}
