using System.Collections;
using System.Collections.Generic;
using unity.libsodium;
using UnityEngine;

public class KeyPair
{
    public const int SKEY_LEN = 64;  // length of the signing (private) key in bytes   
    public const int VKEY_LEN = 32;  // lenght of verification (public) key in bytes
    public const int SEED_LEN = 32;

    public KeyPair()
    {
        sKey = new byte[SKEY_LEN];
        vKey = new byte[VKEY_LEN];
        NativeLibsodium.crypto_sign_keypair(vKey, sKey);
    }

    public KeyPair(string sk)
    {
        sKey = Helper.StringToByteArray(sk);
        vKey = new byte[VKEY_LEN];
        byte[] seed = new byte[SEED_LEN];
        NativeLibsodium.crypto_sign_ed25519_sk_to_seed(seed, sKey);
        NativeLibsodium.crypto_sign_seed_keypair(vKey, sKey, seed);
        Debug.Log($"sk: {Helper.ByteArrayToHexString(sKey)}, vk: {Helper.ByteArrayToHexString(vKey)} ");
    }

    public byte[] signingKey
    {
        get { return sKey; }
    }

    public byte[] verifyKey
    {
        get { return vKey; } 
    }

    private byte[] sKey;      //  Signing Key (SK) represents 32 byte signing key
    private byte[] vKey;              //  Verify Key (VK) represents a 32 byte verify key

}
