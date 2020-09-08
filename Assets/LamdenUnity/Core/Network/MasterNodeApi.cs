using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static LamdenUnity.ContractMethodsData;
using Random = UnityEngine.Random;

namespace LamdenUnity
{
    public class MasterNodeApi : Network
    {
        int requestIDCounter = 0;

        public enum ApiCall { GetNonce, Ping, GetContractInfo, GetVariable, GetContractMethods, GetCurrencyBalance, SendTransaction };

        public NetworkInfo networkInfo;        


        private void Awake()
        {
            SetNetworkInfo(networkInfo);
        }

        public void GetContractInfo(string contractName, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
               Method.GET,
                "contracts/" + contractName,
                null,
                null,
                 (string json, bool callCompleted) =>
                 {
                     callBack?.Invoke(callCompleted, json);
                 }));
        }

        public void GetVariable(string contractName, string variable, string key, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
                Method.GET,
                 $"contracts/{contractName}/{variable}",
                new Dictionary<string, string> { { "key", key } },
                null,
                 (string json, bool callCompleted) =>
                 {
                     callBack?.Invoke(callCompleted, json);
                 }));

        }

        public void GetContractMethods(string contractName, Action<bool, Dictionary<string, Methods>> callBack)
        {
            StartCoroutine(
               SendRequest(
                Method.GET,
                $"contracts/{contractName}/methods",
                null,
                null,
                 (string json, bool callCompleted) =>
                 {
                     if (callCompleted)
                     {
                         try
                         {
                             ContractMethodsData methods = JsonUtility.FromJson<ContractMethodsData>(json);
                             Dictionary<string, Methods> methodDic = new Dictionary<string, Methods>();
                             for (int i = 0; i < methods.methods.Length; i++)
                             {
                                 methodDic.Add(methods.methods[i].name, methods.methods[i]);
                             }
                             callBack?.Invoke(callCompleted, methodDic);
                         }
                         catch (Exception ex)
                         {
                             Debug.LogError($"GetCurrencyBalance: Failed json string: {json}, ex: {ex.Message}");
                             callBack?.Invoke(false, null);
                         }
                     }
                     else
                     {
                         callBack?.Invoke(callCompleted, null);
                         Debug.LogError($"GetCurrencyBalance: Failed: {json}");
                     }
                 }));
        }

        public void PingServer(Action<bool, string> callBack)
        {
            StartCoroutine(
                SendRequest(
                    Method.GET,
                    "ping",
                    null,
                    null,
                    (string json, bool callCompleted) =>
                    {
                        if (callCompleted && json.Contains("online"))
                            callBack.Invoke(true, json);
                        else
                            callBack.Invoke(false, json);
                    }));
        }


        public void GetCurrencyBalance(string key, Action<bool, float> callBack)
        {
            StartCoroutine(
               SendRequest(
                Method.GET,
                 $"contracts/currency/balances",
                new Dictionary<string, string> { { "key", key } },
                null,
                 (string json, bool callCompleted) =>
                 {
                     if (callCompleted)
                     {
                         try
                         {
                             callBack.Invoke(callCompleted, CurrencyBalanceData.GetValue(json));
                         }
                         catch (Exception ex)
                         {
                             Debug.LogError($"GetCurrencyBalance: Failed json string: {json}, ex: {ex.Message}");
                             callBack.Invoke(false, 0);
                         }
                     }
                     else
                         callBack.Invoke(callCompleted, 0);

                 }));
        }

        public void SendTransaction(string jsonData, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
               Method.POST,
               null,
               null,
               jsonData,
               (string json, bool callCompleted) =>
               {
                   callBack.Invoke(callCompleted, json);
               }));
        }

        public void GetNonce(string sender, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
                Method.GET,
                "nonce/" + sender,
                null,
                null,
                  (string json, bool callCompleted) =>
                  {
                      callBack.Invoke(callCompleted, json);
                  }));
        }




       


    }
}


