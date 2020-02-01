using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WeldDraw : MonoBehaviour
{
    public GameObject weldPrefab;
    public TrailRenderer weldLine;
    public Camera screenSpace;
    public ParticleSystem sparks;
    public Transform sparksLocaiton;
    
    private GameObject currentWeld;
    private Vector3 mouseLocation;
    public AudioSource audioSourceWeldSound;

    bool wasSelectingPart;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(mouseLocation);

        if (BrokenBattleBots.BattleBotCustomization.instance != null && BrokenBattleBots.BattleBotCustomization.instance.SelectedBattleBotPart != null)
        {
            this.wasSelectingPart = true;

            if (this.sparks.isPlaying == true)
            {
                sparks.Stop();
                RemoveTrail();

                
            }

            this.audioSourceWeldSound.Stop();

            return;
        }

        if (this.wasSelectingPart == true)
        {
            if (Input.GetMouseButtonUp (0))
            {
                this.wasSelectingPart = false;
            }

            return;
        }

            if (Input.GetMouseButtonDown(0))
        {
            this.audioSourceWeldSound.volume = 0.1f;

            if (this.audioSourceWeldSound.isPlaying == false)
            {
                this.audioSourceWeldSound.Play();
            }
            
            mouseLocation =
                screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
           SpawnTrail();
           /*sparksLocaiton.position = mouseLocation;
           sparks.Play();*/
        }
        else if (Input.GetMouseButton(0))
        {
            mouseLocation =
                screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            sparksLocaiton.position = mouseLocation;
            if (currentWeld)
            {
                currentWeld.transform.position = mouseLocation;
            }

            if (BrokenBattleBots.BattleBotCustomization.instance != null && BrokenBattleBots.BattleBotCustomization.instance.SelectedBattleBotPart == null)
            {
                Ray ray = this.screenSpace.ScreenPointToRay (UnityEngine.Input.mousePosition);

                if (UnityEngine.Physics.Raycast (ray, out RaycastHit raycastHit, float.MaxValue, BrokenBattleBots.BattleBotCustomization.instance.LayerMaskSelect))
                {
                    BrokenBattleBots.BattleBotPart battleBotPart = raycastHit.collider.GetComponent <BrokenBattleBots.BattleBotPart> ();

                    if (battleBotPart == null)
                    {
                        BrokenBattleBots.BattleBotPartSocket battleBotPartSocket = raycastHit.collider.GetComponent <BrokenBattleBots.BattleBotPartSocket> ();

                        if (battleBotPartSocket != null && battleBotPartSocket.battleBotPart != null)
                        {
                            battleBotPart = battleBotPartSocket.battleBotPart;
                        }
                    }

                    if (battleBotPart != null)
                    {
                        if (battleBotPart.Socket != null)
                        {
                            battleBotPart.Weld(UnityEngine.Time.deltaTime);
                        }
                        /*else
                        {
                            battleBotPart.Weld (-UnityEngine.Time.deltaTime);
                        }*/
                        
                        this.audioSourceWeldSound.volume = 1f;

                        if (sparks.isPlaying == false)
                        {
                            sparks.Play ();
                        }

                        return;
                    }
                }
            }

            sparks.Stop ();

            this.audioSourceWeldSound.volume = 0.1f;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            sparks.Stop();
            RemoveTrail();

            this.audioSourceWeldSound.Stop ();
        }
    }

    void SpawnTrail()
    {
        GameObject weldObj = Instantiate(weldPrefab, mouseLocation, Quaternion.identity);
        currentWeld = weldObj;
        weldLine = currentWeld.GetComponent<TrailRenderer>();
    }

    void RemoveTrail ()
    {
        if (this.weldLine != null)
        {
            this.weldLine.Clear();
        }
        
    }
}
