using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //CSV Keywords
    public string namesColumn = "NAMES";

    public TextAsset namesCSV;
    public TextAsset actionsCSV;
    public TextAsset textLinesCSV;


    public GameObject Clock1;
    public GameObject Clock2;
    public GameObject Clock3;
    public GameObject Clock4;
    public GameObject Clock5;

    public ScreenController screenController;

    public float countdown;
    // Start is called before the first frame update
    void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(namesCSV);

        for (var i = 0; i < data.Count; i++) {
            print("name " + data[i][namesColumn]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(countdown < 0) {
            screenController.appendText("Test this method over and over.");
            countdown = Random.Range(1.0f, 3.0f);
        } else {
            countdown -= Time.deltaTime;
        }
        
    }
}
