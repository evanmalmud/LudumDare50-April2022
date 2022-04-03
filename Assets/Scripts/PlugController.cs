using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugController : ClickableBase {

    public ClockController hoveredClockController = null;

    public bool isConnected = false;

    public GameObject glowSprite;
    public GameObject stuckSprite;

    public ConsoleController consoleController;

    public bool isStuck = false;

    private void Start()
    {
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
        //Debug.Log(collision.gameObject.name);
        ClockController clockController = collision.gameObject.GetComponentInParent<ClockController>();
        if(clockController != null) {
            hoveredClockController = clockController;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        ClockController clockController = collision.gameObject.GetComponentInParent<ClockController>();
        if (clockController != null) {
            if(clockController == hoveredClockController) {
                hoveredClockController = null;
            }
        }
    }


}
