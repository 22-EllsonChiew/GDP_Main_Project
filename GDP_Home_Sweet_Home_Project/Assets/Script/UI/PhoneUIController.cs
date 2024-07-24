using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PhoneApp 
{
    // all phone app/screen states
    Stowed,
    Home,
    ChatApp,
    ChatApp_Messages,
    NetApp,
    NetApp_Post,
    Settings,
    Hamburger
}


public class PhoneUIController : MonoBehaviour
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
    private Button backBtn;
    


    [SerializeField]
    private GameObject homeScreen;
    [SerializeField]
    private GameObject chatApp;
    [SerializeField]
    private GameObject chatApp_Messages;
    [SerializeField]
    private GameObject netApp;
    [SerializeField]
    private GameObject netApp_Post;

    private Dictionary<PhoneApp, GameObject> appScreens;
    private Stack<PhoneApp> navigationHistory;

    public bool isPhoneActive { get; private set; }
    public PhoneApp currentApp {  get; private set; }

    public static PhoneUIController instance;

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

        isPhoneActive = false;
        targetPos = phoneTransform.anchoredPosition;
        currentApp = PhoneApp.Stowed;

        homeBtn.onClick.AddListener(() => OpenApp(PhoneApp.Home));
        chatBtn.onClick.AddListener(() => OpenApp(PhoneApp.ChatApp));
        netBtn.onClick.AddListener(() => OpenApp(PhoneApp.NetApp));
        backBtn.onClick.AddListener(BackBtn);

        appScreens = new Dictionary<PhoneApp, GameObject>()
        {
            {PhoneApp.Home, homeScreen},
            {PhoneApp.ChatApp, chatApp},
            {PhoneApp.ChatApp_Messages, chatApp_Messages},
            {PhoneApp.NetApp, netApp},
            {PhoneApp.NetApp_Post, netApp_Post}
        };
        
        navigationHistory = new Stack<PhoneApp>();

        foreach (var screen in appScreens.Values)
        {
            screen.SetActive(false);
        }

        OpenApp(PhoneApp.Home);

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
            if (currentApp != PhoneApp.Stowed)
            {
                navigationHistory.Push(currentApp);
            }

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

    public void OpenMessages()
    {
        OpenApp(PhoneApp.ChatApp_Messages);
        Debug.Log("Opened messages");
    }

    public void OpenNetPost()
    {
        OpenApp(PhoneApp.NetApp_Post);
        Debug.Log("Opened forum post");
    }

    public void BackBtn()
    {
        if (navigationHistory.Count > 0)
        {
            if (currentApp != PhoneApp.Stowed && appScreens.ContainsKey(currentApp))
            {
                appScreens[currentApp].SetActive(false);
            }

            currentApp = navigationHistory.Pop();

            if (currentApp != PhoneApp.Stowed && appScreens.ContainsKey(currentApp))
            {
                appScreens[currentApp].SetActive(true);
            }

            Debug.Log("Returned to " + currentApp.ToString());
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
