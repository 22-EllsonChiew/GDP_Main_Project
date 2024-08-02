using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{

    public float maxHappyBar = 100f;
    public float currentHappiness;

    public AngerBarManager angerBarManager;

    public ChatManager complaintMessage;

    public BoxCollider sherrylSideCollider;
    public BoxCollider hakimSideCollider;
    public Transform player;

    private bool hakimComplained = false;
    private bool sherrylComplained = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHappiness = maxHappyBar;
    }

    public void HeardNoise(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);

        angerBarManager.UpdateHappinessBar();

        if (complaintMessage.neighbourHakim.CheckPlayerInColliderHakim() && complaintMessage.neighbourHakim.currentHappiness < complaintMessage.noiseThreshold)
        {
            //Debug.Log($"Hakim's happiness: {complaintMessage.neighbourHakim.currentHappiness}, threshold: {complaintMessage.noiseThreshold}");
            complaintMessage.ReceiveComplaint("Hakim");
            hakimComplained = true;


        }
        else if (complaintMessage.neighbourSherryl.CheckPlayerInColliderSherryl() && complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.noiseThreshold && !hakimComplained)
        {
            //Debug.Log($"Sherryl's happiness: {complaintMessage.neighbourSherryl.currentHappiness}, threshold: {complaintMessage.noiseThreshold}");
            complaintMessage.ReceiveComplaint("Sherryl");
            sherrylComplained = true;
        }

        // Only send a complaint message to the other NPC if the first NPC didn't complain
        if (!hakimComplained && complaintMessage.neighbourHakim.CheckPlayerInColliderHakim() && complaintMessage.neighbourHakim.currentHappiness < complaintMessage.noiseThreshold)
        {
            complaintMessage.ReceiveComplaint("Hakim");
        }
        else if (!sherrylComplained && complaintMessage.neighbourSherryl.CheckPlayerInColliderSherryl() && complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.noiseThreshold)
        {
            complaintMessage.ReceiveComplaint("Sherryl");
        }
        else
        {
            Debug.Log("Neighbour Happy");
        }



        complaintMessage.CheckNeighbourHappinessValue();
    }

    /*public void RestoreComplaint(float amount)
    {
        currentHappiness += amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);

        angerBarManager.UpdateHappinessBar();

        if(currentHappiness >= complaintMessage.noiseThreshold || currentHappiness >= complaintMessage.noiseThresholdSherryl)
        {
            complaintMessage.ResetComplaint();
        }
    }*/

    public bool CheckPlayerInColliderSherryl()
    {
        return sherrylSideCollider.bounds.Contains(player.transform.position);
    }

    public bool CheckPlayerInColliderHakim()
    {
        return hakimSideCollider.bounds.Contains(player.transform.position);
    }
}
