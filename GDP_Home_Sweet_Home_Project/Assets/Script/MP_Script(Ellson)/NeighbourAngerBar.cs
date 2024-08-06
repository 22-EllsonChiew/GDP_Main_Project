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
        UpdateNeighbourHappinessBar(amount);
        CheckForSherrylComplainMessage();
        CheckForHakimComplainMessage();

        
        

        // If no complaints were received, log that neighbors are happy
        if (!complaintMessage.hakimSentedComplaint && !complaintMessage.sherrylSentedComplaint)
        {
            Debug.Log("Neighbours are happy.");
        }

        //Check neighbour happiness value
        //complaintMessage.CheckNeighbourHappinessValue();
    }

    private void UpdateNeighbourHappinessBar(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);
        angerBarManager.UpdateHappinessBar();

    }

    private void CheckForSherrylComplainMessage()
    {
        //check if player is in sherryl collider side and check if the sherryl current happiness less then the threshold from ChatManger script and check if sherryl has send complain to the player
        if (complaintMessage.neighbourSherryl.CheckPlayerInColliderSherryl() && complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.noiseThresholdSherryl && !complaintMessage.sherrylSentedComplaint)
        {
            //send complaint message and set sherryl send complaint to true so it will not send or dup another message
            // Send complaint from Sherryl
            complaintMessage.ReceiveComplaint("Sherryl");
            complaintMessage.sherrylSentedComplaint = true;
        }
        else if(complaintMessage.neighbourSherryl.CheckPlayerInColliderSherryl() && complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.policeCallThresholdSherryl && complaintMessage.PlayerRepliedToNeighbour("Sherryl") && !complaintMessage.sherrylCallPolice)
        {
            complaintMessage.ReceiveComplaint("Sherryl");
            complaintMessage.sherrylCallPolice = true;
        }
    }

    private void CheckForHakimComplainMessage()
    {
        //check if player is in hakim collider side and check if the hakim current happiness less then the threshold from ChatManger script and check if hakim has send complain to the player
        if (complaintMessage.neighbourHakim.CheckPlayerInColliderHakim() && complaintMessage.neighbourHakim.currentHappiness < complaintMessage.noiseThreshold && !complaintMessage.hakimSentedComplaint)
        {
            //send complaint message and set hakim send complaint to true so it will not send or dup another message
            // Send complaint from Hakim
            complaintMessage.ReceiveComplaint("Hakim");
            complaintMessage.hakimSentedComplaint = true;
        }
        else if(complaintMessage.neighbourHakim.CheckPlayerInColliderHakim() && complaintMessage.neighbourHakim.currentHappiness < complaintMessage.policeCallThresholdHakim && complaintMessage.PlayerRepliedToNeighbour("Hakim") && !complaintMessage.hakimCallPolice)
        {
            complaintMessage.ReceiveComplaint("Hakim");
            complaintMessage.hakimCallPolice = true;
        }
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
