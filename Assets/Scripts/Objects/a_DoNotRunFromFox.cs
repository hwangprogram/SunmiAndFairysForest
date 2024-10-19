using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class a_DoNotRunFromFox : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (Hub.PlayerStatus.currentDirection == Direct.left) moveSpeed = -moveSpeed;
        Hub.SFXManager.dontEscapeFromFox.Play();
        StartCoroutine(Skill());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector2(this.transform.position.x + moveSpeed, this.transform.position.y);
    }

    IEnumerator Skill()
    {
        
        yield return new WaitForSeconds(3f);


        Destroy(this.gameObject);
    }
}
