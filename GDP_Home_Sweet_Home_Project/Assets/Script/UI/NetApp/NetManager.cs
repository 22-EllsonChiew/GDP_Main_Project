using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    [SerializeField]
    private Transform postListParent;
    [SerializeField]
    private ForumPost postPrefab;

    private List<Post> allForumPosts;

    // Start is called before the first frame update
    void Start()
    {
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
