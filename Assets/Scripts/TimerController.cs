using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{

    public TextMeshProUGUI timerText;

    public GameObject timerBlock;

    public Transform fullWaypoint;
    public Transform emptyWaypoint;


    public float defaultTimer;
    public float currentCount;

    public GameStateManager gameStateManager;

    Vector2 distance;

    private void Start()
    {
        defaultTimer = gameStateManager.lengthOfLevel;
        reset();

        distance = emptyWaypoint.localPosition - fullWaypoint.localPosition;
        Debug.Log("DISTANCE" + distance);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentCount < 0) {
            //Stop counting
        } else {
            currentCount -= Time.deltaTime;
            updateTimer();
        }
    }

    public void reset()
    {
        timerText.text = defaultTimer.ToString();
        currentCount = defaultTimer;
        timerBlock.transform.localPosition = fullWaypoint.localPosition;
        timerBlock.transform.DOLocalMove(emptyWaypoint.localPosition, defaultTimer);
    }

    public void updateTimer() {
        ///float newy = fullWaypoint.localPosition.y - (distance.y / (int)defaultTimer) * (int)currentCount;
        ///Vector2 newPos = new Vector2(timerBlock.transform.localPosition.x, newy);
        ///timerBlock.transform.localPosition = newPos;

        timerText.text = ((int)currentCount).ToString();
    }
}
