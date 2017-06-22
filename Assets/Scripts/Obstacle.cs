/*
 * Code by Pietro Romeo
 * June 2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public static GameObject foodPrefab;
    public static AudioSource aud;
    private SpriteRenderer sprite;
    private Vector3 pos;
    private bool wait;


    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(true);
        aud = GetComponent<AudioSource>();
        wait = false;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (!wait)
            StartCoroutine(Flash());

    }

    public void respawn()
    {
        pos = new Vector3( Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0,0,0);
        if (pos == Snake.Position())
            respawn();
        transform.position = pos;
        sprite.enabled = true;
        //aud.Play();
    }

    private IEnumerator<WaitForSeconds> Flash()
    {
        wait = true;
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 3; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(1f / (i+3));
            sprite.enabled = true;
            yield return new WaitForSeconds(1f / (i+3));
            sprite.enabled = false;
        }
        respawn();
        wait = false;
    }

    }