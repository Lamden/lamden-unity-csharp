
using System.Text;
using NUnit.Framework;
using LamdenUnity;

namespace Tests
{
    public class WalletTests
    {
        const string msg = "this is a message";
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
            string sk = "c8a3c5333aa3b058c4fa16d48db52355ab62ddc8daa9a183706a912e522440b6";
            wallet.Load(sk);
            Assert.AreEqual(wallet.GetVK(), "960c002a36c30c3aec8bc670e9b8b40eebcfd545f4e9237579fd7395a21ccebb");      
            Assert.AreEqual(wallet.GetSK(), sk);
            string sig = wallet.GetSignatureString(msgBytes);
            Assert.IsTrue(sig.Equals("4cede72f3bbb88c2ad3ad9c5831b5876657ee33718f41568b1b68ccfb9f296187b67edd95aec55097f88d276dfb14f022c755422607c89ce8e69a10454c30c00"));            
        }


    }
}
