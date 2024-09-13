using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string minigameSceneToLoad;
    public string previousSceneName;
    

    public static SceneTransition Instance;

    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            

            SceneManager.LoadScene("Main Game");
        }
    }


 
}



