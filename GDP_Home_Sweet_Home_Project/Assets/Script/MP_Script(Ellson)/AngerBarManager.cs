using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerBarManager : MonoBehaviour
{

    public NeighbourAngerBar neighbour1;
    public NeighbourAngerBar neighbour2;

    public Slider HappinessBar;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHappinessBar();
    }

   

    public void UpdateHappinessBar()
    {
        //calculate the average happiness by taking both the neighbours indivdual happiness value and adding them then divide by 2 to give the average value for the happiness total decrease
        float averageHappinessDecrease = (neighbour1.currentHappiness + neighbour2.currentHappiness) / 2;

        float maxHappiness = (neighbour1.maxHappyBar + neighbour2.maxHappyBar) / 2; // value will be 100f

        UpdateHappinessGauge(averageHappinessDecrease, maxHappiness);

       
    }

    void UpdateHappinessGauge(float averageHappinessDecrease, float maxHappiness)
    {
        //calculate happiness level based on the average  happiness decrease and the max happiness  
        float happinessLevel = 1 - (averageHappinessDecrease / maxHappiness);

        HappinessBar.value = happinessLevel;
    }
}