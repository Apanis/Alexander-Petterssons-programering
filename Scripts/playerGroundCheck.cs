using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGroundCheck : MonoBehaviour
{
    playerController pController;
    void Awake()
    {
        pController = GetComponentInParent<playerController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(true);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(false);
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(true);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(true);
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(false);
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == pController.gameObject)
            return;
        pController.SetGrounded(true);
    }
}
