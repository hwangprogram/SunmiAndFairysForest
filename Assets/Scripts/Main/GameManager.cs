using System;
using System.Collections;
using System.Diagnostics;
using debug = UnityEngine.Debug;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    menu,
    inGame,
    pause,
}

public class GameManager : MonoBehaviour
{
    public GameState CurrentGameState;
    private Color tmpColor;
    public Stopwatch stopwatch = new Stopwatch();

    public void ResetPlayerPref()
    {
        PlayerPrefs.SetFloat("Record", 9999999999999f);
        PlayerPrefs.SetInt("IsClear", 0);
        PlayerPrefs.Save();
    }

    public void Awake()
    {
        PlayerPrefs.GetFloat("Record", 9999999999999f);
        PlayerPrefs.GetInt("IsClear", 0);
        PlayerPrefs.Save();
    }

    // 이렇게 안 하고 바로 GameState.inGame으로 바꿔버리면
    // dialog가 바로 풀려서 c를 누르게 되어버려서  
    public void DialogClose()
    {
        StartCoroutine(DialogCloseCor());
    }
    IEnumerator DialogCloseCor()
    {
        yield return new WaitForSeconds(0.7f);
        Hub.GameManager.CurrentGameState = GameState.inGame;
    }

    public void TimeAttackOn()
    {
        stopwatch.Start();
    }

    public void TimeAttackOff()
    {
        stopwatch.Stop();        
        double currentRecord = stopwatch.Elapsed.TotalSeconds;
        if (PlayerPrefs.GetFloat("Record") > (float)currentRecord)
        {
            PlayerPrefs.SetFloat("Record", (float)currentRecord);
        }
        PlayerPrefs.Save();

    }

    public void TimeAttackReset()
    {
        stopwatch.Reset();
    }







    /*

    // 초기화
    public void ResetPlayerState()
    {

    }

    public void ResetUIState()
    {

    }

    // 이번 스테이지 다시 
    public void ResetCurrentStage()
    {

    }

    // duration 1 또는 1.7f임.
    public void TransitionOn(float duration)
    {
        StartCoroutine(TransitionOnCor());
    }

    IEnumerator TransitionOnCor()
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1.7f);
    }

    public void TransitionOff()
    {
        StartCoroutine(TransitionOffCor());
    }


    public void Fainting()
    {
        StartCoroutine(FaintingCor());
    }

    public IEnumerator FaintingCor()
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1f);




    }

    public void ToFrontMenu()
    {

    }

    public void GameOn()
    {

    }

    public void ToStage()
    {

    }

    public void GameClear()
    {

    }


    public void InGamePause()
    {

    }

    */











}
