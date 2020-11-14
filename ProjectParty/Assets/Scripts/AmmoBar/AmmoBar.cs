using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;
    public Text text;

    private void Start()
    {
        SetMaxAmmo(GlobalSettings.ammunition);
    }

    private void FixedUpdate()
    {
        SetAmmo(GlobalSettings.ammunition);
    }

    public void SetMaxAmmo(int ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;

        fill.color = gradient.Evaluate(1f);
        text.text = "X"+ammo.ToString();
    }

    public void SetAmmo(int ammo)
    {
        slider.value = ammo;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.text = "X" + ammo.ToString();
    }
}
