using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPHandler : MonoBehaviour
{
    [SerializeField]
        int HP;
    [SerializeField]
    private Gradient gradiante;
    
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image HpBar;


    public void SetHP(int SetHP)
    {
        HP = SetHP;
        slider.maxValue = HP;
        slider.value = HP;
        HpBar.color = gradiante.Evaluate(1f);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        slider.value = HP;
        HpBar.color = gradiante.Evaluate(slider.normalizedValue);
    }
}