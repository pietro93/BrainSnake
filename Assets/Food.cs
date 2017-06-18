using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static GameObject foodPrefab;
    public static Vector3 pos;
   
    // Use this for initialization
    void Start()
    {
        pos = transform.position;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos;
    }


    public static void Respawn()
    {
        pos = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
    }

    public static IEnumerator<WaitForSeconds> Spawn()
    {
        yield return new WaitForSeconds(2f);
        GameObject food = (GameObject)Instantiate(Resources.Load("Food"), Random.insideUnitCircle, Quaternion.identity );
    }
}