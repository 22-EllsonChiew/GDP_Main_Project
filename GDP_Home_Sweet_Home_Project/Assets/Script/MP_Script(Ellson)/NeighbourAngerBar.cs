using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{
    // work on this
    //public float maxHappyBar = 100f;
    //public float currentHappiness;

    public AngerBarManager angerBarManager;


    private Neighbour neighbour;
    public BoxCollider soundCollider;

    public Transform player;


    // Start is called before the first frame update
    void Start()
    {
        neighbour = GetComponent<Neighbour>();
    }

    public void HeardNoise(float amount)
    {

        if (CheckPlayerInCollider())
        {

            if (neighbour.currentRoutine != null && neighbour.currentRoutine.routineType == RoutineType.NotHome)
            {
                Debug.Log("Neighbour is not home");
                return;
            }
            
            if (neighbour.HasBeenPromised)
            {
                neighbour.BreakPromise();
            }

            UpdateNeighbourHappinessBar(amount);

            if (neighbour.currentHappiness <= neighbour.complaintThreshold && neighbour.complaintCount < 2)
            {
                neighbour.EscalateNeighbourComplaint();
                ChatManager.instance.ReceiveComplaint(neighbour.neighbourName, DetermineComplaintType());
            }
            else
            {
                Debug.Log("neighbour disturbed");
            }
        }
    }

    private void UpdateNeighbourHappinessBar(float amount)
    {
        neighbour.ReduceHappiness(amount);
        angerBarManager.UpdateHappinessBar();

    }

    DialogueType DetermineComplaintType()
    {
        if (neighbour.currentHappiness <= neighbour.happinessThreshold_Angry)
        {
            return DialogueType.Complaint_Angry;
        }
        else
        {
            return DialogueType.Complaint_Normal;
        }
    }

    public bool CheckPlayerInCollider()
    {
        return soundCollider.bounds.Contains(player.transform.position);
    }

}
