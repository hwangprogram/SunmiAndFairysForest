using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_DontChaseFox : MonoBehaviour
{
    public Animator animator;
    private float moveSpeed = 0.001f;

    void Start()
    {
        if (Hub.PlayerStatus.currentDirection == Direct.left) this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Hub.SFXManager.dontChaseFox.Play();
        StartCoroutine(Skill());
    }

    void FixedUpdate()
    {
        this.transform.position = new Vector2(this.transform.position.x + moveSpeed, this.transform.position.y);
    }

    IEnumerator Skill()
    {

        yield return new WaitForSeconds(2.5f);
        animator.SetBool("isOff", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") animator.SetBool("isGiveDamage", true);
        // if (collision.tag != "ThrowEmOut")
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") animator.SetBool("isGiveDamage", false);
    }

}
