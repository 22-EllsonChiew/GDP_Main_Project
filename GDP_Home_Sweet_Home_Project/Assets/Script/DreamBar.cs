using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DreamBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;



    // Start is called before the first frame update
    void Start()
    {
        //GameObject[] totalTasks = GameObject.FindGameObjectsWithTag("Object");
        maximum = 10;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();

        if (current == maximum)
        {
            SceneManager.LoadScene("Win Scene");
        }

    }

    public void OnTaskCompletion(bool isCompleted)
    {
        if (isCompleted)
        {
            current += 1;
        }
    }

    //replace above function with onclick, if clicked will increment current by +1
    //if clicked again will -1


    void GetCurrentFill()
    {
        float amtToFill = (float)current / (float)maximum;
        mask.fillAmount = amtToFill;
    }


}
