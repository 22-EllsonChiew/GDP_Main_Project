using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingWall : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject bombShelterWall;
    public GameObject bedRoomWall;
    public GameObject bedRoomWall2;
    public GameObject toiletWall;
    public GameObject SecondFloor;
    public float fadeDura = 2.0f;

    [Header("Mats")]
    private Renderer wallRender;
    private Material[] wallMaterials;
    private Color[] originalColour;
    private float fadeTime;

    private void Start()
    {
        SecondFloor.SetActive(false);

        if (bombShelterWall != null)
        {
            wallRender = bombShelterWall.GetComponent<Renderer>();
            wallRender = bedRoomWall.GetComponent<Renderer>();
            wallRender = bedRoomWall2.GetComponent<Renderer>();
            wallRender = toiletWall.GetComponent<Renderer>();
            wallMaterials = wallRender.materials;
            originalColour = new Color[wallMaterials.Length];
            for (int i = 0; i < wallMaterials.Length; i++)
            {
                originalColour[i] = wallMaterials[i].color;
            }
            fadeTime = 3f;
        }
        else
        {
            Debug.Log("No Wall");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wallRender != null && other.CompareTag("Player"))
        {
            
            fadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, fadeTime / fadeDura);

            for (int i = 0; i < wallMaterials.Length; i++)
            {
                Color newColor = new Color(originalColour[i].r, originalColour[i].g, originalColour[i].b, alpha);
                wallMaterials[i].color = newColor;
            }

            if (fadeTime >= fadeDura)
            {
                // Set the wall to inactive or do something else when the fade is complete
                Debug.Log("Im Faded");
                bombShelterWall.SetActive(false);
                bedRoomWall.SetActive(false);
                bedRoomWall2.SetActive(false);
                toiletWall.SetActive(false);
                SecondFloor.SetActive(false);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (wallRender != null && other.CompareTag("Player"))
        {
            bombShelterWall.SetActive(true);
            bedRoomWall.SetActive(true);
            bedRoomWall2.SetActive(true);
            toiletWall.SetActive(true);
            SecondFloor.SetActive(true);
            fadeTime = 3f;
            for (int i = 0; i < wallMaterials.Length; i++)
            {
                wallMaterials[i].color = originalColour[i];
            }
        }


    }
}
