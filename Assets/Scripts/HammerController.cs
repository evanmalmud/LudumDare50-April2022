using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : ClickableBase {

    FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference hammerEvent;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(hammerEvent);
        additionalStart();
    }

    // Update is called once per frame
    void Update()
    {
        additionalUpdate();
    }

    public override bool onDrop()
    {
        instance.start();
        return false;
    }

}