using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableBase : MonoBehaviour
{
    public bool clickAndHeld = false;

    public Vector3 lastGoodLocation;

    public Vector3 mouseMoveOffset;

    public GameObject hoverGO;

    public void additionalStart()
    {
        lastGoodLocation = transform.position;
        OnMouseExit();
    }

    public void additionalUpdate()
    {
        if (clickAndHeld) {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos += mouseMoveOffset;
            newPos.z = 0;
            transform.position = newPos;
        }
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
        transform.position = lastGoodLocation;
        return true;
    }

    void OnMouseOver()
    {
        if(hoverGO != null) {
            hoverGO.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (hoverGO != null) {
            hoverGO.SetActive(false);
        }
    }
}
