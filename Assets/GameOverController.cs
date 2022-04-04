using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverCanvas;

    public GameObject winScreen;
    public GameObject lostScreen;

    public TextMeshProUGUI roundCount;

    public int winLimit = 10;

    // Start is called before the first frame update
    public void reset()
    {
        gameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    public void enableGameOver(int incomingRoundCount)
    {
        if(incomingRoundCount > winLimit) {
            winScreen.SetActive(true);
            lostScreen.SetActive(false);
        } else {
            winScreen.SetActive(false);
            lostScreen.SetActive(true);
        }
        roundCount.text = "You Lasted " + incomingRoundCount.ToString() + " Rounds";

        gameOverCanvas.SetActive(true);
    }
}
