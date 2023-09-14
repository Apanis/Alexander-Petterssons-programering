using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EyeBoost : MonoBehaviour
{

    public static EyeBoost Instance;
    [SerializeField]MeshRenderer Mesh;

    void Start()
    {
        Mesh = gameObject.GetComponent<MeshRenderer>();
        Instance = this;
        Mesh.enabled = true;
    }
    public void LinesOn()
    {
        Mesh.enabled = true;
    }
    public void LinesOff()
    {
        Mesh.enabled = false;
        Debug.Log("Lines off");
    }
}
