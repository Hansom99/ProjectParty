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
        if(Mathf.Sign(transform.root.localScale.x) != Mathf.Sign(transform.localScale.x))
        {
            Vector3 orientationFix = transform.localScale;
            orientationFix.x *= -1;
            transform.localScale = orientationFix;
        }
        //killCounter.text = networkManager.kills+" Kills \n" + networkManager.deaths + " Death";
        //ammoText.text = networkManager.Ammo+"";
        //healthSlider.value = networkManager.health/networkManager.maxHealth;
    }
}
