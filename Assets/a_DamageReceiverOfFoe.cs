using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    normal,
    fire
}

public enum Classification
{
    enemy,
    obstacle
}

public class a_DamageReceiverOfFoe : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int _HP;    
    public int HP
    {
        get { return _HP; }
        set
        {
            _HP = value;            
            if (_HP <= 0) Fainting();
        }
    }
    public int mobHP;
    public Classification MobClassification;
    public Type MobType;


    public Rigidbody2D rb;
    public float DontEscapeFromFoxForce = 200f;
    public float throwEmOutForce = 350f;
    public bool hitOnce = false;

    public SpriteRenderer[] everyRb;
    public Material defaultMatrial;
    public Material gotHitMatrial;

    //public Animator animator;
    // 움직임 잠시 멈추게 하려고 
    public Enemy enemy;
    


    // Start is called before the first frame update
    void Start()
    {
        HP = mobHP;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 물의 구체의 경우 
        if (collision.tag == "WaterCircle")
        {
            this.gameObject.GetComponent<Enemy>().moveSpeed = this.gameObject.GetComponent<Enemy>().moveSpeed * 0.5f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 피격 collider에 들어갔을 때 
        if ((collision.tag == "DontChaseFox" |
            collision.tag == "DontEscapeFromFox" |
            collision.tag == "WaterCircle" |
            collision.tag == "ThrowEmOut"))
        {
            
            switch (collision.tag)
            {
                case "DontChaseFox":
                    if (MobType != Type.fire & !hitOnce)
                    {
                        hitOnce = true;
                        HP = HP - 100;
                        gotHitReveal();
                        StartCoroutine(HitOnceCor(0.3f));
                    }                    
                    break;
                case "DontEscapeFromFox":
                    if (MobType != Type.fire & !hitOnce)
                    {
                        hitOnce = true;
                        HP = HP - 150;
                        gotHitReveal();
                        StartCoroutine(HitOnceCor(0.5f));
                    }

                    if (MobClassification == Classification.enemy && MobType != Type.fire)
                    {
                        // hitOnce가 true인 상태에서도 데미지는 입지 않아도 타격은 입어야 되니까 
                        enemy.isPauseMovement = true;
                        Vector2 tmpDirectionDontEscapeFromFox;
                        if (collision.GetComponent<a_DoNotRunFromFox>().moveSpeed < 0) tmpDirectionDontEscapeFromFox = new Vector2(-1, 0);
                        else tmpDirectionDontEscapeFromFox = new Vector2(1, 0);
                        rb.AddForce(tmpDirectionDontEscapeFromFox * DontEscapeFromFoxForce, ForceMode2D.Impulse);
                    }
                    
                    Destroy(collision.gameObject);                    

                    break;
                case "WaterCircle":
                    if (MobType == Type.fire & !hitOnce)
                    {
                        hitOnce = true;
                        HP = HP - 100;
                        gotHitReveal();
                        StartCoroutine(HitOnceCor(0.3f));
                    }                    
                    break;
                case "ThrowEmOut":
                    if (!hitOnce)
                    {
                        hitOnce = true;
                        HP = HP - 150;
                        gotHitReveal();
                        if (MobClassification == Classification.enemy)
                        {
                            enemy.isPauseMovement = true;
                            Vector2 tmpDirectionThrowEmOut;
                            if (collision.GetComponent<a_ThrowEmOut>().direct == "left") tmpDirectionThrowEmOut = new Vector2(-1, 0);
                            else tmpDirectionThrowEmOut = new Vector2(1, 0);
                            rb.AddForce(tmpDirectionThrowEmOut * throwEmOutForce, ForceMode2D.Impulse);
                        }
                        StartCoroutine(HitOnceCor(0.7f));
                    }
                    break;
                default:
                    break;
            }
        }              
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WaterCircle")
        {
            this.gameObject.GetComponent<Enemy>().moveSpeed = this.gameObject.GetComponent<Enemy>().moveSpeed * 2f;
        }            
    }

    // CollisionStay 안 쓰는 거 
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 피격 collider에 들어갔을 때 
        if ((collision.collider.tag == "DontChaseFox" |
            collision.collider.tag == "DontEscapeFromFox" |
            collision.collider.tag == "WaterCircle" |
            collision.collider.tag == "ThrowEnOut") &
            !hitOnce)
        {
            hitOnce = true;
            switch (collision.collider.tag)
            {
                case "DontChaseFox":
                    if(MobType != Type.fire)
                    {
                        print("DontChaseFox takes a hit");
                        HP = HP - 50;
                        StartCoroutine(HitOnceCor(0.2f));
                    }
                    break;
                case "DontEscapeFromFox":
                    if (MobType != Type.fire)
                    {
                        HP = HP - 150;
                        StartCoroutine(HitOnceCor(0.2f));
                    }
                    StartCoroutine(HitOnceCor(0.2f));
                    break;
                case "WaterCircle":

                    StartCoroutine(HitOnceCor(0.2f));
                    break;
                case "ThrowEnOut":
                    HP = HP - 150;
                    if (MobClassification == Classification.enemy)
                    {
                        Vector2 direction = (this.transform.position - collision.transform.position).normalized;
                        direction.y = 1;
                        rb.AddForce(direction * force, ForceMode2D.Impulse);
                    }
                    StartCoroutine(HitOnceCor(0.2f));
                    break;
                default:
                    break;
            }
        }

        // 물의 구체의 경우 
        if ( collision.collider.tag == "WaterCircle" )
        {
            //속도 느려지게 하는 거 
        }
    }
    */

    // 피격 
    public void gotHitReveal()
    {
        StartCoroutine(gotHitRevealCor());
    }
    IEnumerator gotHitRevealCor()
    {
        //print("gotHit");
        if (MobClassification == Classification.enemy) Hub.SFXManager.soldierGotHit.Play();
        else Hub.SFXManager.objectGotHIt.Play();
        for (int i = 0; i < everyRb.Length; i++)
        {
            everyRb[i].material = gotHitMatrial;
        }
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < everyRb.Length; i++)
        {
            everyRb[i].material = defaultMatrial;
        }
    }


    IEnumerator HitOnceCor(float amount)
    {
        yield return new WaitForSeconds(amount);
        try { enemy.isPauseMovement = false; } catch { }
        rb.velocity = Vector3.zero;
        hitOnce = false;
    }

    public void Fainting()
    {
        StartCoroutine(FaintingCor());
    }

    IEnumerator FaintingCor()
    {
        /*
        try
        {
            animator.speed = 0f;
            Destroy(rb);
            Destroy(this.gameObject.GetComponent<Enemy>());
        }
        catch { }
        
        for (int i = 0; i < everyRb.Length; i++)
        {
            everyRb[i].material = defaultMatrial;
        }

        Color tmpColor = everyRb[0].GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 1; i++)
        {
            tmpColor.a = i;
            for (int j = 0; j < everyRb.Length; i++)
            {
                everyRb[j].GetComponent<SpriteRenderer>().color = tmpColor;
            }
            yield return new WaitForSeconds(1f);
        }*/
        yield return null;
        Hub.SFXManager.enemyFainted.Play();
        Destroy(this.gameObject);
    }
}
