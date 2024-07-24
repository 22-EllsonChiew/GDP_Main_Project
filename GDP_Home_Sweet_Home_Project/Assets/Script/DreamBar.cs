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
    public GameObject tick;
    public Toggle checkbox;
    //list of checkboxes for multiple tasks
    public List<Toggle> checkboxes; 
    //dictionary to keep track of instantiated tick images
    private Dictionary<Toggle, GameObject> tickImages;

    // Start is called before the first frame update
    void Start()
    {
        maximum = 10;
        current = 0;
        //initialize the dictionary
        tickImages = new Dictionary<Toggle, GameObject>();
        //listeners for each checkbox
        foreach (Toggle checkbox in checkboxes)
        {
            checkbox.onValueChanged.AddListener(delegate { OnCheckboxValueChanged(checkbox); });
        }
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

    //public void OnTaskCompletion(bool isCompleted)
    //{
    //    if (isCompleted)
    //    {
    //        current += 1;
    //    }
    //}

    public void OnCheckboxValueChanged(Toggle changedCheckbox)
    {
        if (changedCheckbox.isOn)
        {
            current += 1;
            //instantiate tick image within the checkbox
            GameObject tickImage = Instantiate(tick, changedCheckbox.transform);
            //enable tick image so that it is visible
            tickImage.SetActive(true);
            //store reference in the dictionary
            tickImages[changedCheckbox] = tickImage; 
        }
        else
        {
            current -= 1;
            //call fucntion to remove the tick image from the checkbox
            DestroyTickImage(changedCheckbox); 
        }
        if(current == 2)
        {
            SceneManager.LoadScene("Main Game 1");
        }
    }

    void DestroyTickImage(Toggle changedCheckbox)
    {
        if (tickImages.TryGetValue(changedCheckbox, out GameObject tickImage))
        {
            //destroy the tick image
            Destroy(tickImage);
            //remove reference from the dictionary
            tickImages.Remove(changedCheckbox); 
        }
    }


    void GetCurrentFill()
    {
        float amtToFill = (float)current / (float)maximum;
        mask.fillAmount = amtToFill;
    }


}
