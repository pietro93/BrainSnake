/*
 * Code by Pietro Romeo
 * June 2017
 */

using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private Text txt;
    private static int score;

	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        txt.text = "Score: " + score;
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
