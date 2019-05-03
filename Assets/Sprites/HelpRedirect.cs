using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpRedirect : MonoBehaviour {

    public Button buttonToActivate;
    private Button exitButton;

	// Use this for initialization
	void Start () {
        exitButton = GameObject.FindGameObjectWithTag("ExitUI").GetComponent<Button>();
	}
	
    public void InvokeOtherButton()
    {
        exitButton.onClick.Invoke();
        buttonToActivate.onClick.Invoke();

    }
}
