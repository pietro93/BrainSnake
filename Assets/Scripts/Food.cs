/*
 * Code by Pietro Romeo
 * June 2017
 */

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
        transform.position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0, 0, 0); 
        gameObject.SetActive(true);
        aud = GetComponent<AudioSource>();
    }

    public void respawn()
    {
        transform.position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0, 0, 0);
        aud.Play();
    }

}