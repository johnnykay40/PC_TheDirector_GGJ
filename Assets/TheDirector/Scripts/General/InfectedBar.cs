using UnityEngine;
using UnityEngine.UI;

public class InfectedBar : MonoBehaviour
{
    private Slider slider;

    public void MaxInfection(float maxInfected)
    {
        slider.maxValue = maxInfected;
    }

    public void ChangeInfection(float infectionQuantity)
    {
        slider.value = infectionQuantity;
    }

    public void StartInfectionBar(float startValue, float maxValue) 
    {
        slider = GetComponent<Slider>();
        ChangeInfection(startValue);
        MaxInfection(maxValue);
    }

}
