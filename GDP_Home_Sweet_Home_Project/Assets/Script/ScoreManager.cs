using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Neighbour angeredNeighbour { get; private set; }

    public Neighbour happiestNeighbour { get; private set; }
    public Neighbour angriestNeighbour { get; private set; }
    public int totalComplaintCount { get; private set; } = 0;
    public int totalDisturbanceCount { get; private set; } = 0;
    public int totalBuiltFurniture { get; private set; } = 0;
    public int promisesMade { get; private set; } = 0;
    public int promisesBroken { get; private set; } = 0;

    public static ScoreManager Instance;

    // Start is called before the first frame update
    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAngeredNeighbour(Neighbour neighbour)
    {
        angeredNeighbour = neighbour;
        Debug.Log("Angered neighbour: " + angeredNeighbour.neighbourName);
    }

    public void IncreaseComplaintCount()
    {
        totalComplaintCount++;
        Debug.Log("ScoreManager - Complaint added");
    }

    public void IncreaseDisturbanceCount()
    {
        totalDisturbanceCount++;
        Debug.Log("ScoreManager - Neighbour disturbed");
    }

    public void IncrementTotalFunitureCount()
    {
        totalBuiltFurniture++;
        Debug.Log("ScoreManager - Furniture built");
    }

    public void IncrementPromiseTotal()
    {
        promisesMade++;
        Debug.Log("ScoreManager - Promise made");
    }

    public void IncrementBrokenPromises()
    {
        promisesBroken++;
        Debug.Log("ScoreManager - Promise broken");
    }
}
