using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : ClickableBase {

    public ClockController hoveredClockController = null;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference scytheEvent;

    // Start is called before the first frame update
    void Start()
    {
        additionalStart();
        instance = FMODUnity.RuntimeManager.CreateInstance(scytheEvent);
    }

    // Update is called once per frame
    void Update()
    {
        additionalUpdate();
    }

    public void moveAndClear(ClockController clockController)
    {
        if (clockController != null && clockController.isDead) {
            instance.start();
            clockController.killClockPerson();
            hoveredClockController = null;
            clickAndHeld = false;
        }
    }

    public override bool onDrop()
    {
        //Debug.Log("plug on drop");
        if (hoveredClockController != null && hoveredClockController.isDead) {
            instance.start();
            hoveredClockController.killClockPerson();
            hoveredClockController = null;
            clickAndHeld = false;
        }
        return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        ClockDeathCollider clockDeath = collision.gameObject.GetComponent<ClockDeathCollider>();
        if (clockDeath != null) {
            hoveredClockController = clockDeath.clockController;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ClockDeathCollider clockDeath = collision.gameObject.GetComponent<ClockDeathCollider>();
        if (clockDeath != null) {
            hoveredClockController = clockDeath.clockController;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        ClockDeathCollider clockDeath = collision.gameObject.GetComponent<ClockDeathCollider>();
        if (clockDeath != null) {
            if (clockDeath.clockController == hoveredClockController) {
                hoveredClockController = null;
            }
        }
    }
}
