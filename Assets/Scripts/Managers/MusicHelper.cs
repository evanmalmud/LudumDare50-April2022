using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHelper: MonoBehaviour
{

    public FMOD.Studio.EventInstance instance;
    public FMOD.Studio.EventInstance instance2;
    public FMOD.Studio.EventInstance instance3;
    public FMODUnity.EventReference music;
    public FMODUnity.EventReference bgnoise;
    public FMODUnity.EventReference neon;
    public GameStateManager gameStateManager;
    public logoNeonHandler neonHandler;

    public bool isPlaying = false;

    // Start is called before the first frame update
    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(music);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(bgnoise);
        instance3 = FMODUnity.RuntimeManager.CreateInstance(neon);
    }

    private void Update()
    {
        if (isPlaying) {
            if(gameStateManager.gameover) {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RoundCount", 0);
                instance.setParameterByName("RoundCount", 0);
            } else {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RoundCount", gameStateManager.roundCount);
                instance.setParameterByName("RoundCount", gameStateManager.roundCount);
            }
        }
    }

    public void setGameOver(int value)
    {
        //Death 
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Death", value);
        instance.setParameterByName("Death", value);
    }

    public void stopNeon() {
        instance3.setPaused(true);
    }

    public void startMusic()
    {
        instance.start();
        instance2.start();
        instance3.start();
        neonHandler.playClip();
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
