using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public string steamURL = "https://store.steampowered.com/";
    public GameObject windowsBackground;
    public GameObject windowCredit;
    public GameObject windowSetting;
    public GameObject clearText;
    public Text clearTextLocation;


    #region 메인 버튼 파트

    void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("IsClear", 0) == 1 &
            Input.GetKey(KeyCode.G) &
            Input.GetKey(KeyCode.E) &
            Input.GetKey(KeyCode.T))
        {
            clearText.SetActive(true);
            clearTextLocation.text = PlayerPrefs.GetFloat("Record", 9999999999999f).ToString();
        }
    }

    public void ButtonGameStart()
    {
        Hub.StageManager.GameOn("1. Stage1");        
    }

    public void ButtonCredit()
    {
        windowsBackground.SetActive(true);
        windowCredit.SetActive(true);
        //Hub.SoundManager.sfxButton();
    }

    public void ButtonExit()
    {
        Application.Quit(); 
    }

    #endregion

    #region 서브 버튼 파트

    public void ButtonSetting()
    {
        windowsBackground.SetActive(true);
        windowSetting.SetActive(true);
    }

    public void ButtonSteam()
    {
        Application.OpenURL(steamURL);
    }

    #endregion


    //////////////////////////////////////
    // 여기부터는 해당 메뉴 파트 부분   //
    //////////////////////////////////////


    #region Credit 메뉴 파트 


    #endregion


    #region Setting 메뉴 파트


    #endregion







}