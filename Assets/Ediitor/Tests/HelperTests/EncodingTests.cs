using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LamdenUnity;

namespace Tests
{
    public class EncodingTests
    {
        string dateString = "2020-07-28T19:16:35.059Z";
        int millisecondsDelta = 475200000;

        [Test]
        public void EncodeDateTimeTest()
        {
            string encoded = new KT_DateTime(dateString).ToString();
            string provided = "[2020,7,28,19,16,35,59]";
            Debug.Log($"EncodeDateTimeTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

       
        [Test]
        public void EncodeTimeDeltaTest()
        {
            string encoded = new KT_DeltaTime(millisecondsDelta).ToString();
            string provided = "[5,43200]";
            Debug.Log($"EncodeTimeDeltaTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

        [Test]
        public void FloatTest ()
        {
            string encoded = new KT_Float(1.1f).ToString();
            string provided = "{\"__fixed__\":\"1.1\"}";
            Debug.Log($"FloatTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

    }
}
