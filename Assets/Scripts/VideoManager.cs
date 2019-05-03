using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoManager : MonoBehaviour
{


    public AudioSource audioSource;
    public RawImage image;
    public Sprite tempImage;
    public VideoPlayer videoPlayer;
    [SerializeField]
    private double startTime;
    private bool hasStarted = false;
    private float timer = 0f;

    public void SetStartTime(float time)
    {
        startTime = time;
    }

    private void OnDisable()
    {
        videoPlayer.Stop();
        Application.runInBackground = false;
        StopCoroutine(playVideo());
        videoPlayer.Stop();
    }


    void OnEnable()
    {
        hasStarted = false;
        timer = 0;
        Application.runInBackground = true;
        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        videoPlayer.Prepare();

        //Wait until Movie is prepared
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (!videoPlayer.isPrepared)
        {
            image.texture = tempImage.texture;
            yield return waitTime;
            break;
        }

        //image.texture = videoPlayer.targetTexture;
        audioSource.enabled = false;

        videoPlayer.time = startTime;


        //Play Movie 
        videoPlayer.Play();


    }
    private void Update()
    {
        if (videoPlayer.isPlaying && !hasStarted)
        {
            timer += Time.deltaTime;
            if (timer >= 0.8f)
            {
                image.texture = videoPlayer.targetTexture;
                audioSource.enabled = true;
                hasStarted = true;
            }
        }
    }
}
