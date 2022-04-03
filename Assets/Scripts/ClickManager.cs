using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    public ClickableBase storedClickable;

    // Update is called once per frame
    void Update()
    {
        //Click and we are holding nothing
        if (Input.GetMouseButtonDown(0) && storedClickable == null) {
            //Debug.Log("Mouse Click");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null) {
                //Debug.Log(hit.collider.gameObject.name);
                ClickableBase clickableBase = hit.collider.gameObject.GetComponent<ClickableBase>();
                if(clickableBase != null) {
                    if(clickableBase.onClick()){
                        //Click success
                        storedClickable = clickableBase;
                    }
                }
            }
        } else 

        //Click and we are holding something
        /*if (Input.GetMouseButtonDown(0) && storedClickable != null) {
            //Debug.Log("Mouse Drop");
            if (storedClickable.onDrop()) {
                //Drop Success
                storedClickable = null;
            }
        } else */

        if (Input.GetMouseButtonUp(0) && storedClickable != null) {
            //Debug.Log("Mouse Drop");
            if (storedClickable.onDrop()) {
                //Drop Success
                storedClickable = null;
            } else {
                storedClickable.onBadDrop();
                storedClickable = null;
            }
        }

    }
}
