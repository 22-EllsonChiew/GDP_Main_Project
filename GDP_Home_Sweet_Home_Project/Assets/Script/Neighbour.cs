using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;

    public float maxHappiness;
    public TextAsset neighbourRoutinesJSON;
    public RoutineData routineArray;

    public float happinessThreshold_Normal {  get; private set; }
    public float happinessThreshold_Angry { get; private set; }
    public float complaintThreshold { get; private set; }
    public int complaintCount { get; private set; }
    public float currentHappiness { get; private set; }
    public DialogueType currentMood { get; private set; }
    public bool IsNeighbourInRoutine {  get; private set; }


    
    public NeighbourRoutines currentRoutine {  get; private set; }
    public NeighbourRoutines upcomingRoutine { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        currentHappiness = maxHappiness;
        happinessThreshold_Normal = maxHappiness * 0.65f;
        happinessThreshold_Angry = maxHappiness * 0.35f;

        complaintThreshold = happinessThreshold_Normal;
        complaintCount = 0;

        routineArray = JsonConvert.DeserializeObject<RoutineData>(neighbourRoutinesJSON.text);
        neighbourTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Neighbour Routine - " + neighbourName + IsNeighbourInRoutine);

        // check if neighbour has a routine at specified time
        CheckNeighbourRoutines();
        CalculateCurrentMood();

    }

    public void EscalateNeighbourComplaint()
    {
        if (complaintCount < 1)
        {
            complaintCount++;
            complaintThreshold = happinessThreshold_Angry;
            Debug.Log("neighbour has made a complaint - less forgiving to player");
        }
        else if (complaintCount < 2)
        {
            complaintCount++;
            complaintThreshold = 0f;
            Debug.Log("neighbour has made final warning - will no longer make informal complaint");
        }
        else
        {
            Debug.Log("neighbour no longer wants to complain");
        }
    }

    public void ReduceHappiness(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappiness);
    }

    void CheckNeighbourRoutines()
    {
        int currentHour = TimeController.Hour;
        int currentMinute = TimeController.Minute;
        bool foundCurrentRoutine = false;

        foreach (var routine in routineArray.routines)
        {
            if (routine.day == TimeController.CurrentDay)
            {
                // Check if the routine is currently active
                if ((routine.routineStartHour < currentHour || (routine.routineStartHour == currentHour && routine.routineStartMinute <= currentMinute)) &&
                    (routine.routineEndHour > currentHour || (routine.routineEndHour == currentHour && routine.routineEndMinute >= currentMinute)))
                {
                    Debug.Log("Neighbour started routine");
                    IsNeighbourInRoutine = true;
                    currentRoutine = routine;
                    foundCurrentRoutine = true;
                    break; // No need to check further once we find the current routine
                }
                else if (routine.routineStartHour > currentHour || (routine.routineStartHour == currentHour && routine.routineStartMinute > currentMinute))
                {
                    // Routine starts in the future, so set it as the upcoming routine if none is already set
                    if (upcomingRoutine == null || routine.routineStartHour < upcomingRoutine.routineStartHour ||
                        (routine.routineStartHour == upcomingRoutine.routineStartHour && routine.routineStartMinute < upcomingRoutine.routineStartMinute))
                    {
                        upcomingRoutine = routine;
                    }
                }
            }
        }

        if (!foundCurrentRoutine)
        {
            Debug.Log("Neighbour ended routine");
            IsNeighbourInRoutine = false;
            currentRoutine = null;
        }
    }

    void CalculateCurrentMood()
    {
        if (currentHappiness <= happinessThreshold_Angry)
        {
            currentMood = DialogueType.Mood_Angry;
        }
        else if (currentHappiness <= happinessThreshold_Normal)
        {
            currentMood = DialogueType.Mood_Normal;
        }
        else 
        {
            currentMood = DialogueType.Mood_Happy;
        }
    }

}
