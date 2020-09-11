using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LamdenUnity;

namespace Tests
{
    public class HelperTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestDateStamp()
        {
            int dateStamp = Helper.GetDateStamp();
            Debug.Log($"Date stamp now (seconds): {dateStamp}.");
        }

     
    }
}
