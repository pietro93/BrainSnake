using UnityEngine;
using UnityEngine.UI;


public class Snake : MonoBehaviour
{
    public static string THETA_LEFT  = "THETA_LEFT";
    public static string THETA_RIGHT = "THETA_RIGHT";
    public static string OFF = "OFF";

    public float speed = 1.5f;
    public float rotationSpeed = 2.5f;
    private bool death = false;
    private Vector3 pos;
    private AudioSource aud;

    private float orgRot = 0;
    private string direction = OFF;
    private bool increasing;
    private float rot;

    private bool isConnected = false;
    private bool rotationActive = false;

    public GameObject deathScreen;
    public GameObject tail;
    public GameObject food;

    private void Start()
    {
        deathScreen.SetActive(true);
        deathScreen.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
    }

    void FixedUpdate()
    {
        if (isConnected && !death && !rotationActive)//only execute when client is connected
        {
            transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        }
        else if (rotationActive)
        {
            if (direction == THETA_LEFT)
            {
                if (increasing) { transform.Rotate(0, 0, rotationSpeed); rot += rotationSpeed; }
                else { transform.Rotate(0, 0, -rotationSpeed); rot -= rotationSpeed; }

                if (this.rot >= 90)
                {
                    increasing = false;
                    transform.rotation = Quaternion.Euler(0, 0, orgRot + 90);
                }
                else if (this.rot <= 0)
                {
                    increasing = true;
                    transform.rotation = Quaternion.Euler(0, 0, orgRot);
                }
            }
            else if (direction == THETA_RIGHT)
            {
                if (increasing) { transform.Rotate(0, 0, -rotationSpeed); rot += rotationSpeed; }
                else { transform.Rotate(0, 0, +rotationSpeed); rot -= rotationSpeed; }

                if (this.rot >= 90)
                {
                    increasing = false;
                    transform.rotation = Quaternion.Euler(0, 0, orgRot - 90);
                }
                else if (this.rot <= 0)
                {
                    increasing = true;
                    transform.rotation = Quaternion.Euler(0, 0, orgRot);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "food")
        {
            //Tail.Grow(10);
            //StartCoroutine(Tail.Flash());
            food.GetComponent<Food>().respawn();
        }
        else if (col.tag == "Wall")
        {
            float rotZ = transform.eulerAngles.z;
            float rotZWall = col.gameObject.transform.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, rotZWall-rotZ);
        }
        else if (col.tag == "Wall_2")
        {
            float rotZ = transform.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, -rotZ);
        }
        else
        {
            TurnOffRotationMode();

            aud = GetComponent<AudioSource>();
            aud.Play();
            death = true;

            deathScreen.GetComponent<Image>().CrossFadeAlpha(1, 1, false);
        }
    }

    //rotate character in the left direction in percentage of the max rotation speed
    public void rotateLeft(float value = 1)
    {
        Debug.Log("Value: " + value);
        transform.Rotate(0, 0, rotationSpeed * value);
    }

    //rotate character in the right direction in percentage of the max rotation speed
    public void rotateRight(float value = 1)
    {
        Debug.Log("Value: " + value);
        transform.Rotate(0, 0, -rotationSpeed * value);
    }

    //call when client is connected
    public void OnConnected()
    {
        isConnected = true;
    }

    //reset character to starting position
    public void resetPlayer()
    {
        TurnOffRotationMode();
        transform.position = new Vector3();
        transform.rotation = new Quaternion();
        deathScreen.GetComponent<Image>().CrossFadeAlpha(0, 1, false);
        //tail.GetComponent<Tail>().reset();
        death = false;
    }

    public void TurnOnRotationMode(string direction)
    {
        if (direction == this.direction || this.direction == OFF)
        {
            if (rotationActive)
            {
                TurnOffRotationMode();
                return;
            }
            rotationActive = true;
            orgRot = transform.eulerAngles.z;
            this.direction = direction;
            increasing = true;
            rot = 0;
        }
    }

    public void TurnOffRotationMode()
    {
        rotationActive = false;
        direction = OFF;
    }
}