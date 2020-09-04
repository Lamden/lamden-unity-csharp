using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class MasterNodeApi : Network
{
    const bool SUCCESS = true;
    const bool FAILED = false;

    int requestIDCounter = 0;

    public enum ApiCall { GetNonce, Ping, GetContractInfo, GetVariable, GetContractMethods, GetCurrencyBalance, SendTransaction };

    public NetworkInfo networkInfo;

    public string host { get{ return hosts[Random.Range(0, hosts.Length)];}}
       

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
                 callBack.Invoke(callCompleted, json);
             }));
    }

    public void GetVariable(string contractName, string variable, string key, Action<bool, string> callBack)
    {
        StartCoroutine(
           SendRequest(
            Method.GET,
             $"contracts/{contractName}/{variable}",
            new Dictionary<string, string> { { "key", key} },
            null,
             (string json, bool callCompleted) =>
             {                 
                     callBack.Invoke(callCompleted, json);                 
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
                         ContractMethods methods = JsonUtility.FromJson<ContractMethods>(json);
                         Dictionary<string, Methods> methodDic = new Dictionary<string, Methods>();
                         for (int i = 0; i < methods.methods.Length; i++)
                         {
                             methodDic.Add(methods.methods[i].name, methods.methods[i]);
                         }
                         callBack.Invoke(callCompleted, methodDic);
                     }
                     catch (Exception ex)
                     {
                         Debug.LogError($"GetCurrencyBalance: Failed json string: {json}, ex: {ex.Message}");
                         callBack.Invoke(false, null);
                     }
                 }
                 else
                     callBack.Invoke(callCompleted, null);
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
                         CurrencyBalance currencyBalance = JsonUtility.FromJson<CurrencyBalance>(json);
                         float balance = float.Parse(currencyBalance.value.__fixed__);
                         callBack.Invoke(callCompleted, balance);
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

 


    private IEnumerator SendRequest(Method method, string path, Dictionary<string, string> parms, string jsonData, Action<string, bool> callBack)
    {
        string uri = host;

        if (!string.IsNullOrEmpty(path))
            uri += path;

        if (parms != null)
        {
            uri += "?";
            foreach (var item in parms)
            {
                uri += $"{item.Key}={item.Value}";
            }
        }

        using (UnityWebRequest request = new UnityWebRequest(uri, method.ToString()))
        {
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            request.downloadHandler = dH;

            if (method == Method.POST)
            {
                request.SetRequestHeader("Content-Type", "application/json");
                byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
                UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
                upHandler.contentType = "application/json";
                request.uploadHandler = upHandler;
            }
            
            request.timeout = timeout;
            Debug.Log($"Sending web request to {uri} with method of {method}");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError($"Recieved ERROR response from {uri} of {request.error}");
                callBack?.Invoke(request.error, FAILED);               
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log($"Recieved response from {uri} of {json}");
                callBack?.Invoke(json, SUCCESS);
               
            }
        }
    }


}


