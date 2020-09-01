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

    public enum ApiCall { GetNonce, Ping };

    public NetworkInfo networkInfo;

    public string host { get{ return hosts[Random.Range(0, hosts.Length)];}}

    public string url { get { return host; }}
      

    private void Awake()
    {
        SetNetworkInfo(networkInfo);    
    }


    /*
        send(method, path, data, callback){
    let parms = '';
    if (Object.keys(data).includes('parms')) {
        parms = this.createParms(data.parms)
    }

    let options = {}
    if (method === 'POST'){
        let headers = {'Content-Type': 'application/json'}
        options.method = method
        options.headers = headers;
        options.body = data;
    }

    return fetch(`${this.url}${path}${parms}`, options)
        .then(res => {
            return res.json()
        } )
        .then(json => {
                return callback(json, undefined)
        })
        .catch(err => {
                console.log(err)
                return callback(undefined, err.toString())
            })
        }
        */
    public int GetNonce(string sender, Action<int, ApiCall, string, bool> callBack)
    {        
        return StartRequest(
            ApiCall.GetNonce,
            Method.GET,
            "nonce/" + sender,
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
            callBack);       
    }

    private int StartRequest(ApiCall apiCall, Method method, string path, object parms, Action<int, ApiCall, string, bool> callBack)
    {
        int reqID = requestIDCounter++;
        StartCoroutine(SendRequest(
            reqID,
            apiCall,
            method,
            path,
            parms,
            callBack));
        return reqID;
    }

    private IEnumerator SendRequest(int requestID, ApiCall apiCall, Method method, string path, object parms, Action<int, ApiCall, string, bool> callBack)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(string.Format(host + path)))
        {
            if(method == Method.POST)
            {
                //req.SetRequestHeader()
            }

            req.method = method.ToString();
            req.timeout = timeout;
            yield return req.SendWebRequest();
            if (!string.IsNullOrWhiteSpace(req.error))
            {
                callBack?.Invoke(requestID, apiCall, req.error, FAILED);
            }
            else
            {
                byte[] result = req.downloadHandler.data;
                string json = System.Text.Encoding.Default.GetString(result);
                callBack?.Invoke(requestID,apiCall, json, SUCCESS);
            }
        }
    }

    /*
     
     async getContractInfo(contractName){
        let path = `/contracts/${contractName}`
        return this.send('GET', path, {}, (res, err) => {
            if (err) return;
            return res
        })
    }

    async getVariable(contract, variable, key = ''){
        let parms = {};
        if (validateTypes.isStringWithValue(key)) parms.key = key;

        let path = `/contracts/${contract}/${variable}/`
        return this.send('GET', path, {parms}, (res, err) => {
            try{
                if (res.value) return res.value
            } catch (e){}
            return;
        })
    }

    async getContractMethods(contract){
        let path = `/contracts/${contract}/methods`
        return this.send('GET', path, {}, (res, err) => {
            try{
                if (res.methods) return res.methods
            } catch (e){}
            return [];
        })
        
    }

    async pingServer(){
        return this.send('GET', '/ping', {}, (res, err) => {
            try { 
                if (res.status === 'online') return true;
            } 
            catch (e) {
                return false;
            }
        })
    }

    async getCurrencyBalance(vk){
        let balanceRes = await this.getVariable('currency', 'balances', vk)
        if (isNaN(parseFloat(balanceRes))){
            return 0;
        }
        return parseFloat(balanceRes)
    }

    async contractExists(contractName){
        let path = `/contracts/${contractName}`
        return this.send('GET', path, {}, (res, err) => {
            try{
                if (res.name) return true;
            } catch (e){}
            return false;
        })
    }

    async sendTransaction(data, callback){
        return this.send('POST', '/', JSON.stringify(data), (res, err) => {
            if (err){
                if (callback) {
                    callback(undefined, err);
                    return;
                } 
                else return err
            }
            if (callback) {
                callback(res, undefined);
                return
            }
            return res;
        })   
    }

    async getNonce(sender, callback){
        if (!validateTypes.isStringHex(sender)) return `${sender} is not a hex string.`
        let path = `/nonce/${sender}` 
        return this.send('GET', path, {}, (res, err) => {
            if (err){
                if (callback) {
                    callback(undefined, `Unable to get nonce for "${sender}". ${err}`)
                    return
                } 
                return `Unable to get nonce for "${sender}". ${err}`
            }
            if (callback) {
                callback(res, undefined)
                return
            }
            else return res;
        })
    }

    async checkTransaction(hash, callback){
        const parms = {hash};
        return this.send('GET', '/tx', {parms}, (res, err) => {
            if (err){
                if (callback) {
                    callback(undefined, err);
                    return;
                }
                else return err
            }
            if (callback) {
                callback(res, undefined);
                return
            }
            return res;
        })  
    }


    */


}

