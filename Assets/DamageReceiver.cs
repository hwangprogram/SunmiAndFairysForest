using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float force;
    //PlayerStatus���� isDamageFree�� ������

    // TriggerStay�� ó���ϴϱ� ������ ���Ǵ� ��찡 �־, �� ���� ���ǵ��� 
    private bool hitOnce;



    // Got Hit ���⿡ �ϴ�
    public void GotHit()
    {
        animator.SetBool("isEnergy", false);
        Hub.SFXManager.gathingEnergyEnter.Stop();
        animator.SetBool("isHit", true);
        Hub.SFXManager.playerGotHit.Play();
        StartCoroutine(GotHitCor());
    }

    IEnumerator GotHitCor()
    {
        // isHit�� ���̸�ŭ�� 0.6f�� 0.3f�� ����.
        yield return new WaitForSeconds(0.6f);
        rb.velocity = Vector3.zero;

        // ���������� �׳� �ǰ��ΰ���
        if (Hub.PlayerStatus.isFainting)
        {
            Hub.SFXManager.playerFainted.Play();
            animator.SetBool("isFainting", true);
            yield return new WaitForSeconds(2f);            
            animator.SetBool("isHit", false);
            Hub.UIManager.Fainting();
        }
        else
        {
            Hub.PlayerStatus.currentPlayerState = PlayerState.free;
            animator.SetBool("isHit", false);
            animator.SetBool("isDamageFree", true);            
            yield return new WaitForSeconds(1.2f);
            animator.SetBool("isDamageFree", false);
            Hub.PlayerStatus.isDamageFree = false;
        }
        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // �ǰ�/�ǰݸ��/�������� ���� �����°� ���� ��������
        // Arrow destroy�� ������ arrow.cs���� ó���ؼ�         
        // ���⿡���� �׳� isDamageFree == true ���� ������
        // �ƿ� �ǰݹ��� �ʵ��� �� ����. 
        if ((collision.collider.tag == "EnemyShot0.5" |
            collision.collider.tag == "EnemyShot1.0" |
            collision.collider.tag == "EnemyShot1.5") & 
            !Hub.PlayerStatus.isDamageFree & 
            !hitOnce)
        {
            hitOnce = true;
            Hub.PlayerStatus.isDamageFree = true;
            Hub.a_PlayerUI.SkillListCancel();
            Hub.PlayerStatus.currentPlayerState = PlayerState.gotHit;
            switch (collision.collider.tag)
            {
                case "EnemyShot0.5":
                    Hub.PlayerStatus.AddCurrentHP(-0.5);
                    break;
                case "EnemyShot1.0":
                    Hub.PlayerStatus.AddCurrentHP(-1);
                    break;
                case "EnemyShot1.5":
                    Hub.PlayerStatus.AddCurrentHP(-1.5);
                    break;
                default:
                    print("DamageReceiver.cs");
                    break;
            }
            
            Vector2 direction = (this.transform.position - collision.transform.position).normalized;
            direction.y = 1;
            // ���� �� �ص� �� �� 
            // rb.velocity = Vector3.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            // �ϴ� ���⼭ ���� GotHit() ���ص� AddCurrentHP()�� SetCurrentHP()�ϸ� GotHit() �ǵ��� �� ����.
            // a_GotCha.cs������ GotHit() ��� �ǰ� �߰��� ���� �ִµ� �Ź� �߰��ϱ⺸�� PlayerStatus.CurrentHP���� �ϵ��� ��. 
            // GotHit();
            StartCoroutine(HitOnceCor());
        }      
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "EnemyShot0.5" |
            collision.tag == "EnemyShot1.0" |
            collision.tag == "EnemyShot1.5") &
            !Hub.PlayerStatus.isDamageFree &
            !hitOnce)
        {
            hitOnce = true;
            Hub.PlayerStatus.isDamageFree = true;
            Hub.a_PlayerUI.SkillListCancel();
            Hub.PlayerStatus.currentPlayerState = PlayerState.gotHit;
            switch (collision.tag)
            {
                case "EnemyShot0.5":
                    Hub.PlayerStatus.AddCurrentHP(-0.5);
                    break;
                case "EnemyShot1.0":
                    Hub.PlayerStatus.AddCurrentHP(-1);
                    break;
                case "EnemyShot1.5":
                    Hub.PlayerStatus.AddCurrentHP(-1.5);
                    break;
                default:
                    print("DamageReceiver.cs");
                    break;
            }

            Vector2 direction = (this.transform.position - collision.transform.position).normalized;
            direction.y = 1;
            // ���� �� �ص� �� �� 
            // rb.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            // �ϴ� ���⼭ ���� GotHit() ���ص� AddCurrentHP()�� SetCurrentHP()�ϸ� GotHit() �ǵ��� �� ����.
            // a_GotCha.cs������ GotHit() ��� �ǰ� �߰��� ���� �ִµ� �Ź� �߰��ϱ⺸�� PlayerStatus.CurrentHP���� �ϵ��� ��. 
            // GotHit();
            StartCoroutine(HitOnceCor());
        }
    }


    IEnumerator HitOnceCor()
    {
        yield return new WaitForSeconds(0.2f);
        hitOnce = false;
    }












}
