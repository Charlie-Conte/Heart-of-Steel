using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// script to manage the translucent overlay when the app is inactive
/// </summary>
public class OverlayManager : MonoBehaviour
{

    float timer = 0.0f;

    //overlay
    public Image image;
    //interupting opbjects
    public GameObject web, video, amrcPDF, tsmPDF, bhfPDF;
    /// <summary>
    /// time before overlay begins to appear
    /// </summary>
    public float inactiveTime ;

    //alpha
    Color color = new Color(1,1,1,0);
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>inactiveTime && !web.activeSelf && !video.activeSelf && !amrcPDF.activeSelf && !tsmPDF.activeSelf && !bhfPDF.activeSelf)
        {
            image.enabled = true;
            if(image.color.a < 0.117)
            {
                color.a = color.a + 0.002f;
                image.color = color;
            }
        }
        else if(web.activeInHierarchy || video.activeInHierarchy || amrcPDF.activeInHierarchy || tsmPDF.activeInHierarchy || bhfPDF.activeInHierarchy)
        {
            timer = 0;
            color.a = 0;
        }
        if (Input.anyKeyDown)
        {
            timer = 0;
            color.a = 0;
            image.enabled = false;

        }

    }

}
