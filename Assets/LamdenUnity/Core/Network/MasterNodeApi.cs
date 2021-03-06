﻿using System;
using System.Collections;
using System.Collections.Generic;
using unity.libsodium;
using UnityEngine;
using UnityEngine.Networking;
using static LamdenUnity.ContractMethodsData;
using Random = UnityEngine.Random;

namespace LamdenUnity
{
    public class MasterNodeApi : Network
    {
        public enum ApiCall { GetNonce, Ping, GetContractInfo, GetVariable, GetContractMethods, GetCurrencyBalance, SendTransaction };

        public NetworkInfo networkInfo;    

        private void Awake()
        {
            NativeLibsodium.sodium_init();
            SetNetworkInfo(networkInfo);
        }

        public void GetContractInfo(string contractName, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
                   null,
                   Method.GET,
                    "contracts/" + contractName,
                    null,
                    null,
                     (string json, bool callCompleted) =>
                     {
                         callBack?.Invoke(callCompleted, json);
                     }));
        }

        public void GetVariable(string contractName, string variable, Dictionary<string, string> keys, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
                   null,
                    Method.GET,
                     $"contracts/{contractName}/{variable}",
                    keys,
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
                   null,
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
            PingServer(null, callBack);
        }

        public void PingServer(string uri, Action<bool, string> callBack)
        {
            StartCoroutine(
                SendRequest(
                    uri,
                    Method.GET,
                    "ping",
                    null,
                    null,
                    (string json, bool callCompleted) =>
                    {
                        if (callCompleted && json.Contains("online"))
                            callBack?.Invoke(true, json);
                        else
                            callBack?.Invoke(false, json);
                    }));
        }

        public void GetCurrencyBalance(string key, Action<bool, float> callBack)
        {
            StartCoroutine(
               SendRequest(
                   null,
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
                                 if (json.Contains("_fixed_"))
                                     callBack?.Invoke(true, CurrencyBalanceFloatData.GetValue(json));
                                 else
                                     callBack?.Invoke(true, CurrencyBalanceIntData.GetValue(json));
                             }
                             catch (Exception ex)
                             {
                                 Debug.LogError($"GetCurrencyBalance: Failed json string: {json}, ex: {ex.Message}");
                                 callBack?.Invoke(false, -1);
                             }
                         }
                         else
                             callBack?.Invoke(false, -1); ;

                     }));
        }

        public void SendTransaction(string uri, string jsonData, Action<bool, string> callBack)
        {
            StartCoroutine(
               SendRequest(
                   uri,
                   Method.POST,
                   null,
                   null,
                   jsonData,
                   (string json, bool callCompleted) =>
                   {
                       callBack?.Invoke(callCompleted, json);
                   }));
        }

        public void GetNonce(string sender, Action<bool, string, string> callBack)
        {
            string uri = host;
            StartCoroutine(
               SendRequest(
                   uri,
                Method.GET,
                "nonce/" + sender,
                null,
                null,
                  (string json, bool callCompleted) =>
                  {
                      callBack?.Invoke(callCompleted, json, uri);
                  }));
        }

        public void CheckTransaction(string uri, string hash, Action<bool, string> callBack)
        {
            StartCoroutine(
              SendRequest(
                  uri,
                   Method.GET,
                   "tx",
                   new Dictionary<string, string> {
                        { "hash", hash }
                   },
                   null,
                     (string json, bool callCompleted) =>
                     {
                         if(callCompleted)
                            callBack?.Invoke(SUCCESS, json);
                         else
                             callBack?.Invoke(FAILED, json);
                     }));
        }

        public void GetStampRatio(Action<bool, int> callBack)
        {
            StartCoroutine(
                     SendRequest(
                         null,
                         Method.GET,
                         "contracts/stamp_cost/S?key=value",
                         null,
                         null,
                         (string json, bool callCompleted) =>
                         {
                             if (callCompleted)
                             {
                                 try
                                 {
                                     StampRatioResponse stampRatioResponse = JsonUtility.FromJson<StampRatioResponse>(json);
                                     Debug.Log($"GetStampRatio Response:{stampRatioResponse.value}");
                                     callBack?.Invoke(SUCCESS, stampRatioResponse.value);
                                     return;
                                 }
                                 catch (Exception)
                                 {
                                     Debug.LogError($"GetStampRatio: unable to deserlize json response: {json}");
                                 }
                             }

                             callBack?.Invoke(false, -1);

                         }));
        }

        public void GetMaxStamps(string key, Action<bool, int> callBack)
        {         
          
            GetCurrencyBalance(key, (bool getCurSuccessful, float currency) =>
            {
                if(getCurSuccessful)
                { 
                    GetStampRatio((bool getStampRatioSuccesful, int stampRatio) => {
                        if (getStampRatioSuccesful)
                        {
                            int maxStamps = (int)(stampRatio * currency);
                            Debug.Log($"GetMaxStamps Response:{maxStamps}");
                            callBack?.Invoke(SUCCESS, maxStamps);
                        }
                        else
                        {
                            Debug.LogError($"GetMaxStamps: Unable to get stamp ratio.");
                            callBack?.Invoke(FAILED, -1);
                        }
                    });
                }   
                else
                {
                    callBack?.Invoke(FAILED, -1);
                    Debug.LogError($"GetMaxStamps: Unable to get currency.");
                    return;
                }
            });
        }
    }
}


