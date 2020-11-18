using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathTag : MonoBehaviour
{
    public Text text;
    private int deaths;

    void FixedUpdate()
    {
        SetDeaths(GlobalSettings.deaths);
    }

    public void SetDeaths(int deaths)
    {
        this.deaths = deaths;
        text.text = "Deaths: " + this.deaths.ToString();
    }
}
