/*
 * Code by Marc van Almkerk
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterCtrl : MonoBehaviour {

    public float movementSpeed = 0.05f;
    public float rotationSpeed = 30.00f;
    public Camera viewport;

    public Text valueText;

    private bool isConnected = false;

    // Update is called once per frame
    private void FixedUpdate () {
        if (isConnected)//only execute when client is connected
        {
            //move Character in forward direction
            transform.Translate(Vector3.up * movementSpeed, Space.Self);

            //check whether Character is still within the viewport, or relocate otherwise
            Vector3 screenPoint = viewport.WorldToViewportPoint(transform.position);
            if (screenPoint.x < 0) screenPoint.x += 1;
            else if (screenPoint.x > 1) screenPoint.x -= 1;
            if (screenPoint.y < 0) screenPoint.y += 1;
            else if (screenPoint.y > 1) screenPoint.y -= 1;

            transform.position = viewport.ViewportToWorldPoint(screenPoint);
        }
    }

    //rotate character in the left direction in percentage of the max rotation speed
    public void rotateLeft(float value = 1)
    {
        valueText.text = "Value: " + value;
        Debug.Log("left:" + rotationSpeed * value);
        transform.Rotate(0, 0, rotationSpeed * value);
        rotateRight(1 - value);
    }

    //rotate character in the right direction in percentage of the max rotation speed
    public void rotateRight(float value = 1)
    {
        Debug.Log("right:" + rotationSpeed * value);
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
        transform.position = new Vector3();
        transform.rotation = new Quaternion();
    }
}
