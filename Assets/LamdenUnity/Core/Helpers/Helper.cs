using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static byte[] StringToByteArray(string hex)
    {
        return StringToByteArray(hex, hex.Length >> 1);
    }

    public static byte[] StringToByteArray(string hex, int len)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] arr = new byte[len];
        int actLen = hex.Length >> 1;
        for (int i = 0; i < actLen; i++)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return arr;
    }

    public static int GetHexVal(char hex)
    {
        int val = (int)hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    public static string ByteArrayToHexString(byte[] arr)
    {
        return BitConverter.ToString(arr).Replace("-", "");
    }
}
