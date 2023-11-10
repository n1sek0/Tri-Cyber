using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdtaeHealf : MonoBehaviour
{
    public Slider healthSlider;
    public Boss boss; 

    private void Start()
    {
        healthSlider.maxValue = boss.vida;
    }

    private void Update()
    {
        healthSlider.value = boss.vida;
    }
}
