using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//     For the HealthBar in UI 
//    Sets the value of current health and adjusts slider accordingly 
public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxHealth(float health) // initialisiere max Health
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f); // healthbar soll anfangs komplett gefüllt sein
    }
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
