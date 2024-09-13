using System.Collections;
using UnityEngine;

public class ShelfMiniGameManager : MonoBehaviour
{
    public static ShelfMiniGameManager Instance;

    public Camera gameCamera;
    public Camera drillCamera;
    public Camera hammerCamera;
    public Camera tableCamera;
    public Transform camera2Pos;
    public Transform drillCamera2Pos;
    public Transform hammerCamera2Pos;
    public Transform tableCamera2Pos;
    public Interaction interaction;
    private int mountCount = 0;

    public Camera hammerCamera2;
    public Transform hammerCamera2Pos2;
    public Camera hammerCamera3;
    public Camera hammerCamera4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckCamera();
    }

    public void IncrementMountCount()
    {
        mountCount++;
        Debug.Log("MOUNT COUNT =" + mountCount);
        if (mountCount == 2)
        {
            Debug.Log("LERPING");
            StartCoroutine(LerpCamera(gameCamera.transform.position, camera2Pos.transform.position, gameCamera.transform.rotation, camera2Pos.transform.rotation, 2f));
            mountCount = 0;
        }
    }

    private IEnumerator LerpCamera(Vector3 start, Vector3 end, Quaternion startRotation, Quaternion endRotation, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            gameCamera.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            gameCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            //lerp rotation
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        gameCamera.transform.position = end;
        gameCamera.transform.rotation = endRotation;
    }
    private void FindNearestCameraPosition()
    {
        GameObject[] positions = GameObject.FindGameObjectsWithTag("MinigameCamPos");
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (GameObject position in positions)
        {
            float distance = Vector3.Distance(transform.position, position.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = position.transform;
            }
        }

        if (closestTransform != null)
        {
            camera2Pos = closestTransform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'MinigameCamPos' found.");
        }
    }

    public void CheckCamera()
    {
        if (interaction.hammerGame == true)
        {
            gameCamera = hammerCamera;
            camera2Pos = hammerCamera2Pos;
        }

        if (interaction.drillGame == true)
        {
            gameCamera = drillCamera;
            camera2Pos = drillCamera2Pos;
        }

        if (interaction.tableDrilling == true)
        {
            gameCamera = tableCamera;
            camera2Pos = tableCamera2Pos;
        }

        if (interaction.hammerGame2 == true)
        {
            gameCamera = hammerCamera2;
            camera2Pos = hammerCamera2Pos2;
        }

        if (interaction.hammerGame3 == true)
        {
            gameCamera = hammerCamera3;
        }

        if (interaction.hammerGame4 == true)
        {
            gameCamera = hammerCamera4;
        }
    }
}
