using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Gotcha : MonoBehaviour
{
    public GameObject LookAt;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject playerPos = GameObject.FindGameObjectWithTag("PlayerPos");
            playerPos.transform.SetParent(null);
            StartCoroutine(GotchaCor(collision));            
        }
    }

    IEnumerator GotchaCor(Collider2D collision)
    {
        yield return new WaitForSeconds(0.5f);
        Hub.PlayerStatus.AddCurrentHP(-150);
        //Destroy(collision.attachedRigidbody);
    }


}
