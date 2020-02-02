using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrokenBattleBots;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawners;
    public bool StartFighting;
    public GameObject Robot;
    public GameObject instructionText;
    // Start is called before the first frame update
    void Start()
    {
        StartFighting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleBotCustomization.instance.Standing && !StartFighting)
        {
            foreach (var item in spawners)
            {
                if (item != null)
                {
                    item.gameObject.SetActive(true);
                }
                
            }
            StartFighting = true;
            Robot.AddComponent<ConvertToEntity>();
            instructionText.SetActive(false);
        }
        
    }
}
