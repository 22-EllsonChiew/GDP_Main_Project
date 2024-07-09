using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class ForumPost : MonoBehaviour
{
    public TextMeshProUGUI postTitle;
    public TextMeshProUGUI postSubtitle;
    public Image postImage;

    public void SetPostTitle(string _postTitle)
    {
        postTitle.text = _postTitle;
    }

    public void SetPostSubtitle(string _postSubtitle)
    {
        postSubtitle.text = _postSubtitle;
    }

    public void SetPostImage(Sprite _postImage)
    {
        postImage.sprite = _postImage;
    }

    public string GetPostTitle()
    {
        return postTitle.text;
    }
    
    public string GetPostSubtitle()
    {
        return postTitle.text;
    }

    public Sprite GetPostPhoto()
    {
        return postImage.sprite;
    }

    public void OnClick()
    {
        // open post screen
        // handle transition logic
    }
}
