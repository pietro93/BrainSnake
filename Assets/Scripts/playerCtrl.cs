/*
 * Code by Marc van Almkerk
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerCtrl : NetworkBehaviour {

    private Snake Character;
    private GameObject Screens;
    private GameObject manager;

    private GameObject deathScreen;
    private bool ModeChanged = false;

    public void Start () {
        Character = GameObject.Find("Snake").GetComponent<Snake>();
        Screens = GameObject.Find("Screens");
        manager = GameObject.Find("NetworkManager");
        manager.GetComponent<NetworkManagerHUD>().showGUI = false;

        Screens.transform.GetChild(0).gameObject.SetActive(false);

        //Let server know about connection
        if (!NetworkServer.active) CmdConnect();
        else Character.rotateRandom();
    }

    // Update is called once per frame
    public void Update()
    {
        //UPDATE PLAYER DATA
        if (isLocalPlayer)
        {
            if (AlphaWaveDetection.AlphaDetected && !ModeChanged)
            {
                if (NetworkServer.active) CmdRotateMode(-1,true);
                else CmdRotateMode(1,true);
                ModeChanged = true;
            }
            else if (!AlphaWaveDetection.AlphaDetected && ModeChanged)
            {
                if (NetworkServer.active) CmdRotateMode(-1,false);
                else CmdRotateMode(1, false);
                ModeChanged = false;
            }

            if (NetworkServer.active)// is server
            {
                //rotate left if player is server
                if (Input.GetKey("a")) Character.rotateLeft();
                //rotate right if player is server
                else if (Input.GetKey("d")) Character.rotateRight();
                else if (Input.GetKeyDown("q")) CmdRotateMode(-1, true);
                else if (Input.GetKeyDown("e")) CmdRotateMode(1, true);

                //reset player if neccesary - SERVER ONLY
                if (Input.GetKeyDown("r")) Character.resetPlayer();
                //start player if neccesary - SERVER ONLY
                else if (Input.GetKeyDown("t")) Character.OnConnected();
            }
            else // is client
            {
                //rotate right if player is client
                if (Input.GetKey("d")) CmdRotateRight(1);
                else if (Input.GetKeyDown("e")) CmdRotateMode(1, true);
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

    [Command]
    //Allows client to rotate in the right direction
    private void CmdRotateMode(float value, bool on)
    {
        if (value == -1) Character.TurnOnRotationMode(Snake.THETA_LEFT, on);
        else if (value == 1) Character.TurnOnRotationMode(Snake.THETA_RIGHT, on);
    }

    [Command]
    //Allows client to let the server know it is connected
    private void CmdConnect()
    {
        Character.OnConnected();
    }
}
