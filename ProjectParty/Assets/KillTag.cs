using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillTag : MonoBehaviour
{
    public Text text;
    private int kills;
    void FixedUpdate()
    {
        SetKills(GlobalSettings.kills); 
    }

    public void SetKills(int kills)
    {
        this.kills = kills;
        text.text = "Kills: " + this.kills.ToString();
    }
}
