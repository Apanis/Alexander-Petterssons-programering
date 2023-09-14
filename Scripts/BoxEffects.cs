using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEffects : MonoBehaviour
{
    [SerializeField] GameObject Roof;
    
    void FixedUpdate()
    {
        Roof.transform.Rotate(0, 0.04f, 0);
    }
}
