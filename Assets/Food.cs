using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static GameObject foodPrefab;
    public static Vector3 pos;
    public static AudioSource audio;
   
    // Use this for initialization
    void Start()
    {
        pos = transform.position;
        gameObject.SetActive(true);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos;
    }


    public static void Respawn()
    {
        pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -3);
        audio.Play();
    }

}