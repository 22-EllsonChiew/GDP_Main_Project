using System.Collections;
using UnityEngine;

public class ShelfMiniGameManager : MonoBehaviour
{
    public static ShelfMiniGameManager Instance;

    public Camera gameCamera;
    public Transform camera2Pos;
    private int mountCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementMountCount()
    {
        mountCount++;
        if (mountCount == 2)
        {
            Debug.Log("LERPING");
            StartCoroutine(LerpCamera(gameCamera.transform.position, camera2Pos.transform.position, gameCamera.transform.rotation, camera2Pos.transform.rotation, 2f));
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
        gameCamera.transform.position = end;
        gameCamera.transform.rotation = endRotation;
    }
}
