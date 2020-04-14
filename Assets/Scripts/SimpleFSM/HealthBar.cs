using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetHealth(int _health)
    {
        slider.value = _health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxValue(int _maxHelth)
    {
        slider.maxValue = _maxHelth;
        slider.value = _maxHelth;
        fill.color = gradient.Evaluate(1f);
    }
}
