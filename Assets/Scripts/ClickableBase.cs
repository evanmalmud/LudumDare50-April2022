using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableBase : MonoBehaviour
{
    public bool clickAndHeld = false;

    public Transform lastGoodLocation;

    private void Start()
    {
        lastGoodLocation = transform;
    }

    // Start is called before the first frame update
    public virtual bool onClick()
    {
        clickAndHeld = true;
        return true;
    }

    public virtual bool onDrop()
    {
        clickAndHeld = false;
        return true;
    }

    public virtual bool onBadDrop()
    {
        clickAndHeld = false;
        transform.position = lastGoodLocation.position;
        return true;
    }
}
