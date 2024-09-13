using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AngerBarManager : MonoBehaviour
{

    public Neighbour neighbour_Hakim;
    public Neighbour neighbour_Sherryl;

    public Image noiseColourFill;
    public Gradient noiseGradient;
    public Slider HappinessBar;

    private string loseScene = "Lose Scene";
    // Start is called before the first frame update

    public static AngerBarManager Instance { get; private set; }
    void Start()
    {
        //UpdateHappinessBar();
    }

    private void Update()
    {
        UpdateHappinessBar();

        if (neighbour_Hakim.CurrentHappiness <= 0f)
        {
            ScoreManager.Instance.SetAngeredNeighbour(neighbour_Hakim);
            SceneManager.LoadScene(loseScene);
        }
        else if (neighbour_Sherryl.CurrentHappiness <= 0f)
        {
            ScoreManager.Instance.SetAngeredNeighbour(neighbour_Sherryl);
            SceneManager.LoadScene(loseScene);
        }
    }

    public void UpdateHappinessBar()
    {
        //calculate the average happiness by taking both the neighbours indivdual happiness value and adding them then divide by 2 to give the average value for the happiness total decrease
        float averageHappinessDecrease = (neighbour_Hakim.CurrentHappiness + neighbour_Sherryl.CurrentHappiness) / 2;

        float maxHappiness = (neighbour_Hakim.maxHappiness + neighbour_Sherryl.maxHappiness) / 2; // value will be 100f
        noiseColourFill.color = noiseGradient.Evaluate(HappinessBar.value);

        UpdateHappinessGauge(averageHappinessDecrease, maxHappiness);
        


    }

    void UpdateHappinessGauge(float averageHappinessDecrease, float maxHappiness)
    {
        //calculate happiness level based on the average  happiness decrease and the max happiness  
        float happinessLevel = 1 - (averageHappinessDecrease / maxHappiness);

        HappinessBar.value = happinessLevel;

        //Debug.Log($"Happiness Level: {happinessLevel}");


    }
}