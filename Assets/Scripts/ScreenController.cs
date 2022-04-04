using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    private string storedText;
    public TextMeshProUGUI screenText;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference messageEvent;

    private void Start()
    {
        reset();
        instance = FMODUnity.RuntimeManager.CreateInstance(messageEvent);
    }

    public void reset()
    {
        storedText = "";
        screenText.text = "";
    }


    public void replaceNameStrike(string nameWithColor) {
        Debug.Log("REPLACE NAME - " + nameWithColor);
        storedText = storedText.Replace(nameWithColor, "<u>" + nameWithColor + "</u>");
        screenText.text = storedText;
    }

    // Update is called once per frame
    public void appendText(string text)
    {
        instance.start();
        if (storedText != "") {
            //Append Carriage return
            storedText += "<line-height=65%>\n\n</line-height>";
        }
        storedText += text;
        screenText.text = storedText;
    }

    public void OnDestroy()
    {
        instance.release();
    }
}
