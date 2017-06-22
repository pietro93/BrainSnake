using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private Text txt;
    public static int score;

	// Use this for initialization
	void Start () {

        score = 0;
        txt = GetComponent<Text>();
        txt.text = "Score: " + score;
		
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

}
