using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamdenUnity
{
    [Serializable]
    public class CheckTransactionData
    {
        public string hash;
        public string result;
        public int stamps_used;
        public List<State> state;
        public int status;
        public TxData transaction;

        [Serializable]
        public class State
        {
            public string key;
            public object value;
        }

        [Serializable]
        public class Metadata
        {
            public string signature;
            public int timestamp;
        }
        
        //[Serializable]
        //public class Kwargs
        //{
        //    public int amount;
        //    public string to;
        //}

        //[Serializable]
        //public class Payload
        //{
        //    public string contract;
        //    public string function;
        //    public Kwargs kwargs;
        //    public int nonce;
        //    public string processor;
        //    public string sender;
        //    public int stamps_supplied;
        //}

        //[Serializable]
        //public class Transaction
        //{
        //    public Metadata metadata;
        //    public Payload payload;
        //}

    }
}
