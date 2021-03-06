using PowerTools;
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

    [SerializeField]
    private int daysLeft = 50;
    [SerializeField]
    private int hiddenDaysLeft = 50;

    public TextMeshProUGUI daysLeftText;

    public string name;

    public string nameWithColor;

    public Color nameColor;

    public GameStateManager gameStateManager;

    public GameObject spirit;
    public SpriteAnim spitirAnim;
    public AnimationClip spirit_anim;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference deathGhostEvent;

    FMOD.Studio.EventInstance instance2;
    public FMODUnity.EventReference deathEvent;

    public bool compareDaysEqual() {
        Debug.Log("compareDaysEqual");
        if (daysLeft == hiddenDaysLeft) {
            return true;
        }
        return false;
    }

    public void resetDays() {
        Debug.Log("resetDays");
        daysLeft = hiddenDaysLeft;
    }

    public void printDays() {
        Debug.Log(name + " wrong - " + daysLeft + "  -  " + hiddenDaysLeft);
    }

    public void randomMinus() {
        Debug.Log("randomMinus");
        int minusRand = Random.Range(1, 3);
        hiddenDaysLeft -= minusRand;
        if (hiddenDaysLeft <= 0) {
            hiddenDaysLeft = 1;
        }
        daysLeft = hiddenDaysLeft;
    }

    public void updateHiddenDay(int actionValue) {
        Debug.Log("updateHiddenDay");
        hiddenDaysLeft += actionValue;
    }


    bool lastFrameDead = true;
    public void minusDay() {
        Debug.Log("minusDay");
        daysLeft--;
    }

    public void addDay()
    {
        Debug.Log("addDay");
        daysLeft++;
    }

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(deathGhostEvent);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(deathEvent);
        bgSprite.SetActive(true);
        skullCenterSprite.SetActive(true);
        nameplateSprite.SetActive(true);

        //Add Random Pointer Start Location
        Vector3 newRotPointer1 = new Vector3();
        newRotPointer1.z -= (Random.Range(1,10) * 360 * .1f);
        pointers1.transform.Rotate(newRotPointer1);
        Vector3 newRotPointer2 = new Vector3();
        newRotPointer2.z -= (Random.Range(1, 10) * 360 * .1f) / 12;
        pointers2.transform.Rotate(newRotPointer2);
        rorateRatePerSecond = Random.Range(.2f, 1f);
        reset();
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
        if (gameStateManager.gameActive) {
            if (!isDead) {
                if (hiddenDaysLeft <= 0) {
                    instance2.start();
                    isDead = true;
                }
            }
            rotatePointers();
            selected(isSelected);

            daysLeftText.text = daysLeft.ToString();

            if(lastFrameDead != isDead){
                dead(isDead);
                lastFrameDead = isDead;
            }
        }
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

    public void someoneDied() {
        skullOverlaySprite.SetActive(true);
        gameStateManager.plugStuck();
        gameStateManager.deleteName(nameWithColor);
    }

    void dead(bool isDead)
    {
        if (isDead) {
            skullOverlaySprite.SetActive(true);
            gameStateManager.plugStuck();
            gameStateManager.deleteName(nameWithColor);
        } else {
            skullOverlaySprite.SetActive(false);

        }
    }

    public void setName(string name) {
        this.name = name;
        nameWithColor = "<color=#" + ColorUtility.ToHtmlStringRGB(nameColor) + ">" + name + "</color>";
        namePlate.text = nameWithColor;
    }

    public void reset()
    {
        isDead = false;
        lastFrameDead = true;
        daysLeft = 3 + Random.Range(1, 15);
        hiddenDaysLeft = daysLeft;
        gameStateManager.plugUnStuck();
    }


    public void killClockPerson() {
        //Play anim.
        StartCoroutine(playSpritAnim());
        //Get new name
        setName(gameStateManager.getNewName());

        reset();
    }

    IEnumerator playSpritAnim() {
        spirit.SetActive(true);
        spitirAnim.Play(spirit_anim);
        instance.start();
        yield return new WaitForSeconds(spirit_anim.length);
        spirit.SetActive(false);
        yield return null;
    }
}
