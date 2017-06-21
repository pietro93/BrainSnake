using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static GameObject foodPrefab;
    public static AudioSource aud;
   
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(true);
        aud = GetComponent<AudioSource>();
    }

    public void respawn()
    {
        transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -3);
        aud.Play();
    }

}