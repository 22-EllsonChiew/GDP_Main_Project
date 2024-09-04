using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int totalFurnitureCount = 17;

    public Neighbour AngeredNeighbour { get; private set; }
    public Neighbour AngriestNeighbour { get; private set; }
    public int TotalComplaintCount { get; private set; } = 0;
    public int TotalBuiltFurniture { get; private set; } = 0;
    public int PromisesMade { get; private set; } = 0;
    public int PromisesBroken { get; private set; } = 0;

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
        AngeredNeighbour = neighbour;
        Debug.Log("Angered neighbour: " + AngeredNeighbour.neighbourName);
    }

    public void IncreaseComplaintCount()
    {
        TotalComplaintCount++;
        Debug.Log("ScoreManager - Complaint added");
    }

    public void IncrementTotalFunitureCount()
    {
        TotalBuiltFurniture++;
        Debug.Log("ScoreManager - Furniture built");
    }

    public void IncrementPromiseTotal()
    {
        PromisesMade++;
        Debug.Log("ScoreManager - Promise made");
    }

    public void IncrementBrokenPromises()
    {
        PromisesBroken++;
        Debug.Log("ScoreManager - Promise broken");
    }
}
