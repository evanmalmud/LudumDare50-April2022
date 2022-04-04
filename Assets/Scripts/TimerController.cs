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

    public bool tweenStarted = false;

    private void Start()
    {
        reset();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.gameActive) {
            if(!tweenStarted) {
                timerBlock.transform.DOLocalMove(emptyWaypoint.localPosition, defaultTimer);
                tweenStarted = true;
            }
            if (currentCount < 0) {
                //Stop counting
            } else {
                currentCount -= Time.deltaTime;
                updateTimer();
            }
        } else {
            tweenStarted = false;
        }
    }

    public void reset()
    {
        defaultTimer = gameStateManager.lengthOfLevel;
        defaultTimer++;
        tweenStarted = false;
        timerText.text = defaultTimer.ToString();
        currentCount = defaultTimer;
        timerBlock.transform.localPosition = fullWaypoint.localPosition;
    }

    public void updateTimer() {
        timerText.text = ((int)currentCount).ToString();
    }
}
