using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowEnemyInfo : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;

    [SerializeField] EnemyAI AI;

    void Start()
    {       
        HealthSlider.maxValue = AI.MaxHealth;
    }  

    void Update(){
        HealthSlider.value = AI.Health;
    }
}
