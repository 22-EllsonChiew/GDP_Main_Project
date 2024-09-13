using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseSceneManager : MonoBehaviour
{

    private ScoreManager scoreManager;

    public TextMeshProUGUI angeredNeighbourName;
    public Image angeredNeighbourPhoto;

    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            scoreManager = ScoreManager.Instance;
        }
        else
        {
            Debug.LogWarning("Score Manager not found - Is this intentional?");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSceneUI();
    }

    void UpdateSceneUI()
    {
        if (scoreManager != null)
        {
            angeredNeighbourName.text = scoreManager.AngeredNeighbour.neighbourName;
            if (scoreManager.AngeredNeighbour.neighbourImageSprite != null)
            {
                angeredNeighbourPhoto.sprite = scoreManager.AngeredNeighbour.neighbourImageSprite;
            }
            else
            {
                Debug.LogWarning("No neighbour photo found!");
            }
            
        }
    }

    public void MainMenuBtn()
    {
        Debug.Log("Main Menu pressed - Returning to main menu");
        SceneManager.LoadScene("Menu");
    }

    public void RetryBtn()
    {
        Debug.Log("Retry pressed - Restarting game scene");
        // put scene transition to game scene
    }
}
