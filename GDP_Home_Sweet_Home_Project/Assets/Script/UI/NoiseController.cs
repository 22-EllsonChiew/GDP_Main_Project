using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    [Header("Noise Parameters")]
    private float currentNoise;
    private float noiseThreshold = 1f;
    public float noiseDecreaseRate;



    [Header("UI Elements")]
    public Slider noiseBar;
    public Image noiseColourFill;
    public Gradient noiseGradient;

    public static NoiseController instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();

        if (Input.GetKeyUp(KeyCode.E)) 
        {
            MakeNoise(0.35f);
        }
    }

    void UpdateBar()
    {
        currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

        noiseBar.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

        noiseColourFill.color = noiseGradient.Evaluate(currentNoise);
    }

    void HandleNoise()
    {
        if (currentNoise > 0.65f)
        {
            // interact with neighbour logic
        }
    }

    public void MakeNoise(float noise)
    {
        currentNoise = Mathf.Min(currentNoise + noise, noiseThreshold);
    }
}
