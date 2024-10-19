using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <WinterSleep>
/// Wander 일단 필요 없어서 잘라둠. 
/// 그리고 detectionRange 작게 함(default값: 10f)
/// attackRange 추가함. 
/// box collider로 해놨더니 바닥 collider가 울퉁불퉁하면 못 넘겨서 바퀴 달아줌.
/// Update 리소스 너무 빡세서 FixedUpdate로 바꿈.
/// </summary>









public abstract class Enemy : MonoBehaviour
{
    [HideInInspector] public Transform player;
    public Transform homePosition;
    public float moveSpeed = 2f;

    public float detectionRange = 5.5f;
    public float returnRange = 7f;
    public float attackRange = 2f;

    public float wanderRadius = 8f;
    public float wanderInterval = 6f;

    //public int maxHealth = 100;

    private float wanderTimer;
    private Vector2 wanderTarget;
    private int currentHealth;

    protected bool isChasing = false;
    protected bool isAttackPosition = false;
    private bool _IsReturning = false;
    public bool IsReturning
    {
        get { return _IsReturning; }
        set
        {
            _IsReturning = value;
            if (value) StartCoroutine(ReturningCor());
        }
    }
    public bool isHoldingPosition = false;
    // 피격 시 움직이지 않을 때 사용하는 거 
    public bool isPauseMovement = false;
    protected Rigidbody2D rb;
    public Animator animator;
    private string facingDirectionHolder;
    private bool facingRight = false;

    private float idleThreshold = 2f; // 이동 속도가 이 값보다 작으면 idle 상태로 간주

    private float decisionIdleCooldown = 0f; // Wander 중 Idle 동작 결정 쿨타임
    private float decisionIdleInterval = 5f; // Wander 중 Idle 동작 결정 쿨타임 간격
    private bool idleState = false;

    protected virtual void Start()
    {
        facingDirectionHolder = transform.localScale.x > 0 ? "left" : "right";
        homePosition.SetParent(GameObject.FindGameObjectWithTag("EnemyPosition").transform);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();        
        wanderTimer = wanderInterval;
        SetWanderTarget();
        //currentHealth = maxHealth;
        // 이걸 true로 해둬서 공격은 하게 하기 
    }

    protected virtual void FixedUpdate()
    {
        //Debug.Log($"현재 isChasing : {isChasing}");
        HandleDetection();
        if (!isPauseMovement) HandleMovement();
        UpdateAnimationDirection();
    }

    private void HandleDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        /* 이렇게 안 하고, 공격할 때마다 isAttackPosition on으로  */
        
