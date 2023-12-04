using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public Button replayButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = replayButton.GetComponent<Button>();

        btn.onClick.AddListener(MainGameScene);
    }

    void MainGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
