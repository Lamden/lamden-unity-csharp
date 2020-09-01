using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encoder
{ 
    private static void ThrowError(Type type, object value)
    {
        throw new Exception($"Error encoding {value} to {type.Name}");
    }

    private bool isString(object value)
    {
        return value is string;
    }
}
