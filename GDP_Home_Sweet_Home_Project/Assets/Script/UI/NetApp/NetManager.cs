using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class NetManager : MonoBehaviour
{
    [SerializeField]
    private Transform postListParent;
    [SerializeField]
    private ForumPost postPrefab;

    private List<Post> allForumPosts;

    [Header("Post Details")]
    [SerializeField]
    private TextMeshProUGUI currentPostTitle;
    [SerializeField]
    private TextMeshProUGUI currentPostSubtitle;
    [SerializeField]
    private TextMeshProUGUI currentPostContent;
    [SerializeField]
    private Image currentPostPhoto;

    [SerializeField]
    private PostLoader postLoader;

    public static NetManager instance;

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

        SetForumPosts();
        PopulateThread();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetForumPosts()
    {
        Post[] posts = postLoader.postData.posts;

        allForumPosts = new List<Post>(posts);
    }

    public void ViewPost(string postName)
    {
        Post targetPost = allForumPosts.Find(post => post.title == postName);
        
        if (targetPost != null)
        {
            currentPostTitle.text = targetPost.title;
            currentPostSubtitle.text = targetPost.subtitle;
            currentPostContent.text = targetPost.content;
        }
    }

    public void PopulateThread()
    {
        foreach (Transform child in postListParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < allForumPosts.Count; i++)
        {
            AddPost(allForumPosts[i]);
            Debug.Log(allForumPosts.Count);
        }
    }

    public void AddPost(Post post)
    {
        ForumPost newForumPost = Instantiate(postPrefab, postListParent);
        newForumPost.SetPostTitle(post.title);
        newForumPost.SetPostSubtitle(post.subtitle);
        newForumPost.SetPostImage(post.image);
    }

}
