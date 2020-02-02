using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrokenBattleBots;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleBotCustomization.instance.Standing)
        {
            foreach (var item in spawners)
            {
                item.gameObject.SetActive(true);
            }
        }
        
    }
}
