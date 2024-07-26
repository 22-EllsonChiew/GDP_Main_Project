using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private NeighbourDoor hakimDoor;
    [SerializeField]
    private NeighbourDoor sherrylDoor;

    public static DoorController instance;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleHakimDoor()
    {
        hakimDoor.ToggleDoor();
    }

    public void ToggleSherrylDoor()
    {
        sherrylDoor.ToggleDoor();
    }

}

