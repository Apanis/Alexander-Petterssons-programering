using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMenuRotator : MonoBehaviour
{
    
    void Update()
    {
        transform.Rotate(0.002f, 0.002f, 0.002f);
    }
}
