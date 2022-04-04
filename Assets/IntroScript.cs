using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{

    public Camera mainCamera;

    public float startingProjSize = 7.58f;
    public float endingProjSize = 5.57f;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    public float lengthOfIntro;

    bool donePlaying = false;

    public GameStateManager gameStateManager;

    float current = 0f;

    bool introStarted = false;

    public GameObject mainMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
    }

    public void startIntro()
    {
        mainMenu.SetActive(false);
        mainCamera.orthographicSize = startingProjSize;
        //instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //instance.start();
        introStarted = true;
        mainCamera.DOOrthoSize(endingProjSize, lengthOfIntro);
    }

    // Update is called once per frame
    void Update()
    {
        if (introStarted) {
            if (current >= lengthOfIntro) {
                donePlaying = true;
            } else {
                current += Time.deltaTime;
            }

            if (donePlaying) {
                gameStateManager.gameActive = true;
            }
        }
    }

    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

}
