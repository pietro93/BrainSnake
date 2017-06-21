/*
 * Code by Marc van Almkerk
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerCtrl : NetworkBehaviour {

    private Snake Character;
    private LSLcontroller modifier;
    private GameObject Screens;

    private GameObject deathScreen;

    private float smoothVal = 0.0f;
    private float rawVal = 0.0f;
    private bool AlphaDetected = false;
    private bool ModeChanged = false;

    public float smoothing = 0.8f;
    public float threshold = 0.6f;
    public float offset = 0.5f;

    public void Start () {
        Character = GameObject.Find("Snake").GetComponent<Snake>();
        Screens = GameObject.Find("Screens");

        Screens.transform.GetChild(0).gameObject.SetActive(false);

        modifier = GetComponent<LSLcontroller>();

        //Let server know about connection
        if (!NetworkServer.active) CmdConnect();

        //set graph dimensions
        /*Graph.YMin = 0f;
        //Graph.YMax = 0.7f;

        //Graph.channel[0].isActive = true; //axis
        Graph.channel[1].isActive = true; //raw data
        Graph.channel[1].line = false;
        Graph.channel[2].isActive = true; //smoothed data
        Graph.channel[3].isActive = true; //smoothed data over threshold
        Graph.channel[4].isActive = true; //threshold*/
    }

    // Update is called once per frame
    public void Update()
    {

        //UPDATE GRAPH
        //Graph.channel[0].Feed(0); //axis line (0)
        /*Graph.channel[1].Feed(rawVal); //feed raw data
        if (smoothVal > threshold) //update smoothed data
        {
            //if over threshold make line green...
            Graph.channel[2].Feed(smoothVal);
            Graph.channel[3].Feed(threshold);
        }
        else
        {
            //otherwise keep it yellow
            Graph.channel[2].Feed(smoothVal);
            Graph.channel[3].Feed(smoothVal);
        }
        Graph.channel[4].Feed(threshold); //threshold line*/

        //UPDATE PLAYER DATA
        if (isLocalPlayer && modifier.value > 0)
        {
            //update raw data and smoothed data (IIR)
            rawVal = Mathf.Abs(modifier.value - offset);
            smoothVal = smoothVal * smoothing + rawVal * (1 - smoothing);
            if (smoothVal >= threshold)
            {   AlphaDetected = true;   }

            if (AlphaDetected && !ModeChanged)
            {
                if (NetworkServer.active) RotateMode(-1);
                else CmdRotateMode(1);
                ModeChanged = true;
            }
            else if(!AlphaDetected && ModeChanged)
            {
                if (NetworkServer.active) RotateMode(-1);
                else CmdRotateMode(1);
                ModeChanged = false;
            }

            modifier.value = -1;
        }

        if (isLocalPlayer)
        {
            if (NetworkServer.active)// is server
            {
                //rotate left if player is server
                if (Input.GetKey("a")) Character.rotateLeft();
                //rotate right if player is server
                else if (Input.GetKey("d")) Character.rotateRight();
                else if (Input.GetKeyDown("q")) RotateMode(-1);
                else if (Input.GetKeyDown("e")) RotateMode(1);

                //reset player if neccesary - SERVER ONLY
                if (Input.GetKeyDown("r")) Character.resetPlayer();
                //start player if neccesary - SERVER ONLY
                else if (Input.GetKeyDown("t")) Character.OnConnected();
            }
            else // is client
            {
                //rotate right if player is client
                if (Input.GetKey("d")) CmdRotateRight(1);
                else if (Input.GetKeyDown("e")) CmdRotateMode(1);
            }
        }
    }

    [Command]
    //Allows client to rotate in the left direction
    private void CmdRotateLeft(float value)
    {
        Character.rotateLeft(value);
    }

    [Command]
    //Allows client to rotate in the right direction
    private void CmdRotateRight(float value)
    {
        Character.rotateRight(value);
    }

    private void RotateMode(float value)
    {
        if (value == -1) Character.TurnOnRotationMode(Snake.THETA_LEFT);
        else if (value == 1) Character.TurnOnRotationMode(Snake.THETA_RIGHT);
    }

    [Command]
    //Allows client to rotate in the right direction
    private void CmdRotateMode(float value)
    {
        if(value == -1) Character.TurnOnRotationMode(Snake.THETA_LEFT);
        else if(value == 1) Character.TurnOnRotationMode(Snake.THETA_RIGHT);
    }

    [Command]
    //Allows client to let the server know it is connected
    private void CmdConnect()
    {
        Character.OnConnected();
    }
}
