using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logoNeonHandler : MonoBehaviour
{
    public GameObject glowGO;

    public Image uiImage;
    public Sprite inactive;
    public Sprite active;

    public SpriteAnim spriteAnim;

    public AnimationClip anim;

    public void playClip() {
        spriteAnim.Play(anim);
    }
    public void Start()
    {
        glowOff();


    }
    // Start is called before the first frame update
    public void glowOn() {
        uiImage.sprite = active;
        glowGO.SetActive(true);
    }

    public void glowOff() {
        uiImage.sprite = inactive;
        glowGO.SetActive(false);
    }
}
