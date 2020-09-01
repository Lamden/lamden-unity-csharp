using System;
using System.Collections;
using System.Collections.Generic;
using unity.libsodium;
using UnityEditor;
using UnityEngine;


public class Wallet
{
    private KeyPair keyPair;

    public void New()
    {
        keyPair = new KeyPair();
    }

    public void Load(string sk)
    {
        byte[] skBytes = Helper.StringToByteArray(sk, 64);
        Debug.Log($"original: {sk}, decoded: {Helper.ByteArrayToHexString(skBytes)}");
        keyPair = new KeyPair(sk);
    }

    public string GetSignatureString(byte[] msg)
    {
        return Helper.ByteArrayToHexString(GetSignatureBytes(msg)).ToLower();
    }

    public byte[] GetSignatureBytes(byte[] msg)
    {
        if (keyPair == null)
            throw new Exception("Key pair not initialized");

        if (msg == null)
            throw new Exception("Msg cannot be null");

        byte[] sig = new byte[64];
        long sigLen = 0;
        if (NativeLibsodium.crypto_sign_detached(sig, ref sigLen, msg, msg.Length, keyPair.skBytes) == 0)
            return sig;
        else
            return null;
    }

    public bool Verify(byte[] sig, byte[] msg)
    {
        if (keyPair == null)
            throw new Exception("Key pair not initialized");

        if (sig == null)
            throw new Exception("sig cannot be null");

        if (msg == null)
            throw new Exception("Msg cannot be null");

        return NativeLibsodium.crypto_sign_verify_detached(sig, msg, msg.Length, keyPair.vkBytes) == 0 ? true : false;
    }

    public string GetVK()
    {
        if (keyPair == null)
            throw new Exception("Key pair not initialized");

        return keyPair.vkString;
    }

    public string GetSK()
    {
        if (keyPair == null)
            throw new Exception("Key pair not initialized");

        return keyPair.skString;
    }
}
