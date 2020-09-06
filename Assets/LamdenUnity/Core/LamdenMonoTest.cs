using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using LamdenUnity;

public class LamdenMonoTest : MonoBehaviour
{
    string vk = "d41b8ed0d747ca6dfacdc58b78e1dba86cd9616359014eebd5f3443509111120";
    //string vk = "960c002a36c30c3aec8bc670e9b8b40eebcfd545f4e9237579fd7395a21ccebb";
    string sk = "c8a3c5333aa3b058c4fa16d48db52355ab62ddc8daa9a183706a912e522440b6";

    string msg = "this is a message";
    byte[] msgBytes;

    public MasterNodeApi masterNodeApi;

    public InputField inputSKtoVK, inputMsg, inputSig, inputSK, inputVK, inputContractName, inputContactInfo;

    Wallet wallet;

    public Image imagePing;

   

    // Start is called before the first frame update
    void Start()
    {        
        msgBytes = Encoding.ASCII.GetBytes(msg);
        //long msgLen = msgBytes.Length;

        //LoadWalletTest();

        //NewWalletTest();


        masterNodeApi.GetVariable("currency", "balances", vk, callBack);
        //masterNodeApi.GetContractMethods("currency", callBack);
       // masterNodeApi.GetCurrencyBalance(vk, callBack);

    }

    public void UpdeateSKandVK()
    {
        UpdateSK();
        UpdateVK();
    }

    private void UpdateSK()
    {
        inputSK.text = wallet.GetSK();
    }

    private void UpdateVK()
    {
        inputVK.text = wallet.GetVK();
    }

    public void ClickCreateNewWallet()
    {
        wallet = new Wallet();
        wallet.New();
        UpdeateSKandVK();
    }

    public void ClickContractInfo()
    {

        masterNodeApi.GetContractInfo(inputContractName.text, callBack);
    }

    public void ClickVariableInfo()
    {
        
    }

    public void ClickNewWalletFromSk()
    {
        wallet = new Wallet();
        wallet.Load(inputSKtoVK.text);
        UpdeateSKandVK();
    }

    public void GetSig()
    {
        if(inputMsg.text != "" && wallet != null)
        {
            inputSig.text = wallet.GetSignatureString(Encoding.ASCII.GetBytes(inputMsg.text));
        }
    }

    public void Ping()
    {
        masterNodeApi.PingServer((bool success, string json) =>
        {
            if (success)
                imagePing.color = Color.green;
            else
            {
                imagePing.color = Color.red;
                Debug.LogWarning($"Ping failed with response: {json}");
            }
        });
    }


    void PingAction(bool success, string json)
    {
        if (success)
            imagePing.color = Color.green;
        else
        {
            imagePing.color = Color.red;
            Debug.LogWarning($"Ping failed with response: {json}");
        }
    }

    public void Ping2()
    {
        masterNodeApi.PingServer(PingAction);
    }



    public void GetNonce()
    {
        masterNodeApi.GetNonce(vk, callBack);
    }

    void callBack(bool callCompleted, string json)
    {
        Debug.Log($"Request: {(callCompleted ? "successful": "failed")}: {json}");       
    }

    void LoadWalletTest()
    {
        Wallet wallet = new Wallet();
        wallet.Load(sk);

        byte[] sig = wallet.GetSignatureBytes(msgBytes);

        Debug.Log(Helper.ByteArrayToHexString(sig).ToLower());

        if (wallet.Verify(sig, msgBytes))
        {
            Debug.Log("message was verified");
        }
        else
        {
            Debug.Log("message was *NOT* verified");
        }
    }

    void NewWalletTest()
    {
        Wallet wallet = new Wallet();
        wallet.New();

        byte[] sig = wallet.GetSignatureBytes(msgBytes);

        Debug.Log(Helper.ByteArrayToHexString(sig).ToLower());

        if (wallet.Verify(sig, msgBytes))
        {
            Debug.Log("message was verified");
        }
        else
        {
            Debug.Log("message was *NOT* verified");
        }
    }

    //var x = NativeLibsodium.sodium_init();
    //Debug.Log("NativeLibsodium.sodium_init(): " + x);

    //byte[] vKey = new byte[32];
    //byte[] sKey = new byte[64];

    //NativeLibsodium.crypto_sign_keypair(vKey, sKey);        

    //Debug.Log("vk (public key):" + BitConverter.ToString(vKey).Replace("-", ""));
    //Debug.Log("sk (private key):" + BitConverter.ToString(sKey).Replace("-", ""));

    //byte[] sig = new byte[64];
    //long sigLen = 0;

    //int signRes = NativeLibsodium.crypto_sign_detached(sig, ref sigLen, msgBytes, msgLen, sKey);
    //Debug.Log("sign result: " + signRes);
    //string sigString = BitConverter.ToString(sig).Replace("-", "");
    //Debug.Log("sig: " + sigString);
    //Debug.Log("sigLen: " + sigString.Length);

    //int verRes = NativeLibsodium.crypto_sign_verify_detached(sig, msgBytes, msgLen, vKey);
    //Debug.Log("verify result: " + verRes);


}


