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

    FMOD.Studio.EventInstance instanceDialogue;
    public FMODUnity.EventReference fmodDialogueEvent;

    FMOD.Studio.EventInstance instancehumph;
    public FMODUnity.EventReference humphEvent;

    public float lengthOfIntro;

    bool donePlaying = false;

    public GameStateManager gameStateManager;

    float current = 0f;

    bool introStarted = false;

    public GameObject mainMenu;

    public GameObject IntroCanvas;

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instancehumph = FMODUnity.RuntimeManager.CreateInstance(humphEvent);
        instanceDialogue = FMODUnity.RuntimeManager.CreateInstance(fmodDialogueEvent);
        mainMenu.SetActive(true);
        IntroCanvas.SetActive(false);
    }

    public void startIntro()
    {
        mainMenu.SetActive(false);
        IntroCanvas.SetActive(true);
        mainCamera.orthographicSize = startingProjSize;
        //instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
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
                introStarted = false;
                gameStateManager.activeAfterIntro();
                instance.setPaused(true);
                IntroCanvas.SetActive(false);
                instanceDialogue.start();
            }
        }
    }

    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    public void skipIntro(){
        instance.setPaused(true);
        instancehumph.start();
        donePlaying = true;
        DOTween.Kill(mainCamera);
        mainCamera.orthographicSize = endingProjSize;
        IntroCanvas.SetActive(false);
        instanceDialogue.start();
    }

    //IEnumerator playTutorialActions() {
        //Show People

        // Move Plug and Click Controller

        //Show Prompts

        //Show Scythe Kill

        /*spirit.SetActive(true);
        spitirAnim.Play(spirit_anim);
        instance.start();
        yield return new WaitForSeconds(spirit_anim.length);
        spirit.SetActive(false);
        yield return null;*/
    //}

}
