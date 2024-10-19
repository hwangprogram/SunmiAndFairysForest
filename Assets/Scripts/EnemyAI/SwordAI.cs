using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAI : Enemy
{
    //public float attackRange = 2f;
    public GameObject fireSkillPrefab;
    public float attackCooldown = 1f;
    private float lastAttackTime;

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
        if (Vector2.Distance(transform.position, player.position) <= attackRange && Time.time >= lastAttackTime + attackCooldown &&
            !Hub.PlayerStatus.isFainting)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("SwordAI 공격!");

        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        // 플레이어에게 데미지를 주는 로직 추가
        StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        isAttackPosition = true;
        Hub.SFXManager.swordAttack.Play();
        yield return new WaitForSeconds(0.33f);
        fireSkillPrefab.SetActive(true);
        fireSkillPrefab.transform.localPosition = new Vector2(fireSkillPrefab.transform.localPosition.x, fireSkillPrefab.transform.localPosition.y + 0.01f);
        yield return new WaitForSeconds(0.47f);
        fireSkillPrefab.SetActive(false);
        fireSkillPrefab.transform.localPosition = new Vector2(fireSkillPrefab.transform.localPosition.x, fireSkillPrefab.transform.localPosition.y - 0.01f);
        isAttackPosition = false;
    }
}
