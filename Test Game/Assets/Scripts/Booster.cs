using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float maxSpeedMultiplier;
    public float speedMultiplier;

    PlayerMovement playerScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript = other.GetComponent<PlayerMovement>();

            playerScript.maxForwardSpeed = (int)(playerScript.maxForwardSpeed * maxSpeedMultiplier);
            playerScript.forwardSpeed *= speedMultiplier;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.maxForwardSpeed = (int)(playerScript.maxForwardSpeed / maxSpeedMultiplier);
            playerScript.forwardSpeed /= speedMultiplier;
        }
    }
}
