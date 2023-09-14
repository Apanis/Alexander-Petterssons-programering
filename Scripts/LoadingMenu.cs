using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu : MonoBehaviour
{
    public Image circle1;
    public Image circle2;
    public Image circle3;
    private bool ReCharge = true;

    void Update()
    {
        if(ReCharge)
            StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        ReCharge = false;
        yield return new WaitForSeconds(0.1f);
        circle1.color = Color.white;
        circle2.color = Color.grey;
        circle3.color = Color.grey;
        yield return new WaitForSeconds(0.1f);
        circle1.color = Color.grey;
        circle2.color = Color.white;
        circle3.color = Color.grey;
        yield return new WaitForSeconds(0.1f);
        circle1.color = Color.grey;
        circle2.color = Color.grey;
        circle3.color = Color.white;
        ReCharge = true;
    }
}
