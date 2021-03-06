﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public GameObject dropParts;
    public Button startButton;
    public Animator rFall;
    public AudioSource music;
    public float fadeFactor;
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif

    }

    public void StartGame()
    {
        StartCoroutine("FallThenStart");
        
        rFall.SetTrigger("Fall");
        startButton.interactable = false;
        StartCoroutine("FadeMusic");
    }

    IEnumerator FallThenStart()
    {
        dropParts.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(1);
    }

    IEnumerator FadeMusic()
    {
        while (music.volume > 0)
        {
            music.volume -= fadeFactor;
            yield return new WaitForSeconds(.2f);
        }
    }
}
