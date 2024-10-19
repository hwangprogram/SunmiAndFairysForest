using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_turtle : MonoBehaviour
{
    public float moveSpeed;

    void Start()
    {
        if (Hub.PlayerStatus.currentDirection == Direct.left)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            moveSpeed = -moveSpeed;
        }            
    }

    private void FixedUpdate()
    {
        this.transform.position = new Vector2(this.transform.position.x + moveSpeed, this.transform.position.y);
    }
}
