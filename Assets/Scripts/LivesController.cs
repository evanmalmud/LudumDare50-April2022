using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesController : MonoBehaviour
{


    public GameObject[] lives;

    public int livesCount;

    public GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
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
            gameStateManager.gameOver();
        } else {
            lives[livesCount - 1].SetActive(false);
            livesCount--;
        }
    }
}
