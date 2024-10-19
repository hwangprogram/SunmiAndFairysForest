using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_TmpForStage10 : MonoBehaviour
{
    public GameObject[] movingPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < movingPlatform.Length; i++)
        {
            movingPlatform[i].SetActive(true);
        }
        Destroy(this.gameObject);
    }
}
