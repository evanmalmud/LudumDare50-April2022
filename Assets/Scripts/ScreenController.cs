using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenController : MonoBehaviour
{

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
        screenText.text = "";
    }

    public void replaceNameStrike(string name) {
        screenText.text.Replace(name, "<s>" + name + "</s>");
    }

    // Update is called once per frame
    public void appendText(string text)
    {
        instance.start();
        if (screenText.text != "") {
            //Append Carriage return
            screenText.text += "<line-height=65%>\n\n</line-height>";
        }
        screenText.text += text;
    }

    public void OnDestroy()
    {
        instance.release();
    }
}
