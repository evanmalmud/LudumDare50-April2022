using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : ClickableBase {

    public ClockController hoveredClockController = null;

    // Start is called before the first frame update
    void Start()
    {
        additionalStart();
    }

    // Update is called once per frame
    void Update()
    {
        additionalUpdate();
    }

    public override bool onDrop()
    {
        //Debug.Log("plug on drop");
        if (hoveredClockController != null && hoveredClockController.isDead) {
            hoveredClockController.killClockPerson();
            hoveredClockController = null;
            clickAndHeld = false;
        }
        return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        ClockController clockController = collision.gameObject.GetComponentInParent<ClockController>();
        if (clockController != null) {
            hoveredClockController = clockController;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        ClockController clockController = collision.gameObject.GetComponentInParent<ClockController>();
        if (clockController != null) {
            if (clockController == hoveredClockController) {
                hoveredClockController = null;
            }
        }
    }
}
