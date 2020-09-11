using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using LamdenUnity;
using UnityEngine.Assertions;

public class LamdenTest : MonoBehaviour
{
    string vk = "4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9";   

    public MasterNodeApi masterNodeApi;

    public InputField inputSKtoVK, inputMsg, inputSig, inputSK, inputVK, inputBalance, inputMsgToSign, inputMsgSig;

    Wallet wallet;

    public Image imagePing;

   

    // Start is called before the first frame update
    void Start()
    {
        //Ping();
        //Test_LoadWallet_Sign();
    }

    public void Test_LoadWallet_Sign()
    {
        const string msg = "{\"contract\":\"con_values_testing\",\"function\":\"test_values\",\"kwargs\":{\"ANY\":{\"__fixed__\":\"1.1\"},\"Bool\":false,\"DateTime\":[2020,9,10,22,30,20,600],\"Dict\":{\"test\":\"my test\"},\"Float\":{\"__fixed__\":\"1.1\"},\"Int\":2,\"List\":[{\"__fixed__\":\"1.2\"},\"test2\"],\"Str\":\"this is another string\",\"TimeDelta\":[0,4],\"UID\":\"testing2\"},\"nonce\":29,\"processor\":\"89f67bb871351a1629d66676e4bd92bbacb23bd0649b890542ef98f1b664a497\",\"sender\":\"4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9\",\"stamps_supplied\":100}";
        byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
        byte[] sig = wallet.GetSignatureBytes(msgBytes);
        inputMsgSig.text = Helper.ByteArrayToHexString(sig);
        Assert.IsTrue(wallet.Verify(sig, msgBytes));
    }

    public void UpdeateSKandVK()
    {
        UpdateSK();
        UpdateVK();
    }

    public Wallet GetWallet()
    {
        return wallet;
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

    public void ClickNewWalletFromSk()
    {
        wallet = new Wallet();
        wallet.Load(inputSKtoVK.text);
        UpdeateSKandVK();
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

    public void GetBalance()
    {
        masterNodeApi.GetCurrencyBalance(inputVK.text, (bool success, float amount) =>
        {
            inputBalance.text = $"{amount} {masterNodeApi.networkInfo.currencySymbol}";
        });
    }


}


