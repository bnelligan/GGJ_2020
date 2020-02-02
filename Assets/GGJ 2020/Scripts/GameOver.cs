using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public float changeFrequency, alphaUp, alphaDown;
    public Image canv1, canv2;
    private float timeToChange;
    private bool moveUp;
    public Button MainMenu;
    public Button PlayAgain;
    public Button Quit;
    // Start is called before the first frame update
    void Start()
    {
        timeToChange = changeFrequency;
        var tempColor = canv1.color;
        tempColor.a = 0f;
        canv1.color = tempColor;
        var tempColor2 = canv2.color;
        tempColor2.a = 0f;
        canv2.color = tempColor2;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToChange <= 0 && canv1.color.a < 1)
        {
            timeToChange = changeFrequency;
            if (moveUp)
            {
                var tempColor = canv1.color;
                tempColor.a += alphaUp;
                canv1.color = tempColor;
                var tempColor2 = canv2.color;
                tempColor2.a += alphaUp;
                canv2.color = tempColor2;
                moveUp = false;
            } else {
                var tempColor = canv1.color;
                tempColor.a -= alphaDown;
                canv1.color = tempColor;
                var tempColor2 = canv2.color;
                tempColor2.a -= alphaDown;
                canv2.color = tempColor2;
                moveUp = true;
            }

        } else if(canv1.color.a >= 1) {
            MainMenu.gameObject.SetActive(true);
            PlayAgain.gameObject.SetActive(true);
            Quit.gameObject.SetActive(true);
        } else {
            timeToChange -= Time.deltaTime;
        }
    }
}
