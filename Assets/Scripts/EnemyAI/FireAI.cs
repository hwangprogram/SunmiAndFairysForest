using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAI : Enemy
{
    public GameObject fireSkillPrefab;
    public float attackCooldown = 3f;
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
        if (Vector2.Distance(transform.position, player.position) <= attackRange && 
            Time.time >= lastAttackTime + attackCooldown &
            !Hub.PlayerStatus.isFainting)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("FireAI 공격!");

        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        // 스킬 발사
        // Instantiate(fireSkillPrefab, transform.position, Quaternion.identity);
        // 스킬에 데미지를 주는 스크립트 추가 필요
        StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        Hub.SFXManager.fireSpiritAttack.Play();
        fireSkillPrefab.SetActive(true);
        fireSkillPrefab.transform.localPosition = new Vector2(fireSkillPrefab.transform.localPosition.x, fireSkillPrefab.transform.localPosition.y + 0.01f);
        yield return new WaitForSeconds(0.5f);        
        fireSkillPrefab.SetActive(false);
        fireSkillPrefab.transform.localPosition = new Vector2(fireSkillPrefab.transform.localPosition.x, fireSkillPrefab.transform.localPosition.y - 0.01f);
    }

    

}
