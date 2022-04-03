using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableBase : MonoBehaviour
{
    public bool clickAndHeld = false;

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
}
