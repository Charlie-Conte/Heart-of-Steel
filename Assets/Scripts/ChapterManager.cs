using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChapterManager : MonoBehaviour
{
    public VideoManager videoManager;
    public GameObject videoPlayer;
    public Material steel;
    public Material backplane;
    public List<MeshRenderer> backPlates;
    public List<GameObject> chapterUIList;
    public List<float> chapterStartTime;
    public GameObject ExitUI;
    public GameObject VideoButton;
    public int currentChapterIndex;
    private bool isFullVideo;

    public bool IsFullVideo
    {
        get
        {
            return isFullVideo;
        }

        set
        {
            isFullVideo = value;
        }
    }

    public void SetChapterList()
    {
        foreach (MeshRenderer mesh in backPlates)
        {
            mesh.material = backplane;
        }
        Camera.main.fieldOfView = 33;
        foreach (GameObject obj in chapterUIList)
        {
            obj.GetComponent<Canvas>().enabled = true;
        }
    }
    public void ExitChapterList()
    {
        foreach (MeshRenderer mesh in backPlates)
        {
            mesh.material = steel;
        }
        Camera.main.fieldOfView = 60;
        foreach (GameObject obj in chapterUIList)
        {
            obj.GetComponent<Canvas>().enabled = false;
        }
    }

    public void StartVideo(int index)
    {
        currentChapterIndex = index;
        videoManager.SetStartTime(chapterStartTime[index]);
        videoPlayer.SetActive(true);        
    }
    private void Update()
    {

        if(videoManager.videoPlayer.isPlaying)
        {
            Debug.Log("Playback Time: " + videoManager.videoPlayer.time.ToString("N2") + "/" + (videoManager.videoPlayer.clip.length - 2).ToString() + "\nIsFullVideo: " + IsFullVideo);
            if (chapterStartTime.Count - 1 != currentChapterIndex && !IsFullVideo)
            {
                if (videoManager.videoPlayer.time >= chapterStartTime[currentChapterIndex + 1])
                {
                    ExitUI.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    VideoButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();

                }
            }
            if (IsFullVideo)
            {

                if (videoManager.videoPlayer.time >= videoManager.videoPlayer.clip.length - 2)
                {
                    ExitUI.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    VideoButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
            }
        }


    }

}
