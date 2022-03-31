using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHelper : MonoBehaviour
{

    public FMODUnity.EventReference inSFX;
    public FMODUnity.EventReference outSFX;

    public GameObject canvas;

    // Start is called before the first frame update
    void Awake()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(DamageEvent, transform.position);
        //FMODUnity.RuntimeManager.PlayOneShot(DamageEvent, transform.position);
        canvas = this.gameObject;
    }
}
