using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{

    public bool isConnected = false;

    public GameObject wifiSprite;
    public GameObject wifiStuckSprite;

    public ClockController connectedClock;

    public GameObject plusPress;
    public GameObject minusPress;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    public float timeToKeepPressedState = 5;

    public bool isStuck = false;


    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    // Start is called before the first frame update
    void connected()
    {
        if (isConnected && isStuck) {
            wifiStuckSprite.SetActive(true);
            wifiSprite.SetActive(false);
        } else if (isConnected) {
            wifiStuckSprite.SetActive(false);
            wifiSprite.SetActive(true);
        } else {
            wifiStuckSprite.SetActive(false);
            wifiSprite.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        connected();
    }

    public void onMinusClick() {
        if(connectedClock != null) {
            connectedClock.minusDay();
            StartCoroutine(playPress(minusPress));
            instance.start();
        }
    }

    public void onPlusClick()
    {
        if (connectedClock != null) {
            connectedClock.addDay();
            StartCoroutine(playPress(plusPress));
            instance.start();
        }
    }

    public void onSandClick()
    {

    }

    IEnumerator playPress(GameObject go) {
        go.SetActive(true);
        yield return new WaitForSeconds(timeToKeepPressedState);
        go.SetActive(false);
    }
}
