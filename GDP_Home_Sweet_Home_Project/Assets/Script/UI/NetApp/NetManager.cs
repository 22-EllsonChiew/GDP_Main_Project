using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        allForumPosts = new List<Post>()
        {
            new Post() {title = "Neighbourliness 101", subtitle = "How to be a good neighbour", image = null},
            new Post() {title = "Post 2", subtitle = "Dummy post 2", image= null},
            new Post() {title = "Post 3", subtitle = "Dummy post 3", image = null}
        };

        PopulateThread(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewPost(string postName)
    {
        Post targetPost = allForumPosts.Find(post => post.title == postName);
        
        if (targetPost != null)
        {
            currentPostTitle.text = targetPost.title;
            currentPostSubtitle.text = targetPost.subtitle;
        }
    }

    public void PopulateThread(int postCount)
    {
        for (int i = 0; i < postCount; i++)
        {
            Post randomPost = allForumPosts[Random.Range(0, allForumPosts.Count)];
            AddPost(randomPost);
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
