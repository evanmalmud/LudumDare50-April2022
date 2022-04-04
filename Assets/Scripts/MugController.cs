using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MugController : MonoBehaviour
{
    public bool isActive = true;

    public GameObject activeSprite;
    public GameObject inactiveSprite;

    public void swapSprite(bool active)
    {
        isActive = active;
        if (isActive) {
            activeSprite.SetActive(true);
            inactiveSprite.SetActive(false);
        } else {
            activeSprite.SetActive(false);
            inactiveSprite.SetActive(true);
        }
    }
}
