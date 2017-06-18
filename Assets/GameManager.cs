using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private bool hasEnded = false;

	public void EndGame()
    {
        if (hasEnded) { return; ; }

        hasEnded = true;
        StartCoroutine(PlayEndGameAnimation());
    }

    IEnumerator PlayEndGameAnimation()
    {
        Debug.Log("GAME OVER");
        yield return new WaitForSeconds(1f);
        //SceneManager.LoadScene(SceneManager.GetSceneByName(DeathScene));
    }
}
