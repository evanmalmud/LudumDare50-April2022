using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugController : ClickableBase {

    public ClockController hoveredClockController = null;

    public bool isConnected = false;

    public GameObject glowSprite;
    public GameObject stuckSprite;

    public GameObject redGlow;

    public ConsoleController consoleController;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference plugEvent;

    public bool isStuck = false;

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(plugEvent);
        reset();
        additionalStart();
    }

    public void reset() {
        isStuck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected && isStuck) {
            stuckSprite.SetActive(true);
            glowSprite.SetActive(false);
        } else if(isConnected) {
            glowSprite.SetActive(true);
            stuckSprite.SetActive(false);
        } else {
            glowSprite.SetActive(false);
            stuckSprite.SetActive(false);
        }
        additionalUpdate();
    }

    public override bool onClick()
    {
        if(isStuck) {
            return false;
        }
        //Debug.Log("plug on click");
        isConnected = false;
        consoleController.isConnected = false;
        consoleController.connectedClock = null;
        if (hoveredClockController != null) {
            hoveredClockController.isSelected = false;
        }
        clickAndHeld = true;
        return base.onClick();
    }

    public override bool onDrop()
    {
        //Debug.Log("plug on drop");
        if (hoveredClockController != null) {
            //Debug.Log("plug on drop2");
            //Move to that waypoint position and drop
            isConnected = true;
            instance.start();
            consoleController.isConnected = true;
            consoleController.connectedClock = hoveredClockController;
            transform.position = hoveredClockController.plugLockPoint.transform.position;
            hoveredClockController.isSelected = true;
            clickAndHeld = false;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ClockPlug clockPlug = collision.gameObject.GetComponent<ClockPlug>();
        if(clockPlug != null) {
            //Debug.Log("Enter " + collision.gameObject.name + clockPlug.clockController.name);
            hoveredClockController = clockPlug.clockController;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ClockPlug clockPlug = collision.gameObject.GetComponent<ClockPlug>();
        if (clockPlug != null) {
            if (clockPlug.clockController != hoveredClockController) {
                hoveredClockController = clockPlug.clockController;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ClockPlug clockPlug = collision.gameObject.GetComponent<ClockPlug>();
        if (clockPlug != null) {
            //Debug.Log("Exit " + collision.gameObject.name + clockPlug.clockController.name);
            if (clockPlug.clockController == hoveredClockController) {
                hoveredClockController = null;
            }
        }
    }

    void OnMouseOver()
    {
        if (isStuck) {
            if(redGlow != null) {
                redGlow.SetActive(true);
            }
        } else {
            if (hoverGO != null) {
                hoverGO.SetActive(true);
            }
        }
    }

    void OnMouseExit()
    {
        if (isStuck) {
            if (redGlow != null) {
                redGlow.SetActive(false);
            }
        } else {
            if (hoverGO != null) {
                hoverGO.SetActive(false);
            }
        }
    }

    public void setStuck(bool stuck) {
        if(hoverGO.activeSelf || redGlow.activeSelf) {
            hoverGO.SetActive(false);
            redGlow.SetActive(false);
            if(stuck)
            {
                redGlow.SetActive(true);
            } else {
                hoverGO.SetActive(true);
            }
        }
        isStuck = stuck;
    }
}
