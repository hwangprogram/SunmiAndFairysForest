using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentStage;
    public int[] RequiredKeyAmount;
    private Color tmpColor;

    public void Awake()
    {
        tmpColor = Hub.UIManager.transition.GetComponent<Image>().color;
    }


    /// <summary>
    /// 1. ToMenu
    /// 2. GameOn
    /// 3. ToScene
    /// 4. ResetScene
    /// 5. GameOff
    /// </summary>
        

    public void ToMenu()
    {
        // Pause에서 ToMenu() 사용할 경우엔 Pause 풀어주기로
        Time.timeScale = 1f;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.menu);
        StartCoroutine(ToMenuCor());
    }
    IEnumerator ToMenuCor()
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

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene("0. Menu");
        Hub.UIManager.statusUI.SetActive(false);
        Hub.GameManager.CurrentGameState = GameState.menu;
        tmpColor.a = 0f;
        Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.faintUI.SetActive(false);
        Hub.UIManager.Unpause();        
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.transition.SetActive(false);
        yield return null;
    }
    

    public void GameOn(string sceneName)
    {
        // 여기서 초기화를 담당함. 
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;
        Hub.PlayerStatus.isFainting = false;
        Hub.PlayerStatus.DestroyFullHP();
        Hub.PlayerStatus.CurrentMP = 0;
        Hub.PlayerStatus.IsFireGet = false;
        Hub.PlayerStatus.IsWaterGet = false;
        Hub.PlayerStatus.IsWindGet = false;
        Hub.PlayerStatus.IsStoneGet = false;
        Hub.PlayerStatus.GetFullHP(2);
        Hub.PlayerStatus.SetCurrentHP(2);
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.inGame);
        // 여기서 키 리스트를 없애고 currentStage를 초기화
        Hub.PlayerStatus.DestroyFullKey(RequiredKeyAmount[currentStage]);
        currentStage = 0;

        Hub.GameManager.TimeAttackReset();
        Hub.GameManager.TimeAttackOn();

        StartCoroutine(GameOnCor(sceneName));
    }
    IEnumerator GameOnCor(string sceneName)
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

        ///////////////////////////////////////////////////////////////////////////
        //여기서 key list를 추가함. 초기화는 GameOff()에서 함.
        Hub.PlayerStatus.GetFullKey(RequiredKeyAmount[currentStage]);
        SceneManager.LoadScene(sceneName);
        Hub.PlayerStatus.SaveCurrentState();
        // dialog일 때 player가 움직이지 않게 하려고 pause인거임.
        Hub.GameManager.CurrentGameState = GameState.pause;
        Hub.UIManager.statusUI.SetActive(true);
        Hub.UIManager.dialogUI.SetActive(true);
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
        Hub.CVManager.cv[0].Play();
    }

       
    public void ToScene(string sceneName)
    {   
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;
        StartCoroutine(ToSceneCor(sceneName));        
    }
    IEnumerator ToSceneCor(string sceneName)
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
        yield return new WaitForSeconds(0.7f);

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene(sceneName);
        // sceneName 방식을 사용하고 있어서 string을 파싱하기보다는
        // 일단 currentStage를 매번 +1 하는 방식을 사용함. 
        // 어차피 맨 처음은 GameOn()에서 하고, 엔딩은 GameOff()에서 하니까
        // GameOn에서는 0인 채로 그대로, GameOff에서는 0으로 초기화하는 방식으로 사용.
        // 나중에 다시 시작하기나 이런 걸 추가하게 될 경우 이걸 반드시 다른 방식으로 바꿔야함!! 
        Hub.PlayerStatus.DestroyFullKey(RequiredKeyAmount[currentStage]);
        currentStage += 1;
        Hub.PlayerStatus.GetFullKey(RequiredKeyAmount[currentStage]);
        // 다시 한 번 확인하는 거임.
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.SaveCurrentState();
        // 포탈 사용 도중에 Pause했으면 그거 풀기
        Hub.UIManager.Unpause();
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }


    public void ResetScene()
    {
        Time.timeScale = 1f;
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;        
        StartCoroutine(ResetSceneCor());
    }
    IEnumerator ResetSceneCor()
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

        ///////////////////////////////////////////////////////////////////////////
        int tmpScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(tmpScene, LoadSceneMode.Single);
        Hub.PlayerStatus.LoadCurrentState();
        tmpColor.a = 0f;
        Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.faintUI.SetActive(false);
        Hub.UIManager.Unpause();
        // 이걸 scene 바뀌기 전에 풀어버리면 검은 화면에서 자꾸 enemy 가 공격해서 이것만 잠시 이 위치에 둠.
        Hub.PlayerStatus.isFainting = false;
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }
       

    public void GameOff(string sceneName)
    {
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.gameOff);
        StartCoroutine(GameOffCor(sceneName));

        Hub.GameManager.TimeAttackOff();
        PlayerPrefs.SetInt("IsClear", 1);
        PlayerPrefs.Save();
    }
    IEnumerator GameOffCor(string sceneName)
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
        yield return new WaitForSeconds(2.3f);

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene(sceneName);
        Hub.GameManager.CurrentGameState = GameState.menu;
        Hub.UIManager.dialogUI.SetActive(false);
        Hub.UIManager.statusUI.SetActive(false);        
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }









}
