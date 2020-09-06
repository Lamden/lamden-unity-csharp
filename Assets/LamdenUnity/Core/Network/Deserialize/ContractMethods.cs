using System;

namespace LamdenUnity
{
    [Serializable]
    public class Argument
    {
        public string name;


        public string type;
    }

    [Serializable]
    public class Methods
    {
        public string name;


        public Argument[] arguments;
    }

    [Serializable]
    public class ContractMethods
    {
        public Methods[] methods;
    }
}

