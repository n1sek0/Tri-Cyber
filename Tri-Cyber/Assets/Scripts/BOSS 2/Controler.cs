using UnityEngine;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    public Slider healthSlider;
    public boss2 boss; 

    private void Start()
    {
        healthSlider.maxValue = boss.vida;
    }

    private void Update()
    {
        healthSlider.value = boss.vida;
    }
}