using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Post
{
    public string id;
    public string title;
    public string subtitle;
    public Sprite image;
    public string content;
}

[System.Serializable]
public class PostData
{
    public Post[] posts;
}
