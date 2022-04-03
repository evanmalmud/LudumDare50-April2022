using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubmitController : MonoBehaviour
{
    public TextMeshProUGUI screenText;

    // Start is called before the first frame update
    private void Start()
    {
        reset();
    }


    public void reset()
    {
        screenText.text = "";
    }

    // Update is called once per frame
    public void updateValue(int number)
    {
        screenText.text = "Submit ?\n" + number;
    }
}
