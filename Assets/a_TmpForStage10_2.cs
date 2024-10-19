using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_TmpForStage10_2 : MonoBehaviour
{
    public GameObject movingPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        movingPlatform.GetComponent<MovingPlatform>().isOneWaylock = true;
        movingPlatform.GetComponent<MovingPlatform>().isHopOnReusable = true;
        Destroy(this.gameObject);
    }
}
