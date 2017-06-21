using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class button : MonoBehaviour {
    private string gameMode;
    private Behaviour halo;
    private AudioSource aud;

    // Use this for initialization
    void Start () {
        gameMode = gameObject.name;
        halo = (Behaviour)GetComponent("Halo");
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseOver()
    {
        halo.enabled = true;
    }

    private void OnMouseExit()
    {
        halo.enabled = false;
    }

    private void OnMouseDown()
    {
        if (gameMode == "GameButton")
            GameObject.FindObjectOfType<GameManager>().StartGame();
        else
            GameObject.FindObjectOfType<GameManager>().StartTraining();
        aud.Play();
    }

}
