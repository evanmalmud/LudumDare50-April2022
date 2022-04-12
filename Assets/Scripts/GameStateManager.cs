using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

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


    public List<ClockController> clocks;

    public List<ClockController> futureAddClocks;

    public ScreenController screenController;

    public SubmitController submitController;

    string namesURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTdIufOO1eAzR6q6xyL6-C7iUs_F1z70E2lwy8HfnUKN9Xnxv4POnEhoW12LPXikt1-o9GM7XHOul5p/pub?output=csv";
    string textLineURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRoY4fC9mC8q7u-mY_lRBgqTEAwqxdIBhDBvHcX9Sd-D_8e_Fcp8QqH1LzvuBS0_1u-64VAuKlefmIc/pub?output=csv";

    public List<Dictionary<string, object>> namesLoaded;
    public List<Dictionary<string, object>> namesUnused;
    public List<Dictionary<string, object>> textLinesLoaded;
    float defaultLengthOfLevel;
    public float lengthOfLevel = 10f;
    public float currentLengthOfLevel = 0f;

    public float timeUntilNextText = 0f;

    public float timeToPostLevel = 5f;

    Vector2 defaultminMaxNewText;
    public Vector2 minMaxNewText;

    public Vector2 minMaxActionValues;

    public TimerController timerController;

    public int roundCount = 1;

    public LivesController livesController;

    public PlugController plugController;

    public ScytheController scytheController;

    public ConsoleController consoleController;

    public GameOverController gameOverController;

    public bool gameover = false;
    public bool gameActive = false;

    FMOD.Studio.EventInstance instanceGoodSubmit;
    public FMODUnity.EventReference goodSubmitEvent;

    public MusicHelper musicHelper;

    public Task namewebRequestComplete;
    public Task textwebRequestComplete;
    bool loadComplete = false;

    //FMOD.Studio.EventInstance instanceBadSubmit;
    //public FMODUnity.EventReference badSubmitEvent;

    private void Start()
    {
        instanceGoodSubmit = FMODUnity.RuntimeManager.CreateInstance(goodSubmitEvent);
        //instanceBadSubmit = FMODUnity.RuntimeManager.CreateInstance(badSubmitEvent);
    }
    void Awake()
    {
        foreach (ClockController clock in futureAddClocks) {
            clock.gameObject.SetActive(false);
        }

        defaultLengthOfLevel = lengthOfLevel;
        defaultminMaxNewText = minMaxNewText;

        namewebRequestComplete = new Task(GetNamesRequest());
        textwebRequestComplete = new Task(GetTextRequest());
    }

    public void activeAfterIntro() {
        gameActive = true;

        foreach (ClockController clock in clocks) {
            clock.daysLeft = clock.hiddenDaysLeft;
        }

    }

    public void reset()
    {
        
        lengthOfLevel = defaultLengthOfLevel;
        minMaxNewText = defaultminMaxNewText;

        foreach (ClockController clock in futureAddClocks) {
            clock.gameObject.SetActive(false);
            clocks.Remove(clock);
        }

        gameover = false;
        gameOverController.reset();
        livesController.reset();
        submitController.reset();
        //names = CSVReader.Read(namesCSV);
        //textLines = CSVReader.Read(textLinesCSV);

        //Pick 5 random Names from above
        foreach (ClockController clock in clocks) {
            int indexOfName = Random.Range(0, namesUnused.Count);
            string nameChosen = (string)namesUnused[indexOfName][namesColumn];
            clock.setName(nameChosen);
            clock.reset();
            //Remove used name
            namesUnused.RemoveAt(indexOfName);
        }

        submitController.reset();

        currentLengthOfLevel = 0f;
        screenController.reset();
        timerController.reset();
        roundCount = 0;

    }

    public string getNewName() {
        if(namesUnused.Count <= 0) {
            namesUnused = CSVReader.Read(namesCSV);
        }
        int indexOfName = Random.Range(0, namesUnused.Count);
        string nameChosen = (string)namesUnused[indexOfName][namesColumn];
        //Remove used name
        namesUnused.RemoveAt(indexOfName);
        return nameChosen;
    }

    void levelScalling() {
        lengthOfLevel++;

        foreach(ClockController clock in clocks) {
            int minusRand = Random.Range(1, 3);
            clock.hiddenDaysLeft -= minusRand;
            if(clock.hiddenDaysLeft <= 0) {
                clock.hiddenDaysLeft = 1;
            }
            clock.daysLeft = clock.hiddenDaysLeft;
        }

        if(roundCount % 2 == 0) {
            minMaxNewText = minMaxNewText / 1.1f;
        }

        if(roundCount == 4) {
            //Add More CLOCKS
            foreach(ClockController clock in futureAddClocks) {
                clock.gameObject.SetActive(true);
                clock.setName(getNewName());
                clock.reset();
                clocks.Add(clock);
            }
            minMaxNewText = defaultminMaxNewText;
            lengthOfLevel = defaultLengthOfLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (namewebRequestComplete.Running || textwebRequestComplete.Running) {
            //Wait for this to complete
            Debug.Log("One running");
            return;
        } else if(!namewebRequestComplete.Running && !textwebRequestComplete.Running && !loadComplete){
            loadComplete = true;
            namesUnused = namesLoaded;
            reset();
        }
        if(gameover || !gameActive)
        {
            return;
        }

        //Hot key checker
        checkHotKeys();

        if (currentLengthOfLevel < lengthOfLevel) {
            //Only post text if still in the main part of the level
            if(timeUntilNextText <= 0f) {
                timeUntilNextText = Random.Range(minMaxNewText.x, minMaxNewText.y);


                //Pick random Clock/Person and value of action
                int clockIndex = Random.Range(0, clocks.Count);
                if(clocks[clockIndex].isDead) {
                    //Check all clocks for non-dead
                    bool alldead = true;
                    foreach(ClockController clock in clocks) {
                        if(!clock.isDead) {
                            alldead = false;
                            clockIndex = clocks.IndexOf(clock);
                        }
                    }
                    if(alldead) {
                        gameOver();
                    }
                }


                int actionValue = (int)Random.Range(minMaxActionValues.x, minMaxActionValues.y);


                int textIndex = Random.Range(0, textLinesLoaded.Count);
                try {
                    int multi = (int)textLinesLoaded[textIndex][multiColumn];
                    if(multi > 1) {
                        actionValue += (int)Random.Range(minMaxActionValues.x, minMaxActionValues.y);
                    }
                } catch (Exception e) {
                    Debug.Log(e);
                }
               
                string positveValue = (string)textLinesLoaded[textIndex][positiveColumn];
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

    void checkHotKeys() {
        bool shiftPressed = false;
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            shiftPressed = true;
        }

        int keyIndex = 0;
        if(Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1)) {
            keyIndex = 1;
        } else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) {
            keyIndex = 2;
        } else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) {
            keyIndex = 3;
        } else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) {
            keyIndex = 4;
        } else  if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)) {
            keyIndex = 5;
        }

        if(keyIndex > 0 && keyIndex <= clocks.Count) {
            if(shiftPressed) {
                // Use Scythe on that person
                scytheController.moveAndClear(clocks[keyIndex-1]);
            } else {
                // Move Plug to that person
                plugController.moveAndClear(clocks[keyIndex-1]);
            }
        }
    }


    IEnumerator GetTextRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(namesURL)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = namesURL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    namesLoaded = CSVReader.Read(namesCSV);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    namesLoaded = CSVReader.Read(namesCSV);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    namesLoaded = CSVReader.Read(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator GetNamesRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(textLineURL)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = textLineURL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    textLinesLoaded = CSVReader.Read(textLinesCSV);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    textLinesLoaded = CSVReader.Read(textLinesCSV);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    textLinesLoaded = CSVReader.Read(webRequest.downloadHandler.text);
                    break;
            }
        }
    }


    public string createTextLine(int textIndex, int value, string name) {
        string textLine = (string)textLinesLoaded[textIndex][textColumn];

        textLine = textLine.Replace("NAME", name);

        if(value > 0) {
            textLine += " <nobr><color=\"green\">(+";
            textLine += value;
            textLine += ")</color></nobr>";

        } else {
            textLine += " <nobr><color=\"red\">(";
            textLine += value;
            textLine += ")</color></nobr>";
        }
        return textLine;
    }

    public void submitted() {
        submitController.reset();
        bool anyWrong = false;
        foreach(ClockController clock in clocks) {
            if(clock.hiddenDaysLeft != clock.daysLeft) {
                //Debug.Log(clock.gameObject.name + " wrong - " + clock.daysLeft + "  -  " + clock.hiddenDaysLeft);
                clock.daysLeft = clock.hiddenDaysLeft;
                anyWrong = true;
            }
        }

        currentLengthOfLevel = 0f;
        screenController.reset();
        timerController.reset();
        if (anyWrong){
            //instanceBadSubmit.start();
            livesController.loseLife();
        } else {
            roundCount++;
            instanceGoodSubmit.start();
            levelScalling();
        }
    }

    public void gameOver() {
        gameover = true;
        musicHelper.setGameOver(1);
        gameActive = false;
        //pause things and show game over screen
        gameOverController.enableGameOver(roundCount);
    }

    public void replay()
    {
        musicHelper.setGameOver(0);
        gameActive = true;
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

    public void deleteName(string nameWithColor)
    {
        screenController.replaceNameStrike(nameWithColor);
    }
}
