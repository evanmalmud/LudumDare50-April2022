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
        reset();
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
        defaultTimer = gameStateManager.lengthOfLevel;
        timerText.text = defaultTimer.ToString();
        currentCount = defaultTimer;
        timerBlock.transform.localPosition = fullWaypoint.localPosition;
        timerBlock.transform.DOLocalMove(emptyWaypoint.localPosition, defaultTimer);
    }

    public void updateTimer() {
        timerText.text = ((int)currentCount).ToString();
    }
}
