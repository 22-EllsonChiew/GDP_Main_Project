using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    private Image cursorImage;

    // Start is called before the first frame update
    void Start()
    {
        cursorImage = GetComponent<Image>();
        cursorImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePostion = Input.mousePosition;
        cursorImage.transform.position = mousePostion;
    }

    public void ShowCursor() { cursorImage.enabled = true; }
    public void HideCursor() { cursorImage.enabled = false; }

}
