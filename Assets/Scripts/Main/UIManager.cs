using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Status")]
    // Slider를 교체함.
    //public Slider mpSlider;
    public Image mpSlider;
    public Text tmpTextField;
    public GameObject hpBarObject;
    public GameObject keyBarObject;
    public GameObject keyObject;
    public Sprite keyObject_not_found;
    public Sprite keyObject_found;
    
    [Header("Spirits")]
    public GameObject spiritFireMainUI;
    public GameObject spiritFireCoolDown;
    public GameObject spiritFireCover;
    [Space]
    public GameObject spiritWaterMainUI;
    public GameObject spiritWaterCoolDown;
    public GameObject spiritWaterCover;
    [Space]
    public GameObject spiritWindMainUI;
    public GameObject spiritWindCoolDown;
    public GameObject spiritWindCover;
    [Space]
    public GameObject spiritStoneMainUI;
    public GameObject spiritStoneCoolDown;
    public GameObject spiritStoneCover;

    [Header("UI")]
    public GameObject statusUI;
    public GameObject dialogUI;
    public int currentDialog;
    // 나머지 dialog는 a_Dialog.cs에 있음.
    public GameObject pauseUI;
    public GameObject faintUI;
    public GameObject faintUIComponents;
    public GameObject transition;

    //다른 것들
    private Color tmpColor;



    private void Awake()
    {
        tmpColor = Hub.UIManager.transition.GetComponent<Image>().color;
    }




    public void Pause()
    {
        print("recieved");
        pauseUI.SetActive(true);
        Hub.GameManager.CurrentGameState = GameState.pause;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        pauseUI.SetActive(false);
        Hub.GameManager.CurrentGameState = GameState.inGame;
        Time.timeScale = 1f;
    }

    public void Fainting()
    {
        // 중복 방지
        if (Hub.UIManager.faintUI.activeSelf == false)
        {
            StartCoroutine(FaintingCor());
        }
    }

    public IEnumerator FaintingCor()
    {
        faintUIComponents.SetActive(false);
        Hub.UIManager.faintUI.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1f);

        faintUIComponents.SetActive(true);
    }


}
