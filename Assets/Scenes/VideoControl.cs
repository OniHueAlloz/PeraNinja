using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [SerializeField] private string SceneName;
    private VideoPlayer vp;
    bool playerOn =false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        bool playerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (vp.isPlaying == true)
        {
            playerOn = true;
        }
        if ( vp.isPlaying == false && playerOn == true) 
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
