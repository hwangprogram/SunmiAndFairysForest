using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_TriggerForSpringe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<SwordAI>().enabled = true;
        }
    }
}
