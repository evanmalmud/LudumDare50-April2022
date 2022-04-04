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

    public int roundCount = 1;

    public LivesController livesController;

    public PlugController plugController;

    public ConsoleController consoleController;

    public GameObject gameOverCanvas;

    public bool gameover = false;

    FMOD.Studio.EventInstance instanceGoodSubmit;
    public FMODUnity.EventReference goodSubmitEvent;

    //FMOD.Studio.EventInstance instanceBadSubmit;
    //public FMODUnity.EventReference badSubmitEvent;

    private void Start()
    {
        instanceGoodSubmit = FMODUnity.RuntimeManager.CreateInstance(goodSubmitEvent);
        //instanceBadSubmit = FMODUnity.RuntimeManager.CreateInstance(badSubmitEvent);
    }
    void Awake()
    {
        reset();
    }

    public void reset()
    {
        
        gameover = false;
        gameOverCanvas.SetActive(false);
        livesController.reset();
        submitController.reset();
        //StartCoroutine(GetRequest(namesURL, names, namesCSV));
        //StartCoroutine(GetRequest(textLineURL, textLines, textLinesCSV));
        names = CSVReader.Read(namesCSV);
        textLines = CSVReader.Read(textLinesCSV);

        //Pick 5 random Names from above
        foreach (ClockController clock in clocks) {
            int indexOfName = Random.Range(0, names.Count);
            string nameChosen = (string)names[indexOfName][namesColumn];
            clock.setName(nameChosen);
            //Remove used name
            names.RemoveAt(indexOfName);
        }

        submitController.reset();
        bool anyWrong = false;

        currentLengthOfLevel = 0f;
        screenController.reset();
        timerController.reset();
        roundCount = 0;

    }

    public string getNewName() {
        if(names.Count <= 0) {
            names = CSVReader.Read(namesCSV);
        }
        int indexOfName = Random.Range(0, names.Count);
        string nameChosen = (string)names[indexOfName][namesColumn];
        //Remove used name
        names.RemoveAt(indexOfName);
        return nameChosen;
    }

    void levelScalling() {
        lengthOfLevel++;

        if(roundCount == 4) {
            minMaxNewText = minMaxNewText / 1.5f;
        }

        if (roundCount == 8) {
            minMaxNewText = minMaxNewText / 1.5f;
        }
    }

        // Update is called once per frame
    void Update()
    {
        if(gameover)
        {
            return;
        }
        if(currentLengthOfLevel < lengthOfLevel) {
            //Only post text if still in the main part of the level
            if(timeUntilNextText <= 0f) {
                timeUntilNextText = Random.Range(minMaxNewText.x, minMaxNewText.y);


                //Pick random Clock/Person and value of action
                int clockIndex = Random.Range(0, clocks.Length);
                if(clocks[clockIndex].isDead) {
                    //If dead pick again
                    clockIndex = Random.Range(0, clocks.Length);
                    if (clocks[clockIndex].isDead) {
                        //If dead pick again
                        clockIndex = Random.Range(0, clocks.Length);
                    }
                }

                int actionValue = (int)Random.Range(minMaxActionValues.x, minMaxActionValues.y);

                int textIndex = Random.Range(0, textLines.Count);
                string positveValue = (string)textLines[textIndex][positiveColumn];
                if ((positveValue).Equals("FALSE")) {
                    actionValue *= -1;
                }


                //Pick random Text from list
                string text = createTextLine(textIndex, actionValue, clocks[clockIndex].nameWithColor);

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
            submitController.showTicket();
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
        submitController.reset();
        bool anyWrong = false;
        foreach(ClockController clock in clocks) {
            if(clock.hiddenDaysLeft != clock.daysLeft) {
                clock.daysLeft = clock.hiddenDaysLeft;
                anyWrong = true;
            }
        }

        currentLengthOfLevel = 0f;
        screenController.reset();
        timerController.reset();
        roundCount++;
        if (anyWrong){
            //instanceBadSubmit.start();
            livesController.loseLife();
        } else {
            instanceGoodSubmit.start();
        }
        levelScalling();
    }

    public void gameOver() {
        gameover = true;
        //pause things and show game over screen
        gameOverCanvas.SetActive(true);
    }

    public void replay()
    {
        reset();
    }

    public void plugStuck() {
        //Turn plug red and disable moving
        plugController.setStuck(true);
        //turn controller red
        consoleController.isStuck = true;
    }

    public void plugUnStuck()
    {
        //Turn plug blue and enable moving
        plugController.setStuck(false);
        //turn controller blue
        consoleController.isStuck = false;
    }
}
