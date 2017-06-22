﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private bool hasEnded = false;
    public static int score;


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


    public void StartGame()
    {
        score = 0;
        StartCoroutine(PlayStartGameAnimation());
    }

    public void StartTraining()
    {
        score = 0;
        StartCoroutine(PlayStartGameAnimation());
    }

    public void EndGame()
    {
        if (hasEnded) { return; ; }

        hasEnded = true;
        StartCoroutine(PlayEndGameAnimation());
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator PlayStartGameAnimation()
    {
        Debug.Log("Game starting");
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("MainLevel");

    }

        IEnumerator PlayEndGameAnimation()
    {
        print("GAME OVER");
        print("Final score: " + score);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("DeathScene");
    }

    public static void updateScore(int i)
    {
        score += i;
        Debug.Log("New score: " + score);
    }

    public static int getScore()
    {
        return score;
    }
}