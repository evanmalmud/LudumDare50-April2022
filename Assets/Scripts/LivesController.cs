using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesController : MonoBehaviour
{


    public MugController[] lives;

    public int livesCount;

    public GameStateManager gameStateManager;

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        reset();
    }

    public void reset()
    {
        livesCount = lives.Length;

        foreach(MugController go in lives) {
            go.swapSprite(true);
        }
    }

    public void loseLife() {
        instance.start();
        lives[livesCount - 1].swapSprite(false);
        livesCount--;
        if (livesCount <= 0) {
            gameStateManager.gameOver();
        }
    }
}
