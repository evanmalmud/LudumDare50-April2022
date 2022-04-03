using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{

    public bool isConnected = false;

    public GameObject wifiSprite;

    public ClockController connectedClock;

    public GameObject plusPress;
    public GameObject minusPress;

    public float timeToKeepPressedState = 5;


    // Start is called before the first frame update
    void connected(bool isConnected)
    {
        if(isConnected) {
            wifiSprite.SetActive(true);
        } else {
            wifiSprite.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        connected(isConnected);
    }

    public void onMinusClick() {
        if(connectedClock != null) {
            connectedClock.minusDay();
            StartCoroutine(playPress(minusPress));
        }
    }

    public void onPlusClick()
    {
        if (connectedClock != null) {
            connectedClock.addDay();
            StartCoroutine(playPress(plusPress));
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
