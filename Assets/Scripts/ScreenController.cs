using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenController : MonoBehaviour
{

    public TextMeshProUGUI screenText;

    private void Start()
    {
        reset();
    }


    public void reset()
    {
        screenText.text = "";
    }

    // Update is called once per frame
    public void appendText(string text)
    {
        if(screenText.text != "") {
            //Append Carriage return
            screenText.text += "\n";
        }
        screenText.text += text;
    }
}
