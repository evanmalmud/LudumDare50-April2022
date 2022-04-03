using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{

    public bool isConnected = false;

    public GameObject wifiSprite;

    public ClockController connectedClock;


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
        }
    }

    public void onPlusClick()
    {
        if (connectedClock != null) {
            connectedClock.addDay();
        }
    }

    public void onSandClick()
    {

    }
}
