using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Neighbour angeredNeighbour { get; private set; }

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
        Debug.Log("Angered neighbour: " + angeredNeighbour);
    }
}
