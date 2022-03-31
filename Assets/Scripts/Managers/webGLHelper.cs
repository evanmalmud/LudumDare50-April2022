using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class webGLHelper : MonoBehaviour
{
    public GameObject loadingText;
    public GameObject clickText;
    public MusicHelper musicHelper;

    bool banksLoaded = false;
    // Start is called before the first frame update
    void Awake()
    {
        loadingText.SetActive(true);
        clickText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!banksLoaded && FMODUnity.RuntimeManager.HaveAllBanksLoaded) {
            banksLoaded = true;
            loadingText.SetActive(false);
            clickText.SetActive(true);
        }
    }

    public void onClickButton() {
        if(banksLoaded) {
            musicHelper.startMusic();
            this.gameObject.SetActive(false);

        }
    }
}
