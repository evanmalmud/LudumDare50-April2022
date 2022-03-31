using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    SceneManager sceneManager;
    // Start is called before the first frame update
    public void SwapToLevel()
    {
        SceneManager.LoadScene("Level");
    }

    // Update is called once per frame
    public void SwapToMain()
    {
        SceneManager.LoadScene("Main");
    }
}
