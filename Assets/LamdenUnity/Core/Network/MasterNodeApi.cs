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

    public string url { get { return host; }}
      

    private void Awake()
    {
        SetNetworkInfo(networkInfo);    
    }

    public int GetContractInfo(string contractName, Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
            ApiCall.GetContractInfo,
            Method.GET,
            "contracts/" + contractName,
            null,
            null,
            callBack);
    }

    public int GetVariable(string contractName, string variable, string key, Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
            ApiCall.GetVariable,
            Method.GET,
             $"contracts/{contractName}/{variable}",
            new Dictionary<string, string> { { "key", key} },
            null,
            callBack);

    }

    public int GetContractMethods(string contractName, Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
            ApiCall.GetContractMethods,
            Method.GET,
            $"contracts/{contractName}/methods",
            null,
            null,
            callBack);
    }

    public int PingServer(Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
            ApiCall.Ping,
            Method.GET,
            "ping",
            null,
            null,
            callBack);
    }

    public int GetCurrencyBalance(string key, Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
            ApiCall.GetCurrencyBalance,
            Method.GET,
             $"contracts/currency/balances",
            new Dictionary<string, string> { { "key", key } },
            null,
            callBack);
    }

    public int SendTransaction(string jsonData, Action<int, ApiCall, string, bool> callBack)
    {
        return StartRequest(
           ApiCall.SendTransaction,
           Method.POST,
           null,
           null,
           jsonData,
           callBack);
    }

    public int GetNonce(string sender, Action<int, ApiCall, string, bool> callBack)
    {        
        return StartRequest(
            ApiCall.GetNonce,
            Method.GET,
            "nonce/" + sender,
            null,
            null,
            callBack);       
    }

 

    private int StartRequest(ApiCall apiCall, Method method, string path, Dictionary<string, string> parms, string jsonData, Action<int, ApiCall, string, bool> callBack)
    {
        int reqID = requestIDCounter++;
        StartCoroutine(
            SendRequest(
            reqID,
            apiCall,
            method,
            path,
            parms,
            jsonData,
            callBack));
        return reqID;
    }

    private IEnumerator SendRequest(int requestID, ApiCall apiCall, Method method, string path, Dictionary<string, string> parms, string jsonData, Action<int, ApiCall, string, bool> callBack)
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
            Debug.Log($"Sending web request {requestID} to {uri} with method of {method}");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                callBack?.Invoke(requestID, apiCall, request.error, FAILED);
            }
            else
            {
                string json = request.downloadHandler.text;
                callBack?.Invoke(requestID,apiCall, json, SUCCESS);
            }
        }
    }


}

