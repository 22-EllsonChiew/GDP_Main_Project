using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{

    public float maxHappyBar = 100f;
    public float currentHappiness;

    public AngerBarManager angerBarManager;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHappiness = maxHappyBar;
    }

    public void HeardNoise(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);

        angerBarManager.UpdateHappinessBar();
    }
}
