using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAI : Enemy
{
    public GameObject arrowPrefab;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private float bulletSpeed = 5f;

    protected override void Start()
    {
        base.Start();
        lastAttackTime = -attackCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (isAttackPosition && Time.time >= lastAttackTime + attackCooldown &&
            !Hub.PlayerStatus.isFainting)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        lastAttackTime = Time.time;
        //Debug.Log("ArrowAI 공격!");

        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
        
        StartCoroutine(ArrowCor());
    }

    IEnumerator ArrowCor()
    {
        isAttackPosition = true;
        Hub.SFXManager.arrowAttack.Play();
        yield return new WaitForSeconds(0.6f);
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        direction.x = direction.x < 0 ? -1f : 1f;
        direction.y = 0;
        arrow.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        isAttackPosition = false;
    }



}
