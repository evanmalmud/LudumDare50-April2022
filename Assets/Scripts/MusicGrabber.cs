using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGrabber : MonoBehaviour
{
    //public MusicHelper musicHelper;
    public GameStateManager gameStateManager;

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RoundCount", gameStateManager.roundCount);
        /*if (musicHelper == null){
            musicHelper = FindObjectOfType<MusicHelper>();

        } else {
            musicHelper.instance.setParameterByName("RoundCount", gameStateManager.roundCount);
        }*/

    }
}
