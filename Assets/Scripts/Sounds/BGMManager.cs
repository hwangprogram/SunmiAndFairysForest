using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>




public class BGMManager : MonoBehaviour
{
    [Header("BGM 리스트")]
    public AudioSource menu;
    public AudioSource inGame;
    public AudioSource gameOff;
    [HideInInspector] public AudioSource currentBgm;

    [Header("Adjustment")]
    public float delayDuration = 3f;                                    // 얼마나 딜레이를 두고 다음 브금 실행할건지
    [HideInInspector] public float transitionTime = 3.0f;               // 해봤는데 3초가 좋아서 3초로 픽스, 픽스하고 그냥 숨김.


    private void Awake()
    {
        currentBgm = menu;
        PlayBgm();
    }

    // 메인 브금 play하는 함수
    public void PlayBgm()
    {
        currentBgm.Play();
    }

    // 브금 일시정지하는 함수
    public void PauseBgm()
    {
        // Esc키를 눌렀을 때 브금이 정지된다고 가정
        // playercontroller.cs에서 이 메소드를 사용할 것임.
        if (Hub.GameManager.CurrentGameState == GameState.inGame |
            Hub.GameManager.CurrentGameState == GameState.pause)
        {
            // 현재 브금이 실행 중이라면
            if (Hub.GameManager.CurrentGameState == GameState.inGame)
            {
                currentBgm.Pause();
                Debug.Log("BGM Paused");
            }
            // 아니라면 (이미 Pause 상태라면)
            else
            {
                currentBgm.UnPause();
                Debug.Log("BGM Resumed");
            }
        }
    }

    // 오버로딩으로 바꿈 
    // 상황에 따라 delayDuration을 다르게 해야 할 것 같아서 추가함.
    // delayDuration이 있으면 넣고, 아니면 기본 delayDuration으로 
    public void ChangeBgm(AudioSource newBgm) { StartCoroutine(ChangeBgmCor(newBgm, delayDuration)); }  
    public void ChangeBgm(AudioSource newBgm, float delayDuration) { StartCoroutine(ChangeBgmCor(newBgm, delayDuration)); }

    private IEnumerator ChangeBgmCor(AudioSource newBgm, float delayDuration)
    {
        // 현재 재생중인 Bgm을 FadeOut
        if (currentBgm.isPlaying)
        {
            StartCoroutine(FadeOut(currentBgm));
        }

        // 새로운 Bgm을 FadeIn
        yield return new WaitForSeconds(delayDuration);
        currentBgm = newBgm;        
        StartCoroutine(FadeIn(currentBgm));        
        
    }

    // 자연스러운 변경을 위해 FadeIn, FadeOut 코루틴 생성

    // FadeOut
    private IEnumerator FadeOut(AudioSource currentBgm)
    {
        float startVolume = currentBgm.volume;

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            currentBgm.volume = Mathf.Lerp(startVolume, 0, t / transitionTime);
            yield return null;
        }

        currentBgm.volume = 0;
        currentBgm.Stop();
        currentBgm.volume = startVolume;
    }

    // FadeIn
    private IEnumerator FadeIn(AudioSource currentBgm)
    {
        float tmpVolume = currentBgm.volume;
        currentBgm.volume = 0;
        currentBgm.Play();

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            currentBgm.volume = Mathf.Lerp(0, tmpVolume, t / transitionTime);
            yield return null;
        }

        currentBgm.volume = tmpVolume;
    }
}
