using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHelper: MonoBehaviour
{

    public FMOD.Studio.EventInstance instance;
    public FMOD.Studio.EventInstance instance2;
    public FMODUnity.EventReference music;
    public FMODUnity.EventReference bgnoise;

    public GameStateManager gameStateManager;

    private bool isPlaying = false;

    // Start is called before the first frame update
    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(music);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(bgnoise);
    }

    private void Update()
    {   
        if(isPlaying)
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RoundCount", gameStateManager.roundCount);
    }
    public void startMusic()
    {
        instance.start();
        instance2.start();
        isPlaying = true;
    }

    // Update is called once per frame
    public void pauseMusic()
    {
        instance.setPaused(true);
        instance2.setPaused(true);
    }

    public void unpauseMusic()
    {
        instance.setPaused(false);
        instance2.setPaused(false);
    }

    void OnDestroy()
    {
        //FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        //playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
        instance2.release();
    }
}
