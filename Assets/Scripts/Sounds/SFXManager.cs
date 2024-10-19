using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource objectGet;
    public AudioSource keyGet;
    public AudioSource portal;
    public AudioSource jump;
    public AudioSource landing;
    public AudioSource gathingEnergyEnter;
    public AudioSource gathingEnergyStay;
    public AudioSource skillUse;
    public AudioSource playerGotHit;
    public AudioSource playerFainted;
    public AudioSource dontChaseFox;
    public AudioSource dontEscapeFromFox;
    public AudioSource waterCircle;
    public AudioSource throwEmOut;
    public AudioSource soldierMove;
    public AudioSource swordAttack;
    public AudioSource arrowAttack;
    public AudioSource fireSpiritAttack;
    public AudioSource soldierGotHit;
    public AudioSource objectGotHIt;
    public AudioSource enemyFainted;
    


    public void SFXJump()
    {
        jump.Play();
    }

    void Update()
    {
        /*
        // 점프 시 효과음 출력
        if (Hub.InputManager.isB &&
           (Hub.PlayerStatus.currentPlayerState == PlayerState.free |
           Hub.PlayerStatus.currentPlayerState == PlayerState.move))
        {
            jump.Play();
        }*/

        // 엎드렸을 시 효과음 출력
        /*if (Hub.InputManager.isDown)
        {
            crouch.Play();
        }*/

        // 공격 시 효과음 출력 (Hub의 InputManager에 isZ가 없어서 보류)
        //if (Hub.InputManager.)
        //{
        //    Attack.Play();
        //}
    }
}
