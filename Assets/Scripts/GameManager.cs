/*
 * Code by Pietro Romeo
 * June 2017
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Text ScoreTxt;
    public Text DeathsTxt;
    public Text HighscoreTxt;
    public Tail tail;
    private int score = 0, deaths = 0, highscore = 0;

    public void Start()
    {
        Obstacle.register();
    }

    [ClientRpc]
    public void RpcUpdateScore(int i)
    {
        score += i;
        tail.Grow(15);
        StartCoroutine(Tail.Flash());
        ScoreTxt.text = "Score: " + score;
        GetComponents<AudioSource>()[0].Play();
    }

    [ClientRpc]
    public void RpcUpdateDeath()
    {
        deaths += 1;
        DeathsTxt.text = "Deaths: " + deaths;

        if (score > highscore) highscore = score;
        HighscoreTxt.text = "Highscore: " + highscore;

        score = 0;
        ScoreTxt.text = "Score: " + score;
    }

    [ClientRpc]
    public void RpcSetTailPoint(Vector3 pos)
    {
        tail.SetPoint(pos);
    }

    [ClientRpc]
    public void RpcResetTail()
    {
        tail.GetComponent<Tail>().reset();
    }

    [ClientRpc]
    public void RpcAddBomb()
    {
        Obstacle.deleteBombs = false;
        Obstacle.numBombs++;
        if(NetworkServer.active) Obstacle.AddBomb();
    }

    [ClientRpc]
    public void RpcPlay(int sound)
    {
        GetComponents<AudioSource>()[sound-1].Play();
    }

    [ClientRpc]
    public void RpcStop(int sound)
    {
        GetComponents<AudioSource>()[sound - 1].Stop();
    }

    [ClientRpc]
    public void RpcSetPitch(float value)
    {
        AudioSource aud = GetComponents<AudioSource>()[3];
        aud.pitch = 1 + value;
    }

    public int getScore()
    {
        return score;
    }
}
