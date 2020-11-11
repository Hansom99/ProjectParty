using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountOfAmmoText : MonoBehaviour
{
    public Text ammoAmount;
    public void SetAmmo(int amount)
    {
        string numString = Convert.ToString(amount);
        string ammoCount = "x" + numString;
        ammoAmount.text = ammoCount;
    }
}
