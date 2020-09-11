using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetdTAU : MonoBehaviour
{
    public LamdenTest lamdenTest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Get_dTAU()
    {
        //if(lamdenTest.GetWallet() != null && lamdenTest.GetWallet().initialized)
        //    Application.OpenURL($"https://faucet.lamden.io/{lamdenTest.GetWallet().GetVK()}");
        //else

        if (lamdenTest.GetWallet() != null && lamdenTest.GetWallet().initialized)
            CopyToClipboard(lamdenTest.GetWallet().GetVK());

        Application.OpenURL($"https://faucet.lamden.io/");
    }

    // This needs to be added to a public static class to be used like an extension
    public static void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }
}
