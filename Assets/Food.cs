using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static GameObject foodPrefab;
    public static Vector3 pos;
    public static AudioSource aud;
   
    // Use this for initialization
    void Start()
    {
        pos = transform.position;
        gameObject.SetActive(true);
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos;
    }


    public static void Respawn()
    {
        //int x = (int)Random.Range(WestBorder.position.x, EastBorder.position.x);
        //int y = (int)Random.Range(SouthBorder.position.y, NorthBorder.position.y);

        pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -3);
        aud.Play();
    }

}