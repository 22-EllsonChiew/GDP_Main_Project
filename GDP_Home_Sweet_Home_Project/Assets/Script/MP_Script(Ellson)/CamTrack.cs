using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrack : MonoBehaviour
{

    public Transform camHolder;
    public Transform playerColi;
    public GameObject player;

    public BoxCollider SherrylNeighbourBox;
    public BoxCollider HakimNeighbourBox;

    public Transform SherrylCamHolder;
    public Transform HakimCamHolder;

    public float pLerp = .02f;
    public float rLerp = .04f;

    public float lerpDuration = 1f;

    public float onStayDuration = 10f;

    private bool playerInSherrylNeighbourBox;
    private bool playerInHakimNeighbourBox;

    private bool isLerping = false;

    private bool isInCamHolder = false;

    private Transform currentCamHolder;

    // Start is called before the first frame update
    void Start()
    {
       

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLerping)
        {
            //transform.position = Vector3.Lerp(transform.position, camHolder.position, pLerp);
            //transform.rotation = Quaternion.Lerp(transform.rotation, camHolder.rotation, rLerp);
            TrackPlayer();
        }

        playerInSherrylNeighbourBox = SherrylNeighbourBox.bounds.Contains(playerColi.position);
        playerInHakimNeighbourBox = HakimNeighbourBox.bounds.Contains(playerColi.position);

        if ((playerInSherrylNeighbourBox || playerInHakimNeighbourBox) && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInSherrylNeighbourBox)
            {
                StartCoroutine(LerpToTargetPosition(SherrylCamHolder));
                //player.SetActive(false);
            }
            else if (playerInHakimNeighbourBox)
            {
                StartCoroutine(LerpToTargetPosition(HakimCamHolder));
                //player.SetActive(false);
            }
        }

    }

    void TrackPlayer()
    {
        // Track the player
       // transform.position = Vector3.Lerp(transform.position, playerColi.position, pLerp * 0.5f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, playerColi.rotation, rLerp * 0.5f);

        // Lerp to the cam holder
        transform.position = Vector3.Lerp(transform.position, camHolder.position, pLerp * 0.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, camHolder.rotation, rLerp * 0.5f);
    }


    IEnumerator LerpToTargetPosition(Transform target)
    {
        isLerping = true;
        float timeElapsed = 0f;
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, target.position, timeElapsed / lerpDuration);
            transform.rotation = Quaternion.Lerp(initialRotation, target.rotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly the target's
        transform.position = target.position;
        transform.rotation = target.rotation;

        isLerping = false;
        currentCamHolder = target;

        yield return new WaitForSeconds(onStayDuration);

        isInCamHolder = false;

    }
}

