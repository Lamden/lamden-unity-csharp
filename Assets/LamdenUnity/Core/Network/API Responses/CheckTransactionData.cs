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
        public List<State> state;
        public int status;
        public TxData transaction;

        [Serializable]
        public class State
        {
            public string stateJson;
        }

        [Serializable]
        public class Metadata
        {
            public string signature;
            public int timestamp;
        }
        
        
    }
}
