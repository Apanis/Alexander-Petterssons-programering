using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public Transform[] target;
    public float speed;
    private int current;
    public Transform MiddlePart;
    public Camera mainCamera;
    void Start()
    {
        mainCamera.enabled = false;
    }
    void Update()
    {
        if(!playerManager.isAlive)
        {
            mainCamera.enabled = true;
            mainCamera.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
        }
            
        transform.LookAt(MiddlePart);
        if(transform.position != target[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else
            current = (current + 1) % target.Length;
    }
}
