using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    void Start()
    {
        slider.maxValue = 100; // Valor máximo da barra de vida
        slider.value = 100; // Valor atual da barra de vida
    }

    public void SetHealth(int health)
    {
        slider.value = health; // Atualiza o valor da barra de vida

        // Atualiza a cor do preenchimento da barra de vida com base no gradiente
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}