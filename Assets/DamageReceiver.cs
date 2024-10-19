using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float force;
    //PlayerStatus에서 isDamageFree를 가져옴

    // TriggerStay로 처리하니까 여러번 사용되는 경우가 있어서, 한 번만 사용되도록 
    private bool hitOnce;



    // Got Hit 여기에 일단
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
        // isHit의 길이만큼인 0.6f에 0.3f를 더함.
        yield return new WaitForSeconds(0.6f);
        rb.velocity = Vector3.zero;

        // 쓰러졌는지 그냥 피격인건지
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
        // 피격/피격모션/데미지를 따로 나누는게 가장 좋겠지만
        // Arrow destroy는 각각의 arrow.cs에서 처리해서         
        // 여기에서는 그냥 isDamageFree == true 되지 않으면
        // 아예 피격받지 않도록 해 놓음. 
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
            // 굳이 안 해도 될 듯 
            // rb.velocity = Vector3.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            // 일단 여기서 따로 GotHit() 안해도 AddCurrentHP()나 SetCurrentHP()하면 GotHit() 되도록 해 놓음.
            // a_GotCha.cs에서도 GotHit() 써야 되고 추가될 수도 있는데 매번 추가하기보단 PlayerStatus.CurrentHP에서 하도록 함. 
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
            // 굳이 안 해도 될 듯 
            // rb.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            // 일단 여기서 따로 GotHit() 안해도 AddCurrentHP()나 SetCurrentHP()하면 GotHit() 되도록 해 놓음.
            // a_GotCha.cs에서도 GotHit() 써야 되고 추가될 수도 있는데 매번 추가하기보단 PlayerStatus.CurrentHP에서 하도록 함. 
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
