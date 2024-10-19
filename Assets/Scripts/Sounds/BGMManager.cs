using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>




public class BGMManager : MonoBehaviour
{
    [Header("BGM ����Ʈ")]
    public AudioSource menu;
    public AudioSource inGame;
    public AudioSource gameOff;
    [HideInInspector] public AudioSource currentBgm;

    [Header("Adjustment")]
    public float delayDuration = 3f;                                    // �󸶳� �����̸� �ΰ� ���� ��� �����Ұ���
    [HideInInspector] public float transitionTime = 3.0f;               // �غôµ� 3�ʰ� ���Ƽ� 3�ʷ� �Ƚ�, �Ƚ��ϰ� �׳� ����.


    private void Awake()
    {
        currentBgm = menu;
        PlayBgm();
    }

    // ���� ��� play�ϴ� �Լ�
    public void PlayBgm()
    {
        currentBgm.Play();
    }

    // ��� �Ͻ������ϴ� �Լ�
    public void PauseBgm()
    {
        // EscŰ�� ������ �� ����� �����ȴٰ� ����
        // playercontroller.cs���� �� �޼ҵ带 ����� ����.
        if (Hub.GameManager.CurrentGameState == GameState.inGame |
            Hub.GameManager.CurrentGameState == GameState.pause)
        {
            // ���� ����� ���� ���̶��
            if (Hub.GameManager.CurrentGameState == GameState.inGame)
            {
                currentBgm.Pause();
                Debug.Log("BGM Paused");
            }
            // �ƴ϶�� (�̹� Pause ���¶��)
            else
            {
                currentBgm.UnPause();
                Debug.Log("BGM Resumed");
            }
        }
    }

    // �����ε����� �ٲ� 
    // ��Ȳ�� ���� delayDuration�� �ٸ��� �ؾ� �� �� ���Ƽ� �߰���.
    // delayDuration�� ������ �ְ�, �ƴϸ� �⺻ delayDuration���� 
    public void ChangeBgm(AudioSource newBgm) { StartCoroutine(ChangeBgmCor(newBgm, delayDuration)); }  
    public void ChangeBgm(AudioSource newBgm, float delayDuration) { StartCoroutine(ChangeBgmCor(newBgm, delayDuration)); }

    private IEnumerator ChangeBgmCor(AudioSource newBgm, float delayDuration)
    {
        // ���� ������� Bgm�� FadeOut
        if (currentBgm.isPlaying)
        {
            StartCoroutine(FadeOut(currentBgm));
        }

        // ���ο� Bgm�� FadeIn
        yield return new WaitForSeconds(delayDuration);
        currentBgm = newBgm;        
        StartCoroutine(FadeIn(currentBgm));        
        
    }

    // �ڿ������� ������ ���� FadeIn, FadeOut �ڷ�ƾ ����

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
