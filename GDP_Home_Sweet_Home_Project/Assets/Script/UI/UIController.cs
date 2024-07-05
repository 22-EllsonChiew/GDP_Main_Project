using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PhoneApp 
{
    // all phone app/screen states
    Stowed,
    Home,
    Chat,
    Net,
    Settings,
    Hamburger
}


public class UIController : MonoBehaviour
{
    // Y values for rectTransform
    public float upperY = 0f;
    public float lowerY = -800f;
    public float moveDuration = 1f;

    [SerializeField]
    private RectTransform phoneTransform;
    private Vector2 targetPos;
    private bool isMoving;

    [SerializeField]
    private Button homeBtn;
    [SerializeField]
    private Button chatBtn;
    [SerializeField] 
    private Button netBtn;


    [SerializeField]
    private GameObject homeScreen;
    [SerializeField]
    private GameObject chatScreen;
    [SerializeField]
    private GameObject netScreen;

    private Dictionary<PhoneApp, GameObject> appScreens;

    public bool isPhoneActive { get; private set; }
    public PhoneApp currentApp {  get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        isPhoneActive = false;
        targetPos = phoneTransform.anchoredPosition;
        currentApp = PhoneApp.Home;

        homeBtn.onClick.AddListener(() => OpenApp(PhoneApp.Home));
        chatBtn.onClick.AddListener(() => OpenApp(PhoneApp.Chat));
        netBtn.onClick.AddListener(() => OpenApp(PhoneApp.Net));

        appScreens = new Dictionary<PhoneApp, GameObject>()
        {
            {PhoneApp.Home, homeScreen},
            {PhoneApp.Chat, chatScreen},
            {PhoneApp.Net, netScreen},
        };

        //foreach (var screen in appScreens.Values)
        //{
        //    screen.SetActive(false);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isMoving) 
        {
           TogglePhone();
        }
    }

    public void TogglePhone()
    {
        if (phoneTransform.anchoredPosition.y == lowerY)
        {
            targetPos = new Vector2(phoneTransform.anchoredPosition.x, upperY);
        }
        else
        {
            targetPos = new Vector2(phoneTransform.anchoredPosition.x, lowerY);
        }
        StartCoroutine(MovePhone(targetPos));
    }

    public void OpenApp(PhoneApp app)
    {
        if (currentApp != app)
        {
            if (currentApp != PhoneApp.Stowed && appScreens.ContainsKey(currentApp))
            {
                appScreens[currentApp].SetActive(false);
            }

            if (appScreens.ContainsKey(app))
            {
                appScreens[app].SetActive(true);
            }

            currentApp = app;
            Debug.Log("Opened app: " + app.ToString());
        }
    }

    private IEnumerator MovePhone(Vector2 target)
    {
        isPhoneActive = true;
        isMoving = true;
        Vector2 startPos = phoneTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            phoneTransform.anchoredPosition = Vector2.Lerp(startPos, target, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;    
        }

        phoneTransform.anchoredPosition = target;
        isPhoneActive = true;
        isMoving = false;

    }

}
