using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{


    public NetworkManager networkManager;

    public Slider healthSlider;
    public Text killCounter;
    public Text ammoText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        killCounter.text = networkManager.kills+" Kills \n" + networkManager.deaths + " Death";
        ammoText.text = networkManager.Ammo+"";
        healthSlider.value = networkManager.health/networkManager.maxHealth;
    }
}
