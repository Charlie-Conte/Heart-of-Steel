using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KeyboardManager : MonoBehaviour
{
    private VirtualKeyboard keyboard = new VirtualKeyboard();

    public void LaunchKeyboard()
    {
        keyboard.ShowTouchKeyboard();
    }
    public void HideKeyboard()
    {
        keyboard.HideTouchKeyboard();
    }

}