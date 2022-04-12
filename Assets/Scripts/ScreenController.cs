using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        //Debug.Log("REPLACE NAME - " + nameWithColor);

        IEnumerable<int> indexs = getAllIndexes(storedText, nameWithColor);
        //reverse order so the indexs dont change
        foreach (int index in indexs.Reverse()){
            int indexEnd = storedText.IndexOf(")", index);



            string subString = storedText.Substring(index, (indexEnd - index + 1));
            storedText = storedText.Replace(subString, "<u>" + subString + "</u>");
        }



        //storedText = storedText.Replace(nameWithColor, "<u>" + nameWithColor + "</u>");
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

    public IEnumerable<int> getAllIndexes(string source, string matchString)
    {
        matchString = Regex.Escape(matchString);
        foreach (Match match in Regex.Matches(source, matchString)) {
            yield return match.Index;
        }
    }
}
