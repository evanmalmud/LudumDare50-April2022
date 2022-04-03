using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GameStateManager : MonoBehaviour
{
    //CSV Keywords
    string namesColumn = "Names";
    string totalCount = "TotalCount";

    string textColumn = "Text";
    string multiColumn = "Multi";
    string positiveColumn = "Positive";
    string actionColumn = "Action";
    string nagatesColumn = "Negates";
    string instantColumn = "Instant";

    public TextAsset namesCSV;
    public TextAsset textLinesCSV;


    public ClockController[] clocks;

    public ScreenController screenController;

    public SubmitController submitController;
    public GameObject submitButton;

    string namesURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTdIufOO1eAzR6q6xyL6-C7iUs_F1z70E2lwy8HfnUKN9Xnxv4POnEhoW12LPXikt1-o9GM7XHOul5p/pub?output=csv";
    string textLineURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRoY4fC9mC8q7u-mY_lRBgqTEAwqxdIBhDBvHcX9Sd-D_8e_Fcp8QqH1LzvuBS0_1u-64VAuKlefmIc/pub?output=csv";

    public List<Dictionary<string, object>> names;
    public List<Dictionary<string, object>> textLines;

    public float lengthOfLevel = 10f;
    public float currentLengthOfLevel = 0f;

    public float timeUntilNextText = 0f;

    public float timeToPostLevel = 5f;

    public Vector2 minMaxNewText;

    public Vector2 minMaxActionValues;

    public TimerController timerController;

    void Awake()
    {
        submitButton.SetActive(false);
        //StartCoroutine(GetRequest(namesURL, names, namesCSV));
        //StartCoroutine(GetRequest(textLineURL, textLines, textLinesCSV));
        names = CSVReader.Read(namesCSV);
        textLines = CSVReader.Read(textLinesCSV);

        //Pick 5 random Names from above
        foreach(ClockController clock in clocks) {
            int indexOfName = Random.Range(0, names.Count);
            string nameChosen = (string)names[indexOfName][namesColumn];
            clock.setName(nameChosen);
            //Remove used name
            names.RemoveAt(indexOfName);
        }

    }

        // Update is called once per frame
    void Update()
    {
        if(currentLengthOfLevel < lengthOfLevel) {
            //Only post text if still in the main part of the level
            if(timeUntilNextText <= 0f) {
                timeUntilNextText = Random.Range(minMaxNewText.x, minMaxNewText.y);


                //Pick random Clock/Person and value of action
                int clockIndex = Random.Range(0, clocks.Length);

                int actionValue = (int)Random.Range(minMaxActionValues.x, minMaxActionValues.y);

                int textIndex = Random.Range(0, textLines.Count);
                string positveValue = (string)textLines[textIndex][positiveColumn];
                if ((positveValue).Equals("FALSE")) {
                    actionValue *= -1;
                }


                //Pick random Text from list
                string text = createTextLine(textIndex, actionValue, clocks[clockIndex].name);

                //Calc their hidden score
                clocks[clockIndex].hiddenDaysLeft += actionValue;

                //Print text to player
                screenController.appendText(text);
            } else {
                timeUntilNextText -= Time.deltaTime;
            }
        } else if (currentLengthOfLevel < lengthOfLevel + timeToPostLevel) {
            int timeLeft = (int)(lengthOfLevel + timeToPostLevel - currentLengthOfLevel);
            submitController.updateValue(timeLeft);
            submitButton.SetActive(true);
        } else if (currentLengthOfLevel > lengthOfLevel + timeToPostLevel) {
            //Check Values and Reset
            submitted();
        }
        currentLengthOfLevel += Time.deltaTime;
    }


    IEnumerator GetRequest(string uri, List<Dictionary<string, object>> dictionary, TextAsset failloverAsset)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    dictionary = CSVReader.Read(failloverAsset);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    dictionary = CSVReader.Read(failloverAsset);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    dictionary = CSVReader.Read(webRequest.downloadHandler.text);
                    break;
            }
        }
    }


    public string createTextLine(int textIndex, int value, string name) {
        string textLine = (string)textLines[textIndex][textColumn];

        textLine = textLine.Replace("NAME", name);

        if(value > 0) {
            textLine += " <color=\"green\">(+";
            textLine += value;
            textLine += ")</color>";

        } else {
            textLine += " <color=\"red\">(";
            textLine += value;
            textLine += ")</color>";
        }
        return textLine;
    }

    public void submitted() {
        submitButton.SetActive(false);
        bool anyWrong = false;
        foreach(ClockController clock in clocks) {
            if(clock.hiddenDaysLeft != clock.daysLeft) {
                clock.daysLeft = clock.hiddenDaysLeft;
                anyWrong = true;
            }
        }

        currentLengthOfLevel = 0f;
        submitButton.SetActive(false);
        screenController.reset();
        timerController.reset();

        if (anyWrong){
            Debug.LogError("SOME ANSWERS WRONG");
        } else {
            Debug.Log("ALL RIGHT!!!!");
        }
    }
}
