using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using unity.libsodium;
using UnityEditor;
using UnityEngine;

namespace LamdenUnity
{
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

        public string GetSignatureString(string msg) {
            return GetSignatureString(Encoding.ASCII.GetBytes(msg));
        }

        public string GetSignatureString(byte[] msg)
        {
            return Helper.ByteArrayToHexString(GetSignatureBytes(msg)).ToLower();
        }

        public byte[] GetSignatureBytes(byte[] msg)
        {
            if (keyPair == null)
            {
                Debug.LogError("Key pair not initialized");
                return null;
            }

            if (msg == null)
            {
                Debug.LogError("Msg cannot be null");
                return null;
            }

            byte[] sig = new byte[64];
            long sigLen = 0;
            try
            {
                if (NativeLibsodium.crypto_sign_detached(sig, ref sigLen, msg, msg.Length, keyPair.skBytes) == 0)
                    return sig;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error generating signature: " + ex.Message);
            }

            return null;
        }

        public bool Verify(byte[] sig, byte[] msg)
        {
            if (keyPair == null)
            {
                Debug.LogError("Key pair not initialized");
                return false;
            }

            if (sig == null)
            {
                Debug.LogError("sig cannot be null");
                return false;
            }

            if (msg == null)
            {
                Debug.LogError("Msg cannot be null");
                return false;
            }

            return NativeLibsodium.crypto_sign_verify_detached(sig, msg, msg.Length, keyPair.vkBytes) == 0 ? true : false;
        }

        public string GetVK()
        {
            if (keyPair == null)
            {
                Debug.LogError("Key pair not initialized");
                return null;
            }

            return keyPair.vkString;
        }

        public string GetSK()
        {
            if (keyPair == null)
            {
                Debug.LogError("Key pair not initialized");
                return null;
            }

            return keyPair.skString;
        }
    }
}
