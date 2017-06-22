/*
 * Code by Pietro Romeo
 * June 2017
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private bool hasEnded = false;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


    public void StartGame()
    {
        StartCoroutine(PlayStartGameAnimation());
    }

    public void StartTraining()
    {
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("DeathScene");
    }

    
}
