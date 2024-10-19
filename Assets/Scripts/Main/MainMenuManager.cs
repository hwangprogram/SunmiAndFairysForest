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


    #region ���� ��ư ��Ʈ

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

    #region ���� ��ư ��Ʈ

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
    // ������ʹ� �ش� �޴� ��Ʈ �κ�   //
    //////////////////////////////////////


    #region Credit �޴� ��Ʈ 


    #endregion


    #region Setting �޴� ��Ʈ


    #endregion







}