using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Network : MonoBehaviour
{
    public enum Method { GET, POST };

    protected string type;
    protected string[] hosts;
    protected string currencySymbol;
    protected bool lamden;
    protected string blockExplorer;
    protected bool onliine;

    public int timeout = 10000;    

    public void SetNetworkInfo(NetworkInfo networkInfo)
    {
        if(networkInfo == null)
            throw new Exception("networkInfo cannot be null");

        if (networkInfo.hosts == null)
            throw new Exception("networkInfo.hosts cannot be null");

        type = string.IsNullOrEmpty(networkInfo.networkType)? "custom" : networkInfo.networkType.ToLower();

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


}



