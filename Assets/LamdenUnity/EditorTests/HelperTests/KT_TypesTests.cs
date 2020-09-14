using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LamdenUnity;

namespace Tests
{
    public class KT_TypesTests
    {
        string dateString = "2020-07-28T19:16:35.059Z";
        int millisecondsDelta = 475200000;

        [Test]
        public void KT_DateTimeTest()
        {
            string encoded = new KT_DateTime(dateString).ToString();
            string provided = "[2020,7,28,19,16,35,59]";
            Debug.Log($"EncodeDateTimeTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }


        [Test]
        public void KT_TimeDeltaTest()
        {
            string encoded = new KT_TimeDelta(millisecondsDelta).ToString();
            string provided = "[5,43200]";
            Debug.Log($"EncodeTimeDeltaTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

        [Test]
        public void KT_NumericTest()
        {
            string encoded = new KT_Numeric(1.1f).ToString();
            string provided = "{\"__fixed__\":\"1.1\"}";
            Debug.Log($"FloatTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

        [Test]
        public void KT_DictTest()
        {
            Dictionary<string, KwargType> dict = new Dictionary<string, KwargType>();
            dict.Add("btest", new KT_Bool(false));
            dict.Add("atest", new KT_Int(1));
            string encoded = new KT_Dict(dict).ToString();
            string provided = "{\"atest\":1,\"btest\":false}";
            Debug.Log($"DictTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

        [Test]
        public void KT_ListTest()
        {
            List<KwargType> list = new List<KwargType>();
            list.Add(new KT_Bool(false));
            list.Add(new KT_Int(1));
            string encoded = new KT_List(list).ToString();
            string provided = "[false,1]";
            Debug.Log($"ListTest: {encoded}");
            Assert.AreEqual(encoded, provided);
        }

        [Test]
        public void KT_BoolTest()
        {
            KT_Bool ktBoolTrue = new KT_Bool(true);
            Assert.AreEqual("true", ktBoolTrue.ToString());

            KT_Bool ktBoolFalse = new KT_Bool(false);
            Assert.AreEqual("false", ktBoolFalse.ToString());

        }

        [Test]
        public void KT_StringTest()
        {
            KT_String ktString = new KT_String("testing value");
            Assert.AreEqual("\"testing value\"", ktString.ToString());
        }

        [Test]
        public void KT_UIDTest()
        {
            KT_UID ktUid = new KT_UID("testing value");
            Assert.AreEqual("\"testing value\"", ktUid.ToString());
        }


    }
}
