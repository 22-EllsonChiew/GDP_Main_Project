using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AngerBarManager : MonoBehaviour
{

    public NeighbourAngerBar neighbour1;
    public NeighbourAngerBar neighbour2;

    public Slider HappinessBar;

    private string loseScene = "Lose Scene";
    // Start is called before the first frame update
    void Start()
    {
        UpdateHappinessBar();
        //HappinessBar.value = 1f;
    }

   

    public void UpdateHappinessBar()
    {
        //calculate the average happiness by taking both the neighbours indivdual happiness value and adding them then divide by 2 to give the average value for the happiness total decrease
        float averageHappinessDecrease = (neighbour1.currentHappiness + neighbour2.currentHappiness) / 2;

        float maxHappiness = (neighbour1.maxHappyBar + neighbour2.maxHappyBar) / 2; // value will be 100f

        Debug.Log($"Average Happiness Decrease: {averageHappinessDecrease}");
        Debug.Log($"Max Happiness: {maxHappiness}");

        UpdateHappinessGauge(averageHappinessDecrease, maxHappiness);

       
    }

    void UpdateHappinessGauge(float averageHappinessDecrease, float maxHappiness)
    {
        //calculate happiness level based on the average  happiness decrease and the max happiness  
        float happinessLevel = 1 - (averageHappinessDecrease / maxHappiness);

        Debug.Log($"Happiness Level: {happinessLevel}");

        HappinessBar.value = happinessLevel;

        if(happinessLevel == 1)
        {
            SceneManager.LoadScene(loseScene);
        }
    }
}
