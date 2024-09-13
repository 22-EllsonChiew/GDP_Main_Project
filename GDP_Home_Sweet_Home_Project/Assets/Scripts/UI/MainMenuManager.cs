using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button replayButton;
    public GameObject tutorialPromptGroup;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        Button btn = replayButton.GetComponent<Button>();

    }

    private void Update()
    {
        
    }

    public void ToggleTutorialPrompt()
    {
        tutorialPromptGroup.SetActive(!tutorialPromptGroup.activeSelf);
    }


    public void LoadMainGameScene()
    {
        Debug.Log("Loading Main Game");
        SceneManager.LoadScene("Main Game");
    }

    public void LoadTutorialScene()
    {
        Debug.Log("Loading Tutorial Scene");
        SceneManager.LoadScene("Main Game_Tutorial");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Tutorial");

    }
}