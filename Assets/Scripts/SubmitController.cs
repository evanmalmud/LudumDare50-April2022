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
    // Start is called before the first frame update
    private void Start()
    {
        reset();
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
