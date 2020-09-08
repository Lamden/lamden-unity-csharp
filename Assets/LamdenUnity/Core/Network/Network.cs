using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace LamdenUnity
{
    public class Network : MonoBehaviour
    {
        public const bool SUCCESS = true;
        public const bool FAILED = false;

        public enum Method { GET, POST };

        protected string type;
        protected string[] hosts;
        protected string currencySymbol;
        protected bool lamden;
        protected string blockExplorer;
        protected bool onliine;

        public int timeout = 10000;

        // The host should be locked during a transaction to ensure the transaction 
        // is communicated to the same node that provided the nonce
        private bool hostLocked = false;
        private int hostLockIndex = 0;
        public void LockHost(bool setLocked) 
        {
            if (setLocked)
            {
                hostLockIndex = Random.Range(0, hosts.Length);
                hostLocked = true;
            }
            else
                hostLocked = false;
        }

        public string host { get {
                if (hostLocked)
                    return hosts[hostLockIndex];
                else
                    return hosts[Random.Range(0, hosts.Length)]; 
            } }

        public void SetNetworkInfo(NetworkInfo networkInfo)
        {
            if (networkInfo == null)
                throw new Exception("networkInfo cannot be null");

            if (networkInfo.hosts == null)
                throw new Exception("networkInfo.hosts cannot be null");

            type = string.IsNullOrEmpty(networkInfo.networkType) ? "custom" : networkInfo.networkType.ToLower();

            if (ValidateHosts(networkInfo.hosts))
                hosts = networkInfo.hosts;

            currencySymbol = string.IsNullOrEmpty(networkInfo.networkType) ? "TAU" : currencySymbol = networkInfo.networkType.ToLower();

            blockExplorer = string.IsNullOrEmpty(networkInfo.blockExplorer) ? null : networkInfo.blockExplorer;
        }

        bool ValidateHosts(string[] hosts)
        {
            int len = hosts.Length;
            for (int i = 0; i < len; i++)
            {
                if (!ValidateProtocol(hosts[i].ToLower()))
                    throw new Exception("Host String must start with http:// or https://");

                if (!hosts[i].EndsWith("/"))
                    hosts[i] += "/";
            }

            return true;
        }

        bool ValidateProtocol(string host)
        {
            if (host.StartsWith("http://") || host.StartsWith("https://"))
                return true;
            else
                return false;
        }

        protected IEnumerator SendRequest(Method method, string path, Dictionary<string, string> parms, string jsonData, Action<string, bool> callBack)
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
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    UploadHandlerRaw upHandler = new UploadHandlerRaw(data);                    
                    request.uploadHandler = upHandler;
                    request.SetRequestHeader("Content-Type", "application/json");
                    
                }

                request.timeout = timeout;
                Debug.Log($"Sending web request to {uri} with method of {method}");
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError($"Received ERROR response from {uri} of {request.error}");
                    callBack?.Invoke(request.error, FAILED);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    Debug.Log($"Received response from {uri} of {json}");
                    callBack?.Invoke(json, SUCCESS);

                }
            }
        }
    }

  

}



