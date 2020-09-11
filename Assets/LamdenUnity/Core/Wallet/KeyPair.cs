using System.Collections;
using System.Collections.Generic;
using unity.libsodium;
using UnityEngine;

namespace LamdenUnity
{
    public class KeyPair
    {
        public const int SKEY_LEN = 64;  // length of the signing (private) key in bytes   
        public const int VKEY_LEN = 32;  // lenght of verification (public) key in bytes
        public const int SEED_LEN = 32;

        public byte[] skBytes { get; }
        public string skString { get { return Helper.ByteArrayToHexString(skBytes).Substring(0, 64).ToLower(); } }

        public byte[] vkBytes { get; }
        public string vkString { get; }

        public KeyPair()
        {
            skBytes = new byte[SKEY_LEN];
            vkBytes = new byte[VKEY_LEN];
            NativeLibsodium.crypto_sign_keypair(vkBytes, skBytes);
            Debug.Log($"sk: {Helper.ByteArrayToHexString(skBytes)}, vk: {Helper.ByteArrayToHexString(vkBytes)} ");            
            vkString = Helper.ByteArrayToHexString(vkBytes).ToLower();
        }

        public KeyPair(string sk)
        {
            skBytes = Helper.StringToByteArray(sk);
            vkBytes = new byte[VKEY_LEN];
            byte[] seed = new byte[SEED_LEN];
            NativeLibsodium.crypto_sign_ed25519_sk_to_seed(seed, skBytes);
            NativeLibsodium.crypto_sign_seed_keypair(vkBytes, skBytes, seed);

            // The x64 versions of the library require a 64 byte array for the sk that is the sk+vk
            byte[] temp = new byte[SKEY_LEN];            
            for (int i = 0; i < 32; i++)
            {
                temp[i] = skBytes[i];
            }
            for (int i = 0; i < 32; i++)
            {
                temp[i + 32] = vkBytes[i];
            }
            skBytes = temp;

            Debug.Log($"sk: {Helper.ByteArrayToHexString(skBytes)}, vk: {Helper.ByteArrayToHexString(vkBytes)} ");            
            vkString = Helper.ByteArrayToHexString(vkBytes).ToLower();
        }
    }
}
