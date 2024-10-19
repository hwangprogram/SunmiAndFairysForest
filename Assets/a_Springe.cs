using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Springe : MonoBehaviour
{
    public GameObject[] objects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for(int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i]);
            }
        }
    }
}
