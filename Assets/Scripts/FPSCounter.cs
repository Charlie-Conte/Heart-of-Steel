using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour {


    public float  avgFrameRate;
    public Text display_Text;

    private float nextActionTime = 0.0f;
    public float period = 0.25f;
    int frameCount = 0;
    public void Update()
    {
        float current = 0;
        current = (1f / Time.unscaledDeltaTime);

        avgFrameRate = current + avgFrameRate;
        if (Time.time > nextActionTime)
        {

            nextActionTime += period;
            avgFrameRate = (int)(avgFrameRate / (Time.frameCount-frameCount));
            display_Text.text = avgFrameRate.ToString() + " FPS";
            avgFrameRate = 0;
            frameCount = Time.frameCount;
        }

    }
}
