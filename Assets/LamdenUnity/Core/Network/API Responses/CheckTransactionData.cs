using SimpleJSON;
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
        public JSONNode state;
        //public List<State> state;
        public int status;
        public TxData transaction;

        [Serializable]
        public class Metadata
        {
            public string signature;
            public int timestamp;
        }


        [Serializable]
        public class TxData
        {
            public Metadata metadata = new Metadata();
            public Payload payload = new Payload();

            [Serializable]
            public class Metadata
            {
                public string signature;
                public int timestamp;
            }

            [Serializable]
            public class Payload
            {
                public string contract;
                public string function;
                public JSONNode kwargs;
                public int nonce;
                public string processor;
                public string sender;
                public int stamps_supplied;
            }
        }

    }
}
