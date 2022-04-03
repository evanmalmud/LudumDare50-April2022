using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameObject plugLockPoint;

    public GameObject glowGBSprite;
    public GameObject bgSprite;
    public GameObject skullCenterSprite;
    public GameObject nameplateSprite;
    public GameObject skullOverlaySprite;

    public GameObject pointers1;
    public GameObject pointers2;

    public GameObject glowPointerSprite1;
    public GameObject glowPointerSprite2;

    public TextMeshProUGUI namePlate;
    public bool isSelected = false;
    public bool isDead = false;

    public float rorateRatePerSecond = 1f;

    public int daysLeft = 50;

    public TextMeshProUGUI daysLeftText;
    


    public void minusDay() {
        daysLeft--;
    }

    public void addDay()
    {
        daysLeft++;
    }




    public string name;

    private void Start()
    {
        bgSprite.SetActive(true);
        skullCenterSprite.SetActive(true);
        nameplateSprite.SetActive(true);

    }

    private void rotatePointers() {
        //Rotate pointer 1 at rorateRatePerSecond
        Vector3 newRotPointer1 = new Vector3();
        newRotPointer1.z -= (rorateRatePerSecond * 360 * Time.deltaTime);
        //Debug.Log((rorateRatePerSecond * 360 * Time.deltaTime));
        pointers1.transform.Rotate(newRotPointer1);

        //Rotate pointer 2 at rorateRatePerSecond/12
        Vector3 newRotPointer2 = new Vector3();
        newRotPointer2.z -= (rorateRatePerSecond * 360 * Time.deltaTime)/12;
        pointers2.transform.Rotate(newRotPointer2);

    }
    // Update is called once per frame
    void Update()
    {
        rotatePointers();
        selected(isSelected);
        dead(isDead);

        daysLeftText.text = daysLeft.ToString();
    }

    void selected(bool isSelected)
    {
        if(isSelected) {
            glowGBSprite.SetActive(true);
            glowPointerSprite1.SetActive(true);
            glowPointerSprite2.SetActive(true);
        } else {
            glowGBSprite.SetActive(false);
            glowPointerSprite1.SetActive(false);
            glowPointerSprite2.SetActive(false);
        }
    }

    void dead(bool isDead)
    {
        if (isDead) {
            skullOverlaySprite.SetActive(true);

        } else {
            skullOverlaySprite.SetActive(false);

        }
    }

    public void setName(string name) {
        this.name = name;
        namePlate.text = name;
    }
}
