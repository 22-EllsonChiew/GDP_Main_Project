using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string minigameSceneToLoad;
    public string previousSceneName;
    public GameObject objectToChange;
    public GameObject newModel;

    public static SceneTransition Instance;

    private void Start()
    {
       if(Instance == null)
       {
            Instance = this;
       }
       else if(Instance != this)
       {
            Destroy(this.gameObject);
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.LoadScene("NewMinigameTest");
        }
    }


 
}



