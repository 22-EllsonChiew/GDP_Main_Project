using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostLoader : MonoBehaviour
{
    public TextAsset postJson;
    public PostData postData;

    // Start is called before the first frame update
    void Start()
    {
        LoadPosts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPosts()
    {
        postData = JsonUtility.FromJson<PostData>(postJson.text);

        if (postData != null )
        {
            int totalPosts = postData.posts.Length;
            Debug.Log("Total posts loaded: " + totalPosts);
        }
    }
}
