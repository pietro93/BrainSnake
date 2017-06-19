using UnityEngine;


public class Snake : MonoBehaviour
{

    public float speed = 1.5f;
    public float rotationSpeed = 150f;
    float horizontal = 0f;
    private Vector3 pos;
    private AudioSource aud;


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        pos = transform.position;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        //the - in the front of horizontal is for reverse control
        transform.Rotate(Vector3.forward * -horizontal * rotationSpeed * Time.fixedDeltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        Debug.Log("collider: " + col.gameObject.name);
        if (col.tag == "food")
        {
            Tail.Grow(10);
            StartCoroutine(Tail.Flash());
            Food.Respawn();
            GameManager.updateScore(5);
        }
        else 
        {
            aud = GetComponent<AudioSource>();
            aud.Play();
            speed = 0f;
            rotationSpeed = 0f;
            GameObject.FindObjectOfType<GameManager>().EndGame();
        }
    }
}