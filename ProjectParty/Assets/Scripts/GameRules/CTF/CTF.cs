using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTF : MonoBehaviour
{
    [SerializeField] public GameObject FlagA;
    [SerializeField] public GameObject FlagB;

    [SerializeField] int LayerTeamA;
    [SerializeField] int LayerTeamB;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void broughtFlagHome()
    {
        if(FlagA.transform.parent == FlagB.transform.parent)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        
    }
}
