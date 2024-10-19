using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Springe2 : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject[] enemies;
    public GameObject key;
    public GameObject movingPlatform;

    private void FixedUpdate()
    {
        int tmpInt = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null) tmpInt++;
        }
        if (tmpInt == enemies.Length)
        {
            key.SetActive(true);
            movingPlatform.GetComponent<MovingPlatform>().isPause = false;           

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i]);
            }
        }
        Destroy(this.GetComponent<BoxCollider2D>());
    }
}
