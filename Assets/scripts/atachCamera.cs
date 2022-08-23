using UnityEngine;
using UnityEngine.Video;

public class atachCamera : MonoBehaviour
{
    VideoPlayer video;
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.targetCamera = Camera.main;
    }

}
