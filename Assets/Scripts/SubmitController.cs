using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubmitController : MonoBehaviour
{
    public TextMeshProUGUI screenText;

    public GameObject ticketSubmit;

    public Transform hiddenWaypoint;
    public Transform shownWaypoint;

    public float timeToShow;

    public bool ticketShown = false;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    public GameStateManager gameStateManager;

    // Start is called before the first frame update
    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        reset();
    }

    private void Update()
    {
        if(ticketShown) {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
                gameStateManager.submitted();
            }
        }
    }

    public void reset()
    {
        screenText.text = "";
        DOTween.Kill(ticketSubmit.transform);
        ticketSubmit.transform.localPosition = hiddenWaypoint.localPosition;
        ticketShown = false;
    }

    public void showTicket() {
        if (!ticketShown) {
            instance.start();
            ticketSubmit.transform.DOLocalMove(shownWaypoint.localPosition, timeToShow);
            ticketShown = true;
        }
    }

    // Update is called once per frame
    public void updateValue(int number)
    {
        screenText.text = "Submit ?\n" + number;
    }
}
