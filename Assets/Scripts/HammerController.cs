using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : ClickableBase {

    // Start is called before the first frame update
    void Start()
    {
        additionalStart();
    }

    // Update is called once per frame
    void Update()
    {
        additionalUpdate();
    }

    public override bool onDrop()
    {
        return false;
    }

}