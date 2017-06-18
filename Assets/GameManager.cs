using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static bool hasStarted = false;
    private bool hasEnded = false;

    
    void Start()
    {
        if (hasStarted) { return; ; }

        hasStarted = true;
        StartCoroutine(PlayStartGameAnimation());
    }

    public void EndGame()
    {
        if (hasEnded) { return; ; }

        hasEnded = true;
        StartCoroutine(PlayEndGameAnimation());
    }

    IEnumerator PlayStartGameAnimation()
    {
        Debug.Log("Game starting");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainLevel");
    }

        IEnumerator PlayEndGameAnimation()
    {
        Debug.Log("GAME OVER");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("DeathScene");
    }

}
