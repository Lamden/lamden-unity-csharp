
using System.Text;
using NUnit.Framework;
using LamdenUnity;
using UnityEngine;

namespace Tests
{
    public class WalletTests
    {
        const string msg = "{\"contract\":\"con_values_testing\",\"function\":\"test_values\",\"kwargs\":{\"ANY\":{\"__fixed__\":\"1.1\"},\"Bool\":false,\"DateTime\":[2020,9,10,22,30,20,600],\"Dict\":{\"test\":\"my test\"},\"Float\":{\"__fixed__\":\"1.1\"},\"Int\":2,\"List\":[{\"__fixed__\":\"1.2\"},\"test2\"],\"Str\":\"this is another string\",\"TimeDelta\":[0,4],\"UID\":\"testing2\"},\"nonce\":29,\"processor\":\"89f67bb871351a1629d66676e4bd92bbacb23bd0649b890542ef98f1b664a497\",\"sender\":\"4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9\",\"stamps_supplied\":100}";
        byte[] msgBytes = Encoding.ASCII.GetBytes(msg);

        // Tests that the 
        [Test]
        public void TestNewWallet()
        {
            Wallet wallet = new Wallet();
            Assert.IsNotNull(wallet);
            Assert.DoesNotThrow(wallet.New);
            Assert.IsTrue(Helper.isValidKeyString(wallet.GetVK()));
            Assert.IsTrue(Helper.isValidKeyString(wallet.GetSK()));
            byte[] sig = wallet.GetSignatureBytes(msgBytes);
            Assert.IsTrue(wallet.Verify(sig, msgBytes));
            byte[] msg2 = Encoding.ASCII.GetBytes("other message");
            Assert.IsFalse(wallet.Verify(sig, msg2));            
        }
        
        
        // Tests both loading an wallet from an SK and validating the signature
        [Test]
        public void Test_LoadWallet_Sign()
        {
            Wallet wallet = new Wallet();
            Assert.IsNotNull(wallet);
            string sk = "3b1efc0a2cfd9d92581afc927c443bb53157fffae0f533995f2830643542467c";
            wallet.Load(sk);
            Assert.AreEqual(wallet.GetVK(), "4680c6ea89ffc29b0b670a5712edef2b62bc0cf40bfba2f20bbba759cdd185b9");      
            Assert.AreEqual(wallet.GetSK(), sk);
            for (int i = 0; i < 20; i++)
            {
                byte[] sig = wallet.GetSignatureBytes(msgBytes);
                string sigString = Helper.ByteArrayToHexString(sig);
                Debug.Log("sig: " + sigString);
                Assert.AreEqual("62a2c5d3b56425ec23a4a08eb8983bbca0eb823accf3ccc0bb7bdf2445c9e7ace1c1304247931a82b284dce6fd8dcae9bbfa4c94be1dd0c1f244c4042c64e40b", sigString);
                Assert.IsTrue(wallet.Verify(sig, msgBytes));
            }
            
        }


    }
}
