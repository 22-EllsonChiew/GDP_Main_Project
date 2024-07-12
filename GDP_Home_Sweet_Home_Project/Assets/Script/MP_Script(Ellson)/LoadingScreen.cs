using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public TextAsset textJSON;
    public Text messageText;
    
    [System.Serializable]
    public class MSOMessage
    {
        public string Message;
    }

    [System.Serializable]

    public class MessageList
    {
        public MSOMessage[] message;
    }

    public MessageList myMSOMessage = new MessageList();

    void Start()
    {
        myMSOMessage = JsonUtility.FromJson<MessageList>(textJSON.text);

        ShowRandomMessage();

    }

    private void OnEnable()
    {
        ShowRandomMessage();
    }

    void ShowRandomMessage()
    {
        if (myMSOMessage.message.Length > 0)
        {
            // Select a random index
            int randomIndex = Random.Range(0, myMSOMessage.message.Length);

            // Get the random message
            string randomMessage = myMSOMessage.message[randomIndex].Message;

            // Display the message (assuming you have a Text UI element assigned to messageText)
            if (messageText != null)
            {
                messageText.text = randomMessage;
            }
            else
            {
                Debug.LogError("MessageText UI element is not assigned.");
            }
        }
    }


}
