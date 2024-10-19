using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;

    [HideInInspector] public GameObject playerObject;
    public GameObject playerUI;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    public Animator animator;
    private Vector3 isPositionUpOrDown;

    public float horizontalMove;
    public bool isJumpAvailable = false;

    // 뷰 위치를 어디로 잡느냐에 사용 
    public GameObject playerPos;
    public float crouchDelay = 0.5f;

    [Header("Controll Part")]    
    public float jumpForce;
    public float moveSpeed;


    // 임시적으로 에너지 모으고 바로 앉기 안 되도록 하는 거 
    public bool isEnergyAndCrouch = false;
    

    
    

    private void Awake()
    {
        CatchPlayerObject();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void CatchPlayerObject()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerUI = FindObjectOfType<a_PlayerUI>().gameObject;
    }

    public void Update()
    {
        if (Hub.GameManager.CurrentGameState == GameState.inGame &
            Hub.PlayerStatus.currentPlayerState != PlayerState.gotHit)
        {
            // 점프할때 상승 중인지 하강 중인지 사용할까 말까 하는 부분들
            //animator.SetBool("isOnGround", isOnGround());
            //isPositionUpOrDown = this.transform.position;


            // 왼쪽 오른쪽 
            if (Hub.PlayerStatus.currentPlayerState == PlayerState.free |
                Hub.PlayerStatus.currentPlayerState == PlayerState.move)
            {
                if (Hub.InputManager.isLeft)
                {
                    animator.SetBool("isWalking", true);
                    horizontalMove = Hub.InputManager.horizontalInput * moveSpeed;
                    Hub.PlayerStatus.currentDirection = Direct.left;
                }
                else if (Hub.InputManager.isRight)
                {
                    animator.SetBool("isWalking", true);
                    horizontalMove = Hub.InputManager.horizontalInput * moveSpeed;
                    Hub.PlayerStatus.currentDirection = Direct.right;
                }
            }

            // 왼쪽 오른쪽 모두 떼고 있으면 horizontalMove를 0으로
            if ((Hub.InputManager.isLeft == false &&
                Hub.InputManager.isRight == false) |
                // 일부러 한 번 놔둬보자. 
                // skill 부분은 여기서 없애는게 아니라 PlayerUI에서 SkillList가 켜지면 horizontalMove = 0 되도록 해 놓음.
                // Hub.PlayerStatus.currentPlayerState == PlayerState.skill |
                Hub.PlayerStatus.currentPlayerState == PlayerState.gathering)
            {
                animator.SetBool("isWalking", false);
                //부드럽게 줄여도 될 거 같긴 하지만 일단은 그냥 바로 0으로 
                horizontalMove = 0f;
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
            }
            
            // 위 아래
            if (Hub.InputManager.isB &&
                (Hub.PlayerStatus.currentPlayerState == PlayerState.free |
                Hub.PlayerStatus.currentPlayerState == PlayerState.move |
                Hub.PlayerStatus.currentPlayerState == PlayerState.crouch))
            {
                // 전제조건: 포탈 안이 아닐 시
                // 이렇게 전제조건을 계속 사용하거나 추가할 예정
                // 근데 상호작용을 화살표 위에서 B로 바꾸어서 이거 사용 안 할 예정
                /*if (isOnGround() && Hub.PlayerStatus.isInPortal == false)
                {
                    isJumpAvailable = true;
                    print("jump it");
                }
                else { isJumpAvailable = false; }*/
                animator.SetBool("isJumping", true);
                isJumpAvailable = true;
                Hub.SFXManager.jump.Play();
                //print("jump it");                
            }

            if (Hub.InputManager.isDown &
                !Hub.InputManager.isA &
                (Hub.PlayerStatus.currentPlayerState == PlayerState.free |
                Hub.PlayerStatus.currentPlayerState == PlayerState.crouch) &
                Hub.GameManager.CurrentGameState == GameState.inGame &
                !isEnergyAndCrouch)
            {
                Hub.PlayerStatus.currentPlayerState = PlayerState.crouch;
                animator.SetBool("isSitting", true);
                if (crouchDelay > 0) crouchDelay = crouchDelay - 1.0f * Time.deltaTime;
            }
            else
            {
                animator.SetBool("isSitting", false);
                if (crouchDelay < 0.5f) { crouchDelay = crouchDelay + 1.0f * Time.deltaTime; }
                if (Hub.PlayerStatus.currentPlayerState == PlayerState.crouch) Hub.PlayerStatus.currentPlayerState = PlayerState.free;                
            }

            if (playerObject.transform.IsChildOf(this.transform))
            {
                if (crouchDelay <= 0)
                {
                    if (playerPos.transform.localPosition.y > -2) playerPos.transform.localPosition = new Vector3(0, -2f, 0);
                }
                else if (playerPos.transform.localPosition.y < -1 & playerPos.transform.parent == this.transform) playerPos.transform.localPosition = new Vector3(0, 0, 0);
            }
            


            /*
            if (Hub.PlayerStatus.currentPlayerState == PlayerState.free)
            {
                print("hey");
            }*/

            //스킬 UI 띄우거나 끄기
            if (Hub.PlayerStatus.currentPlayerState == PlayerState.free &&
                Hub.InputManager.isC == true &&
                playerUI.GetComponent<a_PlayerUI>().isOccupied == false)
            {
                // 여기서 없애는게 아니라 PlayerUI에서 SkillList가 켜지면 horizontalMove = 0 되도록 해 놓음.
                // horizontalMove = 0f;
                animator.SetBool("isPraying", true);
                // isCasting 부분을 여기에서 초기화
                Hub.PlayerController.animator.SetBool("isCasting", false);
                Hub.PlayerStatus.currentPlayerState = PlayerState.skill;
                playerUI.GetComponent<a_PlayerUI>().isOccupied = true;
                playerUI.GetComponent<a_PlayerUI>().SkillListOn();
            }

            if (Hub.PlayerStatus.currentPlayerState == PlayerState.skill &&
                Hub.InputManager.isC == true &&
                playerUI.GetComponent<a_PlayerUI>().isOccupied == false)
            {
                animator.SetBool("isPraying", false);
                Hub.PlayerStatus.currentPlayerState = PlayerState.free;
                playerUI.GetComponent<a_PlayerUI>().isOccupied = true;
                playerUI.GetComponent<a_PlayerUI>().SkillListOff();
            }


            if (Hub.InputManager.isESCOnce)
            {
                Hub.InputManager.isESCOnce = false;
                Hub.UIManager.Pause();
                Hub.BGMManager.PauseBgm();
                print("Pause pressed");                
            }


        }

        if (Hub.GameManager.CurrentGameState == GameState.pause &
            Hub.UIManager.pauseUI.activeSelf)
        {            
            if (Hub.InputManager.isESCOnce)
            {
                Hub.InputManager.isESCOnce = false;
                Hub.UIManager.Unpause();
            }
        }
    }

    public void FixedUpdate()
    {
        if (Hub.GameManager.CurrentGameState == GameState.inGame &
            Hub.PlayerStatus.currentPlayerState != PlayerState.gotHit)
        {
            // 이동하기 파트
            // 여기서 CharactorController2D에 값을 줌. 
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, isJumpAvailable);
            isJumpAvailable = false;


            // 에너지 모으기
            if (Hub.InputManager.isDown &&
                Hub.InputManager.isA &&
                (Hub.PlayerStatus.currentPlayerState == PlayerState.free |
                Hub.PlayerStatus.currentPlayerState == PlayerState.move |
                Hub.PlayerStatus.currentPlayerState == PlayerState.gathering))
            {
                animator.SetBool("isEnergy", true);
                if (Hub.SFXManager.gathingEnergyEnter.isPlaying == false) Hub.SFXManager.gathingEnergyEnter.Play();
                Hub.PlayerStatus.currentPlayerState = PlayerState.gathering;
                Hub.SkillManager.GatheringEnergy();
            }
            else if (Hub.PlayerStatus.currentPlayerState == PlayerState.gathering)
            {
                Hub.SFXManager.gathingEnergyEnter.Stop();
                animator.SetBool("isEnergy", false);
                Hub.PlayerStatus.currentPlayerState = PlayerState.free;
                isEnergyAndCrouch = true;
                StartCoroutine(EnergyAndCrouchCor());
            }
        }
    }

    IEnumerator EnergyAndCrouchCor()
    {
        yield return new WaitForSeconds(0.3f);
        isEnergyAndCrouch = false;
    }

    // 이거 inspector에서 이 메소드 사용함.
    public void Landing()
    {
        animator.SetBool("isJumping", false);
        //Hub.SFXManager.landing.Play();
    }

    /*public bool isOnGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, groundLayer.value))
        {            
            return true;
        }
        else
        {
            return false;
        }
    }*/


}
