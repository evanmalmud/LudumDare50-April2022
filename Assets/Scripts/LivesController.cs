using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesController : MonoBehaviour
{


    public GameObject[] lives;

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

        foreach(GameObject go in lives) {
            go.SetActive(true);
        }
    }

    public void loseLife() {
        if (livesCount <= 1) {
            instance.start();
            gameStateManager.gameOver();
        } else {
            instance.start();
            lives[livesCount - 1].SetActive(false);
            livesCount--;
        }
    }
}
