using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    GameObject LoadingScreenObj;
    


   
   IEnumerator LoadSceneAsync()
    {
        LoadingScreenObj.SetActive(true);

        yield return new WaitForSeconds(10);

        LoadingScreenObj.SetActive(false);
    }
}
