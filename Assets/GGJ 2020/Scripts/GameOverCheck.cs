using System.Collections;
using System.Collections.Generic;
using BrokenBattleBots;
using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    private float gen1Health;
    private float gen2Health;
    private float gen3Health;
    
    public float Gen1Health
    {
        get => gen1Health;
        set
        {
            if(value == gen1Health)
                return;
            gen1Health = value;
            if (gen1Health <= 0)
            {
                gen1Dead = true;
            }
        }
    }
    
    public float Gen2Health
    {
        get => gen2Health;
        set
        {
            if(value == gen2Health)
                return;
            gen2Health = value;
            if (gen2Health <= 0)
            {
                gen2Dead = true;
            }
        }
    }
    
    public float Gen3Health
    {
        get => gen3Health;
        set
        {
            if(value == gen3Health)
                return;
            gen3Health = value;
            if (gen3Health <= 0)
            {
                gen3Dead = true;
            }
        }
    }

    public bool gen1Dead;

    public bool gen2Dead;

    public bool gen3Dead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gen1Health = GUI_Info.Gen1HP_Current;
        Gen2Health = GUI_Info.Gen2HP_Current;
        Gen3Health = GUI_Info.Gen3HP_Current;
        
        Debug.Log(gen1Health);
        Debug.Log(gen2Health);
        Debug.Log(gen3Health);

        if (gen1Dead && gen2Dead && gen3Dead)
        {
            Debug.Log("Game Over");
        }
    }
}
