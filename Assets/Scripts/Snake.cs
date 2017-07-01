/*
 * BrainSnake code by
 * Pietro Romeo & Marc van Almkerk
 * June 2017
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Snake : MonoBehaviour
{
    public static string THETA_LEFT  = "THETA_LEFT";
    public static string THETA_RIGHT = "THETA_RIGHT";
    public static string OFF = "OFF";

    public float speed = 1.5f;
    public float rotationSpeed = 2.5f;
    private bool death = false;
    private AudioSource aud;

    private float orgRot = 0;
    private string direction = OFF;
    private bool increasing;
    private float rot;
    private float holdTimer = 0;

    private bool isConnected = false;

    public GameManager GM;
    public GameObject tail;
    public GameObject food;

    private void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isConnected && !death && direction == OFF)//only execute when client is connected
        {
            //move snake forward
            transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        }
        else if (direction != OFF)
        {
            //in rotation mode

            //change the rotation of the snake
            if (holdTimer == 0)
            {
                if (increasing) { transform.Rotate(0, 0, rotationSpeed); rot += Mathf.Abs(rotationSpeed); }
                else { transform.Rotate(0, 0, -rotationSpeed); rot -= Mathf.Abs(rotationSpeed); }

                //change pitch accordangly
                GM.RpcSetPitch((rot / 90));
            }

            //change direction when 90 degrees are reached.
            if (this.rot >= 90 || this.rot <= 0)
            {
                if(holdTimer == 0) holdTimer = 30;
                holdTimer--;
                if (holdTimer == 0)
                {
                    if (this.rot >= 90)
                    {
                        increasing = false;
                        if (direction == THETA_LEFT) transform.rotation = Quaternion.Euler(0, 0, orgRot + 90);
                        else transform.rotation = Quaternion.Euler(0, 0, orgRot - 90);
                    }
                    else
                    {
                        increasing = true;
                        transform.rotation = Quaternion.Euler(0, 0, orgRot);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (NetworkServer.active)
        {
            if (col.tag == "food")
            {
                food.GetComponent<Food>().respawn();
                GM.RpcUpdateScore(50);

                if (Mathf.Floor(GM.getScore() / 150) > Obstacle.numBombs)
                {
                    GM.RpcAddBomb();
                }
            }
            else if (col.tag == "Wall")
            {
                float rotZ = transform.eulerAngles.z;
                float rotZWall = col.gameObject.transform.eulerAngles.z;
                transform.rotation = Quaternion.Euler(0, 0, rotZWall - rotZ);
            }
            else if (col.tag == "Wall_2")
            {
                float rotZ = transform.eulerAngles.z;
                transform.rotation = Quaternion.Euler(0, 0, -rotZ);
            }
            else
            {
                TurnOffRotationMode();

                if (col.tag == "obstacle") {
                    GM.RpcPlay(2);
                }
                else
                {
                    GM.RpcPlay(3);
                }

                resetPlayer();
                GM.RpcUpdateDeath();

            }
        }
    }

    //rotate character in the left direction in percentage of the max rotation speed
    public void rotateLeft(float value = 1)
    {
        //Debug.Log("Value: " + value);
        transform.Rotate(0, 0,  Mathf.Abs(rotationSpeed*3) * value);
    }

    //rotate character in the right direction in percentage of the max rotation speed
    public void rotateRight(float value = 1)
    {
        //Debug.Log("Value: " + value);
        transform.Rotate(0, 0, -Mathf.Abs(rotationSpeed*3) * value);
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
        death = false;
        Obstacle.deleteBombs = true;
        rotateRandom();

        if(NetworkServer.active) GM.RpcResetTail();
    }

    //set snake in rotation mode
    public void TurnOnRotationMode(string direction, bool on = true)
    {
        if (direction == this.direction || this.direction == OFF)
        {
            if (!on)//redundant request, turn of rotation mode
            {
                TurnOffRotationMode();
                return;
            }

            //init rotation mode
            orgRot = transform.eulerAngles.z;
            this.direction = direction;
            increasing = true;
            rot = 0;
            if (direction == THETA_LEFT) rotationSpeed = Mathf.Abs(rotationSpeed);
            else if (direction == THETA_RIGHT) rotationSpeed = -Mathf.Abs(rotationSpeed);

            //play audio
            GM.RpcPlay(4);
        }
    }

    //let snake go out rotation mode
    public void TurnOffRotationMode()
    {
        direction = OFF;
        GM.RpcSetPitch(1);
        GM.RpcStop(4);
    }

    public void rotateRandom()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.value*360);
    }
}