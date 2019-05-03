using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

    public InputField output;

    private GameObject capsPanel;       // 0
    private GameObject lowerPanel;      // 1
    private GameObject symbol1Panel;    // 2
    private GameObject symbol2Panel;    // 3

    private int currentView;
    private enum Panels
    {
        Caps,
        Lower,
        Symbol1,
        Symbol2,
    };


	void Start () {
        capsPanel = transform.Find("Background/Caps").gameObject;
        lowerPanel = transform.Find("Background/Lower").gameObject;
        symbol1Panel = transform.Find("Background/Numbers and Symbols 1").gameObject;
        symbol2Panel = transform.Find("Background/Numbers and Symbols 2").gameObject;

        currentView = (int) Panels.Lower;
    }

    public void SetOutput(Text text)
    {
        output.text = text.text;
    }

    public void ToggleCase()
    {
        switch (currentView)
        {
            case (int) Panels.Caps:
                capsPanel.SetActive(false);
                lowerPanel.SetActive(true);
                currentView = (int)Panels.Lower;
                break;

            case (int)Panels.Lower:
                lowerPanel.SetActive(false);
                capsPanel.SetActive(true);
                currentView = (int)Panels.Caps;
                break;

            case (int)Panels.Symbol1:
                symbol1Panel.SetActive(false);
                lowerPanel.SetActive(true);
                currentView = (int)Panels.Lower;
                break;

            case (int)Panels.Symbol2:
                symbol2Panel.SetActive(false);
                lowerPanel.SetActive(true);
                currentView = (int)Panels.Lower;
                break;
        }
    }

    public void ToggleSymbols()
    {
        switch (currentView)
        {
            case (int)Panels.Caps:
                capsPanel.SetActive(false);
                symbol1Panel.SetActive(true);
                currentView = (int)Panels.Symbol1;
                break;

            case (int)Panels.Lower:
                lowerPanel.SetActive(false);
                symbol1Panel.SetActive(true);
                currentView = (int)Panels.Symbol1;
                break;

            case (int)Panels.Symbol1:
                symbol1Panel.SetActive(false);
                symbol2Panel.SetActive(true);
                currentView = (int)Panels.Symbol2;
                break;

            case (int)Panels.Symbol2:
                symbol2Panel.SetActive(false);
                symbol1Panel.SetActive(true);
                currentView = (int)Panels.Symbol1;
                break;
        }
    }

    public void InputKeyPress(Button button)
    {
        String currentText = output.text;
        String buttonChar = button.GetComponentInChildren<Text>().text;
        String newText = currentText + buttonChar;
        output.text = newText;
    }

    public void BackspacePress()
    {
        String currentText = output.text;

        if (!String.IsNullOrEmpty(currentText))
            output.text = currentText.TrimEnd(currentText[currentText.Length - 1]);
    }

    public void EnterPress()
    {
        output.onEndEdit.Invoke(output.text);
        output.text = "";
        output.gameObject.GetComponent<Canvas>().enabled = false;
    }
}
