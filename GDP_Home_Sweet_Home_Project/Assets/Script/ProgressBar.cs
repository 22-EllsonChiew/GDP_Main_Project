using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;

    [SerializeField]
    private HammerMinigame taskDetection;

    // Start is called before the first frame update
    void Start()
    {
        taskDetection.OnTaskComplete += OnTaskCompletion;
    }

    private void TaskDetection_OnTaskComplete(bool isTaskComplete)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();

        if (current == 5)
        {
            SceneManager.LoadScene("Win Scene");
        }

    }

    private void OnTaskCompletion(bool isCompleted)
    {
        if (isCompleted)
        {
            current += 1;
        }
    }


    void GetCurrentFill()
    {
        float amtToFill = (float) current / (float) maximum;
        mask.fillAmount = amtToFill;
    }

    
}
