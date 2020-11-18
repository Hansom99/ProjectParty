using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperBar : MonoBehaviour
{

    public Slider slider;

    private void Start()
    {
        slider.maxValue = GlobalSettings.maxSpecialAttackEnergy;
    }

    private void FixedUpdate()
    {
        slider.value = GlobalSettings.specialAttackEnergy;
    }
}
