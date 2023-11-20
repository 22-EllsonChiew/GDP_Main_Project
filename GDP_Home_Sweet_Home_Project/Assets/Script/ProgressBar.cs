using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;

    [SerializeField]
    private Interaction taskDetection;

    // Start is called before the first frame update
    void Start()
    {
        taskDetection.OnTaskInteract += OnTaskCompletion;
    }

    private void TaskDetection_OnTaskComplete(bool isTaskComplete)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    private void OnTaskCompletion(bool isTaskComplete)
    {
        if (isTaskComplete)
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
