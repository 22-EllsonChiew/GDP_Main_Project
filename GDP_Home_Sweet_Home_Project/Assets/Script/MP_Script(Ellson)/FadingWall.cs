using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingWall : MonoBehaviour
{

    public GameObject bombShelterWall;
    public GameObject bedRoomWall;
    public float fadeDura = 2.0f;
    private Renderer wallRender;
    private Material[] wallMaterials;
    private Color[] originalColour;
    private float fadeTime;

    private void Start()
    {
        if (bombShelterWall != null)
        {
            wallRender = bombShelterWall.GetComponent<Renderer>();
            wallRender = bedRoomWall.GetComponent<Renderer>();
            wallMaterials = wallRender.materials;
            originalColour = new Color[wallMaterials.Length];
            for (int i = 0; i < wallMaterials.Length; i++)
            {
                originalColour[i] = wallMaterials[i].color;
            }
            fadeTime = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
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
                bombShelterWall.SetActive(false);
                bedRoomWall.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (wallRender != null && other.CompareTag("Player"))
        {
            bombShelterWall.SetActive(true);
            bedRoomWall.SetActive(true);
            fadeTime = 0.0f;
            for (int i = 0; i < wallMaterials.Length; i++)
            {
                wallMaterials[i].color = originalColour[i];
            }
        }


    }
}
