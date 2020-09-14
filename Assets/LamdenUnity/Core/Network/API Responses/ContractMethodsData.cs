using System;

namespace LamdenUnity
{
    [Serializable]
    public class ContractMethodsData
    {
        public Methods[] methods;

        [Serializable]
        public class Methods
        {
            public string name;
            public Argument[] arguments;

            [Serializable]
            public class Argument
            {
                public string name;
                public string type;
            }
        }
    }
}

