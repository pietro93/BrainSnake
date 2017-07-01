/*
 * BrainSnake code by
 * Pietro Romeo & Marc van Almkerk
 * June 2017
 */

using UnityEngine;

public class Food : MonoBehaviour
{
    public static GameObject foodPrefab;
   
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(true);
    }

    public void respawn()
    {
        transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -3);
    }

}