        if (distanceToPlayer <= attackRange) isAttackPosition = true;
        else { isAttackPosition = false; }

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > returnRange)
        {
            isChasing = false;
        }
        // WanderRadius 범위 안에서만 isChasing
        if (transform.position.x <= (homePosition.position.x - wanderRadius) | (homePosition.position.x + wanderRadius) <= transform.position.x)
        {
            isChasing = false;
            IsReturning = true;
        }
    }

    // WanderRadious에서 일단은 homePosition으로 
    IEnumerator ReturningCor()
    {
        yield return new WaitForSeconds(1.5f);
        IsReturning = false;
    }

    private void HandleMovement()
    {
        if (isChasing & !IsReturning & !isHoldingPosition)
        {
            // isChasing 상태이지만 AttackPosition이라서 이동 안 함.
            if (!isAttackPosition) ChasePlayer();
        }
        else if (Vector2.Distance(transform.position, homePosition.position) >= 0.1f)           //ReturnToHome()이랑 같이 수치 조정 필요
        {
            ReturnToHome();
        }
        else
        {
            // 7/1 확률로 가만히 있거나
            // 돌아다니기            
            //Wander();

            // WinterSleep: 임시적으로 Wander 사용하지 않음
            // 대신 방향 
            animator.SetBool("walk", false);
            if ((facingDirectionHolder == "right" & !facingRight) | (facingDirectionHolder == "left" & facingRight)) Flip();
        }
    }

    protected void ChasePlayer()
    {
        //Debug.Log("플레이어에게 따라가기");
        Vector2 direction = (player.position - transform.position);
        direction.x = direction.x > 0 ? 1f : -1f;
        //이렇게 하면 y축이 멋대로 0으로만 되어버려서 이거 안 하고
        //direction.y = 0; // y축 이동을 방지
        //rb.velocity = direction * moveSpeed;
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.x = direction.x * moveSpeed;
        rb.velocity = currentVelocity;

        if (animator != null)
        {
            //Debug.Log("walk");
            animator.SetBool("walk", true);
        }
    }

    protected void ReturnToHome()
    {
        //Debug.Log("지정된 위치로 가기");
        Vector2 direction = (homePosition.position - transform.position).normalized;        
        direction.x = direction.x > 0 ? 1f : -1f; 
        direction.y = 0; // y축 이동을 방지

        if (Vector2.Distance(transform.position, homePosition.position) < 0.3f)         //HandleMovement()이랑 같이 수치 조정 필요
        {
            rb.velocity = Vector3.zero;
            if (animator != null)
            {
                animator.SetBool("walk", false);
            }
        }
        else
        {
            //print(direction);
            //rb.velocity = direction * moveSpeed;
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x = direction.x * moveSpeed;
            rb.velocity = currentVelocity;
            if (animator != null)
            {
                animator.SetBool("walk", true);
            }
        }

    }

    protected void Wander()
    {
        wanderTimer += Time.deltaTime;
        decisionIdleCooldown += Time.deltaTime;

        if (decisionIdleCooldown >= decisionIdleInterval)
        {
            // 결정 쿨타임이 지나면 새로운 행동을 결정
            decisionIdleCooldown = 0;
            if (Random.Range(0, 6) == 0)
            {
                // 6분의 1 확률로 가만히 있기
                rb.velocity = Vector2.zero;
                if (animator != null)
                {
                    //Debug.Log("가만히 있기");
                    animator.SetBool("walk", false);
                    idleState = true;
                }
            }
            else
            {
                idleState = false;
            }
        }

        if (idleState) { return; }

        //Debug.Log("배회하기");


        if (wanderTimer >= wanderInterval || Vector2.Distance(transform.position, wanderTarget) < 0.1f) // 근처로 가거나 다시 배회해야할 때,
        {
            SetWanderTarget();
            wanderTimer = 0;
        }

        Vector2 direction = (wanderTarget - (Vector2)transform.position);
        direction.y = 0; // y축 이동을 방지
        direction = direction.normalized;

        rb.velocity = direction * moveSpeed;

        if (Vector2.Distance(transform.position, wanderTarget) < 0.5f)
        {
            // 목표 위치에 도달하면 정지 상태로 전환
            animator.SetBool("walk", false);
        }
        else
        {
            if (animator != null)
            {
                //Debug.Log("walk");
                animator.SetBool("walk", true);
            }
        }
    }
    protected void SetWanderTarget()
    {
        //Debug.Log("새로운 좌표로 배회");
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        wanderTarget = new Vector2(homePosition.position.x + randomX, transform.position.y);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Faint();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("hit");
            }
        }
    }
    protected void Faint()
    {
        //Debug.Log("적이 쓰러졌습니다!");
        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        gameObject.SetActive(false);
    }

    protected abstract void Attack();

    private void UpdateAnimationDirection()
    {
        if (rb.velocity.x > 0 && !facingRight && !isPauseMovement)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight && !isPauseMovement)
        {
            Flip();
        }

        if (Vector2.Distance(transform.position, homePosition.position) < 0.3f)
        {
            if (animator != null) animator.SetBool("walk", false);            
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, returnRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(homePosition.position, wanderRadius);
        
    }
}
