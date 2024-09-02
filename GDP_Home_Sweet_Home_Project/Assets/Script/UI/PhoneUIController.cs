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
    NotesApp,
    NotesApp_Note,
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

    [Header("Navigation Buttons")]
    [SerializeField]
    private Button homeBtn;
    [SerializeField]
    private Button chatBtn;
    [SerializeField] 
    private Button netBtn;
    [SerializeField] 
    private Button backBtn;


    [Header("App Screens")]
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
    

    [Header("Phone Wallpapers")]
    [SerializeField]
    private Sprite homeBG;
    [SerializeField]
    private Sprite chatAppBG;
    [SerializeField]
    private Sprite netAppBG;
    [SerializeField]
    private Image phoneBG;

    [Header("Notification Bar")]
    [SerializeField]
    private GameObject notificationBarSprites;
    [SerializeField]
    private GameObject notificationClock;
    [SerializeField]
    private GameObject notificationBell;
    [SerializeField]
    private GameObject chatNotification;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip sfx_PhoneNotification;
    [SerializeField] private AudioClip sfx_PhoneAppClick;
    [SerializeField] private AudioClip sfx_PhoneBack;

    private Dictionary<PhoneApp, GameObject> appScreens;
    private Stack<PhoneApp> navigationHistory;

    public bool isPhoneActive { get; private set; }
    public bool hasReceivedNotification { get; private set; }
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

        OpenApp(PhoneApp.Home, true);

    }

    // Update is called once per frame
    void Update()
    {

        UpdateWallpaper();
        HandleNotificationClock();

        if (Input.GetKeyDown(KeyCode.Tab) && !isMoving) 
        {
           TogglePhone();
        }

    }

    public void HandleNotificationClock()
    {
        if (currentApp == PhoneApp.Home && phoneTransform.anchoredPosition.y == upperY)
        {
            notificationClock.SetActive(false);
            // set notification bar decoration
            notificationBarSprites.SetActive(true);
        }
        else
        {
            notificationClock.SetActive(true);
            // set notification bar decoration
            notificationBarSprites.SetActive(false);
        }
    }

    void UpdateWallpaper()
    {
        if (currentApp == PhoneApp.Home)
        {
            phoneBG.sprite = homeBG;
        }

        if (currentApp == PhoneApp.NetApp)
        {
            phoneBG.sprite = netAppBG;
        }

        if (currentApp == PhoneApp.ChatApp)
        {
            phoneBG.sprite = chatAppBG;
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

    public void OpenApp(PhoneApp app, bool isStartUp = false)
    {
        if (!isStartUp) 
        {
            AudioManager.Instance.PlaySFX(sfx_PhoneAppClick);
        }
        
        if (currentApp != app)
        {
            if (hasReceivedNotification && app == PhoneApp.ChatApp)
            {
                chatNotification.SetActive(false);
            }

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
        AudioManager.Instance.PlaySFX(sfx_PhoneAppClick);
        OpenApp(PhoneApp.ChatApp_Messages);
        Debug.Log("Opened messages");
    }

    public void OpenNetPost()
    {
        AudioManager.Instance.PlaySFX(sfx_PhoneAppClick);
        OpenApp(PhoneApp.NetApp_Post);
        Debug.Log("Opened forum post");
    }

    public void ReceiveChatNotification()
    {
        if (currentApp != PhoneApp.ChatApp || currentApp != PhoneApp.ChatApp_Messages)
        {
            chatNotification.SetActive(true);
        }

        notificationBell.gameObject.SetActive(true);
        // play notification sound
        AudioManager.Instance.PlaySFX(sfx_PhoneNotification);
        
        hasReceivedNotification = true;

    }

    public void ReadChatNotification()
    {
        chatNotification.SetActive(false);
        notificationBell.SetActive(false);
        hasReceivedNotification = false;
    }

    public void BackBtn()
    {
        AudioManager.Instance.PlaySFX(sfx_PhoneBack);

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
        isMoving = false;

    }

}
