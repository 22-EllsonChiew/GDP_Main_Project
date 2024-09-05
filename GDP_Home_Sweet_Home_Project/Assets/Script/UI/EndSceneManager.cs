using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    private ScoreManager scoreManager;

    [Header("Grading Modifiers")]
    public float gradePenaltyPerPromiseBroken = 0.05f;
    public float gradePenaltyPerComplaint = 0.02f;

    [Header("UI References")]
    [Header("Player Grade UI")]
    public TextMeshProUGUI playerPerformanceGrade;
    public TextMeshProUGUI playerPerformanceSubtext;
    [Header("Statistic UI")]
    public Image mostAngeredNeighbourImage;
    public TextMeshProUGUI totalComplaintsStatistic;
    public TextMeshProUGUI promisesMadeStatistic;
    public TextMeshProUGUI promisesBrokenStatistic;

    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            scoreManager = ScoreManager.Instance;
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager != null) 
        {
            CalculatePlayerGrade();
            PopulateStatisticTable();
        }

    }

    // functions for button presses
    public void RestartGameBtn()
    {
        Debug.Log("Restart pressed - Restarting game scene");
        SceneManager.LoadScene("Main Game");
    }

    public void MainMenuBtn()
    {
        Debug.Log("Main Menu pressed - Returning to main menu");
        SceneManager.LoadScene("Menu");
    }

    void CalculatePlayerGrade()
    {
        // get/set variables from ScoreManager
        float rawGradePercentage = scoreManager.TotalBuiltFurniture / scoreManager.TotalFurnitureCount;
        int totalComplaints = scoreManager.TotalComplaintCount;
        int promisesBroken = scoreManager.PromisesBroken;

        // calculate any grade penalites
        float brokenPromisePenalties = promisesBroken * gradePenaltyPerPromiseBroken;
        float complaintPenalties = totalComplaints * gradePenaltyPerComplaint;

        float finalGradePercentage;

        // calculate final grade percentage
        if (rawGradePercentage <= 0.5f)
        {
            finalGradePercentage = 0.5f;
        }
        else
        {
            finalGradePercentage = rawGradePercentage - brokenPromisePenalties - complaintPenalties;
            finalGradePercentage = Mathf.Clamp(finalGradePercentage, 0f, 1f);
        }

        string grade;
        string gradeSubtext;

        switch (finalGradePercentage)
        {
            case >= 0.9f:
                grade = "A+";
                gradeSubtext = "You really are a great neighbour! Keep up the good work!";
                break;

            case >= 0.75f:
                grade = "B";
                gradeSubtext = "Not too shabby! You'll be a great neighbour in no time!";
                break;

            case > 0.5f:
                grade = "C";
                gradeSubtext = "A decent enough job. Maybe try and be more considerate next time.";
                break;

            default:
                grade = "D";
                gradeSubtext = "Well, at least... They aren't mad at you?";
                break;

        }

        // update screen UI
        playerPerformanceGrade.text = grade;
        playerPerformanceSubtext.text = gradeSubtext;
    }

    // update player statistics based on scoremanager values
    void PopulateStatisticTable()
    {
        if (scoreManager.AngriestNeighbour != null) 
        {
            mostAngeredNeighbourImage.sprite = scoreManager.AngriestNeighbour.neighbourImageSprite;
        }
        else
        {
            Debug.LogWarning("No neighbour data detected!");
        }

        totalComplaintsStatistic.text = scoreManager.TotalComplaintCount.ToString();
        promisesMadeStatistic.text = scoreManager.PromisesMade.ToString();
        promisesBrokenStatistic.text = scoreManager.PromisesBroken.ToString();
    }

}
