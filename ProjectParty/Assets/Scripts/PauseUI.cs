using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject inGameUI;

    public GameObject menue;
    public GameObject selectUI;


    bool isActive = false;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetButtonDown("Cancel"))
        {
            if(!isActive)
            {
                menue.SetActive(true);
                inGameUI.SetActive(false);
                isActive = true;
            }
            else
            {
                continueGame();
            }
        }
    }


    public void continueGame()
    {
        menue.SetActive(false);
        selectUI.SetActive(false);
        inGameUI.SetActive(true);
        isActive = false;
    }
    public void goToSelect()
    {
        menue.SetActive(false);
        selectUI.SetActive(true);
    }
    public void goToMenue()
    {
        menue.SetActive(true);
        selectUI.SetActive(false);
    }

}
