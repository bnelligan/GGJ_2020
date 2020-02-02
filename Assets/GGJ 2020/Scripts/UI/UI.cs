using System.Collections;
using System.Collections.Generic;
using BrokenBattleBots;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image gen1Fill;

    public Image gen2Fill;

    public Image gen3Fill;

    public Image healthBarFill;
    public GameObject end;
    private bool gen1Dead = false;

    // Update is called once per frame
    void Update()
    {
        gen1Fill.fillAmount = (float)GUI_Info.Gen1HP_Current / GUI_Info.Gen1HP_Max;
        gen2Fill.fillAmount = (float)GUI_Info.Gen2HP_Current / GUI_Info.Gen2HP_Max;
        gen3Fill.fillAmount =(float) GUI_Info.Gen3HP_Current / GUI_Info.Gen3HP_Max;
        healthBarFill.fillAmount =1- ((float)GUI_Info.PlayerHP_Current / (float)100) ;
        
        if (healthBarFill.fillAmount >=.99f)
        {
            end.SetActive(true);
            Debug.Log("Game Over");
            gen1Dead = true;
        }
    }
}
