/*
 * BrainSnake code by
 * Pietro Romeo & Marc van Almkerk
 * June 2017
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Obstacle : MonoBehaviour
{
    public static int numBombs = 0;
    public static bool deleteBombs = false;

    private GameObject snake;
    private Animator Ani;
    private bool respawned = false;

    // Use this for initialization
    void Start()
    {
        Ani = GetComponent<Animator>();
        transform.position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0, 0, 0);

        snake = GameObject.Find("Snake");

        InvokeRepeating("startFlash", 0f, 5f);
    }

    private void Update()
    {
        if (Ani.GetCurrentAnimatorStateInfo(0).IsName("Respawn") && !respawned)
        {
            Ani.ResetTrigger("Flashing");
            Ani.SetTrigger("Idle");
            respawn();
        }
        else if(Obstacle.deleteBombs)
        {
            Destroy(gameObject);
        }
    }

    private void startFlash()
    {
        Ani.SetTrigger("Flashing");
        Ani.ResetTrigger("Idle");
        respawned = false;
    }

    private void respawn()
    {
        Vector3 pos = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f), -3) + new Vector3(0, 0, 0);
        if (pos == snake.transform.position)
        {
            respawn();
            return;
        }
        transform.position = pos;
        respawned = true;
    }

    public static void AddBomb()
    {
        GameObject prefab = (GameObject)Resources.Load("Obstacle", typeof(GameObject));
        GameObject bomb = Instantiate(prefab);
        NetworkServer.Spawn(bomb);
    }

    public static void register()
    {
        GameObject prefab = (GameObject)Resources.Load("Obstacle", typeof(GameObject));
        ClientScene.RegisterPrefab(prefab);
    }
}