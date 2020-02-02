using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WeldDraw : MonoBehaviour
{
    public UnityEngine.Rendering.PostProcessing.PostProcessProfile postProcessProfile;
    public GameObject weldPrefab;
    public TrailRenderer weldLine;
    public Camera screenSpace;
    public ParticleSystem sparks;
    public Transform sparksLocaiton;
    
    private GameObject currentWeld;
    private Vector3 mouseLocation;
    public AudioSource audioSourceWeldSound;

    private void Update ()
    {
        if (Input.GetMouseButton (1) && this.sparks.isPlaying == true)
        {
            this.UpdateChromaticAbberation (1);

            this.UpdateLensDistortion (60f);
        }
        else
        {
            this.UpdateChromaticAbberation (0f);

            this.UpdateLensDistortion (0f);
        }

        if (Input.GetMouseButtonDown (1))
        {
            this.audioSourceWeldSound.volume = 0.1f;

            if (this.audioSourceWeldSound.isPlaying == false)
            {
                this.audioSourceWeldSound.Play();
            }
            
            mouseLocation = screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            SpawnTrail ();
        }
        else if (Input.GetMouseButton(1))
        {
            mouseLocation =
                screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            sparksLocaiton.position = mouseLocation;
            if (currentWeld)
            {
                currentWeld.transform.position = mouseLocation;
            }

            Ray ray = this.screenSpace.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (UnityEngine.Physics.Raycast (ray, out RaycastHit raycastHit, float.MaxValue, BrokenBattleBots.BattleBotCustomization.instance.LayerMaskSelect))
            {
                BrokenBattleBots.BattleBotPart battleBotPart = raycastHit.collider.GetComponent<BrokenBattleBots.BattleBotPart>();

                if (battleBotPart == null)
                {
                    BrokenBattleBots.BattleBotPartSocket battleBotPartSocket = raycastHit.collider.GetComponent<BrokenBattleBots.BattleBotPartSocket>();

                    if (battleBotPartSocket != null && battleBotPartSocket.battleBotPart != null)
                    {
                        battleBotPart = battleBotPartSocket.battleBotPart;
                    }
                }

                if (battleBotPart != null)
                {
                    battleBotPart.Weld (UnityEngine.Time.deltaTime);

                    this.audioSourceWeldSound.volume = 1f;

                    if (sparks.isPlaying == false)
                    {
                        sparks.Play();
                    }

                    return;
                }
            }

            sparks.Stop ();

            this.audioSourceWeldSound.volume = 0.1f;
        }
        else if(Input.GetMouseButtonUp(1))
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

    private void UpdateChromaticAbberation (float intensity, bool force = false)
    {
        if (this.postProcessProfile.TryGetSettings <UnityEngine.Rendering.PostProcessing.ChromaticAberration> (out var chromaticAberration))
        {
            chromaticAberration.intensity.value = Mathf.Lerp (chromaticAberration.intensity.value, intensity, 3f * Time.deltaTime);
        }
    }

    private void UpdateLensDistortion (float intensity, bool force = false )
    {
        if (this.postProcessProfile.TryGetSettings <UnityEngine.Rendering.PostProcessing.LensDistortion> (out var lensDistortion))
        {
            lensDistortion.intensity.value = Mathf.Lerp (lensDistortion.intensity.value, intensity, 3f * Time.deltaTime);
        }
    }

    private void OnApplicationQuit ()
    {
        this.UpdateChromaticAbberation (0f);
        this.UpdateLensDistortion (0f);
    }
}
