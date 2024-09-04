using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;
    public Sprite neighbourImageSprite;

    [Header("Happiness Variables")]
    public float maxHappiness;
    public float happinessRegenAmount;
    public float happinessRegenDuration;
    private float brokenPromisePenalty;  

    public TextAsset neighbourRoutinesJSON;
    public RoutineData routineArray;

    public float HappinessThreshold_Normal {  get; private set; }
    public float HappinessThreshold_Angry { get; private set; }
    public float ComplaintThreshold { get; private set; }
    public int ComplaintCount { get; private set; }
    public float CurrentHappiness { get; private set; }
    public DialogueType CurrentMood { get; private set; }
    public bool IsNeighbourInRoutine {  get; private set; }
    public bool HasBeenPromised {  get; private set; } = false;


    public NeighbourRoutines currentRoutine {  get; private set; }
    public NeighbourRoutines upcomingRoutine { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        CurrentHappiness = maxHappiness;
        HappinessThreshold_Normal = maxHappiness * 0.65f;
        HappinessThreshold_Angry = maxHappiness * 0.35f;
        brokenPromisePenalty = maxHappiness * 0.2f;

        ComplaintThreshold = HappinessThreshold_Normal;
        ComplaintCount = 0;

        routineArray = JsonConvert.DeserializeObject<RoutineData>(neighbourRoutinesJSON.text);
        neighbourTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // check if neighbour has a routine at specified time
        CheckNeighbourRoutines();
        CalculateCurrentMood();

    }

    public void EscalateNeighbourComplaint()
    {
        if (ComplaintCount < 1)
        {
            ComplaintCount++;
            ComplaintThreshold = HappinessThreshold_Angry;
            Debug.Log("neighbour has made a complaint - less forgiving to player");
        }
        else if (ComplaintCount < 2)
        {
            ComplaintCount++;
            ComplaintThreshold = 0f;
            Debug.Log("neighbour has made final warning - will no longer make informal complaint");
        }
        else
        {
            Debug.Log("neighbour no longer wants to complain");
        }
    }

    public void ReduceHappiness(float amount)
    {
        CurrentHappiness -= amount;
        CurrentHappiness = Mathf.Clamp(CurrentHappiness, 0, maxHappiness);
    }

    public void MakePromise()
    {
        HasBeenPromised = true;
        StartCoroutine(RegenerateHappinessOverTime());
        Debug.Log("Promise made with neighbour - regenerating happiness");
    }

    public void BreakPromise()
    {
        StopAllCoroutines();
        HasBeenPromised = false;
        ReduceHappiness(brokenPromisePenalty);
        Debug.Log("Promise broken with neighbour - Neighbour severely angered");
    }

    void CheckNeighbourRoutines()
    {
        int currentHour = TimeController.Hour;
        int currentMinute = TimeController.Minute;
        bool foundCurrentRoutine = false;

        // reset upcoming routine only when that specific routine has been completed
        if (upcomingRoutine != null && (currentHour >= upcomingRoutine.routineEndHour && currentMinute >= upcomingRoutine.routineEndMinute))
        {
            upcomingRoutine = null;
        }
        
        foreach (var routine in routineArray.routines)
        {
            if (routine.day == TimeController.CurrentDay)
            {
                // Check if the routine is currently active
                if ((routine.routineStartHour < currentHour || (routine.routineStartHour == currentHour && routine.routineStartMinute <= currentMinute)) &&
                    (routine.routineEndHour > currentHour || (routine.routineEndHour == currentHour && routine.routineEndMinute >= currentMinute)))
                {
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
            IsNeighbourInRoutine = false;
            currentRoutine = null;
        }
    }

    void CalculateCurrentMood()
    {
        if (CurrentHappiness <= HappinessThreshold_Angry)
        {
            CurrentMood = DialogueType.Mood_Angry;
        }
        else if (CurrentHappiness <= HappinessThreshold_Normal)
        {
            CurrentMood = DialogueType.Mood_Normal;
        }
        else 
        {
            CurrentMood = DialogueType.Mood_Happy;
        }
    }

    IEnumerator RegenerateHappinessOverTime()
    {
        float regenAmount = happinessRegenAmount / happinessRegenDuration;
        float elapsedTime = 0f;

        while (elapsedTime < happinessRegenDuration)
        {
            CurrentHappiness += regenAmount * Time.deltaTime;
            CurrentHappiness = Mathf.Clamp(CurrentHappiness, 0, maxHappiness);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
