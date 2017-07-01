/*
 * Code by Pietro Romeo
 * June 2017
 */

using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour
{

    public static GameObject obstaclePrefab;
    public static AudioSource aud;
    private static Vector3 pos;
    private static SpriteRenderer spr;
    private static Transform obs;
    private CircleCollider2D col;
    private bool active;
    private int id;


    // Use this for initialization
    void Start()
    {
        id = Score.getScore();
        aud = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
        active = false;
        transform.position = new Vector3(-99f, -99f, -3);

    }

    private void Update()
    {
        if ((Score.getScore() - 150 == id) && !active)
        {
            Instantiate(gameObject);
            StartCoroutine(Flash());
            active = true;
        }

    }

    private void respawn()
    {
        pos = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0, 0, 0);
        if (pos == Snake.Position())
            respawn();
        transform.position = pos;
        StartCoroutine(Flash());
    }

    private IEnumerator<WaitForSeconds> Flash()
    {
        col.enabled = false;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f / (i + 5));
            spr.enabled = false;
            yield return new WaitForSeconds(1f / (i + 5));
            spr.enabled = true;
        }
        col.enabled = true;
        yield return new WaitForSeconds(3f);
        col.enabled = false;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f / (i + 5));
            spr.enabled = false;
            yield return new WaitForSeconds(1f / (i + 5));
            spr.enabled = true;
        }
        col.enabled = false;
        respawn();
    }

    public static void Play()
    {
        aud.Play();
    }
}