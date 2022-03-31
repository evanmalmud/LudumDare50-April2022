using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHelper: MonoBehaviour
{

    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference music;

    private bool isPlaying = false;

    // Start is called before the first frame update
    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(music);
    }
    public void startMusic()
    {
        instance.start();
        isPlaying = true;
    }

    // Update is called once per frame
    public void pauseMusic()
    {
        instance.setPaused(true);
    }

    void OnDestroy()
    {
        //FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        //playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
    }
}
